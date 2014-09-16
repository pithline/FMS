using Eqstra.DataProvider.AX.Helpers;
using Eqstra.DataProvider.AX.SSModels;
using Eqstra.DataProvider.AX.TIModels;
using Eqstra.DataProvider.AX.TIModels.CVehicle;
using Eqstra.DataProvider.AX.TIModels.PVehicle;
using Eqstra.DataProvider.AX.TIProxy;
using Eqstra.Framework.Web.DataAccess;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.ServiceModel;

namespace Eqstra.DataProvider.AX.Providers
{
    [DataProvider(Name = "TechnicalInspection")]
    public class TechnicalInspectionProvider : IDataProvider
    {

        TIProxy.MzkVehicleInspectionServiceClient _client;
        public System.Collections.IList GetDataList(object[] criterias)
        {
            try
            {
                GetService();
                System.Collections.IList response = null;
                switch (criterias[0].ToString())
                {
                    case Eqstra.DataProvider.AX.Helpers.ActionSwitch.GetTasks:
                        response = GetTasks();
                        break;
                }
                return response;
            }

            catch (Exception ex)
            {
                throw;
            }
        }

        public object GetService()
        {
            return GetServiceClient();
        }

        public TIProxy.MzkVehicleInspectionServiceClient GetServiceClient()
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
                _client = new TIProxy.MzkVehicleInspectionServiceClient(basicHttpBinding, new EndpointAddress("http://srfmlbispstg01.lfmd.co.za/MicrosoftDynamicsAXAif60/ServiceSchedulingService/xppservice.svc?wsdl"));
                _client.ClientCredentials.UserName.UserName = "lfmd" + "\"" + "sahmed";
                _client.ClientCredentials.UserName.Password = "Password1";
                _client.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Identification;
                _client.ClientCredentials.Windows.ClientCredential = new NetworkCredential("sahmed", "Password1", "lfmd");
            }
            catch (Exception)
            {
                throw;
            }

            return _client;
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
                        response = ValidateUser("sahmed", "Password1");
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

        bool IDataProvider.DeleteData(object[] criterias)
        {
            throw new System.NotImplementedException();
        }

        object IDataProvider.SaveData(object[] criterias)
        {
            throw new System.NotImplementedException();
        }

        object IDataProvider.GetService()
        {
            throw new System.NotImplementedException();
        }

        public UserInfo ValidateUser(string userId, string password)
        {
            UserInfo userInfo = new UserInfo();
            try
            {
                var result = _client.validateUser(new TIProxy.CallContext() { Company = "1000" }, userId, password);
                if (result != null)
                {
                    userInfo.UserId = result.parmUserID;
                    userInfo.CompanyId = result.parmCompany;
                    userInfo.CompanyName = result.parmCompanyName;
                    userInfo.Name = result.parmUserName;
                }




                //userInfo.UserId = "1000";
                //userInfo.CompanyId = "1200";
                //userInfo.CompanyName = "mazik";
                //userInfo.Name = "kasif";

            }
            catch (Exception)
            {
                throw;
            }

           

            return userInfo;
        }
        public List<CustomerDetails> GetCustomerDetails(string cusId)
        {
            try
            {
                var result = _client.getCustDetails(new TIProxy.CallContext() { Company = "1000" }, cusId, "1000");
                List<CustomerDetails> InsertList = new List<CustomerDetails>();
                if (result != null)
                {


                    foreach (var mzkCustomer in result)
                    {

                        InsertList.Add(new CustomerDetails
                          {
                              Address = mzkCustomer.parmCustAddress,
                              CustomerName = mzkCustomer.parmCustName,
                              ContactNumber = mzkCustomer.parmCustPhone,
                              EmailId = mzkCustomer.parmCustEmail,
                              Id = cusId
                          });

                    }

                }
                return InsertList;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public List<Eqstra.DataProvider.AX.TIModels.Task> GetTasks()
        {
            try
            {

                var result = _client.getTasks(new TIProxy.CallContext() { Company = "1000" }, "1000", "sahmed");
                List<Eqstra.DataProvider.AX.TIModels.Task> taskInsertList = new List<Eqstra.DataProvider.AX.TIModels.Task>();
                if (result != null)
                {

                    foreach (var mzkTask in result)
                    {
                        taskInsertList.Add(new Eqstra.DataProvider.AX.TIModels.Task
                         {

                             CaseCategory = mzkTask.parmCaseCategory,
                             CaseNumber = mzkTask.parmCaseID,
                             CaseServiceRecID = mzkTask.parmCaseServiceRecId,
                             CategoryType = mzkTask.parmCategoryType,
                             CollectionRecID = mzkTask.parmCollectionRecId,
                             ConfirmedDate = mzkTask.parmConfirmedDueDate < DateTime.Today ? DateTime.Today : mzkTask.parmConfirmedDueDate.Date,
                             ConfirmedTime = new DateTime(mzkTask.parmConfirmedDueDate.TimeOfDay.Ticks),
                             Address = mzkTask.parmCustAddress,
                             CustomerId = mzkTask.parmCustId,
                             CustomerName = mzkTask.parmCustName,
                             CustPhone = mzkTask.parmCustPhone,
                             ContactName = mzkTask.parmContactPersonName,
                             ContactNumber = mzkTask.parmContactPersonPhone,
                             ProcessStepRecID = mzkTask.parmProcessStepRecID,
                             ServiceRecID = mzkTask.parmServiceRecId,
                             Status = mzkTask.parmStatus,
                             StatusDueDate = mzkTask.parmStatusDueDate.ToShortDateString(),
                             UserId = mzkTask.parmUserID,
                             RegistrationNumber = mzkTask.parmRegistrationNum,
                             //AllocatedTo = _userInfo.Name,
                             VehicleInsRecId = mzkTask.parmRecID,
                             //VehicleType = (VehicleTypeEnum)Enum.Parse(typeof(VehicleTypeEnum), mzkTask.parmVehicleType.ToString())
                         });


                    }

                }

                //taskInsertList.Add(new Eqstra.DataProvider.AX.TIModels.Task
                // {

                    
                //     CaseNumber ="165131",
                //     Status = "wating for AX service",
                //     ContactNumber ="9290650135",
                //     Address="hyderabad,india",
                //     StatusDueDate=DateTime.Now.ToShortDateString()
                // });

                return taskInsertList;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<CommercialVehicle> GetCVehicleDetails(string caseNumber, long vRecId)
        {
            try
            {

                var resp = _client.readVehicleDetails(new TIProxy.CallContext() { Company = "1000" }, "1000", "sahmed");
                List<CommercialVehicle> vehicleInsertList = new List<CommercialVehicle>();
                if (resp != null)
                {

                    foreach (var v in resp)
                    {
                        vehicleInsertList.Add(new CommercialVehicle
                          {
                              ChassisNumber = v.parmChassisNumber.ToString(),
                              Color = v.parmColor,
                              Year = v.parmyear.ToString("MM/dd/yyyy"),
                              ODOReading = v.parmODOReading.ToString(),
                              Make = v.parmMake,
                              EngineNumber = v.parmEngineNumber,
                              VehicleInsRecID = vRecId,
                              RecID = v.parmRecID,
                              CaseNumber = caseNumber,
                              TableId = v.parmTableId,
                              RegistrationNumber = v.parmRegNo,



                          });
                    }

                }

                return vehicleInsertList;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<PassengerVehicle> GetPVehicleDetails(string caseNumber, long vRecId)
        {
            try
            {
                var resp = _client.readVehicleDetails(new TIProxy.CallContext() { Company = "1000" }, caseNumber, "1000");
                List<PassengerVehicle> vehicleInsertList = new List<PassengerVehicle>();
                if (resp != null)
                {

                    foreach (var v in resp)
                    {

                        vehicleInsertList.Add(new PassengerVehicle
                      {
                          ChassisNumber = v.parmChassisNumber.ToString(),
                          Color = v.parmColor,
                          Year = v.parmyear.ToString("MM/dd/yyyy"),
                          ODOReading = v.parmODOReading.ToString(),
                          Make = v.parmMake,
                          EngineNumber = v.parmEngineNumber,
                          VehicleInsRecID = vRecId,
                          RecID = v.parmRecID,
                          CaseNumber = caseNumber,
                          TableId = v.parmTableId,
                          RegistrationNumber = v.parmRegNo,
                      });

                    }
                }
                return vehicleInsertList;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


    }
}
