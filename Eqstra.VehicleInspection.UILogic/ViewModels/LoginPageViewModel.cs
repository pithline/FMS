using Eqstra.VehicleInspection.UILogic.Services;
using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Eqstra.VehicleInspection.UILogic.ViewModels
{
    public class LoginPageViewModel : ViewModel
    {
        private INavigationService _navigationService;
        private IAccountService _accountService;
        public LoginPageViewModel(INavigationService navigationService, IAccountService accountService)
        {
            _navigationService = navigationService;
            _accountService = accountService;

            LoginCommand = DelegateCommand.FromAsyncHandler(
                async () =>
                {
                    var result = await _accountService.SignInAsync(this.UserName, this.Password, this.ShouldSaveCredential);
                    if (result.Item1 != null)
                    {
                        navigationService.Navigate("Main", result.Item1);
                    }
                    else
                    {
                        ErrorMessage = result.Item2;
                    }
                },

                () => { return !string.IsNullOrEmpty(this.username) && !string.IsNullOrEmpty(this.password); });

        }

        async public override void OnNavigatedTo(object navigationParameter, Windows.UI.Xaml.Navigation.NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
        }

        public DelegateCommand LoginCommand { get; private set; }

        private string username;
        [RestorableState]
        public string UserName
        {
            get { return username; }
            set
            {
                if (SetProperty(ref username, value))
                {
                    LoginCommand.RaiseCanExecuteChanged();
                }
            }
        }

        private string password;
        [RestorableState]
        public string Password
        {
            get { return password; }
            set
            {
                if (SetProperty(ref password, value))
                {
                    LoginCommand.RaiseCanExecuteChanged();
                }
            }
        }

        private bool shouldSaveCredential;
        [RestorableState]
        public bool ShouldSaveCredential
        {
            get { return shouldSaveCredential; }
            set { SetProperty(ref shouldSaveCredential, value); }
        }

        private string errorMessage;
        [RestorableState]
        public string ErrorMessage
        {
            get { return errorMessage; }
            set { SetProperty(ref errorMessage, value); }
        }

    }
}
