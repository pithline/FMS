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
    [DataProvider(Name="ServiceScheduling")]
    
    class ServiceSchedulingProvider : IDataProvider
    {
        SSProxy.MzkServiceSchedulingServiceClient client;
        public System.Collections.IList GetDataList(object[] criterias)
        {
            GetService();
            List<object> list = new List<object>();

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
    }
}
