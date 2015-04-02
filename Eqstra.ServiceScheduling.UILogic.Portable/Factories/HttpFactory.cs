using Eqstra.BusinessLogic.Portable.SSModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace Eqstra.ServiceScheduling.UILogic.Portable.Factories
{
   public class HttpFactory : IHttpFactory
    {
       async public Task<Windows.Web.Http.HttpResponseMessage> PostAsync(Windows.Web.Http.HttpStringContent data)
        {
            using (var httpClient = new HttpClient())
            {

                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new Windows.Web.Http.Headers.HttpMediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Accept.Add(new Windows.Web.Http.Headers.HttpMediaTypeWithQualityHeaderValue("Content-Type: application/x-www-form-urlencoded"));
                httpClient.DefaultRequestHeaders.Authorization = new Windows.Web.Http.Headers.HttpCredentialsHeaderValue("Bearer", Constants.TOKEN);
                
                return await httpClient.PostAsync(new Uri(Constants.APIURL),data);
            }
        }
    }
}
