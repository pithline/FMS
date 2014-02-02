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
    public class LoginPageViewModel : ViewModel
    {
        private INavigationService _navigationService;
        public LoginPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            LoginCommand = new DelegateCommand(() => 
            { navigationService.Navigate("Main",null); });

        }
        public ICommand LoginCommand { get;private set; }
    }
}
