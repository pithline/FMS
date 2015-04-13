using Eqstra.BusinessLogic.Portable;
using Eqstra.BusinessLogic.Portable.SSModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eqstra.ServiceScheduling.UILogic.Portable.Services
{
    public interface IUserService
    {
        Task<UserInfo> GetUserInfoAsync(string userId);

        Task<AccessToken> ValidateUserAsync(string userName, string password);
        
    }
}
