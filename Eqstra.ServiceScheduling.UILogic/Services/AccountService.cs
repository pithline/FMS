using Eqstra.BusinessLogic;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eqstra.ServiceScheduling.UILogic.Services
{
   public class AccountService : IAccountService
    {
       public const string SignedInUserKey = "AccountService_UserInfo";
       ISessionStateService _sessionStateService;
       ICredentialStore _credentialStore;
       UserInfo _signedInUser;
       public AccountService(ISessionStateService sessionStateService,ICredentialStore credentialStore)
       {
           _sessionStateService = sessionStateService;
           _credentialStore = credentialStore;
           if (_sessionStateService.SessionState.ContainsKey(SignedInUserKey))
           {
               _signedInUser = _sessionStateService.SessionState[SignedInUserKey] as UserInfo;
           }
          
       }
        public BusinessLogic.UserInfo SignedInUser
        {
            get { return _signedInUser; }
        }

        public void SignInAsync()
        {
            throw new NotImplementedException();
        }
    }
}
