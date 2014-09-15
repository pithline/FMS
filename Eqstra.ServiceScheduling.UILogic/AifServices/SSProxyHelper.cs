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
using System.Collections.ObjectModel;
using System.Collections;
using System.Reflection;
using System.ServiceModel;
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
                basicHttpBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Windows;//http://srfmlaxdev01.lfmd.co.za/MicrosoftDynamicsAXAif60/ServiceSchedulingService/xppservice.svc?wsdl
                client = new Eqstra.ServiceScheduling.UILogic.SSProxy.MzkServiceSchedulingServiceClient(basicHttpBinding, new EndpointAddress("http://srfmlbispstg01.lfmd.co.za/MicrosoftDynamicsAXAif60/ServiceSchedulingService/xppservice.svc"));
                client.ClientCredentials.UserName.UserName = domain + "\"" + userName;
                client.ClientCredentials.UserName.Password = password;
                client.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Impersonation;

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
                            CusEmailId = mzkTask.parmEmail,
                            ScheduledDate = DateTime.Today, //Need to add in Service
                            ScheduledTime = DateTime.Today, // Need to add in Service
                            ServiceRecID = mzkTask.parmServiceRecID,
                           
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
        async public System.Threading.Tasks.Task<List<Province>> GetProvinceListFromSvcAsync(string countryId)
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
                List<Province> provinceList = new List<Province>();
                if (result.response != null)
                {

                    result.response.AsParallel().ForAll(mzk =>
                    {
                        provinceList.Add(new Province { Name = mzk.parmStateName, Id = mzk.parmStateId });
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
        async public System.Threading.Tasks.Task<List<City>> GetCityListFromSvcAsync(string countryId, string stateId)
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
        async public System.Threading.Tasks.Task<List<Suburb>> GetSuburbListFromSvcAsync(string countryId, string stateId)
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
        async public System.Threading.Tasks.Task<List<Region>> GetRegionListFromSvcAsync(string countryId, string stateId)
        {
            try
            {
                var connectionProfile = NetworkInformation.GetInternetConnectionProfile();
                if (connectionProfile == null || connectionProfile.GetNetworkConnectivityLevel() != NetworkConnectivityLevel.InternetAccess)
                    return null;

                var result = await client.getRegionsAsync(countryId, stateId, _userInfo.CompanyId);
                List<Region> regionList = new List<Region>();
                if (result.response != null)
                {
                    result.response.AsParallel().ForAll(mzk =>
                    {
                        regionList.Add(new Region { Name = mzk.parmRegionName, Id = mzk.parmRegion });
                    });
                }
                return regionList;
            }
            catch (Exception ex)
            {
                AppSettings.Instance.ErrorMessage = ex.Message;
                return null;
            }
        }
        async public System.Threading.Tasks.Task<List<string>> GetZipcodeListFromSvcAsync(string countryId, string stateId)
        {
            try
            {
                var connectionProfile = NetworkInformation.GetInternetConnectionProfile();
                if (connectionProfile == null || connectionProfile.GetNetworkConnectivityLevel() != NetworkConnectivityLevel.InternetAccess)
                    return null;

                var result = await client.getZipcodeListAsync(countryId, stateId, _userInfo.CompanyId);
                List<string> zipcodeList = new List<string>();
                if (result.response != null)
                {
                    result.response.AsParallel().ForAll(mzk =>
                    {
                        zipcodeList.Add(mzk.parmZipCode);
                    });
                }
                return zipcodeList;
            }
            catch (Exception ex)
            {
                AppSettings.Instance.ErrorMessage = ex.Message;
                return null;
            }
        }
        async public System.Threading.Tasks.Task<ServiceSchedulingDetail> GetServiceDetailsFromSvcAsync(string caseNumber, long caseServiceRecId, long serviceRecId)
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
                            ServiceDateOption1 = mzk.parmPreferredDateFirstOption < DateTime.Today ? DateTime.Today : mzk.parmPreferredDateFirstOption,
                            ServiceDateOption2 = mzk.parmPreferredDateSecondOption < DateTime.Today ? DateTime.Today : mzk.parmPreferredDateSecondOption,
                            ODOReading = mzk.parmODOReading.ToString(),
                            ODOReadingDate = mzk.parmODOReadingDate < DateTime.Today ? DateTime.Today : mzk.parmODOReadingDate,
                            ServiceType = GetServiceTypesAsync(caseNumber, _userInfo.CompanyId),
                            LocationTypes = GetLocationTypeAsync(serviceRecId, _userInfo.CompanyId).Result,
                            SupplierName = mzk.parmSupplierName,
                            EventDesc = mzk.parmEventDesc,
                            ContactPersonName = mzk.parmContactPersonName,
                            ContactPersonPhone = mzk.parmContactPersonPhone,
                            SupplierDateTime = DateTime.Now,// need to add in service
                            SelectedLocRecId = mzk.parmLiftLocationRecId,
                            SelectedLocType = mzk.parmLocationType,
                            SelectedServiceType = mzk.parmServiceType
                           
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
                destinationTypes = destinationTypes.OrderBy(o => o.ContactName).ToList<DestinationType>();
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
                var result = await client.getVendSupplirerNameAsync(_userInfo.CompanyId);
                List<DestinationType> destinationTypes = new List<DestinationType>();
                if (result.response != null)
                {
                    result.response.AsParallel().ForAll(mzk =>
                    {
                        destinationTypes.Add(new Eqstra.BusinessLogic.ServiceSchedule.DestinationType
                        {
                            ContactName = mzk.parmName,
                            Id = mzk.parmAccountNum,
                            Address = mzk.parmAddress,
                            RecID = mzk.parmRecID
                        });
                    });
                }
                destinationTypes = destinationTypes.OrderBy(o => o.ContactName).ToList<DestinationType>();
                return destinationTypes;
            }
            catch (Exception ex)
            {
                AppSettings.Instance.ErrorMessage = ex.Message;
                return null;
            }
        }
        async public System.Threading.Tasks.Task<IEnumerable<DestinationType>> GetDriversFromSvcAsync()
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
                return destinationTypes.OrderBy(o => o.ContactName);
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
                            AccountNum = mzk.parmAccountNum,
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
        async public System.Threading.Tasks.Task<bool> UpdateConfirmationDatesToSvcAsync(long caseServiceRecId, ServiceSchedulingDetail serviceSchedulingDetail)
        {
            try
            {
                var connectionProfile = NetworkInformation.GetInternetConnectionProfile();
                if (connectionProfile == null || connectionProfile.GetNetworkConnectivityLevel() != NetworkConnectivityLevel.InternetAccess)
                    return false;

                if (_userInfo == null)
                {
                    _userInfo = JsonConvert.DeserializeObject<UserInfo>(ApplicationData.Current.RoamingSettings.Values[Constants.UserInfo].ToString());
                }
                var result = await client.updateConfirmationDatesAsync(caseServiceRecId, new MzkServiceDetailsContract
                {
                    parmPreferredDateFirstOption = serviceSchedulingDetail.ServiceDateOption1,
                    parmPreferredDateSecondOption = serviceSchedulingDetail.ServiceDateOption2
                }, _userInfo.CompanyId);
                return result.response;
            }
            catch (Exception ex)
            {
                AppSettings.Instance.ErrorMessage = ex.Message;
                return false;
            }

        }
        async public System.Threading.Tasks.Task<bool> InsertServiceDetailsToSvcAsync(ServiceSchedulingDetail serviceSchedulingDetail, Address address, string caseNumber, long caseServiceRecId, long _entityRecId)
        {
            try
            {
                var connectionProfile = NetworkInformation.GetInternetConnectionProfile();
                if (connectionProfile == null || connectionProfile.GetNetworkConnectivityLevel() != NetworkConnectivityLevel.InternetAccess)
                    return false;

                if (_userInfo == null)
                {
                    _userInfo = JsonConvert.DeserializeObject<UserInfo>(ApplicationData.Current.RoamingSettings.Values[Constants.UserInfo].ToString());
                }

                var mzkAddressContract = new MzkAddressContract
                {
                    parmCity = address.SelectedCity != null ? address.SelectedCity.Id : string.Empty,
                    parmCountryRegionId = address.SelectedCountry != null ? address.SelectedCountry.Id : string.Empty,
                    parmProvince = address.Selectedprovince != null ? address.Selectedprovince.Id : string.Empty,
                    parmStreet = address.Street,
                    parmSubUrb = address.SelectedSuburb != null ? address.SelectedSuburb.Id : string.Empty,
                    parmZipCode = address.SelectedZip

                };
                var mzkServiceDetailsContract = new MzkServiceDetailsContract
                   {
                       parmAdditionalWork = serviceSchedulingDetail.AdditionalWork,
                       parmAddress = serviceSchedulingDetail.Address,
                       parmEventDesc = serviceSchedulingDetail.EventDesc,
                       parmLiftLocationRecId = serviceSchedulingDetail.SelectedLocationType.RecID,
                       parmODOReading = serviceSchedulingDetail.ODOReading,
                       parmODOReadingDate = serviceSchedulingDetail.ODOReadingDate,
                       parmPreferredDateFirstOption = serviceSchedulingDetail.ServiceDateOption1,
                       parmPreferredDateSecondOption = serviceSchedulingDetail.ServiceDateOption2,
                       parmServiceType = serviceSchedulingDetail.SelectedServiceType,
                       parmSupplierId = serviceSchedulingDetail.SelectedDestinationType.Id

                   };


                var result = await client.insertServiceDetailsAsync(caseNumber, caseServiceRecId, _entityRecId, mzkServiceDetailsContract
                      , mzkAddressContract, _userInfo.CompanyId);

                return result.response;
            }

            catch (Exception ex)
            {
                AppSettings.Instance.ErrorMessage = ex.Message;
                return false;
            }
        }
        async public System.Threading.Tasks.Task<bool> InsertSelectedSupplierToSvcAsync(SupplierSelection supplierSelection, string caseNumber, long caseServiceRecId)
        {
            try
            {
                var connectionProfile = NetworkInformation.GetInternetConnectionProfile();
                if (connectionProfile == null || connectionProfile.GetNetworkConnectivityLevel() != NetworkConnectivityLevel.InternetAccess)
                    return false;

                if (_userInfo == null)
                {
                    _userInfo = JsonConvert.DeserializeObject<UserInfo>(ApplicationData.Current.RoamingSettings.Values[Constants.UserInfo].ToString());
                }
                var result = await client.insertVendDetAsync(caseNumber, caseServiceRecId, default(long), new MzkServiceDetailsContract
                {
                    parmContactPersonName = supplierSelection.SelectedSupplier.SupplierContactName,
                    parmSupplierName = supplierSelection.SelectedSupplier.SupplierName,
                    parmContactPersonPhone = supplierSelection.SelectedSupplier.SupplierContactNumber,
                    parmSupplierId = supplierSelection.SelectedSupplier.AccountNum
                }, new MzkAddressContract(), _userInfo.CompanyId);


                return result != null;
            }


            catch (Exception ex)
            {
                AppSettings.Instance.ErrorMessage = ex.Message;
                return false;
            }
        }
        async public System.Threading.Tasks.Task<bool> InsertConfirmedServiceDetailToSvcAsync(ServiceSchedulingDetail serviceSchedulingDetail, string caseNumber, long caseServiceRecId)
        {
            try
            {
                var connectionProfile = NetworkInformation.GetInternetConnectionProfile();
                if (connectionProfile == null || connectionProfile.GetNetworkConnectivityLevel() != NetworkConnectivityLevel.InternetAccess)
                    return false;

                if (_userInfo == null)
                {
                    _userInfo = JsonConvert.DeserializeObject<UserInfo>(ApplicationData.Current.RoamingSettings.Values[Constants.UserInfo].ToString());
                }
                var result = await client.insertServiceDetailsAsync(caseNumber, caseServiceRecId, default(long), new MzkServiceDetailsContract
                {
                    parmAdditionalWork = serviceSchedulingDetail.AdditionalWork,
                    parmAddress = serviceSchedulingDetail.Address,
                    parmEventDesc = serviceSchedulingDetail.EventDesc,
                    parmLiftLocationRecId = serviceSchedulingDetail.SelectedLocRecId,

                    parmLocationType = serviceSchedulingDetail.SelectedLocType,
                    parmODOReading = serviceSchedulingDetail.ODOReading,
                    parmODOReadingDate = serviceSchedulingDetail.ODOReadingDate,
                    parmPreferredDateFirstOption = serviceSchedulingDetail.ServiceDateOption1,
                    parmPreferredDateSecondOption = serviceSchedulingDetail.ServiceDateOption2,
                    parmServiceType = serviceSchedulingDetail.SelectedServiceType,
                    parmSupplierName = serviceSchedulingDetail.SupplierName,
                    parmContactPersonName = serviceSchedulingDetail.ContactPersonName,
                    parmContactPersonPhone = serviceSchedulingDetail.ContactPersonPhone,

                }, new MzkAddressContract(), _userInfo.CompanyId);

                if (result.response)
                {
                    Util.ShowToast("Thank you very much. Your request has been sent to your selected  supplier, you will receive confirmation via the Car Manager application shortly.");

                }
                return result.response;
            }


            catch (Exception ex)
            {
                AppSettings.Instance.ErrorMessage = ex.Message;
                return false;
            }
        }
        async public System.Threading.Tasks.Task<string> UpdateStatusListToSvcAsync(DriverTask task)
        {
            try
            {
                var connectionProfile = NetworkInformation.GetInternetConnectionProfile();
                if (connectionProfile == null || connectionProfile.GetNetworkConnectivityLevel() != NetworkConnectivityLevel.InternetAccess)
                    return task.Status;

                if (_userInfo == null)
                {
                    _userInfo = JsonConvert.DeserializeObject<UserInfo>(ApplicationData.Current.RoamingSettings.Values[Constants.UserInfo].ToString());
                }

                ObservableCollection<MzkServiceSchdTasksContract> mzkTasks = new ObservableCollection<MzkServiceSchdTasksContract>();

                Dictionary<string, EEPActionStep> actionStepMapping = new Dictionary<string, EEPActionStep>();

                actionStepMapping.Add(Eqstra.BusinessLogic.Helpers.DriverTaskStatus.AwaitServiceDetail, EEPActionStep.AwaitServiceBookingDetail);
                actionStepMapping.Add(Eqstra.BusinessLogic.Helpers.DriverTaskStatus.AwaitSupplierSelection, EEPActionStep.AwaitSupplierSelection);
                //  actionStepMapping.Add(Eqstra.BusinessLogic.Helpers.DriverTaskStatus.AwaitJobCardCapture, EEPActionStep.AwaitServiceConfirmation);
                mzkTasks.Add(new MzkServiceSchdTasksContract
                {
                    parmCaseID = task.CaseNumber,
                    parmCaseCategory = task.CaseCategory,
                    parmCaseServiceRecID = task.CaseServiceRecID,
                    parmStatus = task.Status,
                    parmServiceRecID = task.ServiceRecID,
                    parmStatusDueDate = task.StatusDueDate,
                    parmEEPActionStep = actionStepMapping[task.Status]
                });
                var result = await client.updateStatusListAsync(mzkTasks, _userInfo.CompanyId);

                return result.response.FirstOrDefault().parmStatus;
            }

            catch (Exception ex)
            {
                AppSettings.Instance.ErrorMessage = ex.Message;
                return task.Status;
            }
        }
    }
}