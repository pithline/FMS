using Eqstra.BusinessLogic;
using Eqstra.BusinessLogic.Helpers;
using Eqstra.ServiceScheduling.UILogic.Services;
using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage;

namespace Eqstra.ServiceScheduling.UILogic.ViewModels
{
    public class ProfileUserControlViewModel : BaseViewModel
    {
        INavigationService _navigationService;
        IAccountService _accountService;
        public ProfileUserControlViewModel(INavigationService navigationService, IAccountService accountService)
            : base(navigationService)
        {
            _navigationService = navigationService;
            _accountService = accountService;
            UserInfo = JsonConvert.DeserializeObject<UserInfo>(ApplicationData.Current.RoamingSettings.Values[Constants.UserInfo].ToString());
            LogoutCommand = new DelegateCommand(() =>
            {
                _accountService.SignOut();
                _navigationService.Navigate("Login", string.Empty);
                _navigationService.ClearHistory();
            });
        }
        public ICommand LogoutCommand { get; private set; }

        private UserInfo userInfo;

        public UserInfo UserInfo
        {
            get { return userInfo; }
            set { SetProperty(ref userInfo, value); }
        }

    }
}
