using Eqstra.BusinessLogic;
using Eqstra.BusinessLogic.DeliveryModel;
using Eqstra.BusinessLogic.Helpers;
using Eqstra.DocumentDelivery.UILogic.AifServices;
using Eqstra.DocumentDelivery.UILogic.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Eqstra.DocumentDelivery.UILogic.Services
{
   public class IdentityServiceProxy : IIdentityService
    {
        async public Task<Tuple<CDLogonResult,string>> LogonAsync(string userId, string password)
        {
            try
            {
                DDServiceProxyHelper.Instance.ConnectAsync(userId.Trim(), password.Trim());
                var result = await DDServiceProxyHelper.Instance.ValidateUser(userId.Trim(), password.Trim());
                if (result != null)
                {
                    var userInfo = new CDUserInfo
                    {
                        UserId = result.response.parmUserID,
                        CompanyId = result.response.parmCompany,
                        CompanyName = result.response.parmCompanyName,
                        Name = result.response.parmUserName
                    };
                    PersistentData.Instance.UserInfo = userInfo;
                    string jsonUserInfo = JsonConvert.SerializeObject(userInfo);
                    ApplicationData.Current.RoamingSettings.Values[Constants.UserInfo] = jsonUserInfo;
                    return new Tuple<CDLogonResult, string>(new CDLogonResult
                    {
                        UserInfo = userInfo

                    }, "");
                }
                else
                {
                    return new Tuple<CDLogonResult, string>(null, "Whoa! The entered username or password is incorrect, please verify and try again.");
                }
            }
            catch (Exception)
            {
                 return new Tuple<CDLogonResult, string>(null, "Whoa! The entered username or password is incorrect, please verify and try again.");
            }
        }

        public Task<bool> VerifyAcitveSessionAsync(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
