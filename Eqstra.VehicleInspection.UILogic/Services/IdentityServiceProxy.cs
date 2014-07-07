using Eqstra.BusinessLogic;
using Eqstra.BusinessLogic.Helpers;
using Eqstra.VehicleInspection.UILogic.AifServices;
using Microsoft.Practices.Prism.PubSubEvents;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Eqstra.VehicleInspection.UILogic.Services
{
    public class IdentityServiceProxy : IIdentityService
    {
        IEventAggregator _eventAggregator;
        public IdentityServiceProxy(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }
        async public Task<Tuple<LogonResult, string>> LogonAsync(string userId, string password)
        {
            try
            {
                await VIServiceHelper.Instance.ConnectAsync(userId.Trim(), password.Trim(),_eventAggregator);
                var result = await VIServiceHelper.Instance.ValidateUser(userId.Trim(), password.Trim());
                if (result != null)
                {
                    var userInfo = new UserInfo
                        {
                            UserId = result.response.parmUserID,
                            CompanyId = result.response.parmCompany,
                            CompanyName = result.response.parmCompanyName,
                            Name = result.response.parmUserName
                        };
                    string jsonUserInfo = JsonConvert.SerializeObject(userInfo);
                    ApplicationData.Current.RoamingSettings.Values[Constants.UserInfo] = jsonUserInfo;
                    return new Tuple<LogonResult, string>(new LogonResult
                    {
                        UserInfo = userInfo

                    }, "");
                }
                else
                {
                    return new Tuple<LogonResult, string>(null, "Whoa! The entered password is incorrect, please verify the password you entered.");
                }
            }
            catch (Exception)
            {
                return new Tuple<LogonResult, string>(null, "Whoa! The entered password is incorrect, please verify the password you entered.");
            }
        }

        public Task<bool> VerifyAcitveSessionAsync(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
