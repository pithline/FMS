using Eqstra.BusinessLogic;
using Eqstra.BusinessLogic.Helpers;
using Eqstra.DocumentDelivery.UILogic.Services;
using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Networking.Connectivity;
using Windows.Storage;


namespace Eqstra.DocumentDelivery.UILogic.ViewModels
{
    public class ProfileUserControlViewModel : ViewModel
    {
        INavigationService _navigationService;
        IAccountService _accountService;
        public ProfileUserControlViewModel(INavigationService navigationService,IAccountService accountService)
        {
            _navigationService = navigationService;
            _accountService = accountService;

            LogoutCommand = new DelegateCommand(() =>
            {
                _accountService.SignOut();
                _navigationService.Navigate("Login", string.Empty);
                _navigationService.ClearHistory();
            });
        }

        public ICommand LogoutCommand { get; private set; }
    }
}
