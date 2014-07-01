using Eqstra.BusinessLogic;
using Eqstra.BusinessLogic.Helpers;
using System;
using System.Net;
using Windows.Networking.Connectivity;
using Eqstra.ServiceScheduling.UILogic.SSProxy;
using Newtonsoft.Json;
using Windows.Storage;
using Eqstra.BusinessLogic.ServiceSchedule;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Core;
using System.Diagnostics;
using Eqstra.BusinessLogic.ServiceSchedulingModel;
namespace Eqstra.ServiceScheduling.UILogic.AifServices
{
    public class SSProxyHelper
    {
        private static readonly SSProxyHelper instance = new SSProxyHelper();
        private Eqstra.ServiceScheduling.UILogic.SSProxy.MzkServiceSchedulingServiceClient client;
        ConnectionProfile _connectionProfile;
        Action _syncExecute;
        private UserInfo _userInfo;
        static SSProxyHelper()
        {
        }
        public static SSProxyHelper Instance
        {
            get
            {
                return instance;
            }
        }
        public async System.Threading.Tasks.Task<Eqstra.ServiceScheduling.UILogic.SSProxy.MzkServiceSchedulingServiceClient> ConnectAsync(string userName, string password, string domain = "lfmd")
        {
            try
            {
                client = new Eqstra.ServiceScheduling.UILogic.SSProxy.MzkServiceSchedulingServiceClient();
                client.ClientCredentials.Windows.ClientCredential = new NetworkCredential(userName, password, domain);
                return client;
            }
            catch (Exception ex)
            {
                Util.ShowToast(ex.Message);
                return client;
            }
        }
        async public System.Threading.Tasks.Task<MzkServiceSchedulingServiceValidateUserResponse> ValidateUser(string userId, string password)
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
            if (_connectionProfile != null && _connectionProfile.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess)
            {
                _syncExecute.Invoke();
            }

        }
        async public System.Threading.Tasks.Task<List<DriverTask>> GetTasksFromSvcAsync()
        {
            try
            {
                var connectionProfile = NetworkInformation.GetInternetConnectionProfile();
                if (connectionProfile == null || connectionProfile.GetNetworkConnectivityLevel() != NetworkConnectivityLevel.InternetAccess)
                    return null;

                if (_userInfo == null)
                {
                    _userInfo = JsonConvert.DeserializeObject<UserInfo>(ApplicationData.Current.RoamingSettings.Values[Constants.UserInfo].ToString());
                }
               
                var result = await client.getTasksOptimizeAsync(_userInfo.UserId, _userInfo.CompanyId);              
               
                List<DriverTask> driverTaskList = new List<DriverTask>();
                if (result.response != null)
                {

                    result.response.AsParallel().ForAll(mzkTask =>
                    {
                        driverTaskList.Add(new Eqstra.BusinessLogic.ServiceSchedule.DriverTask
                        {
                            CaseNumber = mzkTask.parmCaseID,
                            Address = mzkTask.parmCustAddress,
                            CustomerName = mzkTask.parmCustName,
                            CustPhone = mzkTask.parmCustPhone,
                            Status = mzkTask.parmStatus,
                            StatusDueDate = mzkTask.parmStatusDueDate,
                            RegistrationNumber = mzkTask.parmRegistrationNum,
                            AllocatedTo = _userInfo.Name,
                            UserId = mzkTask.parmUserID,
                            CaseCategory = mzkTask.parmCaseCategory,
                            CaseServiceRecID = mzkTask.parmCaseServiceRecID,
                            DriverFirstName = mzkTask.parmDriverFirstName,
                            DriverLastName = mzkTask.parmDriverLastName,
                            DriverPhone = mzkTask.parmDriverPhone,
                            Make = mzkTask.parmMake,
                            Model = mzkTask.parmModel,
                            Description = mzkTask.parmVehicleDescription,
                        });

                    });
                }
                return driverTaskList;
            }
            catch (Exception ex)
            {
                AppSettings.Instance.ErrorMessage = ex.Message;
                return null;
            }
        }


        async public System.Threading.Tasks.Task<ServiceSchedulingDetail> GetServiceDetailsFromSvcAsync(string caseNumber, long caseServiceRecId)
        {
            try
            {
                var connectionProfile = NetworkInformation.GetInternetConnectionProfile();
                if (connectionProfile == null || connectionProfile.GetNetworkConnectivityLevel() != NetworkConnectivityLevel.InternetAccess)
                    return null;

                if (_userInfo == null)
                {
                    _userInfo = JsonConvert.DeserializeObject<UserInfo>(ApplicationData.Current.RoamingSettings.Values[Constants.UserInfo].ToString());
                }
                var result = await client.getServiceDetailsAsync(caseNumber, caseServiceRecId, _userInfo.CompanyId);
                ServiceSchedulingDetail detailServiceScheduling = null;
                if (result.response != null)
                {

                    result.response.AsParallel().ForAll(mzkTask =>
                    {
                        detailServiceScheduling = (new Eqstra.BusinessLogic.ServiceSchedule.ServiceSchedulingDetail
                        {
                            Address = mzkTask.parmAddress,
                            AdditionalWork = mzkTask.parmAdditionalWork,
                            ServiceDateOption1 = mzkTask.parmPreferredDateFirstOption,
                            ServiceDateOption2 = mzkTask.parmPreferredDateSecondOption,
                            ODOReading = mzkTask.parmODOReading,
                            ODOReadingDate = mzkTask.parmODOReadingDate,
                            //ServiceType =(await client.getLocationTypeAsync(caseServiceRecId, _userInfo.CompanyId)).response.Select(s=>s.),
                            LocationType = mzkTask.parmLocationType.Split(',').ToList<string>(),
                            SupplierName = mzkTask.parmSupplierName,
                            EventDesc = mzkTask.parmEventDesc,
                            ContactPersonName = mzkTask.parmContactPersonName,
                            ContactPersonPhone = mzkTask.parmContactPersonPhone
                        });
                    });

                }

                return detailServiceScheduling;
            }
            catch (Exception ex)
            {
                AppSettings.Instance.ErrorMessage = ex.Message;
                return null;
            }
        }

        async public System.Threading.Tasks.Task GetServiceDetailsAsync()
        {
          // var result = await client.getServiceDetailsAsync(caseNumber, caseServiceRecId, _userInfo.CompanyId);
        }

        async public System.Threading.Tasks.Task InsertConfirmationServiceSchedulingToSvcAsync(ConfirmationServiceScheduling confirmationServiceScheduling, string caseNumber, long caseServiceRecId)
        {
            try
            {
                var connectionProfile = NetworkInformation.GetInternetConnectionProfile();
                if (connectionProfile == null || connectionProfile.GetNetworkConnectivityLevel() != NetworkConnectivityLevel.InternetAccess)
                    return;

                if (_userInfo == null)
                {
                    _userInfo =  JsonConvert.DeserializeObject<UserInfo>(ApplicationData.Current.RoamingSettings.Values[Constants.UserInfo].ToString());
                }
                var result = client.insertServiceDetailsAsync(caseNumber, caseServiceRecId, new MzkServiceDetailsContract
                {
                    parmAdditionalWork = confirmationServiceScheduling.AdditionalWork,
                    parmAddress = confirmationServiceScheduling.Address,
                    parmContactPersonName = confirmationServiceScheduling.ContactPersonName,
                    parmEventDesc = confirmationServiceScheduling.EventDesc,
                    parmLocationType = confirmationServiceScheduling.LocationType,
                    parmODOReading = confirmationServiceScheduling.ODOReading,
                    parmODOReadingDate = confirmationServiceScheduling.ODOReadingDate,
                    parmPreferredDateFirstOption = confirmationServiceScheduling.ServiceDateOption1,
                    parmPreferredDateSecondOption = confirmationServiceScheduling.ServiceDateOption2,
                    parmServiceType = confirmationServiceScheduling.ServiceType,
                    parmSupplierName = confirmationServiceScheduling.SupplierName,
                    parmContactPersonPhone = confirmationServiceScheduling.ContactPersonPhone,
                }, _userInfo.CompanyId);

            }
            catch (Exception ex)
            {
                AppSettings.Instance.ErrorMessage = ex.Message;
            }
        }
    }
}