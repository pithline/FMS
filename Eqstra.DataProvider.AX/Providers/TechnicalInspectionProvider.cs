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
using System.Threading.Tasks;

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
                GetService();
                System.Collections.IList response = null;
                switch (criterias[0].ToString())
                {
                    case Eqstra.DataProvider.AX.Helpers.ActionSwitch.GetTasks:
                        
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
            throw new System.NotImplementedException();
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

        


    }
}
