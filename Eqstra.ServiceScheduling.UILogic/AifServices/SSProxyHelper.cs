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
                    _userInfo = await JsonConvert.DeserializeObjectAsync<UserInfo>(ApplicationData.Current.RoamingSettings.Values[Constants.UserInfo].ToString());
                }

                var result = await client.getTasksAsync(_userInfo.UserId, _userInfo.CompanyId);
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


        async public System.Threading.Tasks.Task<DetailServiceScheduling> GetServiceDetailsFromSvcAsync(string caseNumber, long caseServiceRecId)
        {
            try
            {
                var connectionProfile = NetworkInformation.GetInternetConnectionProfile();
                if (connectionProfile == null || connectionProfile.GetNetworkConnectivityLevel() != NetworkConnectivityLevel.InternetAccess)
                    return null;

                if (_userInfo == null)
                {
                    _userInfo = await JsonConvert.DeserializeObjectAsync<UserInfo>(ApplicationData.Current.RoamingSettings.Values[Constants.UserInfo].ToString());
                }
                var result = await client.getServiceDetailsAsync(caseNumber, caseServiceRecId, _userInfo.CompanyId);
                DetailServiceScheduling detailServiceScheduling = null;
                if (result.response != null)
                {

                    result.response.AsParallel().ForAll(mzkTask =>
                    {
                        detailServiceScheduling = (new Eqstra.BusinessLogic.ServiceSchedule.DetailServiceScheduling
                        {
                            Address = mzkTask.parmAddress,
                            AdditionalWork = mzkTask.parmAdditionalWork,
                            ServiceDateOption1 = mzkTask.parmPreferredDateFirstOption,
                            ServiceDateOption2 = mzkTask.parmPreferredDateSecondOption,
                            ODOReading = mzkTask.parmODOReading,
                            ODOReadingDate = mzkTask.parmODOReadingDate,
                            ServiceType = mzkTask.parmServiceType,
                            LocationType = new List<string> { mzkTask.parmServiceType },
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


        async public System.Threading.Tasks.Task InsertServiceDetailsToSvcAsync(DetailServiceScheduling detailServiceScheduling, string caseNumber, long caseServiceRecId)
        {
            try
            {
                var connectionProfile = NetworkInformation.GetInternetConnectionProfile();
                if (connectionProfile == null || connectionProfile.GetNetworkConnectivityLevel() != NetworkConnectivityLevel.InternetAccess)
                    return;

                if (_userInfo == null)
                {
                    _userInfo = await JsonConvert.DeserializeObjectAsync<UserInfo>(ApplicationData.Current.RoamingSettings.Values[Constants.UserInfo].ToString());
                }
                var result = client.insertServiceDetailsAsync(caseNumber, caseServiceRecId, new MzkServiceDetailsContract
                {
                    parmAdditionalWork = detailServiceScheduling.AdditionalWork,
                    parmAddress = detailServiceScheduling.Address,
                    parmContactPersonName = detailServiceScheduling.ContactPersonName,
                    parmEventDesc = detailServiceScheduling.EventDesc,
                    parmLocationType = detailServiceScheduling.LocationType.ToString(),
                    parmODOReading = detailServiceScheduling.ODOReading,
                    parmODOReadingDate = detailServiceScheduling.ODOReadingDate,
                    parmPreferredDateFirstOption = detailServiceScheduling.ServiceDateOption1,
                    parmPreferredDateSecondOption = detailServiceScheduling.ServiceDateOption2,
                    parmServiceType = detailServiceScheduling.ServiceType,
                    parmSupplierName = detailServiceScheduling.SupplierName,
                    parmContactPersonPhone = detailServiceScheduling.ContactPersonPhone

                }, _userInfo.CompanyId);

            }
            catch (Exception ex)
            {
                AppSettings.Instance.ErrorMessage = ex.Message;
            }
        }
    }
}