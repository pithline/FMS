using Eqstra.BusinessLogic;
using Eqstra.BusinessLogic.DeliveryModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eqstra.DocumentDelivery.UILogic.Services
{
    public interface IAccountService
    {
        CDUserInfo SignedInUser { get; }
        Task<Tuple<CDUserInfo, string>> SignInAsync(string userId, string password, bool isCredentialStore);
        void SignOut();

        event EventHandler<BusinessLogic.EventArgs.UserChangedEventArgs> UserChanged;

        Task<CDUserInfo> VerifyUserCredentialsAsync();

    }
}
