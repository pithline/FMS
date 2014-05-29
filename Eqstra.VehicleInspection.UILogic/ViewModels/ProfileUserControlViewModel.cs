using Eqstra.VehicleInspection.UILogic.Services;
using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Eqstra.VehicleInspection.UILogic.ViewModels
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
                _navigationService.Navigate("Login", null);
                _navigationService.ClearHistory();
            });
        }
        public ICommand LogoutCommand { get; private set; }
    }
}
