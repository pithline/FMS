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
        public ProfileUserControlViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            LogoutCommand = new DelegateCommand(() =>
            {
                _navigationService.Navigate("Login", null);
                _navigationService.ClearHistory();
            });
        }

        public ICommand LogoutCommand { get; private set; }
    }
}
