using Eqstra.BusinessLogic;
using Eqstra.BusinessLogic.DeliveryModel;
using Eqstra.BusinessLogic.Enums;
using Eqstra.BusinessLogic.Helpers;
using Eqstra.DocumentDelivery.UILogic.AifServices;
using Eqstra.DocumentDelivery.UILogic.Helpers;
using Microsoft.Practices.Prism.PubSubEvents;
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
        IEventAggregator _eventAggregator;
        public IdentityServiceProxy(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }
        async public Task<Tuple<CDLogonResult, string>> LogonAsync(string userId, string password)
        {
            try
            {
                await DDServiceProxyHelper.Instance.ConnectAsync(userId.Trim(), password.Trim(), _eventAggregator);
                var userInfo = await DDServiceProxyHelper.Instance.ValidateUser(userId.Trim(), password.Trim());
                if (userInfo == null)
                {
                    return new Tuple<CDLogonResult, string>(null, "Whoa! The entered password is incorrect, please verify the password you entered.");
                }
                else if (!String.IsNullOrWhiteSpace(userInfo.UserId))
                {
                    string jsonUserInfo = JsonConvert.SerializeObject(userInfo);
                    ApplicationData.Current.RoamingSettings.Values[Constants.UserInfo] = jsonUserInfo;
                    return new Tuple<CDLogonResult, string>(new CDLogonResult
                    {
                        UserInfo = userInfo

                    }, "");
                }
                else
                {
                    return new Tuple<CDLogonResult, string>(null, "Whoa! The entered username or password is incorrect,  please verify the password you entered");
                }
            }
            catch (Exception)
            {
                return new Tuple<CDLogonResult, string>(null, "Whoa! The entered username or password is incorrect, please verify the password you entered.");
            }
        }

        public Task<bool> VerifyAcitveSessionAsync(string userId)
        {
            throw new NotImplementedException();
        }

    }
}
