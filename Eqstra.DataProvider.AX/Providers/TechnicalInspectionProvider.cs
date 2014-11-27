using Eqstra.DataProvider.AX.Helpers;
using Eqstra.DataProvider.AX.SSModels;
using Eqstra.DataProvider.AX.TI;
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
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace Eqstra.DataProvider.AX.Providers
{
    [DataProvider(Name = "TechnicalInspection")]
    public class TechnicalInspectionProvider : IDataProvider
    {

        TIProxy.MzkTechnicalInspectionClient _client;
        public System.Collections.IList GetDataList(object[] criterias)
        {
            try
            {
                System.Collections.IList response = null;
                switch (criterias[0].ToString())
                {
                    case Eqstra.DataProvider.AX.Helpers.ActionSwitch.GetTasks:
                        return GetTask(criterias[1].ToString(), criterias[2].ToString());

                }
                return response;
            }

            catch (Exception ex)
            {
                throw;
            }
        }

        private List<TITask> GetTask(string userId, string companyId)
        {
            try
            {
                var taskList = new List<TITask>();
                

                var client = GetServiceClient();
                var result = client.getTasks(new CallContext(), userId, companyId);
                if (result != null)
                {
                    foreach (var task in result)
                    {
                        var tiTask = new TITask
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
                            StatusDueDate = task.parmStatusDueDate.ToShortDateString(),
                            UserId = task.parmUserID,
                            Address = task.parmCustAddress,
                            CustomerId = task.parmCustAccount,
                            VehicleInsRecId = task.parmCaseServiceRecID,
                            ConfirmedDate = task.parmInspectionDueDate,
                            CustEmailId = task.parmEmail,

                        };


                        var subComponents = client.getSubComponents(new CallContext(), new[] { tiTask.CaseServiceRecID }, companyId);

                        if (subComponents != null)
                        {
                            var subComponentList = new List<MaintenanceRepair>();
                            foreach (var item in subComponents)
                            {
                                var subComponent = new MaintenanceRepair
                                {
                                    SubComponent = item.parmSubComponent,
                                    MajorComponent = item.parmMajorComponent,
                                    Action = item.parmAction,
                                    Cause = item.parmCause,
                                    CaseServiceRecId = item.parmCaseServiceRecID,
                                    Repairid = item.parmRecID

                                };

                                subComponentList.Add(subComponent);
                            }
                        }

                        taskList.Add(tiTask);
                    }
                }
                client.Close();
                return taskList;
            }
            catch (Exception)
            {
                throw;
            }
        }




        public object GetService()
        {
            return GetServiceClient();
        }

        public TIProxy.MzkTechnicalInspectionClient GetServiceClient()
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
                _client = new TIProxy.MzkTechnicalInspectionClient(basicHttpBinding, new EndpointAddress("http://srfmlbispstg01.lfmd.co.za/MicrosoftDynamicsAXAif60/TechnicalInspection/xppservice.svc"));
                _client.ClientCredentials.UserName.UserName = "lfmd" + "\"" + "rchivukula";
                _client.ClientCredentials.UserName.Password = "Password8";
                _client.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Identification;
                _client.ClientCredentials.Windows.ClientCredential = new NetworkCredential("rchivukula", "Password8", "lfmd");
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

        bool IDataProvider.DeleteData(object[] criterias)
        {
            throw new System.NotImplementedException();
        }

        object IDataProvider.SaveData(object[] criterias)
        {
            object response = null;
            switch (criterias[0].ToString())
            {

                case "InsertInspectionData":
                    {
                        var tiData = JsonConvert.DeserializeObject<List<TIData>>(criterias[1].ToString());
                        var tasks = JsonConvert.DeserializeObject<List<Eqstra.DataProvider.AX.TIModels.Task>>(criterias[2].ToString());
                        return InsertInspectionData(tiData, tasks, criterias[3].ToString());


                    }
            }
            return response;
        }

        object IDataProvider.GetService()
        {
            throw new System.NotImplementedException();
        }

        async public Task<bool> ValidateUser(string userId, string password)
        {

            try
            {
                var result = await _client.validateUserAsync(new TIProxy.CallContext() { Company = "1000" }, userId, password);
                return result.response;

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
                var result = _client.getUserDetails(new TIProxy.CallContext() { Company = "1000" }, userId);
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


        private bool InsertInspectionData(List<TIData> tiData, List<Eqstra.DataProvider.AX.TIModels.Task> tasks ,string companyId)
        {
            try
            {
                var client = GetServiceClient();
                var contract = new List<MzkCaseServiceAuthorizationContract>();
                var taskList = new List<MzkTechnicalTasksContract>();
                foreach (var x in tiData)
                {
                    contract.Add(new MzkCaseServiceAuthorizationContract()
                        {
                            parmDamageCause = x.CauseOfDamage,
                            parmRemedy = x.Remedy,
                            parmRecommendation = x.Recommendation,
                            parmCompletionDate = x.CompletionDate,
                            // parmCaseCategoryAuthList = x.CaseCategoryAuthList,
                            parmCaseServiceRecID = x.CaseServiceRecID

                        });
                }

                client.insertTechnicalInspection(new CallContext(), contract.ToArray(), companyId);
                Dictionary<string, EEPActionStep> actionStepMapping = new Dictionary<string, EEPActionStep>();
                actionStepMapping.Add("Completed", EEPActionStep.AwaitTechnicalInspection);

                if (tasks != null)
                {
                    foreach (var task in tasks)
                    {
                        taskList.Add(
                            new MzkTechnicalTasksContract

                            {
                                parmCaseServiceRecID = task.CaseServiceRecID,
                                parmServiceRecID = task.ServiceRecID,
                                parmCaseId = task.CaseNumber,

                                parmCustAddress = task.Address,
                                parmCustName = task.CustomerName,
                                parmCustPhone = task.CustPhone,
                                parmContactPersonPhone = task.ContactNumber,
                                parmContactPersonName = task.ContactName,
                                parmStatus = task.Status,
                                
                                parmUserID = task.UserId,
                                parmEEPActionStep = actionStepMapping[task.Status]
                            });
                    } 
                }
                var res = client.updateStatusList(new CallContext(),taskList.ToArray(), companyId);
                client.Close();
                return res != null && res.Length > 0;
            }
            catch (Exception)
            {
                throw;
            }


        }


    }
}
