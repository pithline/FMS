﻿using Eqstra.BusinessLogic;
using Eqstra.BusinessLogic.Helpers;
using Eqstra.ServiceScheduling.UILogic.AifServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Eqstra.ServiceScheduling.UILogic.Services
{
    public class IdentityServiceProxy : IIdentityService
    {
        async public Task<Tuple<LogonResult, string>> LogonAsync(string userId, string password)
        {
            await SSProxyHelper.Instance.ConnectAsync(userId.Trim(), password.Trim());
            var result = await SSProxyHelper.Instance.ValidateUser(userId.Trim(), password.Trim());
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

        public Task<bool> VerifyAcitveSessionAsync(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
