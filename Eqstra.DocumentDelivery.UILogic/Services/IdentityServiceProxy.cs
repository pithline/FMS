using Eqstra.BusinessLogic;
using Eqstra.BusinessLogic.DeliveryModel;
using Eqstra.BusinessLogic.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eqstra.DocumentDelivery.UILogic.Services
{
   public class IdentityServiceProxy : IIdentityService
    {
        async public Task<Tuple<CDLogonResult,string>> LogonAsync(string userId, string password)
        {
            var isUserExists = (await SqliteHelper.Storage.LoadTableAsync<LoggedInUser>()).Any(x => x.UserId == userId);
            if (isUserExists)
            {
                var result = await SqliteHelper.Storage.GetSingleRecordAsync<LoggedInUser>(x => (x.UserId == userId && x.Password == password));
                if (result != null)
                {
                    return new Tuple<CDLogonResult, string>(new CDLogonResult { UserInfo = new CDUserInfo { UserId = result.UserId } }, "");
                }
                else
                {
                    return new Tuple<CDLogonResult, string>(null, "Whoa! The entered password is incorrect, please verify the password you entered.");
                } 
            }
            else
            {
                return new Tuple<CDLogonResult, string>(null, "Whoa! No such user exists, verfiy if the entered userid is correct.");
            }
        }

        public Task<bool> VerifyAcitveSessionAsync(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
