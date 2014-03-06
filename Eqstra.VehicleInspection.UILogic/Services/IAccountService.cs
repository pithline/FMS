using Eqstra.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eqstra.VehicleInspection.UILogic.Services
{
    public interface IAccountService
    {
        Task<Tuple<UserInfo, string>> SignInAsync(string userId, string password, bool isCredentialStore);

        UserInfo SignedInUser { get; }

        Task<UserInfo> VerifyUserCredentialsAsync();

        void SignOut();

        event EventHandler<Eqstra.BusinessLogic.EventArgs.UserChangedEventArgs> UserChanged;

    }
}
