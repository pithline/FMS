
using Eqstra.BusinessLogic;
using Eqstra.BusinessLogic.Base;
using Eqstra.BusinessLogic.Helpers;
using Eqstra.BusinessLogic.TI;
using Eqstra.TechnicalInspection.UILogic.TIService;
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

namespace Eqstra.TechnicalInspection.UILogic.AifServices
{
    public class TIServiceHelper
    {
        private static readonly TIServiceHelper instance = new TIServiceHelper();
        private TIService.MzkTechnicalInspectionClient client;
        ConnectionProfile _connectionProfile;
        IEventAggregator _eventAggregator;
        Action _syncExecute;
        private UserInfo _userInfo;

        static TIServiceHelper()
        {

        }

        public static TIServiceHelper Instance
        {
            get
            {
                return instance;
            }
        }
        public MzkTechnicalInspectionClient ConnectAsync(string userName, string password, IEventAggregator eventAggregator, string domain = "lfmd")
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
                basicHttpBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Windows;//http://srfmlaxtest01/MicrosoftDynamicsAXAif60/TechnicalInspection/xppservice.svc
                client = new TIService.MzkTechnicalInspectionClient(basicHttpBinding, new EndpointAddress("http://srfmlbispstg01.lfmd.co.za/MicrosoftDynamicsAXAif60/TechnicalInspection/xppservice.svc"));
                client.ClientCredentials.UserName.UserName = domain + "\"" + userName;
                client.ClientCredentials.UserName.Password = password;
                client.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Impersonation;
                client.ClientCredentials.Windows.ClientCredential = new NetworkCredential(userName, password, domain);
                return client;
            }
            catch (Exception ex)
            {
                return client;
            }
        }

        public void Synchronize(Action syncExecute)
        {
            _syncExecute = syncExecute;
            _connectionProfile = NetworkInformation.GetInternetConnectionProfile();
            NetworkInformation.NetworkStatusChanged += NetworkInformation_NetworkStatusChanged;
            if (_connectionProfile != null && _connectionProfile.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess)
            {
                System.Threading.Tasks.Task.Factory.StartNew(syncExecute);
                //_syncExecute.Invoke();
            }
        }

        void NetworkInformation_NetworkStatusChanged(object sender)
        {
            if (_connectionProfile != null && _connectionProfile.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess)
            {
                _syncExecute.Invoke();
            }

        }

        public void Synchronize()
        {
            try
            {

                AppSettings.Instance.IsSynchronizing = 1;


                Synchronize(async () =>
                   {

                       await InsertTechnicalInspectionAsync();
                       await UpdateTaskStatusAsync();

                       await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                       {
                           AppSettings.Instance.IsSynchronizing = 0;
                       });
                   });

            }
            catch (Exception ex)
            {
                Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {

                    AppSettings.Instance.IsSynchronizing = 0;
                    AppSettings.Instance.ErrorMessage = ex.Message + ex.InnerException;
                });
            }
        }
        async public System.Threading.Tasks.Task<MzkTechnicalInspectionValidateUserResponse> ValidateUser(string userId, string password)
        {
            try
            {

                return await client.validateUserAsync(userId, password);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        async public System.Threading.Tasks.Task<MzkTechnicalInspectionGetUserDetailsResponse> GetUserInfoAsync(string userId)
        {
            try
            {
                return await client.getUserDetailsAsync(userId);
            }
            catch (Exception)
            {
                throw;
            }
        }
        async public System.Threading.Tasks.Task SyncFromSvcAsync(BaseModel baseModel)
        {
            try
            {
                if (_userInfo == null)
                {
                    _userInfo = JsonConvert.DeserializeObject<UserInfo>(ApplicationData.Current.RoamingSettings.Values[Constants.UserInfo].ToString());
                }
                if (AppSettings.Instance.IsSynchronizing == 0)
                {
                    Synchronize(async () =>
                    {
                        await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {

                            AppSettings.Instance.IsSynchronizing = 1;
                        });

                        if (baseModel is TIData)
                        {
                            await this.InsertTechnicalInspectionAsync();
                        }

                        await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {

                            AppSettings.Instance.IsSynchronizing = 0;
                        }
                              );
                    });
                }
            }
            catch (Exception ex)
            {
                Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {

                    AppSettings.Instance.IsSynchronizing = 0;
                    AppSettings.Instance.ErrorMessage = ex.Message + ex.InnerException;
                });
            }
        }
        public async System.Threading.Tasks.Task GetTasksAsync()
        {
            try
            {
                if (_userInfo == null)
                {
                    _userInfo = JsonConvert.DeserializeObject<UserInfo>(ApplicationData.Current.RoamingSettings.Values[Constants.UserInfo].ToString());
                }
                var updateList = new List<Eqstra.BusinessLogic.TITask>();
                var insertList = new List<Eqstra.BusinessLogic.TITask>();

                var subCompUpdateList = new List<MaintenanceRepair>();
                var subCompInsertList = new List<MaintenanceRepair>();

                var allTasks = await SqliteHelper.Storage.LoadTableAsync<Eqstra.BusinessLogic.TITask>();

                var result = await client.getTasksAsync(_userInfo.UserId, _userInfo.CompanyId);
                 if (result != null)
                {
                    foreach (var task in result.response)
                    {
                        var tiTask = new Eqstra.BusinessLogic.TITask
                        {
                            CaseServiceRecID = task.parmCaseServiceRecID,
                            ServiceRecID = task.parmCaseRecID,
                            CaseNumber = task.parmCaseId,
                            CaseCategory = task.parmCaseCategory,
                            ContactName = task.parmContactPersonName,
                            ContactNumber = task.parmContactPersonPhone,
                            CustPhone = task.parmCustPhone,
                            CustomerName = task.parmCustName,
                            Status = task.parmStatus,
                            StatusDueDate = task.parmStatusDueDate,
                            UserId = task.parmUserID,
                            Address = task.parmContactPersonAddress,
                            CustomerId = task.parmCustAccount,
                            VehicleInsRecId = task.parmCaseServiceRecID,
                            ConfirmedDate = task.parmInspectionDueDate,
                            AllocatedTo = _userInfo.Name,
                            Email = task.parmEmail

                        };


                        var subComponents = await client.getSubComponentsAsync(new ObservableCollection<long> { tiTask.CaseServiceRecID }, _userInfo.CompanyId);
                        var allSubComponents = await SqliteHelper.Storage.LoadTableAsync<MaintenanceRepair>();

                        if (subComponents != null)
                        {
                            foreach (var item in subComponents.response)
                            {
                                var subComponent = new MaintenanceRepair
                                {
                                    SubComponent = item.parmSubComponent,
                                    MajorComponent = item.parmMajorComponent,
                                    Action = item.parmAction,
                                    Cause = item.parmCause,
                                    CaseServiceRecId = tiTask.CaseServiceRecID,
                                    Repairid = item.parmRecID

                                };

                                if (!allSubComponents.Any(x => x.Repairid == item.parmRecID))
                                {
                                    await SqliteHelper.Storage.InsertSingleRecordAsync(subComponent);

                                }
                            }
                        }

                        insertList.Add(tiTask);

                    }
                    await SqliteHelper.Storage.DropnCreateTableAsync<Eqstra.BusinessLogic.TITask>();

                    await SqliteHelper.Storage.InsertAllAsync(insertList);

                }
            }
            catch (Exception ex)
            {
                Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                                  {

                                      AppSettings.Instance.IsSynchronizing = 0;
                                      AppSettings.Instance.ErrorMessage = ex.Message + ex.InnerException;
                                  });
            }


        }
        public async System.Threading.Tasks.Task InsertTechnicalInspectionAsync()
        {
            try
            {

                if (_userInfo == null)
                {
                    _userInfo = JsonConvert.DeserializeObject<UserInfo>(ApplicationData.Current.RoamingSettings.Values[Constants.UserInfo].ToString());
                }
                var technicalInspData = (await SqliteHelper.Storage.LoadTableAsync<Eqstra.BusinessLogic.TI.TIData>()).Where(x => x.ShouldSave);
                ObservableCollection<MzkCaseServiceAuthorizationContract> mzkMobiTrailerAccessoriesContractColl = new ObservableCollection<MzkCaseServiceAuthorizationContract>();
                if (technicalInspData != null)
                {
                    foreach (var x in technicalInspData)
                    {
                        mzkMobiTrailerAccessoriesContractColl.Add(new MzkCaseServiceAuthorizationContract()
                            {
                                parmDamageCause = x.CauseOfDamage,
                                parmRemedy = x.Remedy,
                                parmRecommendation = x.Recommendation,
                                parmCompletionDate = x.CompletionDate,
                                // parmCaseCategoryAuthList = x.CaseCategoryAuthList,
                                parmCaseServiceRecID = x.VehicleInsRecID

                            });
                    }
                }
                var res = await client.insertTechnicalInspectionAsync(mzkMobiTrailerAccessoriesContractColl, _userInfo.CompanyId);

                var technicalInspList = new ObservableCollection<TIData>();
                if (res.response != null)
                {
                    foreach (var x in res.response.Where(x => x != null))
                    {
                        technicalInspList.Add(new TIData
                        {
                            CauseOfDamage = x.parmDamageCause,
                            Remedy = x.parmRemedy,
                            Recommendation = x.parmRecommendation,
                            CompletionDate = x.parmCompletionDate,
                            ShouldSave = false,
                            // parmCaseCategoryAuthList = x.CaseCategoryAuthList,
                            VehicleInsRecID = x.parmCaseServiceRecID

                        });
                    }
                    await SqliteHelper.Storage.UpdateAllAsync<TIData>(technicalInspList);
                }
            }
            catch (Exception ex)
            {
                Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {

                    AppSettings.Instance.IsSynchronizing = 0;
                    AppSettings.Instance.ErrorMessage = ex.Message;
                });
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
                    _userInfo = JsonConvert.DeserializeObject<UserInfo>(ApplicationData.Current.RoamingSettings.Values[Constants.UserInfo].ToString());
                }
                var tasks = (await SqliteHelper.Storage.LoadTableAsync<Eqstra.BusinessLogic.TITask>());
                ObservableCollection<MzkTechnicalTasksContract> mzkTasks = new ObservableCollection<MzkTechnicalTasksContract>();
                Dictionary<string, EEPActionStep> actionStepMapping = new Dictionary<string, EEPActionStep>();
                actionStepMapping.Add(Eqstra.BusinessLogic.Helpers.TaskStatus.Completed, EEPActionStep.AwaitTechnicalInspection);

                if (tasks != null)
                {

                    foreach (var task in tasks.Where(x =>
                        x.Status == Eqstra.BusinessLogic.Helpers.TaskStatus.Completed))
                    {
                        mzkTasks.Add(
                            new MzkTechnicalTasksContract

                            {
                                parmCaseServiceRecID = task.CaseServiceRecID,
                                parmServiceRecID = task.ServiceRecID,
                                parmCaseId = task.CaseNumber,
                                parmCustAddress = task.Address,
                                parmCustName = task.CustomerName,
                                parmCustPhone = task.CustPhone,
                                parmContactPersonPhone = task.ContactNumber,
                                parmContactPersonName = task.ContactName,
                                parmStatus = task.Status,
                                parmStatusDueDate = task.StatusDueDate,
                                parmUserID = task.UserId,
                                parmEEPActionStep = actionStepMapping[task.Status]
                            });
                    }
                    var res = await client.updateStatusListAsync(mzkTasks, _userInfo.CompanyId);

                    var taskList = new ObservableCollection<Eqstra.BusinessLogic.TITask>();
                    if (res.response != null)
                    {
                        foreach (var item in tasks)
                        {
                            if (res.response.Any(x => x.parmStatus == item.Status && x.parmCaseId == item.CaseNumber))
                            {
                                await SqliteHelper.Storage.DeleteSingleRecordAsync(item);
                            }
                        }
                    }

                }
            }
            catch (SQLite.SQLiteException)
            {

            }
            catch (Exception ex)
            {
                Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {

                    AppSettings.Instance.IsSynchronizing = 0;
                    AppSettings.Instance.ErrorMessage = ex.Message;
                });
            }

        }
    }
}
