using Eqstra.BusinessLogic;
using Eqstra.BusinessLogic.DeliveryModel;
using Eqstra.BusinessLogic.DocumentDelivery;
using Eqstra.BusinessLogic.Enums;
using Eqstra.BusinessLogic.Helpers;
using Eqstra.DocumentDelivery.UILogic.DDServiceProxy;
using Eqstra.DocumentDelivery.UILogic.Helpers;
using Microsoft.Practices.Prism.PubSubEvents;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;
using Windows.Storage;
using Windows.UI.Core;

namespace Eqstra.DocumentDelivery.UILogic.AifServices
{
    public class DDServiceProxyHelper
    {
        private static readonly DDServiceProxyHelper instance = new DDServiceProxyHelper();
        private Eqstra.DocumentDelivery.UILogic.DDServiceProxy.MzkCollectDeliveryServiceClient _client;
        IEventAggregator _eventAggregator;
        ConnectionProfile _connectionProfile;
        Action _syncExecute;
        private CDUserInfo _userInfo;
        public static DDServiceProxyHelper Instance
        {
            get
            {
                return instance;
            }
        }
        public async System.Threading.Tasks.Task<Eqstra.DocumentDelivery.UILogic.DDServiceProxy.MzkCollectDeliveryServiceClient> ConnectAsync(string userName, string password, IEventAggregator eventAggregator, string domain = "lfmd")
        {
            try
            {
                _eventAggregator = eventAggregator;
                BasicHttpBinding basicHttpBinding = new BasicHttpBinding()
                {
                    MaxBufferPoolSize = int.MaxValue,
                    MaxBufferSize = int.MaxValue,
                    MaxReceivedMessageSize = int.MaxValue,
                    OpenTimeout = new TimeSpan(2, 0, 0),
                    ReceiveTimeout = new TimeSpan(2, 0, 0),
                    SendTimeout = new TimeSpan(2, 0, 0),
                    AllowCookies = true
                };

                basicHttpBinding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
                basicHttpBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Windows;
                _client = new DDServiceProxy.MzkCollectDeliveryServiceClient(basicHttpBinding, new EndpointAddress("http://srfmlbispstg01.lfmd.co.za/MicrosoftDynamicsAXAif60/CollectDeliverService/xppservice.svc"));
                _client.ClientCredentials.UserName.UserName = domain + "\"" + userName;
                _client.ClientCredentials.UserName.Password = password;
                _client.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Identification;
                _client.ClientCredentials.Windows.ClientCredential = new NetworkCredential(userName, password, domain);
                return _client;
            }
            catch (Exception ex)
            {
                Util.ShowToast(ex.Message);
                return _client;
            }
        }

        public async System.Threading.Tasks.Task SendMessageToUIThread(string receivedMsg)
        {
            CoreDispatcher dispatcher = Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher;
            await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {

                AppSettings.Instance.IsSynchronizing = 0;
                AppSettings.Instance.ErrorMessage = receivedMsg;
            });
        }

        async public System.Threading.Tasks.Task<bool> ValidateUser(string userId, string password)
        {
            try
            {
                return !(await _client.validateUserAsync(userId, password)).response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        async public System.Threading.Tasks.Task<MzkCollectDeliveryServiceGetUserDetailsResponse> GetUserInfo(string userId)
        {
            return await _client.getUserDetailsAsync(userId);
        }

        public void Synchronize(Action syncExecute)
        {
            _syncExecute = syncExecute;
            _connectionProfile = NetworkInformation.GetInternetConnectionProfile();
            NetworkInformation.NetworkStatusChanged += NetworkInformation_NetworkStatusChanged;
            if (_connectionProfile != null && _connectionProfile.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess)
            {
                System.Threading.Tasks.Task.Factory.StartNew(syncExecute);

            }
        }
        void NetworkInformation_NetworkStatusChanged(object sender)
        {
            try
            {
                if (_connectionProfile != null && _connectionProfile.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess)
                {
                    _syncExecute.Invoke();
                }
            }
            catch (Exception ex)
            {
                this.SendMessageToUIThread(ex.Message);

            }

        }

        async public System.Threading.Tasks.Task SynchronizeAllAsync()
        {
            try
            {
                var connectionProfile = NetworkInformation.GetInternetConnectionProfile();
                if (connectionProfile == null || connectionProfile.GetNetworkConnectivityLevel() != NetworkConnectivityLevel.InternetAccess)
                    return;

                if (_userInfo == null)
                {
                    _userInfo = PersistentData.Instance.UserInfo;
                }
                if (AppSettings.Instance.IsSynchronizing == 0)
                {
                    Synchronize(async () =>
                       {
                           await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                           {

                               AppSettings.Instance.IsSynchronizing = 1;
                           });


                           await this.SyncTasksFromSvcAsync();
                           await this.InsertContactDetailsToSvcAsync();
                           await this.InsertDocumentCollectedDetailsToSvcAsync();
                           await this.InsertDocumentDeliveryDetailsToSvcAsync();
                           await this.UpdateTaskStatusAsync();
                           await this.GetCollectedFromSvcAsync();

                           await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                           {

                               AppSettings.Instance.IsSynchronizing = 0;
                           });

                       });
                }
            }
            catch (Exception ex)
            {
                this.SendMessageToUIThread(ex.Message);
            }
        }

        public async System.Threading.Tasks.Task<ObservableCollection<CollectDeliveryTask>> GroupTasksByCustomer()
        {
            try
            {
                var allTaskList = (await SqliteHelper.Storage.LoadTableAsync<CollectDeliveryTask>()).Where(w => w.Status != CDTaskStatus.Completed);
                var taskbuckets = allTaskList.GroupBy(x => new { x.CustomerId, x.TaskType }).Select(s => s.First());

                ObservableCollection<CollectDeliveryTask> tasks = new ObservableCollection<CollectDeliveryTask>();
                foreach (var singleTask in taskbuckets)
                {
                    singleTask.CDTasksDetails = new ObservableCollection<CDTaskDetails>();
                    foreach (var item in allTaskList)
                    {
                        if (item != null && item.CustomerId == singleTask.CustomerId && item.TaskType == singleTask.TaskType)
                        {
                            singleTask.CDTasksDetails.Add(new CDTaskDetails
                            {
                                CaseNumber = item.CaseNumber,
                                CustomerName = item.CustomerName,
                                TaskType = item.TaskType,
                                ContactName = item.ContactName,
                                Address =  item.Address.Replace(System.Environment.NewLine, ","),
                                DocumentCount = item.DocumentCount,
                                AllocatedTo = item.AllocatedTo,
                                CaseCategoryRecID = item.CaseCategoryRecID,
                                SerialNumber = item.SerialNumber,
                                RegistrationNumber = item.RegistrationNumber,
                                MakeModel = item.MakeModel,
                                Status = item.Status,
                                StatusDueDate = item.StatusDueDate,
                                DeliveryDate = item.DeliveryDate,

                            });

                        }
                    }
                    tasks.Add(singleTask);
                }
                return tasks;
            }
            catch (Exception ex)
            {
                this.SendMessageToUIThread(ex.Message);
                throw;
            }
        }

        async public System.Threading.Tasks.Task SyncTasksFromSvcAsync()
        {
            try
            {
                var connectionProfile = NetworkInformation.GetInternetConnectionProfile();
                if (connectionProfile == null || connectionProfile.GetNetworkConnectivityLevel() != NetworkConnectivityLevel.InternetAccess)
                    return;

                if (_userInfo == null)
                {
                    _userInfo = PersistentData.Instance.UserInfo;
                }
              
                var result = await _client.getTasksAsync(_userInfo.UserId, _userInfo.CompanyId);
                var taskData = await SqliteHelper.Storage.LoadTableAsync<CollectDeliveryTask>();
                List<CollectDeliveryTask> taskInsertList = new List<CollectDeliveryTask>();
                List<CollectDeliveryTask> taskUpdateList = new List<CollectDeliveryTask>();
                ObservableCollection<long> caseCategoryRecIdList = new ObservableCollection<long>();
                if (result.response != null)
                {
                    foreach (var mzkTask in result.response.GroupBy(g => g.parmCaseId).Select(s => s.First()))
                    {
                        var taskTosave = new CollectDeliveryTask
                          {
                              CaseNumber = mzkTask.parmCaseId,
                              Address = mzkTask.parmCustAddress,
                              CustomerName = mzkTask.parmCustName,
                              CustomerNumber = mzkTask.parmCustPhone,
                              Status = Eqstra.BusinessLogic.Helpers.TaskStatus.AwaitCollectionDetail,
                              StatusDueDate = mzkTask.parmStatusDueDate,
                              RegistrationNumber = mzkTask.parmRegNo,
                              AllocatedTo = _userInfo.Name,
                              MakeModel = mzkTask.parmMake + Environment.NewLine + mzkTask.parmModel,
                              CaseCategoryRecID = mzkTask.parmCaseCategoryRecID,

                              DeliveryDate = mzkTask.parmDeliveryDateTime,
                              EmailId = mzkTask.parmCustomerEmail,
                              CustPartyId = mzkTask.parmCustPartyId,
                              CaseRecID = mzkTask.parmCaseRecID,
                              CaseServiceRecID = mzkTask.parmCaseServiceRecID,
                              TaskType = (CDTaskType)Enum.Parse(typeof(CDTaskType), mzkTask.parmCollectDeliverType.ToString()),
                              CustomerId = mzkTask.parmCustAccount,
                              //DocumentCount = Int32.Parse(mzkTask.parmNoOfRecords),
                              ServiceId = mzkTask.parmServiceId,
                              ServiceRecID = mzkTask.parmServiceRecID,
                              UserID = mzkTask.parmUserID,
                              SerialNumber = mzkTask.parmSerialNumber,
                              ContactName = mzkTask.parmContactPersonName,
                             // DocumentType
                          };
                        if (taskData.Any(s => s.CaseNumber == mzkTask.parmCaseId))
                        {
                            taskUpdateList.Add(taskTosave);
                        }
                        else
                        {
                            taskInsertList.Add(taskTosave);
                        }
                        caseCategoryRecIdList.Add(mzkTask.parmCaseCategoryRecID);

                    }

                    if (taskUpdateList.Any())
                        await SqliteHelper.Storage.UpdateAllAsync<CollectDeliveryTask>(taskUpdateList);


                    if (taskInsertList.Any())
                        await SqliteHelper.Storage.InsertAllAsync<CollectDeliveryTask>(taskInsertList);
                }
                await GetCollectedFromSvcAsync();
            }
            catch (Exception ex)
            {
                this.SendMessageToUIThread(ex.Message);

            }

        }
    
        async public System.Threading.Tasks.Task GetCollectedFromSvcAsync()
        {
            try
            {
                var connectionProfile = NetworkInformation.GetInternetConnectionProfile();
                if (connectionProfile == null || connectionProfile.GetNetworkConnectivityLevel() != NetworkConnectivityLevel.InternetAccess)
                    return;

                if (_userInfo == null)
                {
                    _userInfo = PersistentData.Instance.UserInfo;
                }
                var collectedFromData = (await SqliteHelper.Storage.LoadTableAsync<CollectedFromData>());
                 var result = await _client.getCollectedFromAsync(_userInfo.CompanyId);
                if (result.response != null)
                {
                    foreach (var mzk in result.response)
                    {
                        if (collectedFromData != null && collectedFromData.Any(a => a.UserID != mzk.parmUserID))
                        {
                            await SqliteHelper.Storage.InsertSingleRecordAsync<CollectedFromData>(new CollectedFromData { UserID = mzk.parmUserID, UserName = mzk.parmUserName });
                        }
                        else
                        {
                            await SqliteHelper.Storage.UpdateSingleRecordAsync<CollectedFromData>(new CollectedFromData { UserID = mzk.parmUserID, UserName = mzk.parmUserName });
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                this.SendMessageToUIThread(ex.Message);
            }
        }
        async public System.Threading.Tasks.Task InsertDocumentCollectedDetailsToSvcAsync()
        {
            try
            {
                var connectionProfile = NetworkInformation.GetInternetConnectionProfile();
                if (connectionProfile == null || connectionProfile.GetNetworkConnectivityLevel() != NetworkConnectivityLevel.InternetAccess)
                    return;

                if (_userInfo == null)
                {
                    _userInfo = PersistentData.Instance.UserInfo;
                }
                var cdDataList = (await SqliteHelper.Storage.LoadTableAsync<DocumentDeliveryDetails>()).Where(x => x.IsCollected && !x.IsDelivered);
                ObservableCollection<MZKDocumentCollectedDetailsContract> mzkDocumentCollectedDetailsContractColl = new ObservableCollection<MZKDocumentCollectedDetailsContract>();
                if (cdDataList != null)
                {
                    foreach (var doc in cdDataList)
                    {
                        mzkDocumentCollectedDetailsContractColl.Add(new MZKDocumentCollectedDetailsContract()
                        {
                            parmCaseId = doc.CaseNumber,
                            parmCollectedFrom = doc.SelectedCollectedFrom,
                            parmComment = doc.Comment,
                            parmEmail = doc.Email,
                            parmPosition = doc.Position,
                            parmReceivedBy = doc.ReceivedBy,
                            parmReceivedDate = doc.ReceivedDate,
                            parmReferenceNo = doc.ReferenceNo,
                            parmSignature = doc.CRSignature,
                            parmTelePhone = doc.Phone

                        });
                    }
                }
                var res = await _client.insertDocumentCollectedDetailsAsync(mzkDocumentCollectedDetailsContractColl, _userInfo.CompanyId);
            }
            catch (Exception ex)
            {
                this.SendMessageToUIThread(ex.Message);
            }
        }


        async public System.Threading.Tasks.Task InsertDocumentDeliveryDetailsToSvcAsync()
        {
            try
            {
                var connectionProfile = NetworkInformation.GetInternetConnectionProfile();
                if (connectionProfile == null || connectionProfile.GetNetworkConnectivityLevel() != NetworkConnectivityLevel.InternetAccess)
                    return;

                if (_userInfo == null)
                {
                    _userInfo = PersistentData.Instance.UserInfo;
                }
                var cdDataList = (await SqliteHelper.Storage.LoadTableAsync<DocumentDeliveryDetails>()).Where(x => x.IsDelivered);
                ObservableCollection<MzkDocumentDeliveryDetailsContrcat> mzkDocumentDeliverDetailsContractColl = new ObservableCollection<MzkDocumentDeliveryDetailsContrcat>();
                if (cdDataList != null)
                {
                    foreach (var doc in cdDataList)
                    {
                        mzkDocumentDeliverDetailsContractColl.Add(new MzkDocumentDeliveryDetailsContrcat()
                        {
                            parmCaseId = doc.CaseNumber,
                            parmComment = doc.Comment,
                            parmEmail = doc.Email,
                            parmPosition = doc.Position,
                            parmSignature = doc.CRSignature,
                            parmTelephone = doc.Phone,
                            parmDeliveryDetailDateTime = doc.DeliveryDate,
                            parmDeliveryPerson = doc.DeliveryPersonName,
                        });
                    }
                }
                var res = await _client.insertDocumentDeliveryDetailsAsync(mzkDocumentDeliverDetailsContractColl, _userInfo.CompanyId);
            }
            catch (Exception ex)
            {
                this.SendMessageToUIThread(ex.Message);
            }
        }


        async public System.Threading.Tasks.Task InsertContactDetailsToSvcAsync()
        {
            try
            {
                var connectionProfile = NetworkInformation.GetInternetConnectionProfile();
                if (connectionProfile == null || connectionProfile.GetNetworkConnectivityLevel() != NetworkConnectivityLevel.InternetAccess)
                    return;

                if (_userInfo == null)
                {
                    _userInfo = PersistentData.Instance.UserInfo;
                }
                var ContactsData = (await SqliteHelper.Storage.LoadTableAsync<ContactPerson>());
                ObservableCollection<MZKDocumentNewContactPersonContract> mzkDocumentNewContactPersonContractColl = new ObservableCollection<MZKDocumentNewContactPersonContract>();
                if (ContactsData != null)
                {
                    foreach (var contact in ContactsData)
                    {
                        mzkDocumentNewContactPersonContractColl.Add(new MZKDocumentNewContactPersonContract()
                        {
                            parmEmail = contact.Email,
                            parmFirstName = contact.FirstName,
                            parmSurname = contact.Surname,
                            parmPhone = contact.CellPhone

                        });
                    }
                }
                var res = await _client.insertDocumentNewContactPersonAsync(mzkDocumentNewContactPersonContractColl, _userInfo.CompanyId);
            }
            catch (Exception ex)
            {
                this.SendMessageToUIThread(ex.Message);
            }
        }


        public async System.Threading.Tasks.Task UpdateTaskStatusAsync()
        {
            try
            {
                var connectionProfile = NetworkInformation.GetInternetConnectionProfile();
                if (connectionProfile == null || connectionProfile.GetNetworkConnectivityLevel() != NetworkConnectivityLevel.InternetAccess)
                    return;
                if (_userInfo == null)
                {
                    _userInfo = PersistentData.Instance.UserInfo;
                }
                var tasks = (await SqliteHelper.Storage.LoadTableAsync<CollectDeliveryTask>());
                ObservableCollection<MZKCollectDeliverTasksContract> mzkTasks = new ObservableCollection<MZKCollectDeliverTasksContract>();
                Dictionary<string, EEPActionStep> actionStepMapping = new Dictionary<string, EEPActionStep>();

                //actionStepMapping.Add(Eqstra.BusinessLogic.Enums.CDTaskStatus.AwaitLicenceDisc, EEPActionStep.AwaitLicenceDisc);
                //actionStepMapping.Add(Eqstra.BusinessLogic.Enums.CDTaskStatus.Completed, EEPActionStep.Completed);
                //actionStepMapping.Add(Eqstra.BusinessLogic.Enums.CDTaskStatus.AwaitInvoice, EEPActionStep.AwaitInvoice);
                //actionStepMapping.Add(Eqstra.BusinessLogic.Enums.CDTaskStatus.AwaitDeliveryConfirmation, EEPActionStep.AwaitDeliveryConfirmation);

                if (tasks != null)
                {
                    foreach (var task in tasks)
                    {
                        mzkTasks.Add(
                            new MZKCollectDeliverTasksContract
                            {
                                parmCaseId = task.CaseNumber,
                                parmStatus = task.Status,
                                parmStatusDueDate = task.StatusDueDate,
                                parmEEPActionStep = actionStepMapping[task.Status]
                            });
                    }
                    var res = await _client.updateStatusListAsync(mzkTasks, (MzkLoginType)Enum.Parse(typeof(MzkLoginType), _userInfo.CDUserType.ToString()), _userInfo.CompanyId);

                    var taskList = new ObservableCollection<CollectDeliveryTask>();
                    if (res.response != null)
                    {
                        foreach (var x in res.response.Where(x => x != null))
                        {
                            taskList.Add(new CollectDeliveryTask
                            {
                                CaseNumber = x.parmCaseId,
                                Address = x.parmCustAddress,
                                CustomerName = x.parmCustName,
                                CustomerNumber = x.parmCustPhone,
                                Status = x.parmStatus,
                                StatusDueDate = x.parmStatusDueDate,
                                RegistrationNumber = x.parmRegNo,
                                AllocatedTo = _userInfo.Name,
                                CaseCategoryRecID = x.parmCaseCategoryRecID,
                                // CaseType = mzkTask.parmCaseCategory,
                                DeliveryDate = x.parmDeliveryDateTime,
                                DeliveryTime = x.parmDeliveryDateTime,
                                EmailId = x.parmCustomerEmail,
                                CustPartyId = x.parmCustPartyId

                            });
                        }
                    }
                    await SqliteHelper.Storage.UpdateAllAsync<CollectDeliveryTask>(taskList);
                }
            }

            catch (Exception ex)
            {
                this.SendMessageToUIThread(ex.Message);
            }

        }
    }
}
