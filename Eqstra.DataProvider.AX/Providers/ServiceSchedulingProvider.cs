using Eqstra.DataProvider.AX.Helpers;
using Eqstra.DataProvider.AX.SSModels;
using Eqstra.DataProvider.AX.SSProxy;
using Eqstra.Framework.Web.DataAccess;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Text.RegularExpressions;

namespace Eqstra.DataProvider.AX.Providers
{
    [DataProvider(Name = "ServiceScheduling")]
    public class ServiceSchedulingProvider : IDataProvider
    {
        SSProxy.MzkServiceSchedulingServiceClient _client;

        public System.Collections.IList GetDataList(object[] criterias)
        {
            try
            {
                GetService();
                System.Collections.IList response = null;
                switch (criterias[0].ToString())
                {
                    case ActionSwitch.GetTasks:
                        response = GetTasks(JsonConvert.DeserializeObject<UserInfo>(criterias[1].ToString()));
                        break;

                    case ActionSwitch.GetCountryList:
                        response = GetCountryList(JsonConvert.DeserializeObject<UserInfo>(criterias[1].ToString()));
                        break;

                    case ActionSwitch.GetProvinceList:
                        response = GetProvinceList(criterias[1].ToString(), JsonConvert.DeserializeObject<UserInfo>(criterias[2].ToString()));
                        break;

                    case ActionSwitch.GetCityList:
                        response = GetCityList(criterias[1].ToString(), criterias[2].ToString(), JsonConvert.DeserializeObject<UserInfo>(criterias[3].ToString()));
                        break;

                    case ActionSwitch.GetSuburbList:
                        response = GetSuburbList(criterias[1].ToString(), criterias[2].ToString(), JsonConvert.DeserializeObject<UserInfo>(criterias[3].ToString()));
                        break;

                    case ActionSwitch.GetRegionList:
                        response = GetRegionList(criterias[1].ToString(), criterias[2].ToString(), JsonConvert.DeserializeObject<UserInfo>(criterias[3].ToString()));
                        break;

                    case ActionSwitch.GetZipcodeList:
                        response = GetZipcodeList(criterias[1].ToString(), criterias[2].ToString(), JsonConvert.DeserializeObject<UserInfo>(criterias[3].ToString()));
                        break;

                    case ActionSwitch.GetDestinationTypeList:
                        response = GetDestinationTypeList(criterias[1].ToString(), criterias[2].ToString(), JsonConvert.DeserializeObject<UserInfo>(criterias[3].ToString()));
                        break;

                    case ActionSwitch.FilterSuppliersByCriteria:
                        response = FilterSuppliersByCriteria(criterias[1].ToString(), criterias[2].ToString(), criterias[3].ToString(), criterias[4].ToString(), criterias[5].ToString(), JsonConvert.DeserializeObject<UserInfo>(criterias[6].ToString()));
                        break;
                    case ActionSwitch.FilterSuppliersByGeoLocation:
                        response = FilterSuppliersByGeoLocation(criterias[1].ToString(), criterias[2].ToString(), JsonConvert.DeserializeObject<UserInfo>(criterias[3].ToString()));
                        break;

                }
                _client.Close();
                return response;
            }
            catch (Exception)
            {
                _client.Close();
                throw;
            }
        }

        public object GetSingleData(object[] criterias)
        {
            try
            {
                object response = null;
                GetService();
                switch (criterias[0].ToString())
                {
                    case ActionSwitch.ValidateUser:
                        response = ValidateUser(criterias[2].ToString(), criterias[3].ToString());
                        break;

                    case ActionSwitch.GetServiceDetails:
                        response = GetServiceDetails(criterias[1].ToString(), long.Parse(criterias[2].ToString()), JsonConvert.DeserializeObject<UserInfo>(criterias[3].ToString()));
                        break;

                    case ActionSwitch.GetUserInfo:
                        response = GetUserInfo(criterias[1].ToString());
                        break;
                }
                _client.Close();
                return response;
            }
            catch (Exception)
            {
                _client.Close();
                throw;
            }
        }

        public bool DeleteData(object[] criterias)
        {
            throw new NotImplementedException();
        }

        public object SaveData(object[] criterias)
        {
            try
            {
                object response = null;
                GetService();
                switch (criterias[0].ToString())
                {
                    case ActionSwitch.InsertServiceDetails:
                        response = InsertServiceDetails(JsonConvert.DeserializeObject<ServiceSchedulingDetail>(criterias[1].ToString()), JsonConvert.DeserializeObject<Address>(criterias[2].ToString()), JsonConvert.DeserializeObject<UserInfo>(criterias[3].ToString()));
                        break;

                    case ActionSwitch.InsertSelectedSupplier:
                        response = InsertSelectedSupplier(JsonConvert.DeserializeObject<SupplierSelection>(criterias[1].ToString()), JsonConvert.DeserializeObject<UserInfo>(criterias[2].ToString()));
                        break;

                    case ActionSwitch.InsertConfirmedServiceDetail:
                        response = InsertConfirmedServiceDetail(JsonConvert.DeserializeObject<ServiceSchedulingDetail>(criterias[1].ToString()), JsonConvert.DeserializeObject<UserInfo>(criterias[2].ToString()));
                        break;

                    case ActionSwitch.UpdateConfirmationDates:
                        response = UpdateConfirmationDates(long.Parse(criterias[1].ToString()), JsonConvert.DeserializeObject<ServiceSchedulingDetail>(criterias[2].ToString()), JsonConvert.DeserializeObject<UserInfo>(criterias[3].ToString()));
                        break;

                    case ActionSwitch.UpdateStatusList:
                        response = UpdateStatusList(JsonConvert.DeserializeObject<Eqstra.DataProvider.AX.SSModels.Task>(criterias[1].ToString()), JsonConvert.DeserializeObject<UserInfo>(criterias[2].ToString()));
                        break;
                }
                _client.Close();
                return response;
            }
            catch (Exception)
            {
                _client.Close();
                throw;
            }
        }

        public object GetService()
        {
            return GetServiceClient();
        }

        private SSProxy.MzkServiceSchedulingServiceClient GetServiceClient()
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
                basicHttpBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Windows;
                _client = new SSProxy.MzkServiceSchedulingServiceClient(basicHttpBinding, new EndpointAddress("http://srfmlbispstg01.lfmd.co.za/MicrosoftDynamicsAXAif60/SSService/xppservice.svc?wsdl"));
                _client.ClientCredentials.UserName.UserName = "lfmd" + "\"" + "rchivukula";
                _client.ClientCredentials.UserName.Password = "Password8";
                _client.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Impersonation;
                _client.ClientCredentials.Windows.ClientCredential = new NetworkCredential("rchivukula", "Password8", "lfmd");
            }
            catch (Exception)
            {
                throw;
            }

            return _client;
        }

        private bool ValidateUser(string userId, string password)
        {
            try
            {
                return !_client.validateUser(new SSProxy.CallContext() { }, userId, password);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private UserInfo GetUserInfo(string userId)
        {
            try
            {
                var result = _client.getUserDetails(new SSProxy.CallContext() { }, userId);
                if (result != null)
                {
                    return new UserInfo
                    {
                        UserId = result.parmUserID,
                        CompanyId = result.parmCompany,
                        CompanyName = result.parmCompanyName,
                        Name = result.parmUserName
                    };
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private List<Eqstra.DataProvider.AX.SSModels.Task> GetTasks(UserInfo userInfo)
        {
            try
            {

                var result = _client.getTasks(new SSProxy.CallContext() { }, userInfo.UserId, userInfo.CompanyId);
                List<Eqstra.DataProvider.AX.SSModels.Task> driverTaskList = new List<Eqstra.DataProvider.AX.SSModels.Task>();
                if (result != null)
                {

                    foreach (var mzkTask in result.Reverse())
                    {
                        var startTime = new DateTime(mzkTask.parmConfirmationDate.Year, mzkTask.parmConfirmationDate.Month, mzkTask.parmConfirmationDate.Day);

                        driverTaskList.Add(new Eqstra.DataProvider.AX.SSModels.Task
                        {
                            CaseNumber = mzkTask.parmCaseID,
                            Address = Regex.Replace(mzkTask.parmContactPersonAddress, "\n", ","),
                            CustomerName = mzkTask.parmCustName,
                            CustPhone = mzkTask.parmCustPhone,
                            Status = mzkTask.parmStatus,
                            StatusDueDate = mzkTask.parmStatusDueDate.ToShortDateString(),
                            RegistrationNumber = mzkTask.parmRegistrationNum,
                             AllocatedTo = userInfo.Name,
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
                            ServiceRecID = mzkTask.parmServiceRecID,
                            CustomerId = mzkTask.parmCustAccount,
                            ConfirmedDate = mzkTask.parmConfirmationDate.ToShortDateString(),
                            ContactName = mzkTask.parmContactPersonName,
                            AppointmentStart = mzkTask.parmStatus == Eqstra.DataProvider.AX.Helpers.TaskStatus.Completed ? startTime : DateTime.MinValue,
                            AppointmentEnd = mzkTask.parmStatus == Eqstra.DataProvider.AX.Helpers.TaskStatus.Completed ? startTime.AddHours(24) : DateTime.MinValue

                        });


                    }

                }

                return driverTaskList;

            }
            catch (Exception)
            {
                throw;
            }
        }

        private List<Country> GetCountryList(UserInfo userInfo)
        {
            try
            {

                var result = _client.getCountryRegionList(new SSProxy.CallContext() { }, userInfo.CompanyId);
                List<Country> countryList = new List<Country>();
                if (result != null)
                {

                    foreach (var mzk in result.OrderBy(o => o.parmCountryRegionName))
                    {
                        countryList.Add(
                                           new Country
                                           {
                                               Name = mzk.parmCountryRegionName,
                                               Id = mzk.parmCountryRegionId
                                           }
                                           );
                    };
                }

                return countryList;

            }
            catch (Exception)
            {
                throw;
            }
        }
        private List<Province> GetProvinceList(string countryId, UserInfo userInfo)
        {
            try
            {

                var result = _client.getProvinceList(new SSProxy.CallContext() { }, countryId, userInfo.CompanyId);
                List<Province> provinceList = new List<Province>();
                if (result != null)
                {
                    foreach (var mzk in result)
                    {
                        provinceList.Add(new Province { Name = mzk.parmStateName, Id = mzk.parmStateId });
                    }
                }
                return provinceList;

            }
            catch (Exception)
            {
                throw;
            }
        }
        private List<City> GetCityList(string countryId, string stateId, UserInfo userInfo)
        {
            try
            {

                var result = _client.getCityList(new SSProxy.CallContext() { }, countryId, stateId, userInfo.CompanyId);
                List<City> cityList = new List<City>();
                if (result != null)
                {
                    foreach (var mzk in result)
                    {
                        cityList.Add(new City { Name = mzk.parmCountyId, Id = mzk.parmStateId });
                    }
                }
                return cityList;

            }
            catch (Exception)
            {
                throw;
            }
        }
        private List<Suburb> GetSuburbList(string countryId, string stateId, UserInfo userInfo)
        {
            try
            {

                var result = _client.getSuburbList(new SSProxy.CallContext() { }, countryId, stateId, userInfo.CompanyId);
                List<Suburb> suburbList = new List<Suburb>();
                if (result != null)
                {
                    foreach (var mzk in result)
                    {
                        suburbList.Add(new Suburb { Name = mzk.parmCity, Id = mzk.parmStateId });
                    }
                }
                return suburbList;

            }
            catch (Exception)
            {
                throw;
            }
        }
        private List<Region> GetRegionList(string countryId, string stateId, UserInfo userInfo)
        {
            try
            {

                var result = _client.getRegions(new SSProxy.CallContext() { }, countryId, stateId, userInfo.CompanyId);
                List<Region> regionList = new List<Region>();
                if (result != null)
                {
                    foreach (var mzk in result)
                    {
                        regionList.Add(new Region { Name = mzk.parmRegionName, Id = mzk.parmRegion });
                    }
                }
                return regionList;

            }
            catch (Exception)
            {
                throw;
            }
        }
        private List<string> GetZipcodeList(string countryId, string stateId, UserInfo userInfo)
        {
            try
            {

                var result = _client.getZipcodeList(new SSProxy.CallContext() { }, countryId, stateId, userInfo.CompanyId);
                List<string> zipcodeList = new List<string>();
                if (result != null)
                {
                    foreach (var mzk in result)
                    {
                        zipcodeList.Add(mzk.parmZipCode);
                    }
                }
                return zipcodeList;

            }
            catch (Exception)
            {
                throw;
            }
        }
        private ServiceSchedulingDetail GetServiceDetails(string caseNumber, long serviceRecId, UserInfo userInfo)
        {
            try
            {

                var result = _client.getServiceDetails(new SSProxy.CallContext() { }, caseNumber, serviceRecId, userInfo.CompanyId);
                ServiceSchedulingDetail detailServiceScheduling = null;
                if (result != null)
                {

                    foreach (var mzk in result)
                    {
                        detailServiceScheduling = (new ServiceSchedulingDetail
                        {
                            Address = mzk.parmAddress,
                            AdditionalWork = mzk.parmAdditionalWork,
                            ServiceDateOption1 = (mzk.parmPreferredDateFirstOption < DateTime.Today ? DateTime.Today : mzk.parmPreferredDateFirstOption).ToShortDateString(),
                            ServiceDateOption2 = (mzk.parmPreferredDateSecondOption < DateTime.Today ? DateTime.Today : mzk.parmPreferredDateSecondOption).ToShortDateString(),
                            ODOReading = Int64.Parse(mzk.parmODOReading.ToString()),
                            ODOReadingDate = mzk.parmODOReadingDate == DateTime.MinValue ? string.Empty : mzk.parmODOReadingDate.ToShortDateString(),
                            ServiceType = GetServiceTypes(caseNumber, userInfo.CompanyId),
                            LocationTypes = GetLocationType(serviceRecId, userInfo.CompanyId),
                            SupplierName = mzk.parmSupplierName,
                            EventDesc = mzk.parmEventDesc,
                            ContactPersonName = mzk.parmContactPersonName,
                            ContactPersonPhone = mzk.parmContactPersonPhone,
                            SupplierDateTime = DateTime.Now,// need to add in service
                            CaseNumber = caseNumber,
                            SelectedLocationType = mzk.parmLocationType,
                            SelectedServiceType = mzk.parmServiceType,
                            IsLiftRequired = mzk.parmLiftRequired == NoYes.Yes ? true : false
                        });
                    }

                }

                return detailServiceScheduling;

            }
            catch (Exception)
            {
                throw;
            }
        }
        private List<string> GetServiceTypes(string caseNumber, string companyId)
        {

            var result = _client.getServiceTypes(new SSProxy.CallContext() { }, caseNumber, companyId);
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
        private List<LocationType> GetLocationType(long serviceRecId, string companyId)
        {
            try
            {
                var result = _client.getLocationType(new SSProxy.CallContext() { }, serviceRecId, companyId);
                List<LocationType> locationTypes = new List<LocationType>();
                if (result != null)
                {
                    foreach (var mzk in result)
                    {
                        locationTypes.Add(new LocationType
                        {
                            LocationName = mzk.parmLocationName,
                            LocType = mzk.parmLocationType.ToString(),
                            RecID = mzk.parmRecID,
                        });
                    }
                }
                return locationTypes;

            }
            catch (Exception)
            {
                throw;
            }
        }


        public List<DestinationType> GetDestinationTypeList(string callerKey, string cusId, UserInfo userInfo)
        {
            try
            {
                List<DestinationType> destinationTypes = new List<DestinationType>();
                if (callerKey.Contains("Customer"))
                {
                    var result = _client.getCustomers(new SSProxy.CallContext() { }, cusId, userInfo.CompanyId);

                    if (result != null)
                    {
                        foreach (var mzk in result)
                        {
                            destinationTypes.Add(new DestinationType
                            {
                                ContactName = mzk.parmName,
                                Id = mzk.parmAccountNum,
                                RecID = mzk.parmRecID,
                                Address = mzk.parmAddress
                            });
                        }
                    }
                    destinationTypes = destinationTypes.OrderBy(o => o.ContactName).ToList<DestinationType>();

                }


                if (callerKey.Contains("Supplier") || callerKey.Contains("Vendor"))
                {
                    var result = _client.getVendSupplirerName(new SSProxy.CallContext() { }, userInfo.CompanyId);

                    if (result != null)
                    {
                        foreach (var mzk in result)
                        {
                            destinationTypes.Add(new DestinationType
                            {
                                ContactName = mzk.parmName,
                                Id = mzk.parmAccountNum,
                                Address = mzk.parmAddress,
                                RecID = mzk.parmRecID

                            });
                        }
                    }
                    destinationTypes = destinationTypes.OrderBy(o => o.ContactName).ToList<DestinationType>();
                }


                if (callerKey.Contains("Driver"))
                {
                    var result = _client.getDrivers(new SSProxy.CallContext() { }, cusId, userInfo.CompanyId);

                    if (result != null)
                    {
                        foreach (var mzk in result)
                        {
                            destinationTypes.Add(new DestinationType
                            {
                                ContactName = mzk.parmName,
                                Id = mzk.parmDriverId,
                                RecID = mzk.parmRecID,
                                Address = mzk.parmAddress
                            });
                        }
                    }

                }

                return destinationTypes.OrderBy(o => o.ContactName).ToList<DestinationType>();

            }
            catch (Exception)
            {
                throw;
            }

        }

        private bool UpdateConfirmationDates(long caseServiceRecId, ServiceSchedulingDetail serviceSchedulingDetail, UserInfo userInfo)
        {
            try
            {

                var result = _client.updateConfirmationDates(new SSProxy.CallContext() { }, caseServiceRecId, new MzkServiceDetailsContract
                {
                    parmPreferredDateFirstOption = DateTime.Parse(serviceSchedulingDetail.ServiceDateOption1),
                    parmPreferredDateSecondOption = DateTime.Parse(serviceSchedulingDetail.ServiceDateOption2)
                }, userInfo.CompanyId);
                return result;

            }
            catch (Exception)
            {
                throw;
            }

        }
        private bool InsertServiceDetails(ServiceSchedulingDetail serviceSchedulingDetail, Address address, UserInfo userInfo)
        {
            try
            {

                var mzkAddressContract = new MzkAddressContract
                {
                    parmCity = address.SelectedCity != null ? address.SelectedCity : string.Empty,
                    parmCountryRegionId = address.SelectedCountry != null ? address.SelectedCountry : string.Empty,
                    parmProvince = address.Selectedprovince != null ? address.Selectedprovince : string.Empty,
                    parmStreet = address.Street,
                    parmSubUrb = address.SelectedSuburb != null ? address.SelectedSuburb : string.Empty,
                    parmZipCode = address.SelectedZip

                };
                var mzkServiceDetailsContract = new MzkServiceDetailsContract
                {

                    parmAdditionalWork = serviceSchedulingDetail.AdditionalWork,
                    parmAddress = serviceSchedulingDetail.Address,
                    parmEventDesc = serviceSchedulingDetail.EventDesc,

                    parmODOReading = serviceSchedulingDetail.ODOReading.ToString(),
                    parmODOReadingDate = DateTime.Parse(serviceSchedulingDetail.ODOReadingDate),
                    parmPreferredDateFirstOption = DateTime.Parse(serviceSchedulingDetail.ServiceDateOption1),
                    parmPreferredDateSecondOption = DateTime.Parse(serviceSchedulingDetail.ServiceDateOption2),
                    parmServiceType = serviceSchedulingDetail.SelectedServiceType,
                    parmLiftLocationRecId = serviceSchedulingDetail.SelectedLocType.RecID,
                    parmSupplierId = serviceSchedulingDetail.SelectedDestinationType.Id,
                    parmLocationType = serviceSchedulingDetail.SelectedLocType.LocationName,
                    parmLiftRequired = serviceSchedulingDetail.IsLiftRequired == true ? NoYes.Yes : NoYes.No
                };


                var result = _client.insertServiceDetails(new SSProxy.CallContext() { }, serviceSchedulingDetail.CaseNumber, serviceSchedulingDetail.CaseServiceRecID, Convert.ToInt64(serviceSchedulingDetail.SelectedDestinationType.RecID), mzkServiceDetailsContract
                      , mzkAddressContract, userInfo.CompanyId);

                return true;

            }

            catch (Exception)
            {
                throw;
            }
        }



        public List<Supplier> FilterSuppliersByGeoLocation(string countryName, string cityName, UserInfo userInfo)
        {
            try
            {
                var result = _client.getVendorByName(new SSProxy.CallContext() { }, countryName, cityName, userInfo.CompanyId);

                List<Supplier> suppliers = new List<Supplier>();

                if (result != null)
                {
                    foreach (var mzk in result)
                    {
                        suppliers.Add(new Supplier
                        {
                            AccountNum = mzk.parmAccountNum,
                            SupplierContactName = mzk.parmContactPersonName,
                            SupplierContactNumber = mzk.parmContactPersonPhone,
                            SupplierName = mzk.parmName,
                            Country = mzk.parmCountry,
                            Province = mzk.parmState,
                            City = mzk.parmCityName,
                            Suburb = mzk.parmSuburban,
                            Email = mzk.parmEmail,
                            Address = mzk.parmAddress,
                            CountryName = mzk.parmCountryName,
                            ProvinceName = mzk.parmStateName,
                            CityName = mzk.parmCityName,
                        });
                    }
                }

                return suppliers;

            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<Supplier> FilterSuppliersByCriteria(string countryId, string provinceId, string cityId, string suburbId, string regionId, UserInfo userInfo)
        {

            try
            {
                var result = _client.getVendorBySelection(new SSProxy.CallContext() { }, userInfo.CompanyId, countryId, provinceId, suburbId, cityId, regionId);

                List<Supplier> suppliers = new List<Supplier>();

                if (result != null)
                {
                    foreach (var mzk in result)
                    {
                        suppliers.Add(new Supplier
                        {
                            AccountNum = mzk.parmAccountNum,
                            SupplierContactName = mzk.parmContactPersonName,
                            SupplierContactNumber = mzk.parmContactPersonPhone,
                            SupplierName = mzk.parmName,
                            Country = mzk.parmCountry,
                            Province = mzk.parmState,
                            City = mzk.parmCityName,
                            Suburb = mzk.parmSuburban,
                            Email = mzk.parmEmail,
                            Address = mzk.parmAddress,
                            CountryName = mzk.parmCountryName,
                            ProvinceName = mzk.parmStateName,
                            CityName = mzk.parmCityName,
                        });
                    }
                }
                return suppliers;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private bool InsertSelectedSupplier(SupplierSelection supplierSelection, UserInfo userInfo)
        {
            try
            {

                _client.insertVendDet(new SSProxy.CallContext() { }, supplierSelection.CaseNumber, supplierSelection.CaseServiceRecID, default(long), new MzkServiceDetailsContract
                {
                    parmContactPersonName = supplierSelection.SelectedSupplier.SupplierContactName,
                    parmSupplierName = supplierSelection.SelectedSupplier.SupplierName,
                    parmContactPersonPhone = supplierSelection.SelectedSupplier.SupplierContactNumber,
                    parmSupplierId = supplierSelection.SelectedSupplier.AccountNum
                }, new MzkAddressContract(), userInfo.CompanyId);

                return true;

            }

            catch (Exception)
            {
                throw;
            }
        }
        private bool InsertConfirmedServiceDetail(ServiceSchedulingDetail serviceSchedulingDetail, UserInfo userInfo)
        {
            try
            {

                var result = _client.insertServiceDetails(new SSProxy.CallContext() { }, serviceSchedulingDetail.CaseNumber, serviceSchedulingDetail.CaseServiceRecID, default(long), new MzkServiceDetailsContract
                {
                    parmAdditionalWork = serviceSchedulingDetail.AdditionalWork,
                    parmAddress = serviceSchedulingDetail.Address,
                    parmEventDesc = serviceSchedulingDetail.EventDesc,
                    parmLocationType = serviceSchedulingDetail.SelectedLocType.LocationName,
                    parmODOReading = serviceSchedulingDetail.ODOReading.ToString(),
                    parmODOReadingDate = DateTime.Parse(serviceSchedulingDetail.ODOReadingDate),
                    parmPreferredDateFirstOption = DateTime.Parse(serviceSchedulingDetail.ServiceDateOption1),
                    parmPreferredDateSecondOption = DateTime.Parse(serviceSchedulingDetail.ServiceDateOption2),
                    parmServiceType = serviceSchedulingDetail.SelectedServiceType,
                    parmSupplierName = serviceSchedulingDetail.SupplierName,
                    parmContactPersonName = serviceSchedulingDetail.ContactPersonName,
                    parmContactPersonPhone = serviceSchedulingDetail.ContactPersonPhone,

                }, new MzkAddressContract(), userInfo.CompanyId);


                return result;

            }

            catch (Exception)
            {
                throw;
            }
        }
        private string UpdateStatusList(Eqstra.DataProvider.AX.SSModels.Task task, UserInfo userInfo)
        {
            try
            {

                ObservableCollection<MzkServiceSchdTasksContract> mzkTasks = new ObservableCollection<MzkServiceSchdTasksContract>();

                Dictionary<string, EEPActionStep> actionStepMapping = new Dictionary<string, EEPActionStep>();

                actionStepMapping.Add(DriverTaskStatus.AwaitServiceBookingDetail, EEPActionStep.AwaitServiceBookingDetail);
                actionStepMapping.Add(DriverTaskStatus.AwaitSupplierSelection, EEPActionStep.AwaitSupplierSelection);
                actionStepMapping.Add(DriverTaskStatus.AwaitServiceBookingConfirmation, EEPActionStep.AwaitServiceBookingConfirmation);

                mzkTasks.Add(new MzkServiceSchdTasksContract
                {
                    parmCaseID = task.CaseNumber,
                    parmCaseCategory = task.CaseCategory,
                    parmCaseServiceRecID = task.CaseServiceRecID,
                    parmStatus = task.Status,
                    parmServiceRecID = task.ServiceRecID,
                    parmStatusDueDate = DateTime.Parse(task.StatusDueDate),
                    parmEEPActionStep = actionStepMapping[task.Status]
                });
                var result = _client.updateStatusList(new SSProxy.CallContext() { }, mzkTasks.ToArray(), userInfo.CompanyId);

                return result.FirstOrDefault().parmStatus;

            }

            catch (Exception)
            {
                throw;
            }
        }

    }
}
