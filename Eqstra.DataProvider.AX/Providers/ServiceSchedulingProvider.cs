using Eqstra.DataProvider.AX.Helpers;
using Eqstra.DataProvider.AX.SSModels;
using Eqstra.DataProvider.AX.SSProxy;
using Eqstra.Framework.Web.DataAccess;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Eqstra.DataProvider.AX.Providers
{
    [DataProvider(Name = "ServiceScheduling")]
    [Export(typeof(IDataProvider))]
    public class ServiceSchedulingProvider : IDataProvider
    {
        SSProxy.MzkServiceSchedulingServiceClient client;

        public System.Collections.IList GetDataList(object[] criterias)
        {
            try
            {
                GetService();
                System.Collections.IList response = null;
                switch (criterias[0].ToString())
                {
                    case ActionSwitch.GetTasks:
                        response = GetTasks();
                        break;

                    case ActionSwitch.GetCountryRegionList:
                        response = GetCountryRegionList();
                        break;

                    case ActionSwitch.GetProvinceList:
                        response = GetProvinceList("");
                        break;

                    case ActionSwitch.GetCityList:
                        response = GetCityList("", "");
                        break;

                    case ActionSwitch.GetSuburbList:
                        response = GetSuburbList("", "");
                        break;

                    case ActionSwitch.GetRegionList:
                        response = GetRegionList("", "");
                        break;

                    case ActionSwitch.GetZipcodeList:
                        // response = GetZipcodeList();
                        break;

                    case ActionSwitch.GetServiceDetails:
                        //response = GetServiceDetails();
                        break;

                    case ActionSwitch.GetLocationType:
                        //response = GetLocationType();
                        break;

                    case ActionSwitch.GetCustomers:
                        //response = GetCustomers();
                        break;

                    case ActionSwitch.GetVendors:
                        response = GetVendors();
                        break;

                    case ActionSwitch.GetDrivers:
                        //response = GetDrivers();
                        break;

                    case ActionSwitch.GetVendSupplirerSvc:
                        response = GetVendSupplirerSvc();
                        break;

                    case ActionSwitch.UpdateConfirmationDates:
                        // response = UpdateConfirmationDates();
                        break;

                    case ActionSwitch.InsertServiceDetails:
                        // response = InsertServiceDetails();
                        break;

                    case ActionSwitch.InsertSelectedSupplier:
                        // response = InsertSelectedSupplier();
                        break;

                    case ActionSwitch.InsertConfirmedServiceDetail:
                        // response = InsertConfirmedServiceDetail();
                        break;

                    case ActionSwitch.UpdateStatusList:
                        // response = UpdateStatusList();
                        break;
                }
                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public object GetSingleData(object[] criterias)
        {
            object response = null;
            GetService();
            switch (criterias[1].ToString())
            {
                case ActionSwitch.ValidateUser:
                    response = ValidateUser("sahmed", "Password6");
                    break;
            }
            return response;
        }

        public bool DeleteData(object[] criterias)
        {
            throw new NotImplementedException();
        }

        public object SaveData(object[] criterias)
        {
            GetService();
            return null;
        }

        public object GetService()
        {
            return GetServiceClient();
        }

        public SSProxy.MzkServiceSchedulingServiceClient GetServiceClient()
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
                client = new SSProxy.MzkServiceSchedulingServiceClient(basicHttpBinding, new EndpointAddress("http://srfmlbispstg01.lfmd.co.za/MicrosoftDynamicsAXAif60/ServiceSchedulingService/xppservice.svc?wsdl"));
                client.ClientCredentials.UserName.UserName = "lfmd" + "\"" + "rchivukula";
                client.ClientCredentials.UserName.Password = "Password1";
                client.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Identification;
                client.ClientCredentials.Windows.ClientCredential = new NetworkCredential("rchivukula", "Password1", "lfmd");
            }
            catch (Exception)
            {
                throw;
            }

            return client;
        }

        public UserInfo ValidateUser(string userId, string password)
        {
            UserInfo userInfo = new UserInfo();
            try
            {
                var result = client.validateUser(null, userId, password);
                if (result != null)
                {
                    userInfo.UserId = result.parmUserID;
                    userInfo.CompanyId = result.parmCompany;
                    userInfo.CompanyName = result.parmCompanyName;
                    userInfo.Name = result.parmUserName;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return userInfo;
        }

        public List<Eqstra.DataProvider.AX.SSModels.Task> GetTasks()
        {
            try
            {
                var result = client.getTasksOptimize(new SSProxy.CallContext() { Company = "1000" }, "rchivukula", "1000");
                List<Eqstra.DataProvider.AX.SSModels.Task> driverTaskList = new List<Eqstra.DataProvider.AX.SSModels.Task>();
                if (result != null)
                {

                    foreach (var mzkTask in result)
                    {
                        driverTaskList.Add(new Eqstra.DataProvider.AX.SSModels.Task
                        {
                            CaseNumber = mzkTask.parmCaseID,
                            Address = mzkTask.parmCustAddress,
                            CustomerName = mzkTask.parmCustName,
                            CustPhone = mzkTask.parmCustPhone,
                            Status = mzkTask.parmStatus,
                            StatusDueDate = mzkTask.parmStatusDueDate,
                            RegistrationNumber = mzkTask.parmRegistrationNum,
                            // AllocatedTo = _userInfo.Name,
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
                            ServiceRecID = mzkTask.parmServiceRecID
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

        public List<Country> GetCountryRegionList()
        {
            try
            {
                var result = client.getCountryRegionList(new SSProxy.CallContext() { Company = "1000" }, "1000");
                var vendor_result = client.getVendSupplirerName(new SSProxy.CallContext() { Company = "1000" }, "1000");

                List<Country> countryList = new List<Country>();
                if (result != null)
                {

                    result.OrderBy(o => o.parmCountryRegionName).AsParallel().ForAll(mzk =>
                    {
                        if (vendor_result.Any(a => a.parmCountry == mzk.parmCountryRegionId))
                        {
                            countryList.Add(
                                               new Country
                                               {
                                                   Name = mzk.parmCountryRegionName,
                                                   Id = mzk.parmCountryRegionId
                                               }
                                               );
                        }
                    });
                }

                return countryList;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public List<Province> GetProvinceList(string countryId)
        {
            try
            {
                var result = client.getProvinceList(new SSProxy.CallContext() { Company = "1000" }, countryId, "1000");
                List<Province> provinceList = new List<Province>();
                if (result != null)
                {
                    result.AsParallel().ForAll(mzk =>
                    {
                        provinceList.Add(new Province { Name = mzk.parmStateName, Id = mzk.parmStateId });
                    });
                }
                return provinceList;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public List<City> GetCityList(string countryId, string stateId)
        {
            try
            {
                var result = client.getCityList(new SSProxy.CallContext() { Company = "1000" }, countryId, stateId, "1000");
                List<City> cityList = new List<City>();
                if (result != null)
                {
                    result.AsParallel().ForAll(mzk =>
                    {
                        cityList.Add(new City { Name = mzk.parmCountyId, Id = mzk.parmStateId });
                    });
                }
                return cityList;
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        public List<Suburb> GetSuburbList(string countryId, string stateId)
        {
            try
            {
                var result = client.getSuburbList(new SSProxy.CallContext() { Company = "1000" }, countryId, stateId, "1000");
                List<Suburb> suburbList = new List<Suburb>();
                if (result != null)
                {
                    result.AsParallel().ForAll(mzk =>
                    {
                        suburbList.Add(new Suburb { Name = mzk.parmCity, Id = mzk.parmStateId });
                    });
                }
                return suburbList;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public List<Region> GetRegionList(string countryId, string stateId)
        {
            try
            {
                var result = client.getRegions(new SSProxy.CallContext() { Company = "1000" }, countryId, stateId, "1000");
                List<Region> regionList = new List<Region>();
                if (result != null)
                {
                    result.AsParallel().ForAll(mzk =>
                    {
                        regionList.Add(new Region { Name = mzk.parmRegionName, Id = mzk.parmRegion });
                    });
                }
                return regionList;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public List<string> GetZipcodeList(string countryId, string stateId)
        {
            try
            {
                var result = client.getZipcodeList(new SSProxy.CallContext() { Company = "1000" }, countryId, stateId, "1000");
                List<string> zipcodeList = new List<string>();
                if (result != null)
                {
                    result.AsParallel().ForAll(mzk =>
                    {
                        zipcodeList.Add(mzk.parmZipCode);
                    });
                }
                return zipcodeList;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public ServiceSchedulingDetail GetServiceDetails(string caseNumber, long caseServiceRecId)
        {
            try
            {
                var result = client.getServiceDetails(new SSProxy.CallContext() { Company = "1000" }, caseNumber, caseServiceRecId, "1000");
                ServiceSchedulingDetail detailServiceScheduling = null;
                if (result != null)
                {

                    result.AsParallel().ForAll(mzk =>
                    {
                        detailServiceScheduling = (new ServiceSchedulingDetail
                        {
                            Address = mzk.parmAddress,
                            AdditionalWork = mzk.parmAdditionalWork,
                            ServiceDateOption1 = mzk.parmPreferredDateFirstOption < DateTime.Today ? DateTime.Today : mzk.parmPreferredDateFirstOption,
                            ServiceDateOption2 = mzk.parmPreferredDateSecondOption < DateTime.Today ? DateTime.Today : mzk.parmPreferredDateSecondOption,
                            ODOReading = mzk.parmODOReading.ToString(),
                            ODOReadingDate = mzk.parmODOReadingDate < DateTime.Today ? DateTime.Today : mzk.parmODOReadingDate,
                            ServiceType = GetServiceTypes(caseNumber, "1000"),
                            LocationTypes = GetLocationType(caseServiceRecId, "1000"),
                            SupplierName = mzk.parmSupplierName,
                            EventDesc = mzk.parmEventDesc,
                            ContactPersonName = mzk.parmContactPersonName,
                            ContactPersonPhone = mzk.parmContactPersonPhone,
                            SupplierDateTime = DateTime.Now// need to add in service

                        });
                    });

                }
                return detailServiceScheduling;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        private List<string> GetServiceTypes(string caseNumber, string companyId)
        {
            var result = client.getServiceTypes(new SSProxy.CallContext() { Company = "1000" }, caseNumber, "1000");
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
        public List<LocationType> GetLocationType(long caseServiceRecId, string companyId)
        {
            try
            {
                var result = client.getLocationType(new SSProxy.CallContext() { Company = "1000" }, caseServiceRecId, companyId);
                List<LocationType> locationTypes = new List<LocationType>();
                if (result != null)
                {
                    result.AsParallel().ForAll(mzk =>
                    {
                        locationTypes.Add(new LocationType
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
                return null;
            }
        }
        public List<DestinationType> GetCustomers()
        {
            try
            {
                var result = client.getCustomers(new SSProxy.CallContext() { Company = "1000" }, "1000");
                List<DestinationType> destinationTypes = new List<DestinationType>();
                if (result != null)
                {
                    result.AsParallel().ForAll(mzk =>
                    {
                        destinationTypes.Add(new DestinationType
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

                return null;
            }
        }
        public List<DestinationType> GetVendors()
        {
            try
            {
                var result = client.getVendSupplirerName(new SSProxy.CallContext() { Company = "1000" }, "1000");
                List<DestinationType> destinationTypes = new List<DestinationType>();
                if (result != null)
                {
                    result.AsParallel().ForAll(mzk =>
                    {
                        destinationTypes.Add(new DestinationType
                        {
                            ContactName = mzk.parmName,
                            Id = mzk.parmAccountNum,
                            Address = mzk.parmAddress
                        });
                    });
                }
                destinationTypes = destinationTypes.OrderBy(o => o.ContactName).ToList<DestinationType>();
                return destinationTypes;
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        public IEnumerable<DestinationType> GetDrivers()
        {
            try
            {
                var result = client.getDrivers(new SSProxy.CallContext() { Company = "1000" }, "1000");
                List<DestinationType> destinationTypes = new List<DestinationType>();
                if (result != null)
                {
                    result.AsParallel().ForAll(mzk =>
                    {
                        destinationTypes.Add(new DestinationType
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

                return null;
            }
        }
        public List<Supplier> GetVendSupplirerSvc()
        {
            try
            {
                var result = client.getVendSupplirerName(new SSProxy.CallContext() { Company = "1000" }, "1000");
                List<Supplier> suppliers = new List<Supplier>();

                if (result != null)
                {
                    result.AsParallel().ForAll(mzk =>
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
                        });
                    });
                }

                return suppliers;
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        public bool UpdateConfirmationDates(long caseServiceRecId, ServiceSchedulingDetail serviceSchedulingDetail)
        {
            try
            {
                var result = client.updateConfirmationDates(new SSProxy.CallContext() { Company = "1000" }, caseServiceRecId, new MzkServiceDetailsContract
                {
                    parmPreferredDateFirstOption = serviceSchedulingDetail.ServiceDateOption1,
                    parmPreferredDateSecondOption = serviceSchedulingDetail.ServiceDateOption2
                }, "1000");
                return result;
            }
            catch (Exception ex)
            {

                return false;
            }

        }
        public bool InsertServiceDetails(ServiceSchedulingDetail serviceSchedulingDetail, Address address, string caseNumber, long caseServiceRecId, long _entityRecId)
        {
            try
            {
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
                    parmLocationType = serviceSchedulingDetail.SelectedLocationType != null ? serviceSchedulingDetail.SelectedLocationType.LocType : string.Empty,
                    parmODOReading = serviceSchedulingDetail.ODOReading,
                    parmODOReadingDate = serviceSchedulingDetail.ODOReadingDate,
                    parmPreferredDateFirstOption = serviceSchedulingDetail.ServiceDateOption1,
                    parmPreferredDateSecondOption = serviceSchedulingDetail.ServiceDateOption2,
                    parmServiceType = serviceSchedulingDetail.SelectedServiceType
                };


                var result = client.insertServiceDetails(new SSProxy.CallContext() { Company = "1000" }, caseNumber, caseServiceRecId, _entityRecId, mzkServiceDetailsContract
                      , mzkAddressContract, "1000");

                return result;
            }

            catch (Exception ex)
            {

                return false;
            }
        }
        public void InsertSelectedSupplier(SupplierSelection supplierSelection, string caseNumber, long caseServiceRecId)
        {
            try
            {
                client.insertVendDet(new SSProxy.CallContext() { Company = "1000" }, caseNumber, caseServiceRecId, default(long), new MzkServiceDetailsContract
                {
                    parmContactPersonName = supplierSelection.SelectedSupplier.SupplierContactName,
                    parmSupplierName = supplierSelection.SelectedSupplier.SupplierName,
                    parmContactPersonPhone = supplierSelection.SelectedSupplier.SupplierContactNumber,
                    parmSupplierId = supplierSelection.SelectedSupplier.AccountNum
                }, new MzkAddressContract(), "1000");

            }

            catch (Exception ex)
            {

            }
        }
        public bool InsertConfirmedServiceDetail(ServiceSchedulingDetail serviceSchedulingDetail, string caseNumber, long caseServiceRecId)
        {
            try
            {

                var result = client.insertServiceDetails(new SSProxy.CallContext() { Company = "1000" }, caseNumber, caseServiceRecId, default(long), new MzkServiceDetailsContract
                {
                    parmAdditionalWork = serviceSchedulingDetail.AdditionalWork,
                    parmAddress = serviceSchedulingDetail.Address,
                    parmEventDesc = serviceSchedulingDetail.EventDesc,
                    parmLocationType = serviceSchedulingDetail.SelectedLocationType.LocType,
                    parmODOReading = serviceSchedulingDetail.ODOReading,
                    parmODOReadingDate = serviceSchedulingDetail.ODOReadingDate,
                    parmPreferredDateFirstOption = serviceSchedulingDetail.ServiceDateOption1,
                    parmPreferredDateSecondOption = serviceSchedulingDetail.ServiceDateOption2,
                    parmServiceType = serviceSchedulingDetail.SelectedServiceType,
                    parmSupplierName = serviceSchedulingDetail.SupplierName,
                    parmContactPersonName = serviceSchedulingDetail.ContactPersonName,
                    parmContactPersonPhone = serviceSchedulingDetail.ContactPersonPhone,

                }, new MzkAddressContract(), "1000");


                return result;
            }

            catch (Exception ex)
            {
                return false;
            }
        }
        public string UpdateStatusList(Eqstra.DataProvider.AX.SSModels.Task task)
        {
            try
            {
                ObservableCollection<MzkServiceSchdTasksContract> mzkTasks = new ObservableCollection<MzkServiceSchdTasksContract>();

                Dictionary<string, EEPActionStep> actionStepMapping = new Dictionary<string, EEPActionStep>();

                //actionStepMapping.Add(DriverTaskStatus.AwaitSupplierSelection, EEPActionStep.AwaitServiceDetail);
                //actionStepMapping.Add(DriverTaskStatus.AwaitServiceConfirmation, EEPActionStep.AwaitSupplierSelection);
                //actionStepMapping.Add(DriverTaskStatus.AwaitJobCardCapture, EEPActionStep.AwaitServiceConfirmation);
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
                var result = client.updateStatusList(new SSProxy.CallContext() { Company = "1000" }, mzkTasks.ToArray(), "1000");

                return result.FirstOrDefault().parmStatus;
            }

            catch (Exception ex)
            {
                return task.Status;
            }
        }

    }
}
