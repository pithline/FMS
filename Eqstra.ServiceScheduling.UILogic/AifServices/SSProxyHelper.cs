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


        async public System.Threading.Tasks.Task<List<Country>> GetCountryRegionListFromSvcAsync()
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
                var result = await client.getCountryRegionListAsync(_userInfo.CompanyId);
                List<Country> countryList = new List<Country>();
                if (result.response != null)
                {

                    result.response.OrderBy(o => o.parmCountryRegionName).AsParallel().ForAll(mzk =>
                    {
                        countryList.Add(
                            new Country
                            {
                                Name = mzk.parmCountryRegionName,
                                Id = mzk.parmCountryRegionId
                            }
                            );
                    });
                }

                return countryList;

            }
            catch (Exception ex)
            {
                AppSettings.Instance.ErrorMessage = ex.Message;
                return null;
            }
        }


        async public System.Threading.Tasks.Task<List<province>> GetProvinceListFromSvcAsync(string countryId)
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
                var result = await client.getProvinceListAsync(countryId, _userInfo.CompanyId);
                List<province> provinceList = new List<province>();
                if (result.response != null)
                {

                    result.response.AsParallel().ForAll(mzk =>
                    {
                        provinceList.Add(new province { Name = mzk.parmStateName, Id = mzk.parmStateId });
                    });
                }
                return provinceList;
            }
            catch (Exception ex)
            {
                AppSettings.Instance.ErrorMessage = ex.Message;
                return null;
            }
        }


        async public System.Threading.Tasks.Task<List<City>> getCityListFromSvcAsync(string countryId, string stateId)
        {
            try
            {
                var connectionProfile = NetworkInformation.GetInternetConnectionProfile();
                if (connectionProfile == null || connectionProfile.GetNetworkConnectivityLevel() != NetworkConnectivityLevel.InternetAccess)
                    return null;

                var result = await client.getCityListAsync(countryId, stateId, _userInfo.CompanyId);
                List<City> cityList = new List<City>();
                if (result.response != null)
                {

                    result.response.AsParallel().ForAll(mzk =>
                    {
                        cityList.Add(new City { Name = mzk.parmCountyId, Id = mzk.parmStateId });
                    });
                }
                return cityList;
            }
            catch (Exception ex)
            {
                AppSettings.Instance.ErrorMessage = ex.Message;
                return null;
            }
        }


        async public System.Threading.Tasks.Task<List<Suburb>> getSuburbListFromSvcAsync(string countryId, string stateId)
        {
            try
            {
                var connectionProfile = NetworkInformation.GetInternetConnectionProfile();
                if (connectionProfile == null || connectionProfile.GetNetworkConnectivityLevel() != NetworkConnectivityLevel.InternetAccess)
                    return null;

                var result = await client.getSuburbListAsync(countryId, stateId, _userInfo.CompanyId);
                List<Suburb> suburbList = new List<Suburb>();
                if (result.response != null)
                {

                    result.response.AsParallel().ForAll(mzk =>
                    {
                        suburbList.Add(new Suburb { Name = mzk.parmCity, Id = mzk.parmStateId });
                    });
                }
                return suburbList;
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

                    result.response.AsParallel().ForAll(mzk =>
                    {
                        detailServiceScheduling = (new Eqstra.BusinessLogic.ServiceSchedule.ServiceSchedulingDetail
                        {
                            Address = mzk.parmAddress,
                            AdditionalWork = mzk.parmAdditionalWork,
                            ServiceDateOption1 = mzk.parmPreferredDateFirstOption,
                            ServiceDateOption2 = mzk.parmPreferredDateSecondOption,
                            ODOReading = mzk.parmODOReading.ToString(),
                            ODOReadingDate = mzk.parmODOReadingDate,
                            ServiceType = GetServiceTypesAsync(caseNumber, _userInfo.CompanyId),
                            LocationTypes = GetLocationTypeAsync(caseServiceRecId, _userInfo.CompanyId).Result,
                            SupplierName = mzk.parmSupplierName,
                            EventDesc = mzk.parmEventDesc,
                            ContactPersonName = mzk.parmContactPersonName,
                            ContactPersonPhone = mzk.parmContactPersonPhone
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
        private List<string> GetServiceTypesAsync(string caseNumber, string companyId)
        {
            var result = (client.getServiceTypesAsync(caseNumber, _userInfo.CompanyId).Result).response;
            List<string> results = new List<string>();
            if (result.IndexOf("~") > 1)
            {
                results.AddRange(result.Split('~'));
            }
            if (result.IndexOf(",") > 1)
            {
                results.AddRange(result.Split(','));
            }
            else
            {
                results.Add(result);
            }

            return results;
        }


        async public System.Threading.Tasks.Task<List<LocationType>> GetLocationTypeAsync(long caseServiceRecId, string companyId)
        {
            try
            {
                var result = await client.getLocationTypeAsync(caseServiceRecId, companyId);
                List<LocationType> locationTypes = new List<LocationType>();
                if (result.response != null)
                {
                    result.response.AsParallel().ForAll(mzk =>
                    {
                        locationTypes.Add(new Eqstra.BusinessLogic.ServiceSchedule.LocationType
                        {
                            LocationName = mzk.parmLocationName,
                            LocType = mzk.parmLocationType.ToString(),
                            RecID = mzk.parmRecID,
                        });
                    });
                }
                return locationTypes;
            }
            catch (Exception ex)
            {
                AppSettings.Instance.ErrorMessage = ex.Message;
                return null;
            }
        }

        async public System.Threading.Tasks.Task<List<DestinationType>> GetCustomersFromSvcAsync()
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
                var result = await client.getCustomersAsync(_userInfo.CompanyId);
                List<DestinationType> destinationTypes = new List<DestinationType>();
                if (result.response != null)
                {
                    result.response.AsParallel().ForAll(mzk =>
                    {
                        destinationTypes.Add(new Eqstra.BusinessLogic.ServiceSchedule.DestinationType
                       {
                           ContactName = mzk.parmName,
                           Id = mzk.parmAccountNum,
                           RecID = mzk.parmRecID,
                           Address = mzk.parmAddress
                       });
                    });
                }
                return destinationTypes;
            }
            catch (Exception ex)
            {
                AppSettings.Instance.ErrorMessage = ex.Message;
                return null;
            }
        }


        async public System.Threading.Tasks.Task<List<DestinationType>> GetVendorsFromSvcAsync()
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
                var result = await client.getVendorsAsync(_userInfo.CompanyId);
                List<DestinationType> destinationTypes = new List<DestinationType>();
                if (result.response != null)
                {
                    result.response.AsParallel().ForAll(mzk =>
                    {
                        destinationTypes.Add(new Eqstra.BusinessLogic.ServiceSchedule.DestinationType
                        {
                            ContactName = mzk.parmName,
                            Id = mzk.parmAccountNum,
                            RecID = mzk.parmRecID,
                            Address = mzk.parmAddress
                        });
                    });
                }
                return destinationTypes;
            }
            catch (Exception ex)
            {
                AppSettings.Instance.ErrorMessage = ex.Message;
                return null;
            }
        }

        async public System.Threading.Tasks.Task<List<DestinationType>> GetDriversFromSvcAsync()
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
                var result = await client.getDriversAsync(_userInfo.CompanyId);
                List<DestinationType> destinationTypes = new List<DestinationType>();
                if (result.response != null)
                {
                    result.response.AsParallel().ForAll(mzk =>
                    {
                        destinationTypes.Add(new Eqstra.BusinessLogic.ServiceSchedule.DestinationType
                        {
                            ContactName = mzk.parmName,
                            Id = mzk.parmDriverId,
                            RecID = mzk.parmRecID,
                            Address = mzk.parmAddress
                        });
                    });
                }
                return destinationTypes;
            }
            catch (Exception ex)
            {
                AppSettings.Instance.ErrorMessage = ex.Message;
                return null;
            }
        }

        async public System.Threading.Tasks.Task<List<Supplier>> GetVendSupplirerSvcAsync()
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
                var result = await client.getVendSupplirerNameAsync(_userInfo.CompanyId);
                List<Supplier> suppliers = new List<Supplier>();
                if (result.response != null)
                {
                    result.response.AsParallel().ForAll(mzk =>
                    {
                        suppliers.Add(new Eqstra.BusinessLogic.ServiceSchedule.Supplier
                        {
                            SupplierContactName = mzk.parmContactPersonName,
                            SupplierContactNumber = mzk.parmContactPersonPhone,
                            SupplierName = mzk.parmName,
                            Country = mzk.parmCountry,
                            Province = mzk.parmState,
                            City = mzk.parmCityName,
                            Suburb = mzk.parmSuburban,

                        });
                    });
                }
                return suppliers;
            }
            catch (Exception ex)
            {
                AppSettings.Instance.ErrorMessage = ex.Message;
                return null;
            }
        }

        async public System.Threading.Tasks.Task InsertConfirmationServiceSchedulingToSvcAsync(ServiceSchedulingDetail serviceSchedulingDetail, SupplierSelection supplierSelection, string caseNumber, long caseServiceRecId, long _entityRecId)
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
                var result = client.insertServiceDetailsAsync(caseNumber, caseServiceRecId, _entityRecId, new MzkServiceDetailsContract
                {
                    parmAdditionalWork = serviceSchedulingDetail.AdditionalWork,
                    parmAddress = serviceSchedulingDetail.Address,
                    parmContactPersonName = serviceSchedulingDetail.ContactPersonName,
                    parmEventDesc = serviceSchedulingDetail.EventDesc,
                    parmLocationType = serviceSchedulingDetail.SelectedLocationType.LocType,
                    parmODOReading = serviceSchedulingDetail.ODOReading,
                    parmODOReadingDate = serviceSchedulingDetail.ODOReadingDate,
                    parmPreferredDateFirstOption = serviceSchedulingDetail.ServiceDateOption1,
                    parmPreferredDateSecondOption = serviceSchedulingDetail.ServiceDateOption2,
                    parmServiceType = serviceSchedulingDetail.SelectedServiceType,
                    parmSupplierName = supplierSelection.SelectedSupplier.SupplierName,
                    parmContactPersonPhone = supplierSelection.SelectedSupplier.SupplierContactNumber,
                }, _userInfo.CompanyId);

            }
            catch (Exception ex)
            {
                AppSettings.Instance.ErrorMessage = ex.Message;
            }
        }
    }
}