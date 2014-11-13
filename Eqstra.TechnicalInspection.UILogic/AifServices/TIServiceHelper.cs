
using Eqstra.BusinessLogic;
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
                basicHttpBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Windows;
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

        public async System.Threading.Tasks.Task Synchronize()
        {
            try
            {
                if (AppSettings.Instance.IsSynchronizing == 0)
                {
                    Synchronize(async () =>
                       {


                           await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                           {
                               AppSettings.Instance.IsSynchronizing = 0;
                           });
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
                var updateList = new List<Eqstra.BusinessLogic.Task>();
                var insertList = new List<Eqstra.BusinessLogic.Task>();

                var subCompUpdateList = new List<MaintenanceRepair>();
                var subCompInsertList = new List<MaintenanceRepair>();

                var allTasks = await SqliteHelper.Storage.LoadTableAsync<Eqstra.BusinessLogic.Task>();
                var allSubComponents = await SqliteHelper.Storage.LoadTableAsync<MaintenanceRepair>();
                var result = await client.getTasksAsync(_userInfo.UserId, _userInfo.CompanyId);
                if (result != null)
                {
                    foreach (var task in result.response)
                    {
                        var tiTask = new Eqstra.BusinessLogic.Task
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
                            Address = task.parmCustAddress,
                            CustomerId = task.parmCustAccount,

                        };

                       var subComponents =  await client.getSubComponentsAsync(new ObservableCollection<long>{ tiTask.CaseServiceRecID}, _userInfo.CompanyId);

                       foreach (var item in subComponents.response)
                       {
                           var subComponent = new MaintenanceRepair
                           {
                               SubComponent = item.parmSubComponent,
                               MajorComponent = item.parmMajorComponent,
                               Action = item.parmAction,
                               Cause = item.parmCause,
                               CaseServiceRecId = item.parmCaseServiceRecID,
                               RecId = item.parmRecID

                           };

                           if (allSubComponents.Any(x=>x.RecId == item.parmRecID ))
                           {
                               subCompUpdateList.Add(subComponent);
                           }
                           else
                           {
                               subCompInsertList.Add(subComponent); 
                           }


                       }
                       await SqliteHelper.Storage.InsertAllAsync(subCompInsertList);
                       await SqliteHelper.Storage.UpdateAllAsync(subCompUpdateList);
                        if (allTasks.Any(x => x.CaseNumber == task.parmCaseId))
                        {
                            updateList.Add(tiTask);

                        }
                        else
                        {
                            insertList.Add(tiTask);
                        }

                    }

                    await SqliteHelper.Storage.InsertAllAsync(insertList);
                    await SqliteHelper.Storage.UpdateAllAsync(updateList);
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

        public async System.Threading.Tasks.Task GetSubComponentsAsync()
        {

            try
            {
                if (_userInfo == null)
                {
                    _userInfo = JsonConvert.DeserializeObject<UserInfo>(ApplicationData.Current.RoamingSettings.Values[Constants.UserInfo].ToString());
                }
                ObservableCollection<long> caseServiceRedIds = new ObservableCollection<long>();
                ObservableCollection<MaintenanceRepair> subCompInsertList = new ObservableCollection<MaintenanceRepair>();
                ObservableCollection<MaintenanceRepair> subCompUpdateList = new ObservableCollection<MaintenanceRepair>();
                var tasks = await SqliteHelper.Storage.LoadTableAsync<Eqstra.BusinessLogic.Task>();
                var ids = tasks.Select(x => x.CaseServiceRecID);
                foreach (var caseServiceRecId in ids)
                {
                    caseServiceRedIds.Add(caseServiceRecId);
                }

                var result = await client.getSubComponentsAsync(caseServiceRedIds, _userInfo.CompanyId);

                if (result != null)
                {
                    foreach (var item in result.response)
                    {
                        var subComponent = new MaintenanceRepair
                        {
                            SubComponent = item.parmSubComponent,
                            MajorComponent = item.parmMajorComponent,
                            Action = item.parmAction,
                            Cause = item.parmCause,
                            CaseServiceRecId = item.parmCaseServiceRecID,
                            RecId = item.parmRecID

                        };
                       
                            subCompInsertList.Add(subComponent); 
                        
                    }
                }

                await SqliteHelper.Storage.InsertAllAsync(subCompInsertList);
            }
            catch (Exception  ex)
            {
                Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    AppSettings.Instance.IsSynchronizing = 0;
                    AppSettings.Instance.ErrorMessage = ex.Message + ex.InnerException;
                });
            }

        }


        
    }
}
