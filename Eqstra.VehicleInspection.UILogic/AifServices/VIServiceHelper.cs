﻿using Eqstra.BusinessLogic;
using Eqstra.BusinessLogic.Base;
using Eqstra.BusinessLogic.Commercial;
using Eqstra.BusinessLogic.Enums;
using Eqstra.BusinessLogic.Helpers;
using Eqstra.BusinessLogic.Passenger;
using Eqstra.VehicleInspection.UILogic.VIService;
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
using Windows.UI.Xaml;

namespace Eqstra.VehicleInspection.UILogic.AifServices
{
    public class VIServiceHelper
    {
        private static readonly VIServiceHelper instance = new VIServiceHelper();
        private VIService.MzkVehicleInspectionServiceClient client;
        ConnectionProfile _connectionProfile;
        Action _syncExecute;
        private UserInfo _userInfo;
        static VIServiceHelper()
        {
        }
        public static VIServiceHelper Instance
        {
            get
            {
                return instance;
            }
        }
        public async System.Threading.Tasks.Task<VIService.MzkVehicleInspectionServiceClient> ConnectAsync(string userName, string password, string domain = "lfmd")
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
                client = new VIService.MzkVehicleInspectionServiceClient(basicHttpBinding, new EndpointAddress("http://196.35.126.189/MicrosoftDynamicsAXAif60/VehicleInspection/xppservice.svc"));
                client.ClientCredentials.UserName.UserName = userName;
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
        async public System.Threading.Tasks.Task<MzkVehicleInspectionServiceValidateUserResponse> ValidateUser(string userId, string password)
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
        async public System.Threading.Tasks.Task SyncFromSvcAsync(BaseModel baseModel)
        {
            try
            {
                if (_userInfo == null)
                {
                    _userInfo =  JsonConvert.DeserializeObject<UserInfo>(ApplicationData.Current.RoamingSettings.Values[Constants.UserInfo].ToString());
                }
                if (AppSettings.Instance.IsSynchronizing == 0)
                {
                    Synchronize(async () =>
                       {
                           await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                           {

                               AppSettings.Instance.IsSynchronizing = 1;
                           }
                                );

                           if (baseModel is PAccessories)
                           {
                               await this.EditPassengerAccessoriesToSvcAsync();
                           }
                           if (baseModel is PBodywork)
                           {
                               await this.EditPassengerBodyworkToSvcAsync();
                           }
                           if (baseModel is PVehicleDetails)
                           {
                               await this.EditPassengerVehicleDetailsToSvcAsync();
                           }
                           if (baseModel is PTrimInterior)
                           {
                               await this.EditPassengerTrimInteriorToSvcAsync();
                           }
                           if (baseModel is PTyreCondition)
                           {
                               await this.EditPassengerTyreConditionToSvcAsync();
                           }
                           if (baseModel is PGlass)
                           {
                               await this.EditPassengerGlassToSvcAsync();
                           }
                           if (baseModel is PMechanicalCond)
                           {
                               await this.EditPassengerMechanicalConditionAsync();
                           }
                           if (baseModel is CAccessories)
                           {
                               await this.EditCommercialAccessoriesToSvcAsync();
                           }
                           if (baseModel is CVehicleDetails)
                           {
                               await this.EditCommercialVehicleDetailsToSvcAsync();
                           }
                           if (baseModel is CCabTrimInter)
                           {
                               await this.EditCommercialTrimInteriorToSvcAsync();
                           }
                           if (baseModel is CChassisBody)
                           {
                               await this.EditCommercialChassisBodyToSvcAsync();
                           }
                           if (baseModel is CTyres)
                           {
                               await this.EditCommercialTyreConditionToSvcAsync();
                           }
                           if (baseModel is CGlass)
                           {
                               await this.EditCommercialGlassToSvcAsync();
                           }
                           if (baseModel is CMechanicalCond)
                           {
                               await this.EditCommercialMechConditionToSvcAsync();
                           }
                           if (baseModel is CPOI)
                           {
                               await this.EditCommercialInspectionProofToSvcAsync();
                           }
                           if (baseModel is PInspectionProof)
                           {
                               await this.EditPassengerInspectionProofToSvcAsync();
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
        async private System.Threading.Tasks.Task GetCustDetailsFromSvcAsync(string cusId,string contactName,string contactNumber)
        {
            try
            {
                if (_userInfo == null)
                {
                    _userInfo =  JsonConvert.DeserializeObject<UserInfo>(ApplicationData.Current.RoamingSettings.Values[Constants.UserInfo].ToString());
                }

                var result = await client.getCustDetailsAsync(cusId,_userInfo.CompanyId);
                if (result.response != null)
                {
                    List<Eqstra.BusinessLogic.Customer> cusInsertList = new List<Customer>();
                    List<Eqstra.BusinessLogic.Customer> cusUpdateList = new List<Customer>();
                    var customerData = await SqliteHelper.Storage.LoadTableAsync<Customer>();
                    result.response.AsParallel().ForAll(mzkCustomer =>
                    {
                        var custToSave = new Eqstra.BusinessLogic.Customer
                             {
                                 Address = mzkCustomer.parmCustAddress,
                                 CustomerName = mzkCustomer.parmCustName,
                                 ContactNumber = mzkCustomer.parmCustPhone,
                                 ContactName = contactName,
                                EmailId = mzkCustomer.parmCustEmail,                                
                                 Id = cusId

                             };
                        if (customerData.Any(cus => cus.Id == cusId))
                        {
                            cusUpdateList.Add(custToSave);
                        }
                        else
                        {
                            cusInsertList.Add(custToSave);
                        }
                    });

                    if (cusUpdateList.Any())
                        await SqliteHelper.Storage.UpdateAllAsync<Customer>(cusUpdateList);


                    if (cusInsertList.Any())
                        await SqliteHelper.Storage.InsertAllAsync<Customer>(cusInsertList);
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
        async public System.Threading.Tasks.Task SyncTasksFromSvcAsync()
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
                var result = await client.getTasksAsync(_userInfo.CompanyId, _userInfo.UserId);
                var taskData = await SqliteHelper.Storage.LoadTableAsync<Eqstra.BusinessLogic.Task>();
                if (result.response != null)
                {
                    List<Eqstra.BusinessLogic.Task> taskInsertList = new List<BusinessLogic.Task>();
                    List<Eqstra.BusinessLogic.Task> taskUpdateList = new List<BusinessLogic.Task>();
                    foreach (var mzkTask in result.response)
                    {
                        var taskTosave = new Eqstra.BusinessLogic.Task
                           {

                               CaseCategory = mzkTask.parmCaseCategory,
                               CaseNumber = mzkTask.parmCaseID,
                               CaseServiceRecID = mzkTask.parmCaseServiceRecId,
                               CategoryType = mzkTask.parmCategoryType,
                               CollectionRecID = mzkTask.parmCollectionRecId,
                               ConfirmedDate = mzkTask.parmConfirmedDueDate < DateTime.Today ? DateTime.Today : mzkTask.parmConfirmedDueDate,
                               Address = mzkTask.parmCustAddress,
                               CustomerId = mzkTask.parmCustId,
                               CustomerName = mzkTask.parmCustName,                               
                               CustPhone = mzkTask.parmCustPhone,
                               ContactName = mzkTask.parmContactPersonName,
                               ContactNumber = mzkTask.parmContactPersonPhone,
                               ProcessStepRecID = mzkTask.parmProcessStepRecID,
                               ServiceRecID = mzkTask.parmServiceRecId,
                               Status = mzkTask.parmStatus,
                               StatusDueDate = mzkTask.parmStatusDueDate,
                               UserId = mzkTask.parmUserID,
                               RegistrationNumber = mzkTask.parmRegistrationNum,
                               AllocatedTo = _userInfo.Name,
                               VehicleInsRecId = mzkTask.parmRecID,
                               VehicleType = (VehicleTypeEnum)Enum.Parse(typeof(VehicleTypeEnum), mzkTask.parmVehicleType.ToString())
                           };

                        GetCustDetailsFromSvcAsync(mzkTask.parmCustId,mzkTask.parmContactPersonName,mzkTask.parmContactPersonPhone);
                        if (mzkTask.parmVehicleType == MzkVehicleType.Passenger)
                        {
                            GetPVehicleDetailsAsync(mzkTask.parmCaseID, mzkTask.parmRecID);
                        }
                        else
                        {
                           GetCVehicleDetailsAsync(mzkTask.parmCaseID, mzkTask.parmRecID);
                        }
                        if (taskData.Any(s => s.CaseNumber == mzkTask.parmCaseID))
                        {
                            taskUpdateList.Add(taskTosave);
                        }
                        else
                        {
                            taskInsertList.Add(taskTosave);
                        }
                    }

                    if (taskUpdateList.Any())
                        await SqliteHelper.Storage.UpdateAllAsync<BusinessLogic.Task>(taskUpdateList);


                    if (taskInsertList.Any())
                        await SqliteHelper.Storage.InsertAllAsync<BusinessLogic.Task>(taskInsertList);
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

        async private System.Threading.Tasks.Task GetCVehicleDetailsAsync(string caseNumber, long vRecId)
        {
            try
            {
                if (_userInfo == null)
                {
                    _userInfo =  JsonConvert.DeserializeObject<UserInfo>(ApplicationData.Current.RoamingSettings.Values[Constants.UserInfo].ToString());
                }
                var resp = await client.readVehicleDetailsAsync(caseNumber, _userInfo.CompanyId);
                var vehicleData = await SqliteHelper.Storage.LoadTableAsync<BusinessLogic.Commercial.CVehicleDetails>();
                if (resp.response != null && resp.response.Any())
                {
                    List<BusinessLogic.Commercial.CVehicleDetails> vehicleInsertList = new List<BusinessLogic.Commercial.CVehicleDetails>();
                    List<BusinessLogic.Commercial.CVehicleDetails> vehicleUpdateList = new List<BusinessLogic.Commercial.CVehicleDetails>();
                    resp.response.AsParallel().ForAll(v =>
                    {

                        var vehicleTosave = new BusinessLogic.Commercial.CVehicleDetails
                        {
                            ChassisNumber = v.parmChassisNumber.ToString(),
                            Color = v.parmColor,
                            Year = v.parmyear.ToString(),
                            ODOReading = v.parmODOReading.ToString(),
                            Make = v.parmMake,
                            EngineNumber = v.parmEngineNumber,
                            VehicleInsRecID = vRecId,
                            RecID = v.parmRecID,
                            CaseNumber = caseNumber,
                            TableId = v.parmTableId,
                            RegistrationNumber = v.parmRegNo,



                        };

                        if (vehicleData.Any(s => s.VehicleInsRecID == vRecId))
                        {
                            vehicleUpdateList.Add(vehicleTosave);
                        }
                        else
                        {
                            vehicleInsertList.Add(vehicleTosave);
                        }

                    });

                    if (vehicleUpdateList.Any())
                        await SqliteHelper.Storage.UpdateAllAsync<CVehicleDetails>(vehicleUpdateList);

                    if (vehicleInsertList.Any())
                        await SqliteHelper.Storage.InsertAllAsync<CVehicleDetails>(vehicleInsertList);

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

        async private System.Threading.Tasks.Task GetPVehicleDetailsAsync(string caseNumber, long vRecId)
        {
            try
            {
                if (_userInfo == null)
                {
                    _userInfo = JsonConvert.DeserializeObject<UserInfo>(ApplicationData.Current.RoamingSettings.Values[Constants.UserInfo].ToString());
                }
                var resp = await client.readVehicleDetailsAsync(caseNumber, _userInfo.CompanyId);
                var vehicleData = await SqliteHelper.Storage.LoadTableAsync<BusinessLogic.Passenger.PVehicleDetails>();
                if (resp.response != null && resp.response.Any())
                {
                    List<BusinessLogic.Passenger.PVehicleDetails> vehicleInsertList = new List<BusinessLogic.Passenger.PVehicleDetails>();
                    List<BusinessLogic.Passenger.PVehicleDetails> vehicleUpdateList = new List<BusinessLogic.Passenger.PVehicleDetails>();
                    resp.response.AsParallel().ForAll(v =>
                    {

                        var vehicleTosave = new BusinessLogic.Passenger.PVehicleDetails
                           {
                               ChassisNumber = v.parmChassisNumber.ToString(),
                               Color = v.parmColor,
                               Year = v.parmyear.ToString(),
                               ODOReading = v.parmODOReading.ToString(),
                               Make = v.parmMake,
                               EngineNumber = v.parmEngineNumber,
                               VehicleInsRecID = vRecId,
                               RecID = v.parmRecID,
                               CaseNumber = caseNumber,
                               TableId = v.parmTableId,
                               RegistrationNumber = v.parmRegNo,
                           };

                        if (vehicleData.Any(s => s.VehicleInsRecID == vRecId))
                        {
                            vehicleUpdateList.Add(vehicleTosave);
                        }
                        else
                        {
                            vehicleInsertList.Add(vehicleTosave);
                        }

                    });

                    if (vehicleUpdateList.Any())
                        await SqliteHelper.Storage.UpdateAllAsync<PVehicleDetails>(vehicleUpdateList);

                    if (vehicleInsertList.Any())
                        await SqliteHelper.Storage.InsertAllAsync<PVehicleDetails>(vehicleInsertList);

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
        async private System.Threading.Tasks.Task EditPassengerVehicleDetailsToSvcAsync()
        {
            try
            {
                if (_userInfo == null)
                {
                    _userInfo =  JsonConvert.DeserializeObject<UserInfo>(ApplicationData.Current.RoamingSettings.Values[Constants.UserInfo].ToString());
                }
                ObservableCollection<MzkVehicleDetailsContract> mzkVehicleDetailsContractColl = new ObservableCollection<MzkVehicleDetailsContract>();
                var pVehicleDetailsData = (await SqliteHelper.Storage.LoadTableAsync<Eqstra.BusinessLogic.Passenger.PVehicleDetails>()).Where(x => x.ShouldSave);

                if (pVehicleDetailsData != null)
                {
                    pVehicleDetailsData.AsParallel().ForAll(pVehicleDetails =>
                       {
                           mzkVehicleDetailsContractColl.Add(new MzkVehicleDetailsContract()
                           {
                               parmLisenceDiscCurrent = pVehicleDetails.IsLicenseDiscCurrent ? NoYes.Yes : NoYes.No,
                               parmRecID = pVehicleDetails.RecID,
                               parmRegNo = pVehicleDetails.RegistrationNumber,
                               parmTableId = pVehicleDetails.TableId,
                               parmVehicleInsRecID = pVehicleDetails.VehicleInsRecID

                           });
                       });
                }

                var resp = await client.editVehicleDetailsAsync(mzkVehicleDetailsContractColl, _userInfo.CompanyId);
                var pVehicleDetailsList = new ObservableCollection<PVehicleDetails>();
                if (resp.response != null)
                {
                    resp.response.AsParallel().ForAll(x =>
                pVehicleDetailsList.Add(new PVehicleDetails
                {
                    IsLicenseDiscCurrent = x.parmLisenceDiscCurrent == NoYes.Yes ? true : false,
                    RecID = x.parmRecID,
                    RegistrationNumber = x.parmRegNo,
                    TableId = x.parmTableId,
                    VehicleInsRecID = x.parmVehicleInsRecID

                }));
                    await SqliteHelper.Storage.UpdateAllAsync<PVehicleDetails>(pVehicleDetailsList);
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
        async private System.Threading.Tasks.Task EditPassengerAccessoriesToSvcAsync()
        {
            try
            {
                if (_userInfo == null)
                {
                    _userInfo =  JsonConvert.DeserializeObject<UserInfo>(ApplicationData.Current.RoamingSettings.Values[Constants.UserInfo].ToString());
                }
                var pAccessoriesData = (await SqliteHelper.Storage.LoadTableAsync<Eqstra.BusinessLogic.Passenger.PAccessories>()).Where(x => x.ShouldSave);
                ObservableCollection<MzkMobiPassengerAccessoriesContract> mzkMobiPassengerAccessoriesContractColl = new ObservableCollection<MzkMobiPassengerAccessoriesContract>();
                if (pAccessoriesData != null)
                {
                    pAccessoriesData.AsParallel().ForAll(pAccessories =>
            {
                mzkMobiPassengerAccessoriesContractColl.Add(new MzkMobiPassengerAccessoriesContract()
                {
                    parmHasAircon = pAccessories.HasAircon ? NoYes.Yes : NoYes.No,
                    parmHasAlarm = pAccessories.HasAlarm ? NoYes.Yes : NoYes.No,
                    parmHasCanopy = pAccessories.HasCanopy ? NoYes.Yes : NoYes.No,
                    parmHasCDShuttle = pAccessories.HasCDShuffle ? NoYes.Yes : NoYes.No,
                    parmHasJack = pAccessories.HasJack ? NoYes.Yes : NoYes.No,
                    parmHasKey = pAccessories.HasKey ? NoYes.Yes : NoYes.No,
                    parmHasMaps = pAccessories.HasMags ? NoYes.Yes : NoYes.No,
                    parmHasNavigation = pAccessories.HasNavigation ? NoYes.Yes : NoYes.No,
                    parmHasRadio = pAccessories.HasRadio ? NoYes.Yes : NoYes.No,
                    parmHasServicesBook = pAccessories.HasServicesBook ? NoYes.Yes : NoYes.No,
                    parmHasSparekey = pAccessories.HasSpareKeys ? NoYes.Yes : NoYes.No,
                    parmHasSpareType = pAccessories.HasSpareTyre ? NoYes.Yes : NoYes.No,
                    parmHasTools = pAccessories.HasTools ? NoYes.Yes : NoYes.No,
                    parmHasTrackingUnit = pAccessories.HasTrackingUnit ? NoYes.Yes : NoYes.No,
                    parmHasOthers = pAccessories.IsOthers ? NoYes.Yes : NoYes.No,

                    parmIsDamagedAircon = pAccessories.IsAirconDmg ? NoYes.Yes : NoYes.No,
                    parmIsDamagedAlarm = pAccessories.IsAlarmDmg ? NoYes.Yes : NoYes.No,
                    parmIsDamagedCanopy = pAccessories.IsCanopyDmg ? NoYes.Yes : NoYes.No,
                    parmIsDamagedCDShuttle = pAccessories.IsCDShuffleDmg ? NoYes.Yes : NoYes.No,
                    parmIsDamagedJack = pAccessories.IsJackDmg ? NoYes.Yes : NoYes.No,
                    parmIsDamagedKey = pAccessories.IsKeyDmg ? NoYes.Yes : NoYes.No,
                    parmIsDamagedMaps = pAccessories.IsMagsDmg ? NoYes.Yes : NoYes.No,
                    parmIsDamagedNavigation = pAccessories.IsNavigationDmg ? NoYes.Yes : NoYes.No,
                    parmIsDamagedRadio = pAccessories.IsRadioDmg ? NoYes.Yes : NoYes.No,
                    parmIsDamagedServicesBook = pAccessories.IsServicesBookDmg ? NoYes.Yes : NoYes.No,
                    parmIsDamagedSparekey = pAccessories.IsSpareKeysDmg ? NoYes.Yes : NoYes.No,
                    parmIsDamagedSpareType = pAccessories.IsSpareTyreDmg ? NoYes.Yes : NoYes.No,
                    parmIsDamagedTools = pAccessories.IsToolsDmg ? NoYes.Yes : NoYes.No,
                    parmIsDamagedTrackingUnit = pAccessories.IsTrackingUnitDmg ? NoYes.Yes : NoYes.No,

                    parmAirconComments = pAccessories.AirconComment,
                    parmAlarmComments = pAccessories.AlarmComment,
                    parmCanopyComments = pAccessories.CanopyComment,
                    parmCDShuttleComments = pAccessories.CDShuffleComment,
                    parmJackComments = pAccessories.JackComment,
                    parmKeyComments = pAccessories.KeyComment,
                    parmMapsComments = pAccessories.MagsComment,
                    parmNavigationComments = pAccessories.NavigationComment,
                    parmRadioComments = pAccessories.RadioComment,
                    parmServiceBooksComments = pAccessories.ServicesBookComment,
                    parmSpareKeysComments = pAccessories.SpareKeysComment,
                    parmSpareTypeComments = pAccessories.SpareTyreComment,
                    parmToolComments = pAccessories.ToolsComment,
                    parmTrackLineComments = pAccessories.TrackingUnitComment,
                    parmOtherComments = pAccessories.OthersComment,

                    parmVehicleInsRecID = pAccessories.VehicleInsRecID,
                    parmTableId = pAccessories.TableId,
                    parmRecID = pAccessories.RecID,
                });

            });

                }
                var res = await client.editPassengerAccessoriesAsync(mzkMobiPassengerAccessoriesContractColl, _userInfo.CompanyId);
                var pAccessoriesList = new ObservableCollection<PAccessories>();
                if (res.response != null)
                {
                    res.response.AsParallel().ForAll(x =>
                               pAccessoriesList.Add(new PAccessories
                               {
                                   HasAircon = x.parmHasAircon == NoYes.Yes ? true : false,
                                   HasAlarm = x.parmHasAlarm == NoYes.Yes ? true : false,
                                   HasCanopy = x.parmHasCanopy == NoYes.Yes ? true : false,
                                   HasCDShuffle = x.parmHasCDShuttle == NoYes.Yes ? true : false,
                                   HasJack = x.parmHasJack == NoYes.Yes ? true : false,
                                   HasKey = x.parmHasKey == NoYes.Yes ? true : false,
                                   HasMags = x.parmHasMaps == NoYes.Yes ? true : false,
                                   HasNavigation = x.parmHasNavigation == NoYes.Yes ? true : false,
                                   HasRadio = x.parmHasRadio == NoYes.Yes ? true : false,
                                   HasServicesBook = x.parmHasServicesBook == NoYes.Yes ? true : false,
                                   HasSpareKeys = x.parmHasSparekey == NoYes.Yes ? true : false,
                                   HasSpareTyre = x.parmHasSpareType == NoYes.Yes ? true : false,
                                   HasTools = x.parmHasTools == NoYes.Yes ? true : false,
                                   HasTrackingUnit = x.parmHasTrackingUnit == NoYes.Yes ? true : false,
                                   IsOthers = x.parmHasOthers == NoYes.Yes ? true : false,
                                   IsAirconDmg = x.parmIsDamagedAircon == NoYes.Yes ? true : false,
                                   IsAlarmDmg = x.parmIsDamagedAlarm == NoYes.Yes ? true : false,
                                   IsCanopyDmg = x.parmIsDamagedCanopy == NoYes.Yes ? true : false,
                                   IsCDShuffleDmg = x.parmIsDamagedCDShuttle == NoYes.Yes ? true : false,
                                   IsJackDmg = x.parmIsDamagedJack == NoYes.Yes ? true : false,
                                   IsKeyDmg = x.parmIsDamagedKey == NoYes.Yes ? true : false,
                                   IsMagsDmg = x.parmIsDamagedMaps == NoYes.Yes ? true : false,
                                   IsNavigationDmg = x.parmIsDamagedNavigation == NoYes.Yes ? true : false,
                                   IsRadioDmg = x.parmIsDamagedRadio == NoYes.Yes ? true : false,
                                   IsServicesBookDmg = x.parmIsDamagedServicesBook == NoYes.Yes ? true : false,
                                   IsSpareKeysDmg = x.parmIsDamagedSparekey == NoYes.Yes ? true : false,
                                   IsSpareTyreDmg = x.parmIsDamagedSpareType == NoYes.Yes ? true : false,
                                   IsToolsDmg = x.parmIsDamagedTools == NoYes.Yes ? true : false,
                                   IsTrackingUnitDmg = x.parmIsDamagedTrackingUnit == NoYes.Yes ? true : false,
                                   AirconComment = x.parmAirconComments,
                                   AlarmComment = x.parmAlarmComments,
                                   CanopyComment = x.parmCanopyComments,
                                   CDShuffleComment = x.parmCDShuttleComments,
                                   JackComment = x.parmJackComments,
                                   KeyComment = x.parmKeyComments,
                                   MagsComment = x.parmMapsComments,
                                   NavigationComment = x.parmNavigationComments,
                                   RadioComment = x.parmRadioComments,
                                   ServicesBookComment = x.parmServiceBooksComments,
                                   SpareKeysComment = x.parmSpareKeysComments,
                                   SpareTyreComment = x.parmSpareTypeComments,
                                   ToolsComment = x.parmToolComments,
                                   TrackingUnitComment = x.parmTrackLineComments,
                                   OthersComment = x.parmOtherComments,
                                   VehicleInsRecID = x.parmVehicleInsRecID,
                                   TableId = x.parmTableId,
                                   RecID = x.parmRecID,

                               }));
                    await SqliteHelper.Storage.UpdateAllAsync<PAccessories>(pAccessoriesList);
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
        async private System.Threading.Tasks.Task EditPassengerBodyworkToSvcAsync()
        {
            try
            {
                if (_userInfo == null)
                {
                    _userInfo =  JsonConvert.DeserializeObject<UserInfo>(ApplicationData.Current.RoamingSettings.Values[Constants.UserInfo].ToString());
                }
                var pBodyworkData = (await SqliteHelper.Storage.LoadTableAsync<Eqstra.BusinessLogic.Passenger.PBodywork>()).Where(x => x.ShouldSave);
                ObservableCollection<MzkMobiPassengerBodyworkContract> mzkMobiPassengerBodyworkContractColl = new ObservableCollection<MzkMobiPassengerBodyworkContract>();
                if (pBodyworkData != null)
                {
                    pBodyworkData.AsParallel().ForAll(pBodywork =>
                           {

                               mzkMobiPassengerBodyworkContractColl.Add(new MzkMobiPassengerBodyworkContract()
                               {
                                   parmIsDamagedBonnet = pBodywork.IsBonnetDmg ? NoYes.Yes : NoYes.No,
                                   parmIsDamagedDoorHandles = pBodywork.IsDoorHandleDmg ? NoYes.Yes : NoYes.No,
                                   parmIsDamagedHailDamage = pBodywork.IsHailDmg ? NoYes.Yes : NoYes.No,
                                   parmIsDamagedHubcaps = pBodywork.IsHubcapsDmg ? NoYes.Yes : NoYes.No,
                                   parmIsDamagedLFBumper = pBodywork.IsLFBumperDmg ? NoYes.Yes : NoYes.No,
                                   parmIsDamagedLFDoor = pBodywork.IsLFDoorDmg ? NoYes.Yes : NoYes.No,
                                   parmIsDamagedLFWheelArch = pBodywork.IsLFWheelArchDmg ? NoYes.Yes : NoYes.No,
                                   parmIsDamagedLRBumper = pBodywork.IsLRBumperDmg ? NoYes.Yes : NoYes.No,
                                   parmIsDamagedLRDoor = pBodywork.IsLRDoorDmg ? NoYes.Yes : NoYes.No,
                                   parmIsDamagedLRWheelArch = pBodywork.IsLRWheelArchDmg ? NoYes.Yes : NoYes.No,
                                   parmIsDamagedLeftSide = pBodywork.IsLeftSideDmg ? NoYes.Yes : NoYes.No,
                                   parmIsDamagedRFBumper = pBodywork.IsRFBumperDmg ? NoYes.Yes : NoYes.No,
                                   parmIsDamagedRFDoor = pBodywork.IsRFDoorDmg ? NoYes.Yes : NoYes.No,
                                   parmIsDamagedRFWheelArch = pBodywork.IsRFWheelArchDmg ? NoYes.Yes : NoYes.No,
                                   parmIsDamagedRRBumper = pBodywork.IsRRBumperDmg ? NoYes.Yes : NoYes.No,
                                   parmIsDamagedRRDoor = pBodywork.IsRRDoorDmg ? NoYes.Yes : NoYes.No,
                                   parmIsDamagedRRWheelArch = pBodywork.IsRRWheelArchDmg ? NoYes.Yes : NoYes.No,
                                   parmIsDamagedRightSide = pBodywork.IsRightSideDmg ? NoYes.Yes : NoYes.No,
                                   parmIsDamagedRoof = pBodywork.IsRoofDmg ? NoYes.Yes : NoYes.No,
                                   parmIsDamagedWipers = pBodywork.IsWipersDmg ? NoYes.Yes : NoYes.No,

                                   parmHailDamageComments = pBodywork.GVHailDamageComment,
                                   parmHubcapsComments = pBodywork.GVHubcapsComment,
                                   parmLFDoorComments = pBodywork.GVLFDoorComment,
                                   parmLRBumperComments = pBodywork.GVLRBumperComment,
                                   parmLRDoorComments = pBodywork.GVLRDoorComment,
                                   parmLRWheelArchComments = pBodywork.GVLRWheelArchComment,
                                   parmRFBumperComments = pBodywork.GVRFBumperComment,
                                   parmRFDoorComments = pBodywork.GVRFDoorComment,
                                   parmRFWheelArchComments = pBodywork.GVRFWheelArchComment,
                                   parmRRBumperComments = pBodywork.GVRRBumperComment,
                                   parmRRDoorComments = pBodywork.GVRRDoorComment,
                                   parmRRWheelArchComments = pBodywork.GVRRWheelArchComment,
                                   parmRightSideComments = pBodywork.GVRightSideComment,
                                   parmRoofComments = pBodywork.GVRoofComment,
                                   parmWipersComments = pBodywork.GVWipersComment,

                                   parmVehicleInsRecID = pBodywork.VehicleInsRecID,
                                   parmTableId = pBodywork.TableId,
                                   parmRecID = pBodywork.RecID,

                               });
                           });
                }
                var res = await client.editPassengerBodyworkAsync(mzkMobiPassengerBodyworkContractColl, _userInfo.CompanyId);

                var pBodyworkList = new ObservableCollection<PBodywork>();

                if (res.response != null)
                {
                    res.response.AsParallel().ForAll(x =>
                               pBodyworkList.Add(new PBodywork
                               {
                                   IsBonnetDmg = x.parmIsDamagedBonnet == NoYes.Yes ? true : false,
                                   IsDoorHandleDmg = x.parmIsDamagedDoorHandles == NoYes.Yes ? true : false,
                                   IsHailDmg = x.parmIsDamagedHailDamage == NoYes.Yes ? true : false,
                                   IsHubcapsDmg = x.parmIsDamagedHubcaps == NoYes.Yes ? true : false,
                                   IsLFBumperDmg = x.parmIsDamagedLFBumper == NoYes.Yes ? true : false,
                                   IsLFDoorDmg = x.parmIsDamagedLFDoor == NoYes.Yes ? true : false,
                                   IsLFWheelArchDmg = x.parmIsDamagedLFWheelArch == NoYes.Yes ? true : false,
                                   IsLRBumperDmg = x.parmIsDamagedLRBumper == NoYes.Yes ? true : false,
                                   IsLRDoorDmg = x.parmIsDamagedLRDoor == NoYes.Yes ? true : false,
                                   IsLRWheelArchDmg = x.parmIsDamagedLRWheelArch == NoYes.Yes ? true : false,
                                   IsLeftSideDmg = x.parmIsDamagedLeftSide == NoYes.Yes ? true : false,
                                   IsRFBumperDmg = x.parmIsDamagedRFBumper == NoYes.Yes ? true : false,
                                   IsRFDoorDmg = x.parmIsDamagedRFDoor == NoYes.Yes ? true : false,
                                   IsRFWheelArchDmg = x.parmIsDamagedRFWheelArch == NoYes.Yes ? true : false,
                                   IsRRBumperDmg = x.parmIsDamagedRRBumper == NoYes.Yes ? true : false,
                                   IsRRDoorDmg = x.parmIsDamagedRRDoor == NoYes.Yes ? true : false,
                                   IsRRWheelArchDmg = x.parmIsDamagedRRWheelArch == NoYes.Yes ? true : false,
                                   IsRightSideDmg = x.parmIsDamagedRightSide == NoYes.Yes ? true : false,
                                   IsRoofDmg = x.parmIsDamagedRoof == NoYes.Yes ? true : false,
                                   IsWipersDmg = x.parmIsDamagedWipers == NoYes.Yes ? true : false,
                                   GVHailDamageComment = x.parmHailDamageComments,
                                   GVHubcapsComment = x.parmHubcapsComments,
                                   GVLFDoorComment = x.parmLFDoorComments,
                                   GVLRBumperComment = x.parmLRBumperComments,
                                   GVLRDoorComment = x.parmLRDoorComments,
                                   GVLRWheelArchComment = x.parmLRWheelArchComments,
                                   GVRFBumperComment = x.parmRFBumperComments,
                                   GVRFDoorComment = x.parmRFDoorComments,
                                   GVRFWheelArchComment = x.parmRFWheelArchComments,
                                   GVRRBumperComment = x.parmRRBumperComments,
                                   GVRRDoorComment = x.parmRRDoorComments,
                                   GVRRWheelArchComment = x.parmRRWheelArchComments,
                                   GVRightSideComment = x.parmRightSideComments,
                                   GVRoofComment = x.parmRoofComments,
                                   GVWipersComment = x.parmWipersComments,
                                   VehicleInsRecID = x.parmVehicleInsRecID,
                                   TableId = x.parmTableId,
                                   RecID = x.parmRecID,

                               }));
                    await SqliteHelper.Storage.UpdateAllAsync<PBodywork>(pBodyworkList);
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
        async private System.Threading.Tasks.Task EditPassengerTrimInteriorToSvcAsync()
        {
            try
            {
                if (_userInfo == null)
                {
                    _userInfo =  JsonConvert.DeserializeObject<UserInfo>(ApplicationData.Current.RoamingSettings.Values[Constants.UserInfo].ToString());
                }
                var pTrimInteriorData = (await SqliteHelper.Storage.LoadTableAsync<Eqstra.BusinessLogic.Passenger.PTrimInterior>()).Where(x => x.ShouldSave);
                ObservableCollection<MzkMobiPassengerTrimInteriorContract> mzkMobiPassengerTrimInteriorContractColl = new ObservableCollection<MzkMobiPassengerTrimInteriorContract>();
                if (pTrimInteriorData != null)
                {
                    pTrimInteriorData.AsParallel().ForAll(pTrimInterior =>
                           {
                               mzkMobiPassengerTrimInteriorContractColl.Add(new MzkMobiPassengerTrimInteriorContract()
                               {
                                   parmCarpetsComments = pTrimInterior.CarpetComment,
                                   parmDashComments = pTrimInterior.DashComment,
                                   parmInternalTrimComments = pTrimInterior.InternalTrimComment,
                                   parmLFDoorTrimComments = pTrimInterior.LFDoorTrimComment,
                                   parmLRDoorTrimComments = pTrimInterior.LRDoorTrimComment,
                                   parmPassengerSeatComments = pTrimInterior.PassengerSeatComment,
                                   parmRFDoorTrimComments = pTrimInterior.RFDoorTrimComment,
                                   parmRRDoorTrimComments = pTrimInterior.RRDoorTrimComment,
                                   parmRearSeatComments = pTrimInterior.RearSeatComment,

                                   parmIsDamagedCarpets = pTrimInterior.IsCarpetDmg ? NoYes.Yes : NoYes.No,
                                   parmIsDamagedDash = pTrimInterior.IsDashDmg ? NoYes.Yes : NoYes.No,
                                   parmIsDamagedInternalTrim = pTrimInterior.IsInternalTrimDmg ? NoYes.Yes : NoYes.No,
                                   parmIsDamagedLFDoorTrim = pTrimInterior.IsLFDoorTrimDmg ? NoYes.Yes : NoYes.No,
                                   parmIsDamagedLRDoorTrim = pTrimInterior.IsLRDoorTrimDmg ? NoYes.Yes : NoYes.No,
                                   parmIsDamagedPassengerSeat = pTrimInterior.IsPassengerSeatDmg ? NoYes.Yes : NoYes.No,
                                   parmIsDamagedRFDoorTrim = pTrimInterior.IsRFDoorTrimDmg ? NoYes.Yes : NoYes.No,
                                   parmIsDamagedRRDoorTrim = pTrimInterior.IsRRDoorTrimDmg ? NoYes.Yes : NoYes.No,
                                   parmIsDamagedRearSeat = pTrimInterior.IsRearSeatDmg ? NoYes.Yes : NoYes.No,

                                   parmVehicleInsRecID = pTrimInterior.VehicleInsRecID,
                                   parmTableId = pTrimInterior.TableId,
                                   parmRecID = pTrimInterior.RecID,
                               });
                           });
                }
                var res = await client.editPassengerTrimInteriorAsync(mzkMobiPassengerTrimInteriorContractColl, _userInfo.CompanyId);

                if (res.response != null)
                {
                    var pTrimInteriorList = new ObservableCollection<PTrimInterior>();
                    res.response.AsParallel().ForAll(x =>
                        pTrimInteriorList.Add(new PTrimInterior
                        {
                            CarpetComment = x.parmCarpetsComments,
                            DashComment = x.parmDashComments,
                            InternalTrimComment = x.parmInternalTrimComments,
                            LFDoorTrimComment = x.parmLFDoorTrimComments,
                            LRDoorTrimComment = x.parmLRDoorTrimComments,
                            PassengerSeatComment = x.parmPassengerSeatComments,
                            RFDoorTrimComment = x.parmRFDoorTrimComments,
                            RRDoorTrimComment = x.parmRRDoorTrimComments,
                            RearSeatComment = x.parmRearSeatComments,
                            IsCarpetDmg = x.parmIsDamagedCarpets == NoYes.Yes ? true : false,
                            IsDashDmg = x.parmIsDamagedDash == NoYes.Yes ? true : false,
                            IsInternalTrimDmg = x.parmIsDamagedInternalTrim == NoYes.Yes ? true : false,
                            IsLFDoorTrimDmg = x.parmIsDamagedLFDoorTrim == NoYes.Yes ? true : false,
                            IsLRDoorTrimDmg = x.parmIsDamagedLRDoorTrim == NoYes.Yes ? true : false,
                            IsPassengerSeatDmg = x.parmIsDamagedPassengerSeat == NoYes.Yes ? true : false,
                            IsRFDoorTrimDmg = x.parmIsDamagedRFDoorTrim == NoYes.Yes ? true : false,
                            IsRRDoorTrimDmg = x.parmIsDamagedRRDoorTrim == NoYes.Yes ? true : false,
                            IsRearSeatDmg = x.parmIsDamagedRearSeat == NoYes.Yes ? true : false,
                            VehicleInsRecID = x.parmVehicleInsRecID,
                            TableId = x.parmTableId,
                            RecID = x.parmRecID,

                        }));
                    await SqliteHelper.Storage.UpdateAllAsync<PTrimInterior>(pTrimInteriorList);
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
        async private System.Threading.Tasks.Task EditPassengerTyreConditionToSvcAsync()
        {

            try
            {
                if (_userInfo == null)
                {
                    _userInfo =  JsonConvert.DeserializeObject<UserInfo>(ApplicationData.Current.RoamingSettings.Values[Constants.UserInfo].ToString());
                }
                var pTyreConditionData = (await SqliteHelper.Storage.LoadTableAsync<Eqstra.BusinessLogic.Passenger.PTyreCondition>()).Where(x => x.ShouldSave);
                ObservableCollection<MZKMobiPassengerTyreConditionContract> mZKMobiPassengerTyreConditionContractColl = new ObservableCollection<MZKMobiPassengerTyreConditionContract>();
                if (pTyreConditionData != null)
                {
                    pTyreConditionData.AsParallel().ForAll(pTyreCondition =>
                           {
                               mZKMobiPassengerTyreConditionContractColl.Add(new MZKMobiPassengerTyreConditionContract()
                               {
                                   parmLFComments = pTyreCondition.LFComment,
                                   parmLFMake = pTyreCondition.LFMake,
                                   parmLRComments = pTyreCondition.LRComment,
                                   parmLRMake = pTyreCondition.LRMake,
                                   parmRFComments = pTyreCondition.RFComment,
                                   parmRFMake = pTyreCondition.RFMake,
                                   parmRRComments = pTyreCondition.RRComment,
                                   parmRRMake = pTyreCondition.RRMake,
                                   parmSpareComments = pTyreCondition.SpareComment,
                                   parmSpareMake = pTyreCondition.SpareMake,
                                   parmLFDepth = pTyreCondition.LFTreadDepth,
                                   parmLRDepth = pTyreCondition.LRTreadDepth,
                                   parmRFDepth = pTyreCondition.RFTreadDepth,
                                   parmRRDepth = pTyreCondition.RRTreadDepth,
                                   parmSpareDepth = pTyreCondition.SpareTreadDepth,

                                   parmLFCondition = pTyreCondition.IsLFFChecked ? MZKConditionEnum.Fair : (pTyreCondition.IsLFGChecked ? MZKConditionEnum.Good : (pTyreCondition.IsLFPChecked ? MZKConditionEnum.Poor : MZKConditionEnum.None)),
                                   parmLRCondition = pTyreCondition.IsLRFChecked ? MZKConditionEnum.Fair : (pTyreCondition.IsLRGChecked ? MZKConditionEnum.Good : (pTyreCondition.IsLRPChecked ? MZKConditionEnum.Poor : MZKConditionEnum.None)),
                                   parmRFCondition = pTyreCondition.IsRFFChecked ? MZKConditionEnum.Fair : (pTyreCondition.IsRFGChecked ? MZKConditionEnum.Good : (pTyreCondition.IsRFPChecked ? MZKConditionEnum.Poor : MZKConditionEnum.None)),
                                   parmRRCondition = pTyreCondition.IsRRFChecked ? MZKConditionEnum.Fair : (pTyreCondition.IsRRGChecked ? MZKConditionEnum.Good : (pTyreCondition.IsRRPChecked ? MZKConditionEnum.Poor : MZKConditionEnum.None)),
                                   parmSpareCondition = pTyreCondition.IsSpareFChecked ? MZKConditionEnum.Fair : (pTyreCondition.IsSpareGChecked ? MZKConditionEnum.Good : (pTyreCondition.IsSparePChecked ? MZKConditionEnum.Poor : MZKConditionEnum.None)),

                                   parmVehicleInsRecID = pTyreCondition.VehicleInsRecID,
                                   parmTableId = pTyreCondition.TableId,
                                   parmRecID = pTyreCondition.RecID,
                               });
                           });
                }
                var res = await client.editPassengerTyreConditionAsync(mZKMobiPassengerTyreConditionContractColl, _userInfo.CompanyId);

                var pTyreConditionList = new ObservableCollection<PTyreCondition>();
                if (res.response != null)
                {
                    res.response.AsParallel().ForAll(x =>
                               pTyreConditionList.Add(new PTyreCondition
                               {
                                   LFComment = x.parmLFComments,
                                   LFMake = x.parmLFMake,
                                   LRComment = x.parmLRComments,
                                   LRMake = x.parmLRMake,
                                   RFComment = x.parmRFComments,
                                   RFMake = x.parmRFMake,
                                   RRComment = x.parmRRComments,
                                   RRMake = x.parmRRMake,
                                   SpareComment = x.parmSpareComments,
                                   SpareMake = x.parmSpareMake,
                                   LFTreadDepth = x.parmLFDepth,
                                   LRTreadDepth = x.parmLRDepth,
                                   RFTreadDepth = x.parmRFDepth,
                                   RRTreadDepth = x.parmRRDepth,
                                   SpareTreadDepth = x.parmSpareDepth,
                                   VehicleInsRecID = x.parmVehicleInsRecID,
                                   RecID = x.parmRecID,
                                   TableId = x.parmTableId,
                                   IsLFFChecked = MZKConditionEnum.Fair == x.parmLFCondition ? true : false,
                                   IsLFGChecked = MZKConditionEnum.Good == x.parmLFCondition ? true : false,
                                   IsLFPChecked = MZKConditionEnum.Poor == x.parmLFCondition ? true : false,
                                   IsLRFChecked = MZKConditionEnum.Fair == x.parmLRCondition ? true : false,
                                   IsLRGChecked = MZKConditionEnum.Good == x.parmLRCondition ? true : false,
                                   IsLRPChecked = MZKConditionEnum.Poor == x.parmLRCondition ? true : false,
                                   IsRFFChecked = MZKConditionEnum.Fair == x.parmRFCondition ? true : false,
                                   IsRFGChecked = MZKConditionEnum.Good == x.parmRFCondition ? true : false,
                                   IsRFPChecked = MZKConditionEnum.Poor == x.parmRFCondition ? true : false,
                                   IsRRFChecked = MZKConditionEnum.Fair == x.parmRRCondition ? true : false,
                                   IsRRGChecked = MZKConditionEnum.Good == x.parmRRCondition ? true : false,
                                   IsRRPChecked = MZKConditionEnum.Poor == x.parmRRCondition ? true : false,
                                   IsSpareFChecked = MZKConditionEnum.Fair == x.parmSpareCondition ? true : false,
                                   IsSpareGChecked = MZKConditionEnum.Good == x.parmSpareCondition ? true : false,
                                   IsSparePChecked = MZKConditionEnum.Poor == x.parmSpareCondition ? true : false,

                               }));
                    await SqliteHelper.Storage.UpdateAllAsync<PTyreCondition>(pTyreConditionList);
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
        async private System.Threading.Tasks.Task EditPassengerGlassToSvcAsync()
        {

            try
            {
                if (_userInfo == null)
                {
                    _userInfo = JsonConvert.DeserializeObject<UserInfo>(ApplicationData.Current.RoamingSettings.Values[Constants.UserInfo].ToString());
                }
                var pGlassData = (await SqliteHelper.Storage.LoadTableAsync<Eqstra.BusinessLogic.Passenger.PGlass>()).Where(x => x.ShouldSave);

                ObservableCollection<MzkMobiPassengerGlassContract> mzkMobiPassengerGlassContractColl = new ObservableCollection<MzkMobiPassengerGlassContract>();
                if (pGlassData != null)
                {
                    pGlassData.AsParallel().ForAll(pglass =>
                mzkMobiPassengerGlassContractColl.Add(new MzkMobiPassengerGlassContract()
                {
                    parmExitRearViewMirrorComments = pglass.GVExtRearViewMirrorComment,
                    parmHeadLightsComments = pglass.GVHeadLightsComment,
                    parmIndicatorLensesComments = pglass.GVInductorLensesComment,
                    parmRearGlassComments = pglass.GVRearGlassComment,
                    parmSideGlassComments = pglass.GVSideGlassComment,
                    parmTailLightsComments = pglass.GVTailLightsComment,
                    parmWindScreenComments = pglass.GVWindscreenComment,

                    parmIsDamagedExitRearViewMirror = pglass.IsExtRearViewMirror ? NoYes.Yes : NoYes.No,
                    parmIsDamagedHeadLights = pglass.IsHeadLights ? NoYes.Yes : NoYes.No,
                    parmIsDamagedIndicatorLenses = pglass.IsInductorLenses ? NoYes.Yes : NoYes.No,
                    parmVehicleType = MzkVehicleType.Passenger,
                    parmIsDamagedRearGlass = pglass.IsRearGlass ? NoYes.Yes : NoYes.No,
                    parmIsDamagedSideGlass = pglass.IsSideGlass ? NoYes.Yes : NoYes.No,
                    parmIsDamagedTailLights = pglass.IsTailLights ? NoYes.Yes : NoYes.No,
                    parmIsDamagedWindScreen = pglass.IsWindscreen ? NoYes.Yes : NoYes.No,
                    parmRecID = pglass.RecID,
                    parmVehicleInsRecID = pglass.VehicleInsRecID,
                    parmTableId = pglass.TableId,
                })

            );
                }
                var res = await client.editGlassAsync(mzkMobiPassengerGlassContractColl, _userInfo.CompanyId);

                var glassList = new ObservableCollection<PGlass>();
                if (res.response != null)
                {
                    res.response.AsParallel().ForAll(x =>
                              glassList.Add(new PGlass
                              {
                                  GVExtRearViewMirrorComment = x.parmExitRearViewMirrorComments,
                                  GVHeadLightsComment = x.parmHeadLightsComments,
                                  GVInductorLensesComment = x.parmIndicatorLensesComments,
                                  GVRearGlassComment = x.parmRearGlassComments,
                                  GVSideGlassComment = x.parmSideGlassComments,
                                  GVTailLightsComment = x.parmTailLightsComments,
                                  GVWindscreenComment = x.parmWindScreenComments,
                                  IsExtRearViewMirror = x.parmIsDamagedExitRearViewMirror == NoYes.Yes ? true : false,
                                  IsHeadLights = x.parmIsDamagedHeadLights == NoYes.Yes ? true : false,
                                  IsInductorLenses = x.parmIsDamagedIndicatorLenses == NoYes.Yes ? true : false,
                                  IsRearGlass = x.parmIsDamagedRearGlass == NoYes.Yes ? true : false,
                                  IsSideGlass = x.parmIsDamagedSideGlass == NoYes.Yes ? true : false,
                                  IsTailLights = x.parmIsDamagedTailLights == NoYes.Yes ? true : false,
                                  IsWindscreen = x.parmIsDamagedWindScreen == NoYes.Yes ? true : false,
                                  RecID = x.parmRecID,
                                  VehicleInsRecID = x.parmVehicleInsRecID,
                                  TableId = x.parmTableId,

                              }));
                    await SqliteHelper.Storage.UpdateAllAsync<PGlass>(glassList);
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

        async private System.Threading.Tasks.Task EditPassengerMechanicalConditionAsync()
        {
            if (_userInfo == null)
            {
                _userInfo = JsonConvert.DeserializeObject<UserInfo>(ApplicationData.Current.RoamingSettings.Values[Constants.UserInfo].ToString());
            }
            var machTable = await SqliteHelper.Storage.LoadTableAsync<PMechanicalCond>();

            var mechConditionContract = new ObservableCollection<MzkMobiPassengerMechConditionContract>();
            machTable.AsParallel().ForAll(m =>
            mechConditionContract.Add(
               new MzkMobiPassengerMechConditionContract
               {
                   parmVehicleInsRecID = m.VehicleInsRecID,
                   parmRecID = m.RecID,
                   parmRemarks = m.Remarks
               }
           ));
            var res = await client.editPassengerMechConditionAsync(mechConditionContract, _userInfo.CompanyId);
            var pMechConditionColl = new ObservableCollection<PMechanicalCond>();
            if (res.response != null)
            {
                foreach (var result in res.response.Where(x => x.parmRecID == 0))
                {
                    await SqliteHelper.Storage.InsertSingleRecordAsync<PMechanicalCond>(new PMechanicalCond { Remarks = result.parmRemarks, RecID = result.parmRecID, VehicleInsRecID = result.parmVehicleInsRecID });
                }
                foreach (var result in res.response.Where(x => x.parmRecID != 0))
                {
                    await SqliteHelper.Storage.UpdateSingleRecordAsync<PMechanicalCond>(new PMechanicalCond { RecID = result.parmRecID, Remarks = result.parmRemarks, VehicleInsRecID = result.parmVehicleInsRecID });
                }
            }
        }
        async private System.Threading.Tasks.Task EditPassengerInspectionProofToSvcAsync()
        {
            try
            {
                if (_userInfo == null)
                {
                    _userInfo =  JsonConvert.DeserializeObject<UserInfo>(ApplicationData.Current.RoamingSettings.Values[Constants.UserInfo].ToString());
                }
                var inspectionProofData = (await SqliteHelper.Storage.LoadTableAsync<Eqstra.BusinessLogic.Passenger.PInspectionProof>()).Where(x => x.ShouldSave);
                ObservableCollection<MZKVehicleInspectionTableContract> MZKVehicleInspectionTableContractColl = new ObservableCollection<MZKVehicleInspectionTableContract>();
                if (inspectionProofData != null)
                {
                    inspectionProofData.AsParallel().ForAll(insProof =>
             {
                 MZKVehicleInspectionTableContractColl.Add(new MZKVehicleInspectionTableContract()
                 {
                     parmCustomerComments = insProof.CRSignComment,
                     parmComments = insProof.EQRSignComment,
                     parmCompanyRepSignDateTime = insProof.EQRDate,
                     parmCustomerRepSignDateTime = insProof.CRDate,
                     parmRecID = insProof.VehicleInsRecID
                 });
             });
                }
                var res = await client.editVehicleInspectionAsync(MZKVehicleInspectionTableContractColl);

                var inspectionProofList = new ObservableCollection<PInspectionProof>();
                if (res.response != null)
                {
                    res.response.AsParallel().ForAll(x =>
                               inspectionProofList.Add(new PInspectionProof
                               {
                                   EQRDate = x.parmCompanyRepSignDateTime,
                                   CRDate = x.parmCustomerRepSignDateTime,
                                   CRSignComment = x.parmComments,
                                   VehicleInsRecID = x.parmRecID
                               }));
                }
                await SqliteHelper.Storage.UpdateAllAsync<PInspectionProof>(inspectionProofList);
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
        async private System.Threading.Tasks.Task EditCommercialVehicleDetailsToSvcAsync()
        {
            try
            {
                if (_userInfo == null)
                {
                    _userInfo =  JsonConvert.DeserializeObject<UserInfo>(ApplicationData.Current.RoamingSettings.Values[Constants.UserInfo].ToString());
                }
                ObservableCollection<MzkVehicleDetailsContract> mzkVehicleDetailsContractColl = new ObservableCollection<MzkVehicleDetailsContract>();
                var cVehicleDetailsData = (await SqliteHelper.Storage.LoadTableAsync<Eqstra.BusinessLogic.Commercial.CVehicleDetails>()).Where(x => x.ShouldSave);
                if (cVehicleDetailsData != null)
                {
                    cVehicleDetailsData.AsParallel().ForAll(cVehicleDetails =>
                           {
                               mzkVehicleDetailsContractColl.Add(new MzkVehicleDetailsContract()
                               {
                                   parmLisenceDiscCurrent = cVehicleDetails.IsLicenseDiscCurrent ? NoYes.Yes : NoYes.No,
                                   parmlisenceDiscExpiryDate = cVehicleDetails.LicenseDiscExpireDate,
                                   parmRecID = cVehicleDetails.RecID,
                                   parmRegNo = cVehicleDetails.RegistrationNumber,
                                   parmSparseKeyShown = cVehicleDetails.IsSpareKeysShown ? NoYes.Yes : NoYes.No,
                                   parmSparseKeyTested = cVehicleDetails.IsSpareKeysTested ? NoYes.Yes : NoYes.No,
                                   parmTableId = cVehicleDetails.TableId,
                                   parmVehicleInsRecID = cVehicleDetails.VehicleInsRecID
                               });
                           });
                }
                var resp = await client.editVehicleDetailsAsync(mzkVehicleDetailsContractColl, _userInfo.CompanyId);

                var cVehicleDetailsList = new ObservableCollection<CVehicleDetails>();
                if (resp.response != null)
                {
                    resp.response.AsParallel().ForAll(x =>
                               cVehicleDetailsList.Add(new CVehicleDetails
                               {
                                   IsLicenseDiscCurrent = x.parmLisenceDiscCurrent == NoYes.Yes ? true : false,
                                   LicenseDiscExpireDate = x.parmlisenceDiscExpiryDate,
                                   RecID = x.parmRecID,
                                   RegistrationNumber = x.parmRegNo,
                                   IsSpareKeysShown = x.parmSparseKeyShown == NoYes.Yes ? true : false,
                                   IsSpareKeysTested = x.parmSparseKeyTested == NoYes.Yes ? true : false,
                                   TableId = x.parmTableId,
                                   VehicleInsRecID = x.parmVehicleInsRecID

                               }));
                }
                await SqliteHelper.Storage.UpdateAllAsync<CVehicleDetails>(cVehicleDetailsList);
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
        async private System.Threading.Tasks.Task EditCommercialGlassToSvcAsync()
        {

            try
            {
                if (_userInfo == null)
                {
                    _userInfo =  JsonConvert.DeserializeObject<UserInfo>(ApplicationData.Current.RoamingSettings.Values[Constants.UserInfo].ToString());
                }
                var cGlassData = (await SqliteHelper.Storage.LoadTableAsync<Eqstra.BusinessLogic.Commercial.CGlass>()).Where(x => x.ShouldSave);
                ObservableCollection<MzkMobiPassengerGlassContract> mzkMobiCommercialGlassContractColl = new ObservableCollection<MzkMobiPassengerGlassContract>();
                if (cGlassData != null)
                {
                    cGlassData.AsParallel().ForAll(cglass =>
                               mzkMobiCommercialGlassContractColl.Add(new MzkMobiPassengerGlassContract()
                               {
                                   parmExitRearViewMirrorComments = cglass.ExtRearViewMirrorComment,
                                   parmHeadLightsComments = cglass.HeadLightsComment,
                                   parmIndicatorLensesComments = cglass.InductorLensesComment,
                                   parmRearGlassComments = cglass.RearGlassComment,
                                   parmSideGlassComments = cglass.SideGlassComment,
                                   parmTailLightsComments = cglass.TailLightsComment,
                                   parmWindScreenComments = cglass.WindscreenComment,

                                   parmIsDamagedExitRearViewMirror = cglass.IsRearGlassDmg ? NoYes.Yes : NoYes.No,
                                   parmIsDamagedHeadLights = cglass.IsHeadLightsDmg ? NoYes.Yes : NoYes.No,
                                   parmIsDamagedIndicatorLenses = cglass.IsInductorLensesDmg ? NoYes.Yes : NoYes.No,
                                   parmVehicleType = MzkVehicleType.Commercial,
                                   parmIsDamagedRearGlass = cglass.IsRearGlassDmg ? NoYes.Yes : NoYes.No,
                                   parmIsDamagedSideGlass = cglass.IsSideGlassDmg ? NoYes.Yes : NoYes.No,
                                   parmIsDamagedTailLights = cglass.IsTailLightsDmg ? NoYes.Yes : NoYes.No,
                                   parmIsDamagedWindScreen = cglass.IsWindscreenDmg ? NoYes.Yes : NoYes.No,
                                   parmRecID = cglass.RecID,
                                   parmVehicleInsRecID = cglass.VehicleInsRecID,
                                   parmTableId = cglass.TableId,
                               })

                           );
                }
                var res = await client.editGlassAsync(mzkMobiCommercialGlassContractColl, _userInfo.CompanyId);

                var glassList = new ObservableCollection<CGlass>();
                if (res.response != null)
                {
                    res.response.AsParallel().ForAll(x =>
                               glassList.Add(new CGlass
                               {
                                   ExtRearViewMirrorComment = x.parmExitRearViewMirrorComments,
                                   HeadLightsComment = x.parmHeadLightsComments,
                                   InductorLensesComment = x.parmIndicatorLensesComments,
                                   RearGlassComment = x.parmRearGlassComments,
                                   SideGlassComment = x.parmSideGlassComments,
                                   TailLightsComment = x.parmTailLightsComments,
                                   WindscreenComment = x.parmWindScreenComments,
                                   IsExtRearViewMirrorDmg = x.parmIsDamagedExitRearViewMirror == NoYes.Yes ? true : false,
                                   IsHeadLightsDmg = x.parmIsDamagedHeadLights == NoYes.Yes ? true : false,
                                   IsInductorLensesDmg = x.parmIsDamagedIndicatorLenses == NoYes.Yes ? true : false,
                                   IsRearGlassDmg = x.parmIsDamagedRearGlass == NoYes.Yes ? true : false,
                                   IsSideGlassDmg = x.parmIsDamagedSideGlass == NoYes.Yes ? true : false,
                                   IsTailLightsDmg = x.parmIsDamagedTailLights == NoYes.Yes ? true : false,
                                   IsWindscreenDmg = x.parmIsDamagedWindScreen == NoYes.Yes ? true : false,
                                   RecID = x.parmRecID,
                                   VehicleInsRecID = x.parmVehicleInsRecID,
                                   TableId = x.parmTableId,

                               }));
                    await SqliteHelper.Storage.UpdateAllAsync<CGlass>(glassList);
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
        async private System.Threading.Tasks.Task EditCommercialAccessoriesToSvcAsync()
        {
            try
            {
                if (_userInfo == null)
                {
                    _userInfo =  JsonConvert.DeserializeObject<UserInfo>(ApplicationData.Current.RoamingSettings.Values[Constants.UserInfo].ToString());
                }
                var cAccessoriesData = (await SqliteHelper.Storage.LoadTableAsync<Eqstra.BusinessLogic.Commercial.CAccessories>()).Where(x => x.ShouldSave);
                ObservableCollection<MzkMobiCommercialAccessoriesContract> mzkMobiCommercialAccessoriesContractColl = new ObservableCollection<MzkMobiCommercialAccessoriesContract>();
                if (cAccessoriesData != null)
                {
                    cAccessoriesData.AsParallel().ForAll(cAccessories =>
                              {
                                  mzkMobiCommercialAccessoriesContractColl.Add(new MzkMobiCommercialAccessoriesContract()
                                  {
                                      parmBullbarComments = cAccessories.BullBarComment,
                                      parmEngineProtectionUnitComments = cAccessories.EngineProtectionUnitComment,
                                      parmJackComments = cAccessories.JackComment,
                                      parmReflectiveTapeComments = cAccessories.ReflectiveTapeComment,
                                      parmTrackingDeviceComments = cAccessories.TrackingDeviceComment,
                                      parmToolComments = cAccessories.ToolsComment,
                                      parmSignWritingComments = cAccessories.DecalSignWritingComment,
                                      parmServiceBooksComments = cAccessories.ServiceBlockComment,

                                      parmHasEngineProtectionUnit = cAccessories.HasEngineProtectionUnitDmg ? NoYes.Yes : NoYes.No,
                                      parmHasJack = cAccessories.HasJackDmg ? NoYes.Yes : NoYes.No,
                                      parmHasReflectiveTape = cAccessories.HasReflectiveTapeDmg ? NoYes.Yes : NoYes.No,
                                      parmHasTools = cAccessories.HasToolsDmg ? NoYes.Yes : NoYes.No,
                                      parmHasTrackingDevice = cAccessories.HasTrackingDeviceDmg ? NoYes.Yes : NoYes.No,
                                      parmHasBullbar = cAccessories.HasBullBarDmg ? NoYes.Yes : NoYes.No,
                                      parmHasDecals = cAccessories.HasDecalSignWritingDmg ? NoYes.Yes : NoYes.No,
                                      parmHasServiceBook = cAccessories.HasServiceBlockDmg ? NoYes.Yes : NoYes.No,

                                      parmIsDamagedBullbar = cAccessories.IsBullBarDmg ? NoYes.Yes : NoYes.No,
                                      parmIsDamagedEngineProtectionUnit = cAccessories.IsEngineProtectionUnitDmg ? NoYes.Yes : NoYes.No,
                                      parmIsDamagedJack = cAccessories.IsJackDmg ? NoYes.Yes : NoYes.No,
                                      parmIsDamagedReflectiveTape = cAccessories.IsReflectiveTapeDmg ? NoYes.Yes : NoYes.No,
                                      parmIsDamagedSignWriting = cAccessories.IsDecalSignWritingDmg ? NoYes.Yes : NoYes.No,
                                      parmIsDamagedTools = cAccessories.IsToolsDmg ? NoYes.Yes : NoYes.No,
                                      parmIsDamagedTrackingDevice = cAccessories.IsTrackingDeviceDmg ? NoYes.Yes : NoYes.No,
                                      parmIsDamagedServicesBook = cAccessories.IsServiceBlockDmg ? NoYes.Yes : NoYes.No,

                                      parmVehicleInsRecID = cAccessories.VehicleInsRecID,
                                      parmTableId = cAccessories.TableId,
                                      parmRecID = cAccessories.RecID,

                                  });
                              });
                }
                var res = await client.editCommercialAccessoriesAsync(mzkMobiCommercialAccessoriesContractColl, _userInfo.CompanyId);

                var cAccessoriesList = new ObservableCollection<CAccessories>();
                if (res.response != null)
                {
                    res.response.AsParallel().ForAll(x =>
                               cAccessoriesList.Add(new CAccessories
                               {
                                   BullBarComment = x.parmBullbarComments,
                                   EngineProtectionUnitComment = x.parmEngineProtectionUnitComments,
                                   JackComment = x.parmJackComments,
                                   ReflectiveTapeComment = x.parmReflectiveTapeComments,
                                   TrackingDeviceComment = x.parmTrackingDeviceComments,
                                   ToolsComment = x.parmToolComments,
                                   DecalSignWritingComment = x.parmSignWritingComments,
                                   ServiceBlockComment = x.parmServiceBooksComments,
                                   HasEngineProtectionUnitDmg = x.parmHasEngineProtectionUnit == NoYes.Yes ? true : false,
                                   HasJackDmg = x.parmHasJack == NoYes.Yes ? true : false,
                                   HasReflectiveTapeDmg = x.parmHasReflectiveTape == NoYes.Yes ? true : false,
                                   HasToolsDmg = x.parmHasTools == NoYes.Yes ? true : false,
                                   HasTrackingDeviceDmg = x.parmHasTrackingDevice == NoYes.Yes ? true : false,
                                   HasBullBarDmg = x.parmHasBullbar == NoYes.Yes ? true : false,
                                   HasDecalSignWritingDmg = x.parmHasDecals == NoYes.Yes ? true : false,
                                   HasServiceBlockDmg = x.parmHasServiceBook == NoYes.Yes ? true : false,
                                   IsBullBarDmg = x.parmIsDamagedBullbar == NoYes.Yes ? true : false,
                                   IsEngineProtectionUnitDmg = x.parmIsDamagedEngineProtectionUnit == NoYes.Yes ? true : false,
                                   IsJackDmg = x.parmIsDamagedJack == NoYes.Yes ? true : false,
                                   IsReflectiveTapeDmg = x.parmIsDamagedReflectiveTape == NoYes.Yes ? true : false,
                                   IsDecalSignWritingDmg = x.parmIsDamagedSignWriting == NoYes.Yes ? true : false,
                                   IsToolsDmg = x.parmIsDamagedTools == NoYes.Yes ? true : false,
                                   IsTrackingDeviceDmg = x.parmIsDamagedTrackingDevice == NoYes.Yes ? true : false,
                                   IsServiceBlockDmg = x.parmIsDamagedServicesBook == NoYes.Yes ? true : false,
                                   VehicleInsRecID = x.parmVehicleInsRecID,
                                   TableId = x.parmTableId,
                                   RecID = x.parmRecID,

                               }));
                    await SqliteHelper.Storage.UpdateAllAsync<CAccessories>(cAccessoriesList);
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
        async private System.Threading.Tasks.Task EditCommercialTrimInteriorToSvcAsync()
        {
            try
            {
                if (_userInfo == null)
                {
                    _userInfo =  JsonConvert.DeserializeObject<UserInfo>(ApplicationData.Current.RoamingSettings.Values[Constants.UserInfo].ToString());
                }
                var cCabTrimInterData = (await SqliteHelper.Storage.LoadTableAsync<Eqstra.BusinessLogic.Commercial.CCabTrimInter>()).Where(x => x.ShouldSave);
                ObservableCollection<MZKMobiCommercialTrimInteriorContract> mZKMobiCommercialTrimInteriorContractColl = new ObservableCollection<MZKMobiCommercialTrimInteriorContract>();
                if (cCabTrimInterData != null)
                {
                    cCabTrimInterData.AsParallel().ForAll(cCabTrimInter =>
                                      {
                                          mZKMobiCommercialTrimInteriorContractColl.Add(new MZKMobiCommercialTrimInteriorContract()
                                          {
                                              parmBumperComments = cCabTrimInter.BumperComment,
                                              parmDoorHandleLeftComments = cCabTrimInter.DoorHandleLeftComment,
                                              parmDoorHandleRightComments = cCabTrimInter.DoorHandleRightComment,
                                              parmDriverSeatComments = cCabTrimInter.DriverSeatComment,
                                              parmFrontViewComments = cCabTrimInter.FrontViewComment,
                                              parmGrillComments = cCabTrimInter.GrillComment,
                                              parmInternalTriesComments = cCabTrimInter.InternalTrimComment,
                                              parmLeftDoorComments = cCabTrimInter.LeftDoorComment,
                                              parmLFQuarterPanelComments = cCabTrimInter.LFQuatPanelComment,
                                              parmLRQuarterPanelComments = cCabTrimInter.LRQuatPanelComment,
                                              parmMatsComments = cCabTrimInter.MatsComment,
                                              parmPassengerSeatComments = cCabTrimInter.PassengerSeatComment,
                                              parmRearViewMirrorComments = cCabTrimInter.RearMirrorComment,
                                              parmRFQuarterPanelComments = cCabTrimInter.RFQuatPanelComment,
                                              parmRightDoorComments = cCabTrimInter.RightDoorComment,
                                              parmRoofComments = cCabTrimInter.RoofComment,
                                              parmRRQuarterPanelComments = cCabTrimInter.RRQuatPanelComment,
                                              parmWheelArchesLeftComments = cCabTrimInter.WheelArchLeftComment,
                                              parmWheelArchesRightComments = cCabTrimInter.WheelArchRightComment,
                                              parmWipersComments = cCabTrimInter.WipersComment,

                                              parmIsDamagedBumper = cCabTrimInter.IsBumperDmg ? NoYes.Yes : NoYes.No,
                                              parmIsDamagedDoorHandleLeft = cCabTrimInter.IsDoorHandleLeftDmg ? NoYes.Yes : NoYes.No,
                                              parmIsDamagedDoorHandleRight = cCabTrimInter.IsDoorHandleRightDmg ? NoYes.Yes : NoYes.No,
                                              parmIsDamagedGrill = cCabTrimInter.IsGrillDmg ? NoYes.Yes : NoYes.No,
                                              parmIsDamagedLeftDoor = cCabTrimInter.IsLeftDoorDmg ? NoYes.Yes : NoYes.No,
                                              parmIsDamagedMats = cCabTrimInter.IsMatsDmg ? NoYes.Yes : NoYes.No,
                                              parmIsDamagedPassengerSeat = cCabTrimInter.IsPassengerSeatDmg ? NoYes.Yes : NoYes.No,
                                              parmIsDamagedRightDoor = cCabTrimInter.IsRightDoorDmg ? NoYes.Yes : NoYes.No,
                                              parmIsDamagedRoof = cCabTrimInter.IsRoofDmg ? NoYes.Yes : NoYes.No,
                                              parmIsDamagedWipers = cCabTrimInter.IsWipersDmg ? NoYes.Yes : NoYes.No,
                                              parmIsDamagedDriversSeat = cCabTrimInter.IsDriverSeatDmg ? NoYes.Yes : NoYes.No,
                                              parmIsDamagedFrontview = cCabTrimInter.IsFrontViewDmg ? NoYes.Yes : NoYes.No,
                                              parmIsDamagedInternalTries = cCabTrimInter.IsInternalTrimDmg ? NoYes.Yes : NoYes.No,
                                              parmIsDamagedLFQuarterPanel = cCabTrimInter.IsLFQuatPanelDmg ? NoYes.Yes : NoYes.No,
                                              parmIsDamagedLRQuarterPanel = cCabTrimInter.IsLRQuatPanelDmg ? NoYes.Yes : NoYes.No,
                                              parmIsDamagedRearViewMirror = cCabTrimInter.IsRearMirrorDmg ? NoYes.Yes : NoYes.No,
                                              parmIsDamagedRFQuarterPanel = cCabTrimInter.IsRFQuatPanelDmg ? NoYes.Yes : NoYes.No,
                                              parmIsDamagedRRQuarterPanel = cCabTrimInter.IsRRQuatPanelDmg ? NoYes.Yes : NoYes.No,
                                              parmIsDamagedWheelArchesLeft = cCabTrimInter.IsWheelArchLeftDmg ? NoYes.Yes : NoYes.No,
                                              parmIsDamagedWheelArchesRight = cCabTrimInter.IsWheelArchRightDmg ? NoYes.Yes : NoYes.No,

                                              parmVehicleInsRecID = cCabTrimInter.VehicleInsRecID,
                                              parmTableId = cCabTrimInter.TableId,
                                              parmRecID = cCabTrimInter.RecID,

                                          });
                                      });
                }

                var res = await client.editCommercialTrimInteriorAsync(mZKMobiCommercialTrimInteriorContractColl, _userInfo.CompanyId);

                var cCabTrimInterList = new ObservableCollection<CCabTrimInter>();
                if (res.response != null)
                {
                    res.response.AsParallel().ForAll(x =>
                               cCabTrimInterList.Add(new CCabTrimInter
                               {
                                   BumperComment = x.parmBumperComments,
                                   DoorHandleLeftComment = x.parmDoorHandleLeftComments,
                                   DoorHandleRightComment = x.parmDoorHandleRightComments,
                                   DriverSeatComment = x.parmDriverSeatComments,
                                   FrontViewComment = x.parmFrontViewComments,
                                   GrillComment = x.parmGrillComments,
                                   InternalTrimComment = x.parmInternalTriesComments,
                                   LeftDoorComment = x.parmLeftDoorComments,
                                   LFQuatPanelComment = x.parmLFQuarterPanelComments,
                                   LRQuatPanelComment = x.parmLRQuarterPanelComments,
                                   MatsComment = x.parmMatsComments,
                                   PassengerSeatComment = x.parmPassengerSeatComments,
                                   RearMirrorComment = x.parmRearViewMirrorComments,
                                   RFQuatPanelComment = x.parmRFQuarterPanelComments,
                                   RightDoorComment = x.parmRightDoorComments,
                                   RoofComment = x.parmRoofComments,
                                   RRQuatPanelComment = x.parmRRQuarterPanelComments,
                                   WheelArchLeftComment = x.parmWheelArchesLeftComments,
                                   WheelArchRightComment = x.parmWheelArchesRightComments,
                                   WipersComment = x.parmWipersComments,
                                   IsBumperDmg = x.parmIsDamagedBumper == NoYes.Yes ? true : false,
                                   IsDoorHandleLeftDmg = x.parmIsDamagedDoorHandleLeft == NoYes.Yes ? true : false,
                                   IsDoorHandleRightDmg = x.parmIsDamagedDoorHandleRight == NoYes.Yes ? true : false,
                                   IsGrillDmg = x.parmIsDamagedGrill == NoYes.Yes ? true : false,
                                   IsLeftDoorDmg = x.parmIsDamagedLeftDoor == NoYes.Yes ? true : false,
                                   IsMatsDmg = x.parmIsDamagedMats == NoYes.Yes ? true : false,
                                   IsPassengerSeatDmg = x.parmIsDamagedPassengerSeat == NoYes.Yes ? true : false,
                                   IsRightDoorDmg = x.parmIsDamagedRightDoor == NoYes.Yes ? true : false,
                                   IsRoofDmg = x.parmIsDamagedRoof == NoYes.Yes ? true : false,
                                   IsWipersDmg = x.parmIsDamagedWipers == NoYes.Yes ? true : false,
                                   IsDriverSeatDmg = x.parmIsDamagedDriversSeat == NoYes.Yes ? true : false,
                                   IsFrontViewDmg = x.parmIsDamagedFrontview == NoYes.Yes ? true : false,
                                   IsInternalTrimDmg = x.parmIsDamagedInternalTries == NoYes.Yes ? true : false,
                                   IsLFQuatPanelDmg = x.parmIsDamagedLFQuarterPanel == NoYes.Yes ? true : false,
                                   IsLRQuatPanelDmg = x.parmIsDamagedLRQuarterPanel == NoYes.Yes ? true : false,
                                   IsRearMirrorDmg = x.parmIsDamagedRearViewMirror == NoYes.Yes ? true : false,
                                   IsRFQuatPanelDmg = x.parmIsDamagedRFQuarterPanel == NoYes.Yes ? true : false,
                                   IsRRQuatPanelDmg = x.parmIsDamagedRRQuarterPanel == NoYes.Yes ? true : false,
                                   IsWheelArchLeftDmg = x.parmIsDamagedWheelArchesLeft == NoYes.Yes ? true : false,
                                   IsWheelArchRightDmg = x.parmIsDamagedWheelArchesRight == NoYes.Yes ? true : false,
                                   VehicleInsRecID = x.parmVehicleInsRecID,
                                   TableId = x.parmTableId,
                                   RecID = x.parmRecID,

                               }));
                    await SqliteHelper.Storage.UpdateAllAsync<CCabTrimInter>(cCabTrimInterList);
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
        async private System.Threading.Tasks.Task EditCommercialChassisBodyToSvcAsync()
        {
            try
            {
                if (_userInfo == null)
                {
                    _userInfo =  JsonConvert.DeserializeObject<UserInfo>(ApplicationData.Current.RoamingSettings.Values[Constants.UserInfo].ToString());
                }
                var cChassisBodyData = (await SqliteHelper.Storage.LoadTableAsync<Eqstra.BusinessLogic.Commercial.CChassisBody>()).Where(x => x.ShouldSave);
                ObservableCollection<MzkMobiCommercialChassisBodyContract> mzkMobiCommercialChassisBodyContractColl = new ObservableCollection<MzkMobiCommercialChassisBodyContract>();
                if (cChassisBodyData != null)
                {
                    cChassisBodyData.AsParallel().ForAll(cChassisBody =>
                             {
                                 mzkMobiCommercialChassisBodyContractColl.Add(new MzkMobiCommercialChassisBodyContract()
                                 {
                                     parmChassisComments = cChassisBody.ChassisComment,
                                     parmChevronComments = cChassisBody.ChevronComment,
                                     parmDoorsComments = cChassisBody.DoorsComment,
                                     parmFloorComments = cChassisBody.FloorComment,
                                     parmLandingLegsComments = cChassisBody.LandingLegsComment,
                                     parmSpareWheelCarrierComments = cChassisBody.SpareWheelCarrierComment,
                                     parmUnderRunBumperComments = cChassisBody.UnderRunBumperComment,
                                     parmFuelTanksComments = cChassisBody.FuelTankComment,
                                     parmHeadBoardComments = cChassisBody.HeadboardComment,
                                     parmPanelFrontComments = cChassisBody.DropSideFrontComment,
                                     parmPanelLeftComments = cChassisBody.DropSideLeftComment,
                                     parmPanelRearComments = cChassisBody.DropSideRearComment,
                                     parmPanelRightComments = cChassisBody.DropSideRightComment,

                                     parmIsDamagedChassis = cChassisBody.IsChassisDmg ? NoYes.Yes : NoYes.No,
                                     parmIsDamagedChevron = cChassisBody.IsChevronDmg ? NoYes.Yes : NoYes.No,
                                     parmIsDamagedDoors = cChassisBody.IsDoorsDmg ? NoYes.Yes : NoYes.No,
                                     parmIsDamagedFloor = cChassisBody.IsFloorDmg ? NoYes.Yes : NoYes.No,
                                     parmIsDamagedFuelTanks = cChassisBody.IsFuelTankDmg ? NoYes.Yes : NoYes.No,
                                     parmIsDamagedHeadBoard = cChassisBody.IsHeadboardDmg ? NoYes.Yes : NoYes.No,
                                     parmIsDamagedLandingLegs = cChassisBody.IsLandingLegsDmg ? NoYes.Yes : NoYes.No,
                                     parmIsDamagedSpareWheelCarrier = cChassisBody.IsSpareWheelCarrierDmg ? NoYes.Yes : NoYes.No,
                                     parmIsDamagedUnderRunBumper = cChassisBody.IsUnderRunBumperDmg ? NoYes.Yes : NoYes.No,
                                     parmIsDamagedPanelFront = cChassisBody.IsDropSideFrontDmg ? NoYes.Yes : NoYes.No,
                                     parmIsDamagedPanelLeft = cChassisBody.IsDropSideLeftDmg ? NoYes.Yes : NoYes.No,
                                     parmIsDamagedPanelRear = cChassisBody.IsDropSideRearDmg ? NoYes.Yes : NoYes.No,
                                     parmIsDamagedPanelRight = cChassisBody.IsDropSideRightDmg ? NoYes.Yes : NoYes.No,

                                     parmVehicleInsRecID = cChassisBody.VehicleInsRecID,
                                     parmTableId = cChassisBody.TableId,
                                     parmRecID = cChassisBody.RecID,

                                 }
                                 );

                             });
                }

                var res = await client.editCommercialChassisBodyAsync(mzkMobiCommercialChassisBodyContractColl, _userInfo.CompanyId);

                var cChassisBodyList = new ObservableCollection<CChassisBody>();
                if (res.response != null)
                {
                    res.response.AsParallel().ForAll(x =>
                                cChassisBodyList.Add(new CChassisBody
                                {
                                    ChassisComment = x.parmChassisComments,
                                    ChevronComment = x.parmChevronComments,
                                    DoorsComment = x.parmDoorsComments,
                                    FloorComment = x.parmFloorComments,
                                    LandingLegsComment = x.parmLandingLegsComments,
                                    SpareWheelCarrierComment = x.parmSpareWheelCarrierComments,
                                    UnderRunBumperComment = x.parmUnderRunBumperComments,
                                    FuelTankComment = x.parmFuelTanksComments,
                                    HeadboardComment = x.parmHeadBoardComments,
                                    DropSideFrontComment = x.parmPanelFrontComments,
                                    DropSideLeftComment = x.parmPanelLeftComments,
                                    DropSideRearComment = x.parmPanelRearComments,
                                    DropSideRightComment = x.parmPanelRightComments,
                                    IsChassisDmg = x.parmIsDamagedChassis == NoYes.Yes ? true : false,
                                    IsChevronDmg = x.parmIsDamagedChevron == NoYes.Yes ? true : false,
                                    IsDoorsDmg = x.parmIsDamagedDoors == NoYes.Yes ? true : false,
                                    IsFloorDmg = x.parmIsDamagedFloor == NoYes.Yes ? true : false,
                                    IsFuelTankDmg = x.parmIsDamagedFuelTanks == NoYes.Yes ? true : false,
                                    IsHeadboardDmg = x.parmIsDamagedHeadBoard == NoYes.Yes ? true : false,
                                    IsLandingLegsDmg = x.parmIsDamagedLandingLegs == NoYes.Yes ? true : false,
                                    IsSpareWheelCarrierDmg = x.parmIsDamagedSpareWheelCarrier == NoYes.Yes ? true : false,
                                    IsUnderRunBumperDmg = x.parmIsDamagedUnderRunBumper == NoYes.Yes ? true : false,
                                    IsDropSideFrontDmg = x.parmIsDamagedPanelFront == NoYes.Yes ? true : false,
                                    IsDropSideLeftDmg = x.parmIsDamagedPanelLeft == NoYes.Yes ? true : false,
                                    IsDropSideRearDmg = x.parmIsDamagedPanelRear == NoYes.Yes ? true : false,
                                    IsDropSideRightDmg = x.parmIsDamagedPanelRight == NoYes.Yes ? true : false,
                                    VehicleInsRecID = x.parmVehicleInsRecID,
                                    TableId = x.parmTableId,
                                    RecID = x.parmRecID,

                                }));
                    await SqliteHelper.Storage.UpdateAllAsync<CChassisBody>(cChassisBodyList);
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
        async private System.Threading.Tasks.Task EditCommercialMechConditionToSvcAsync()
        {
            try
            {
                if (_userInfo == null)
                {
                    _userInfo =  JsonConvert.DeserializeObject<UserInfo>(ApplicationData.Current.RoamingSettings.Values[Constants.UserInfo].ToString());
                }
                var cMechanicalCondData = (await SqliteHelper.Storage.LoadTableAsync<Eqstra.BusinessLogic.Commercial.CMechanicalCond>()).Where(x => x.ShouldSave);
                ObservableCollection<MzkMobiCommercialMechConditionContract> mzkMobiCommercialMechConditionContractColl = new ObservableCollection<MzkMobiCommercialMechConditionContract>();
                if (cMechanicalCondData != null)
                {
                    cMechanicalCondData.AsParallel().ForAll(cMechanicalCond =>
                                     {
                                         mzkMobiCommercialMechConditionContractColl.Add(new MzkMobiCommercialMechConditionContract()
                                         {
                                             parmAutoTransmissionComments = cMechanicalCond.AutoTransmissionComment,
                                             parmDifferentialComments = cMechanicalCond.DifferentialComment,
                                             parmEngineComments = cMechanicalCond.EngineComment,
                                             parmExhaustComments = cMechanicalCond.ExhaustComment,
                                             parmFootBrakeComments = cMechanicalCond.FootBrakeComment,
                                             parmfrontSuppressionComments = cMechanicalCond.FrontSuspComment,
                                             parmGearBoxComments = cMechanicalCond.GearboxComment,
                                             parmHandBrakeComments = cMechanicalCond.HandBrakeComment,
                                             parmHydraulicPowerSteeringComments = cMechanicalCond.HPSComment,
                                             parmRearSuppressionComments = cMechanicalCond.RearSuspComment,
                                             parmOilLeaksComments = cMechanicalCond.OilLeaksComment,
                                             parmBatteryComments = cMechanicalCond.BatteryComment,
                                             parmSteeringComments = cMechanicalCond.SteeringComment,


                                             parmIsDamagedBatteryComments = cMechanicalCond.IsBatteryDmg ? NoYes.Yes : NoYes.No,
                                             parmIsDamagedAutoTransmission = cMechanicalCond.IsAutoTransmissionDmg ? NoYes.Yes : NoYes.No,
                                             parmIsDamagedDifferential = cMechanicalCond.IsDifferentialDmg ? NoYes.Yes : NoYes.No,
                                             parmIsDamagedEngine = cMechanicalCond.IsEngineDmg ? NoYes.Yes : NoYes.No,
                                             parmIsDamagedExhaust = cMechanicalCond.IsExhaustDmg ? NoYes.Yes : NoYes.No,
                                             parmIsDamagedFootBrake = cMechanicalCond.IsFootBrakeDmg ? NoYes.Yes : NoYes.No,
                                             parmIsDamagedFrontSuppression = cMechanicalCond.IsFrontSuspDmg ? NoYes.Yes : NoYes.No,
                                             parmIsDamagedGearBox = cMechanicalCond.IsGearboxDmg ? NoYes.Yes : NoYes.No,
                                             parmIsDamagedHandBrake = cMechanicalCond.IsHandBrakeDmg ? NoYes.Yes : NoYes.No,
                                             parmIsDamagedHydraulicPowerSteering = cMechanicalCond.IsHPSDmg ? NoYes.Yes : NoYes.No,
                                             parmIsDamagedOilLeeks = cMechanicalCond.IsOilLeaksDmg ? NoYes.Yes : NoYes.No,
                                             parmIsDamagedRearSuppression = cMechanicalCond.IsRearSuspDmg ? NoYes.Yes : NoYes.No,
                                             parmIsDamagedSteering = cMechanicalCond.IsSteeringDmg ? NoYes.Yes : NoYes.No,


                                             parmVehicleInsRecID = cMechanicalCond.VehicleInsRecID,
                                             parmTableId = cMechanicalCond.TableId,
                                             parmRecID = cMechanicalCond.RecID,

                                         });

                                     });
                }
                var res = await client.editCommercialMechConditionAsync(mzkMobiCommercialMechConditionContractColl, _userInfo.CompanyId);

                var cMechanicalCondList = new ObservableCollection<CMechanicalCond>();
                if (res.response != null)
                {
                    res.response.AsParallel().ForAll(x =>
                                cMechanicalCondList.Add(new CMechanicalCond
                                {
                                    AutoTransmissionComment = x.parmAutoTransmissionComments,
                                    DifferentialComment = x.parmDifferentialComments,
                                    EngineComment = x.parmEngineComments,
                                    ExhaustComment = x.parmExhaustComments,
                                    FootBrakeComment = x.parmFootBrakeComments,
                                    FrontSuspComment = x.parmfrontSuppressionComments,
                                    GearboxComment = x.parmGearBoxComments,
                                    HandBrakeComment = x.parmHandBrakeComments,
                                    HPSComment = x.parmHydraulicPowerSteeringComments,
                                    RearSuspComment = x.parmRearSuppressionComments,
                                    OilLeaksComment = x.parmOilLeaksComments,
                                    BatteryComment = x.parmBatteryComments,
                                    SteeringComment = x.parmSteeringComments,
                                    IsBatteryDmg = x.parmIsDamagedBatteryComments == NoYes.Yes ? true : false,
                                    IsAutoTransmissionDmg = x.parmIsDamagedAutoTransmission == NoYes.Yes ? true : false,
                                    IsDifferentialDmg = x.parmIsDamagedDifferential == NoYes.Yes ? true : false,
                                    IsEngineDmg = x.parmIsDamagedEngine == NoYes.Yes ? true : false,
                                    IsExhaustDmg = x.parmIsDamagedExhaust == NoYes.Yes ? true : false,
                                    IsFootBrakeDmg = x.parmIsDamagedFootBrake == NoYes.Yes ? true : false,
                                    IsFrontSuspDmg = x.parmIsDamagedFrontSuppression == NoYes.Yes ? true : false,
                                    IsGearboxDmg = x.parmIsDamagedGearBox == NoYes.Yes ? true : false,
                                    IsHandBrakeDmg = x.parmIsDamagedHandBrake == NoYes.Yes ? true : false,
                                    IsHPSDmg = x.parmIsDamagedHydraulicPowerSteering == NoYes.Yes ? true : false,
                                    IsOilLeaksDmg = x.parmIsDamagedOilLeeks == NoYes.Yes ? true : false,
                                    IsRearSuspDmg = x.parmIsDamagedRearSuppression == NoYes.Yes ? true : false,
                                    IsSteeringDmg = x.parmIsDamagedSteering == NoYes.Yes ? true : false,
                                    VehicleInsRecID = x.parmVehicleInsRecID,
                                    TableId = x.parmTableId,
                                    RecID = x.parmRecID,

                                }));
                    await SqliteHelper.Storage.UpdateAllAsync<CMechanicalCond>(cMechanicalCondList);
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
        async private System.Threading.Tasks.Task EditCommercialTyreConditionToSvcAsync()
        {
            try
            {
                if (_userInfo == null)
                {
                    _userInfo =  JsonConvert.DeserializeObject<UserInfo>(ApplicationData.Current.RoamingSettings.Values[Constants.UserInfo].ToString());
                }
                var cTyresData = (await SqliteHelper.Storage.LoadTableAsync<Eqstra.BusinessLogic.Commercial.CTyres>()).Where(x => x.ShouldSave);
                ObservableCollection<MzkMobiCommercialTyresContract> mzkMobiCommercialTyresContractColl = new ObservableCollection<MzkMobiCommercialTyresContract>();
                if (cTyresData != null)
                {
                    cTyresData.AsParallel().ForAll(cTyres =>
                              {
                                  mzkMobiCommercialTyresContractColl.Add(new MzkMobiCommercialTyresContract()
                                  {
                                      parmLFComments = cTyres.LFComment,
                                      parmLInnerAxleInComments = cTyres.LInnerAxleInComment,
                                      parmLInnerAxleOutComments = cTyres.LInnerAxleOutComment,
                                      parmLRInnerComments = cTyres.LRInnerComment,
                                      parmLROuterComments = cTyres.LROuterComment,
                                      parmRFComments = cTyres.RFComment,
                                      parmRInnerAxleInComments = cTyres.RInnerAxleInComment,
                                      parmRInnerAxleOutComments = cTyres.RInnerAxleOutComment,
                                      parmRRInnerComments = cTyres.RRInnerComment,
                                      parmRROuterComments = cTyres.RROuterComment,
                                      parmSpareComments = cTyres.SpareComment,

                                      parmLFCondition = cTyres.HasFLF ? MZKConditionEnum.Fair : (cTyres.HasGLF ? MZKConditionEnum.Good : (cTyres.HasPLF ? MZKConditionEnum.Poor : MZKConditionEnum.None)),
                                      parmLInnerAxleInCondition = cTyres.HasFLInnerAxleIn ? MZKConditionEnum.Fair : (cTyres.HasGLInnerAxleIn ? MZKConditionEnum.Good : (cTyres.HasPLInnerAxleIn ? MZKConditionEnum.Poor : MZKConditionEnum.None)),
                                      parmLInnerAxleOutCondition = cTyres.HasFLInnerAxleOut ? MZKConditionEnum.Fair : (cTyres.HasGLInnerAxleOut ? MZKConditionEnum.Good : (cTyres.HasPLInnerAxleOut ? MZKConditionEnum.Poor : MZKConditionEnum.None)),
                                      parmLRInnerCondition = cTyres.HasFLRInner ? MZKConditionEnum.Fair : (cTyres.HasGLRInner ? MZKConditionEnum.Good : (cTyres.HasPLRInner ? MZKConditionEnum.Poor : MZKConditionEnum.None)),
                                      parmRInnerAxleOutCondition = cTyres.HasFRInnerAxleOut ? MZKConditionEnum.Fair : (cTyres.HasGRInnerAxleOut ? MZKConditionEnum.Good : (cTyres.HasPRInnerAxleOut ? MZKConditionEnum.Poor : MZKConditionEnum.None)),
                                      parmRRInnerCondition = cTyres.HasFRRInner ? MZKConditionEnum.Fair : (cTyres.HasGRRInner ? MZKConditionEnum.Good : (cTyres.HasPRRInner ? MZKConditionEnum.Poor : MZKConditionEnum.None)),
                                      parmRROuterCondition = cTyres.HasFRROuter ? MZKConditionEnum.Fair : (cTyres.HasGRROuter ? MZKConditionEnum.Good : (cTyres.HasPRROuter ? MZKConditionEnum.Poor : MZKConditionEnum.None)),
                                      parmSpareCondition = cTyres.HasFSpare ? MZKConditionEnum.Fair : (cTyres.HasGSpare ? MZKConditionEnum.Good : (cTyres.HasPSpare ? MZKConditionEnum.Poor : MZKConditionEnum.None)),
                                      parmRInnerAxleInCondition = cTyres.HasFRInnerAxleIn ? MZKConditionEnum.Fair : (cTyres.HasGRInnerAxleIn ? MZKConditionEnum.Good : (cTyres.HasPRInnerAxleIn ? MZKConditionEnum.Poor : MZKConditionEnum.None)),
                                      parmRFCondition = cTyres.HasFRF ? MZKConditionEnum.Fair : (cTyres.HasGRF ? MZKConditionEnum.Good : (cTyres.HasPRF ? MZKConditionEnum.Poor : MZKConditionEnum.None)),
                                      parmLROuterCondition = cTyres.HasFLROuter ? MZKConditionEnum.Fair : (cTyres.HasGLROuter ? MZKConditionEnum.Good : (cTyres.HasPLROuter ? MZKConditionEnum.Poor : MZKConditionEnum.None)),

                                      parmVehicleInsRecID = cTyres.VehicleInsRecID,
                                      parmTableId = cTyres.TableId,
                                      parmRecID = cTyres.RecID,



                                  });
                              });
                }

                var res = await client.editCommercialTyreConditionAsync(mzkMobiCommercialTyresContractColl, _userInfo.CompanyId);

                var cTyresList = new ObservableCollection<CTyres>();
                if (res.response != null)
                {
                    res.response.AsParallel().ForAll(x =>
                               cTyresList.Add(new CTyres
                               {
                                   LFComment = x.parmLFComments,
                                   LInnerAxleInComment = x.parmLInnerAxleInComments,
                                   LInnerAxleOutComment = x.parmLInnerAxleOutComments,
                                   LRInnerComment = x.parmLRInnerComments,
                                   LROuterComment = x.parmLROuterComments,
                                   RFComment = x.parmRFComments,
                                   RInnerAxleInComment = x.parmRInnerAxleInComments,
                                   RInnerAxleOutComment = x.parmRInnerAxleOutComments,
                                   RRInnerComment = x.parmRRInnerComments,
                                   RROuterComment = x.parmRROuterComments,
                                   SpareComment = x.parmSpareComments,
                                   VehicleInsRecID = x.parmVehicleInsRecID,
                                   TableId = x.parmTableId,
                                   RecID = x.parmRecID,

                                   HasFLF = MZKConditionEnum.Fair == x.parmLFCondition ? true : false,
                                   HasGLF = MZKConditionEnum.Good == x.parmLFCondition ? true : false,
                                   HasPLF = MZKConditionEnum.Poor == x.parmLFCondition ? true : false,
                                   HasFLInnerAxleIn = MZKConditionEnum.Fair == x.parmLInnerAxleInCondition ? true : false,
                                   HasGLInnerAxleIn = MZKConditionEnum.Good == x.parmLInnerAxleInCondition ? true : false,
                                   HasPLInnerAxleIn = MZKConditionEnum.Poor == x.parmLInnerAxleInCondition ? true : false,
                                   HasFLInnerAxleOut = MZKConditionEnum.Fair == x.parmLInnerAxleOutCondition ? true : false,
                                   HasGLInnerAxleOut = MZKConditionEnum.Good == x.parmLInnerAxleOutCondition ? true : false,
                                   HasPLInnerAxleOut = MZKConditionEnum.Poor == x.parmLInnerAxleOutCondition ? true : false,
                                   HasFLRInner = MZKConditionEnum.Fair == x.parmLRInnerCondition ? true : false,
                                   HasGLRInner = MZKConditionEnum.Good == x.parmLRInnerCondition ? true : false,
                                   HasPLRInner = MZKConditionEnum.Poor == x.parmLRInnerCondition ? true : false,
                                   HasFRInnerAxleOut = MZKConditionEnum.Fair == x.parmRInnerAxleOutCondition ? true : false,
                                   HasGRInnerAxleOut = MZKConditionEnum.Good == x.parmRInnerAxleOutCondition ? true : false,
                                   HasPRInnerAxleOut = MZKConditionEnum.Poor == x.parmRInnerAxleOutCondition ? true : false,
                                   HasFRRInner = MZKConditionEnum.Fair == x.parmRRInnerCondition ? true : false,
                                   HasGRRInner = MZKConditionEnum.Good == x.parmRRInnerCondition ? true : false,
                                   HasPRRInner = MZKConditionEnum.Poor == x.parmRRInnerCondition ? true : false,
                                   HasFRROuter = MZKConditionEnum.Fair == x.parmRROuterCondition ? true : false,
                                   HasGRROuter = MZKConditionEnum.Good == x.parmRROuterCondition ? true : false,
                                   HasPRROuter = MZKConditionEnum.Poor == x.parmRROuterCondition ? true : false,
                                   HasFSpare = MZKConditionEnum.Fair == x.parmSpareCondition ? true : false,
                                   HasGSpare = MZKConditionEnum.Good == x.parmSpareCondition ? true : false,
                                   HasPSpare = MZKConditionEnum.Poor == x.parmSpareCondition ? true : false,
                                   HasFRInnerAxleIn = MZKConditionEnum.Fair == x.parmRInnerAxleInCondition ? true : false,
                                   HasGRInnerAxleIn = MZKConditionEnum.Good == x.parmRInnerAxleInCondition ? true : false,
                                   HasPRInnerAxleIn = MZKConditionEnum.Poor == x.parmRInnerAxleInCondition ? true : false,
                                   HasFRF = MZKConditionEnum.Fair == x.parmRFCondition ? true : false,
                                   HasGRF = MZKConditionEnum.Good == x.parmRFCondition ? true : false,
                                   HasPRF = MZKConditionEnum.Poor == x.parmRFCondition ? true : false,
                                   HasFLROuter = MZKConditionEnum.Fair == x.parmLROuterCondition ? true : false,
                                   HasGLROuter = MZKConditionEnum.Good == x.parmLROuterCondition ? true : false,
                                   HasPLROuter = MZKConditionEnum.Poor == x.parmLROuterCondition ? true : false,


                               }));
                    await SqliteHelper.Storage.UpdateAllAsync<CTyres>(cTyresList);
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
        async private System.Threading.Tasks.Task EditCommercialInspectionProofToSvcAsync()
        {
            try
            {
                if (_userInfo == null)
                {
                    _userInfo =  JsonConvert.DeserializeObject<UserInfo>(ApplicationData.Current.RoamingSettings.Values[Constants.UserInfo].ToString());
                }
                var inspectionProofData = (await SqliteHelper.Storage.LoadTableAsync<Eqstra.BusinessLogic.Commercial.CPOI>()).Where(x => x.ShouldSave);
                ObservableCollection<MZKVehicleInspectionTableContract> MZKVehicleInspectionTableContractColl = new ObservableCollection<MZKVehicleInspectionTableContract>();
                if (inspectionProofData != null)
                {
                    inspectionProofData.AsParallel().ForAll(insProof =>
                           {
                               MZKVehicleInspectionTableContractColl.Add(new MZKVehicleInspectionTableContract()
                               {
                                   parmCompanyRepSignDateTime = insProof.EQRDate,
                                   parmCustomerRepSignDateTime = insProof.CRDate,
                                   parmRecommendation = insProof.IsRetainChecked ? MZKRecommendationEnum.Retain : (insProof.IsSellChecked ? MZKRecommendationEnum.Sell : (insProof.IsNotFeasChecked ? MZKRecommendationEnum.NotFeasible : MZKRecommendationEnum.None)),
                                   parmVehicleType = MzkVehicleType.Commercial,
                                   parmRecID = insProof.VehicleInsRecID,
                                   parmGeneralCondition = insProof.IsFairChecked ? MZKConditionEnum.Fair : (insProof.IsGoodChecked ? MZKConditionEnum.Good : (insProof.IsPoorChecked ? MZKConditionEnum.Poor : MZKConditionEnum.None))

                               });
                           });
                }
                var res = await client.editVehicleInspectionAsync(MZKVehicleInspectionTableContractColl);

                var cpoiList = new ObservableCollection<CPOI>();
                if (res.response != null)
                {
                    res.response.AsParallel().ForAll(x =>
                               cpoiList.Add(new CPOI
                               {
                                   EQRDate = x.parmCompanyRepSignDateTime,
                                   CRDate = x.parmCustomerRepSignDateTime,
                                   IsFairChecked = x.parmGeneralCondition == MZKConditionEnum.Fair ? true : false,
                                   IsGoodChecked = x.parmGeneralCondition == MZKConditionEnum.Good ? true : false,
                                   IsPoorChecked = x.parmGeneralCondition == MZKConditionEnum.Poor ? true : false,
                                   IsNotFeasChecked = x.parmRecommendation == MZKRecommendationEnum.NotFeasible ? true : false,
                                   IsRetainChecked = x.parmRecommendation == MZKRecommendationEnum.Retain ? true : false,
                                   IsSellChecked = x.parmRecommendation == MZKRecommendationEnum.Sell ? true : false,
                                   VehicleInsRecID = x.parmRecID

                               }));
                }

                await SqliteHelper.Storage.UpdateAllAsync<CPOI>(cpoiList);
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
                var tasks = (await SqliteHelper.Storage.LoadTableAsync<Eqstra.BusinessLogic.Task>());
                ObservableCollection<MzkTasksContract> mzkTasks = new ObservableCollection<MzkTasksContract>();
                Dictionary<string, EEPActionStep> actionStepMapping = new Dictionary<string, EEPActionStep>();

                actionStepMapping.Add(Eqstra.BusinessLogic.Helpers.TaskStatus.AwaitInspectionDataCapture, EEPActionStep.AwaitInspectionConfirmation);
                actionStepMapping.Add(Eqstra.BusinessLogic.Helpers.TaskStatus.AwaitInspectionAcceptance, EEPActionStep.AwaitInspectionDataCapture);
                actionStepMapping.Add(Eqstra.BusinessLogic.Helpers.TaskStatus.AwaitDamageConfirmation, EEPActionStep.AwaitInspectionAcceptance);

                if (tasks != null)
                {
                    var q = tasks.Where(x =>
                        x.Status != Eqstra.BusinessLogic.Helpers.TaskStatus.AwaitInspectionDetail ||
                        x.Status != Eqstra.BusinessLogic.Helpers.TaskStatus.AwaitVehicleCollection ||
                        x.Status != Eqstra.BusinessLogic.Helpers.TaskStatus.AwaitCollectionDetail).ToArray();

                    foreach (var task in tasks.Where(x =>
                        x.Status != Eqstra.BusinessLogic.Helpers.TaskStatus.AwaitInspectionDetail &&
                        x.Status != Eqstra.BusinessLogic.Helpers.TaskStatus.AwaitVehicleCollection &&
                        x.Status != Eqstra.BusinessLogic.Helpers.TaskStatus.AwaitCollectionDetail))
                    {
                        mzkTasks.Add(
                            new MzkTasksContract
                            {
                                parmCaseID = task.CaseNumber,
                                parmCaseServiceRecId = task.CaseServiceRecID,
                                parmProcessStepRecID = task.ProcessStepRecID,
                                parmServiceRecId = task.ServiceRecID,
                                parmCaseCategory = task.CaseCategory,
                                parmCategoryType = task.CategoryType,
                                parmCollectionRecId = task.CollectionRecID,
                                parmConfirmedDueDate = task.ConfirmedDate,
                                parmCustAddress = task.Address,
                                parmCustId = task.CustomerId,
                                parmCustName = task.CustomerName,
                                parmCustPhone = task.CustPhone,
                                parmContactPersonPhone = task.ContactNumber,
                                parmContactPersonName = task.ContactName,
                                parmRegistrationNum = task.RegistrationNumber,
                                parmStatus = task.Status,
                                parmStatusDueDate = task.StatusDueDate,
                                parmUserID = task.UserId,
                                parmVehicleType = (MzkVehicleType)Enum.Parse(typeof(MzkVehicleType), task.VehicleType.ToString()),
                                parmProcessStep = task.ProcessStep,
                                parmRecID = task.VehicleInsRecId,
                                parmEEPActionStep = actionStepMapping[task.Status]
                            });
                    }
                    var res = await client.updateStatusListAsync(mzkTasks, _userInfo.CompanyId);

                    var taskList = new ObservableCollection<Eqstra.BusinessLogic.Task>();
                    if (res.response != null)
                    {
                        res.response.AsParallel().ForAll(x =>
                                   taskList.Add(new Eqstra.BusinessLogic.Task
                                   {
                                       CaseNumber = x.parmCaseID,
                                       CaseServiceRecID = x.parmCaseServiceRecId,
                                       ProcessStepRecID = x.parmProcessStepRecID,
                                       ServiceRecID = x.parmServiceRecId,
                                       CaseCategory = x.parmCaseCategory,
                                       CollectionRecID = x.parmCollectionRecId,
                                       ConfirmedDate = x.parmConfirmedDueDate,
                                       Address = x.parmCustAddress,
                                       CustomerId = x.parmCustId,
                                       CustomerName = x.parmCustName,
                                       CustPhone = x.parmCustPhone,
                                       ContactName = x.parmContactPersonName,
                                       ContactNumber = x.parmContactPersonPhone,
                                       RegistrationNumber = x.parmRegistrationNum,
                                       Status = x.parmStatus,
                                       StatusDueDate = x.parmStatusDueDate,
                                       UserId = x.parmUserID,
                                       ProcessStep = x.parmProcessStep,
                                       CategoryType = x.parmCaseCategory,
                                       VehicleType = (VehicleTypeEnum)Enum.Parse(typeof(VehicleTypeEnum), x.parmVehicleType.ToString()),
                                       VehicleInsRecId = x.parmRecID,

                                   }));
                    }

                    await SqliteHelper.Storage.UpdateAllAsync<Eqstra.BusinessLogic.Task>(taskList);
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

    }
}