using Eqstra.BusinessLogic;
using Eqstra.BusinessLogic.DeliveryModel;
using Eqstra.BusinessLogic.DocumentDelivery;
using Eqstra.BusinessLogic.Helpers;
using Eqstra.DocumentDelivery.UILogic.DDServiceProxy;
using Eqstra.DocumentDelivery.UILogic.Helpers;
using Microsoft.Practices.Prism.PubSubEvents;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
        private Eqstra.DocumentDelivery.UILogic.DDServiceProxy.MzkCollectDeliveryServiceClient client;
        IEventAggregator _eventAggregator;
        ConnectionProfile _connectionProfile;
        Action _syncExecute;
        private CDUserInfo _userInfo;
        private Func<Exception, bool> _errorHandler;

        public static DDServiceProxyHelper Instance
        {
            get
            {
                return instance;
            }
        }
        public Eqstra.DocumentDelivery.UILogic.DDServiceProxy.MzkCollectDeliveryServiceClient ConnectAsync(string userName, string password, IEventAggregator eventAggregator, string domain = "lfmd")
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
                client = new DDServiceProxy.MzkCollectDeliveryServiceClient(basicHttpBinding, new EndpointAddress("http://srfmlbispstg01.lfmd.co.za/MicrosoftDynamicsAXAif60/CollectDeliveryService/xppservice.svc"));
                client.ClientCredentials.UserName.UserName = domain + "\"" + userName;
                client.ClientCredentials.UserName.Password = password;
                client.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Identification;
                client.ClientCredentials.Windows.ClientCredential = new NetworkCredential(userName, password, domain);
                return client;
            }
            catch (Exception ex)
            {
                Util.ShowToast(ex.Message);
                return client;
            }
        }

        async public System.Threading.Tasks.Task<MzkCollectDeliveryServiceValidateUserResponse> ValidateUser(string userId, string password)
        {
            try
            {
                return await client.validateUserAsync(userId, password);
            }
            catch (Exception)
            {
                throw;
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
            try
            {
                if (_connectionProfile != null && _connectionProfile.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess)
                {
                    _syncExecute.Invoke();
                }
            }
            catch (Exception ex)
            {
                AppSettings.Instance.ErrorMessage = ex.Message;

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
                var result = await client.getTasksAsync(_userInfo.UserId, _userInfo.CompanyId);

                List<CollectDeliveryTask> cdTaskList = new List<CollectDeliveryTask>();
                if (result.response != null)
                {
                    result.response.AsParallel().ForAll(mzkTask =>
                    {
                        cdTaskList.Add(new Eqstra.BusinessLogic.CollectDeliveryTask
                        {
                            CaseNumber = mzkTask.parmCaseId,
                            Address = mzkTask.parmCustAddress,
                            CustomerName = mzkTask.parmCustName,
                            CustomerNumber = mzkTask.parmCustPhone,
                            Status = mzkTask.parmStatus,
                            StatusDueDate = mzkTask.parmStatusDueDate,
                            RegistrationNumber = mzkTask.parmRegNo,
                            AllocatedTo = _userInfo.Name,
                            Make = mzkTask.parmMake,
                            Model = mzkTask.parmModel,
                            VehicleInsRecId = mzkTask.parmCaseCategoryRecID,
                            // CaseType = mzkTask.parmCaseCategory,
                            DeliveryDate = mzkTask.parmDeliveryDateTime,
                            DeliveryTime = mzkTask.parmDeliveryDateTime,
                            EmailId = mzkTask.parmCustomerEmail,
                            CustPartyId = mzkTask.parmCustPartyId
                        });

                    });
                }
                return cdTaskList;
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
        async public System.Threading.Tasks.Task<List<Document>> GetDocumentsInfoFromSvcAsync()
        {
            try
            {
                var connectionProfile = NetworkInformation.GetInternetConnectionProfile();
                if (connectionProfile == null || connectionProfile.GetNetworkConnectivityLevel() != NetworkConnectivityLevel.InternetAccess)
                    return null;

                if (_userInfo == null)
                {
                    _userInfo = PersistentData.Instance.UserInfo;
                }
                var result = await client.getDocumentsInfoAsync(null, _userInfo.CompanyId);

                List<Document> documentList = new List<Document>();
                if (result.response != null)
                {

                    result.response.AsParallel().ForAll(mzkTask =>
                    {
                        documentList.Add(new Eqstra.BusinessLogic.Document
                        {
                            //CaseNumber=mzkTask.parmCaseCategoryRecID,
                            DocumentType = mzkTask.parmDocuTypeID,
                            Make = mzkTask.parmMake,
                            Model = mzkTask.parmModel,
                            RegistrationNumber = mzkTask.parmRegNo,
                            // SerialNumber="",


                        });
                    });
                }
                return documentList;
            }
            catch (Exception ex)
            {
                AppSettings.Instance.ErrorMessage = ex.Message;
                return null;
            }
        }
        async public System.Threading.Tasks.Task<List<CDCustomerDetails>> GetCustomerInfoFromSvcAsync(long partyId)
        {
            try
            {
                var connectionProfile = NetworkInformation.GetInternetConnectionProfile();
                if (connectionProfile == null || connectionProfile.GetNetworkConnectivityLevel() != NetworkConnectivityLevel.InternetAccess)
                    return null;

                if (_userInfo == null)
                {
                    _userInfo = PersistentData.Instance.UserInfo;
                }
                var result = await client.getCustomerInfoAsync(partyId, _userInfo.CompanyId);

                List<CDCustomerDetails> customerDetailsList = new List<CDCustomerDetails>();
                if (result.response != null)
                {

                    result.response.AsParallel().ForAll(mzkTask =>
                    {
                        customerDetailsList.Add(new Eqstra.BusinessLogic.DocumentDelivery.CDCustomerDetails
                        {
                            CustomerName = mzkTask.parmContactPersonName
                        });
                    });
                }
                return customerDetailsList;
            }
            catch (Exception ex)
            {
                AppSettings.Instance.ErrorMessage = ex.Message;
                return null;
            }
        }


    }
}
