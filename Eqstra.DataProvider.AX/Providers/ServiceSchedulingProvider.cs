using Eqstra.DataProvider.AX.SSModels;
using Eqstra.Framework.Web.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Eqstra.DataProvider.AX.Providers
{
    [DataProvider(Name = "ServiceScheduling")]

    class ServiceSchedulingProvider : IDataProvider
    {
        SSProxy.MzkServiceSchedulingServiceClient client;
        public System.Collections.IList GetDataList(object[] criterias)
        {
            client = GetService();
            JsonRecords list = new JsonRecords();
            switch (criterias[0].ToString())
            {
                case "GetTasks":
                   list = GetTasks();
                    break;
            }
            return list;

           
            
        }

        public object GetSingleData(object[] criterias)
        {
            throw new NotImplementedException();
        }

        public bool DeleteData(object[] criterias)
        {
            throw new NotImplementedException();
        }

        public object SaveData(object[] criterias)
        {
            throw new NotImplementedException();
        }



        public SSProxy.MzkServiceSchedulingServiceClient GetService()
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
            client.ClientCredentials.UserName.UserName = "lfmd" + "\"" + "rchivukula";
            client.ClientCredentials.UserName.Password = "Password1";
            client.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Identification;
            client = new SSProxy.MzkServiceSchedulingServiceClient(basicHttpBinding, new EndpointAddress("http://srfmlbispstg01.lfmd.co.za/MicrosoftDynamicsAXAif60/ServiceSchedulingService/xppservice.svc?wsdl"));
            client.ClientCredentials.Windows.ClientCredential = new NetworkCredential("rchivukula", "Password1", "lfmd");
            return client;
        }


        async public System.Threading.Tasks.Task<JsonRecord> ValidateUser(string userId, string password)
        {
            JsonRecord jsonRecord = new JsonRecord();
            try
            {
                var result = await client.validateUserAsync(null, userId, password);
                if (result != null)
                {
                    jsonRecord.Add("parmUserID", result.response.parmUserID);
                    jsonRecord.Add("parmCompany", result.response.parmCompany);
                    jsonRecord.Add("parmCompanyName", result.response.parmCompanyName);
                    jsonRecord.Add("parmUserName", result.response.parmUserName);
                }
               else
                {
                    jsonRecord.Add("loginFail", "Whoa! The entered password is incorrect, please verify the password you entered.");
                }
            }
            catch (Exception)
            {
                throw;
            }
            return jsonRecord;
        }

        public JsonRecords GetTasks()
        {
            try
            {
                var result = client.getTasksOptimize(new SSProxy.CallContext() { Company = "1000"},"rchivukula","1000");
                JsonRecords jsonRecords = new JsonRecords();
                if (result != null)
                {
                    result.AsParallel().ForAll(mzkTask =>
                    {
                        JsonRecord jsonRecord = new JsonRecord();
                        jsonRecord.Add("CaseID", mzkTask.parmCaseID);
                        jsonRecord.Add("CustAddress", mzkTask.parmCustAddress);
                        jsonRecord.Add("CustName", mzkTask.parmCustName);
                        jsonRecord.Add("CustPhone", mzkTask.parmCustPhone);
                        jsonRecord.Add("Status", mzkTask.parmStatus);
                        jsonRecord.Add("StatusDueDate", mzkTask.parmStatusDueDate);
                        jsonRecord.Add("RegistrationNum", mzkTask.parmRegistrationNum);
                        //jsonRecord.Add("Allocatedto",_userInfo.Name);
                        jsonRecord.Add("UserID", mzkTask.parmUserID);
                        jsonRecord.Add("CaseCategory", mzkTask.parmCaseCategory);
                        jsonRecord.Add("CaseServiceRecID", mzkTask.parmCaseServiceRecID);
                        jsonRecord.Add("DriverFirstName", mzkTask.parmDriverFirstName);
                        jsonRecord.Add("DriverLastName", mzkTask.parmDriverLastName);
                        jsonRecord.Add("DriverPhone", mzkTask.parmDriverPhone);
                        jsonRecord.Add("Make", mzkTask.parmMake);
                        jsonRecord.Add("Model", mzkTask.parmModel);
                        jsonRecord.Add("VehicleDescription", mzkTask.parmVehicleDescription);
                        //jsonRecord.Add("ScheduledDate",mzkTask.paramScheduledDate);
                        jsonRecord.Add("Email", mzkTask.parmEmail);
                        // jsonRecord.Add("ScheduledTime",mzkTask.paramScheduledTime);
                        jsonRecord.Add("ServiceRecID", mzkTask.parmServiceRecID);

                        jsonRecords.Add(jsonRecord);

                    });

                }
                return jsonRecords;
            }
            catch (Exception)
            {
                throw;
            }
        }



        object IDataProvider.GetService()
        {
            throw new NotImplementedException();
        }
    }
}
