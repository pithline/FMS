using Eqstra.BusinessLogic.Portable.SSModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace Eqstra.ServiceScheduling.UILogic.Portable.Services
{
   public class ServiceDetailService : IServiceDetailService
    {
        public async Task<bool> InsertServiceDetailsAsync(BusinessLogic.Portable.SSModels.ServiceSchedulingDetail serviceSchedulingDetail, BusinessLogic.Portable.SSModels.Address address, BusinessLogic.Portable.SSModels.UserInfo userInfo)
        {
             using (var httpClient = new HttpClient())
            {
                userInfo = new UserInfo { UserId = "axbcsvc", CompanyId = "1095" };
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new Windows.Web.Http.Headers.HttpMediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Authorization = new Windows.Web.Http.Headers.HttpCredentialsHeaderValue("Bearer", Constants.TOKEN);
                var postData = new { target = "ServiceScheduling", parameters = new[] { "InsertServiceDetails",JsonConvert.SerializeObject(serviceSchedulingDetail), JsonConvert.SerializeObject(address) ,JsonConvert.SerializeObject(userInfo) } };
                var response = await httpClient.PostAsync(new Uri(Constants.APIURL), new HttpStringContent(JsonConvert.SerializeObject(postData)));
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    var tasks = await response.Content.ReadAsStringAsync();
                }

                return JsonConvert.DeserializeObject<bool>(await response.Content.ReadAsStringAsync());
            } 
        }

        public async Task<BusinessLogic.Portable.SSModels.ServiceSchedulingDetail> GetServiceDetailAsync(string caseNumber, long caseServiceRecId, long serviceRecId, BusinessLogic.Portable.SSModels.UserInfo userInfo)
        {
            using (var httpClient = new HttpClient())
            {
                userInfo = new UserInfo { UserId = "axbcsvc", CompanyId = "1095" };
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new Windows.Web.Http.Headers.HttpMediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Authorization = new Windows.Web.Http.Headers.HttpCredentialsHeaderValue("Bearer", Constants.TOKEN);
                var postData = new { target = "ServiceScheduling", parameters = new[] { "GetServiceDetails", caseNumber, caseServiceRecId.ToString(),serviceRecId.ToString(), JsonConvert.SerializeObject(userInfo) } };
                var response = await httpClient.PostAsync(new Uri(Constants.APIURL), new HttpStringContent(JsonConvert.SerializeObject(postData)));
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    var tasks = await response.Content.ReadAsStringAsync();
                }

                return JsonConvert.DeserializeObject<ServiceSchedulingDetail>(await response.Content.ReadAsStringAsync());
            } 
        }
    }
}
