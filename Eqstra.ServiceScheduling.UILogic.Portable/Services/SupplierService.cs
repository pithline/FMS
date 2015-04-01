using Eqstra.BusinessLogic.Portable.SSModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace Eqstra.ServiceScheduling.UILogic.Portable.Services
{
   public class SupplierService : ISupplierService
    {
        public async Task<System.Collections.ObjectModel.ObservableCollection<BusinessLogic.Portable.SSModels.Supplier>> GetSuppliersByClassAsync(string classId, BusinessLogic.Portable.SSModels.UserInfo userInfo)
        {
            using (var httpClient = new HttpClient())
            {
                userInfo = new UserInfo { UserId = "axbcsvc", CompanyId = "1095" };
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new Windows.Web.Http.Headers.HttpMediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Authorization = new Windows.Web.Http.Headers.HttpCredentialsHeaderValue("Bearer", Constants.TOKEN);
                var postData = new { target = "ServiceScheduling", parameters = new[] { "GetSuppliersByClass",classId, Newtonsoft.Json.JsonConvert.SerializeObject(userInfo) } };
                var response = await httpClient.PostAsync(new Uri(Constants.APIURL), new HttpStringContent(JsonConvert.SerializeObject(postData)));
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    var tasks = await response.Content.ReadAsStringAsync();
                }

                return JsonConvert.DeserializeObject<ObservableCollection<Supplier>>(await response.Content.ReadAsStringAsync());
            }  
        }
    }
}
