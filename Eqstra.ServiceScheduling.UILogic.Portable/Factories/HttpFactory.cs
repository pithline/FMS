using Eqstra.BusinessLogic.Portable.SSModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.Web.Http;

namespace Eqstra.ServiceScheduling.UILogic.Portable.Factories
{
    public class HttpFactory : IHttpFactory
    {
        async public Task<Windows.Web.Http.HttpResponseMessage> PostAsync(Windows.Web.Http.HttpStringContent data)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {

                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new Windows.Web.Http.Headers.HttpMediaTypeWithQualityHeaderValue("application/json"));
                    httpClient.DefaultRequestHeaders.Accept.Add(new Windows.Web.Http.Headers.HttpMediaTypeWithQualityHeaderValue("Content-Type: application/x-www-form-urlencoded"));
                    var token = JsonConvert.DeserializeObject<AccessToken>(ApplicationData.Current.RoamingSettings.Values[Constants.ACCESSTOKEN].ToString());
                    httpClient.DefaultRequestHeaders.Authorization = new Windows.Web.Http.Headers.HttpCredentialsHeaderValue("Bearer", token.Access_Token);

                    return await httpClient.PostAsync(new Uri(Constants.APIURL), data);
                }
            }
            catch (Exception ex)
            {
                new MessageDialog(ex.Message).ShowAsync();
                return null;
            }
        }
    }
}
