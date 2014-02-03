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
            LoginCommand = DelegateCommand.FromAsyncHandler(
                () => { navigationService.Navigate("Main", null); return Task.FromResult<object>(null); },
                () => { return !string.IsNullOrEmpty(this.username) && !string.IsNullOrEmpty(this.password); });

        }
        public DelegateCommand LoginCommand { get;private set; }

        private string username;
        [RestorableState]
        public string UserName
        {
            get { return username; }
            set {
                if(SetProperty(ref username, value))
                    LoginCommand.RaiseCanExecuteChanged(); }
        }

        private string password;

        public string Password
        {
            get { return password; }
            set { 
                if(SetProperty(ref password, value))
                    LoginCommand.RaiseCanExecuteChanged(); }
        }


    }
}
