using Eqstra.BusinessLogic.Portable.SSModels;
using Eqstra.ServiceScheduling.UILogic.Portable.Services;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Eqstra.ServiceScheduling.UILogic.Portable
{
    public class LoginPageViewModel : ViewModel
    {

        private INavigationService _navigationService;
        public LoginPageViewModel(INavigationService navigationService, IUserService userService)
        {
            _navigationService = navigationService;
            ProgressDialogPopup = new ProgressDialog();
            LoginCommand = DelegateCommand.FromAsyncHandler(
                async () =>
                {
                    try
                    {

                        ProgressDialogPopup.Open(this);


                        var token = await userService.ValidateUserAsync(this.UserName, this.Password);

                        if (token != null)
                        {
                            ApplicationData.Current.RoamingSettings.Values[Constants.ACCESSTOKEN] = JsonConvert.SerializeObject(token);
                            var userInfo = await userService.GetUserInfoAsync(this.UserName);
                            if (userInfo != null)
                            {
                                ApplicationData.Current.RoamingSettings.Values[Constants.UserInfo] = JsonConvert.SerializeObject(userInfo);
                                navigationService.Navigate("Main", string.Empty);
                            }
                        }

                        ProgressDialogPopup.Close();

                    }
                    catch (Exception ex)
                    {
                        ProgressDialogPopup.Close();
                    }
                    finally
                    {
                        IsLoggingIn = false;
                    }
                },

                 () => { return true || !string.IsNullOrEmpty(this.username) && !string.IsNullOrEmpty(this.password); });

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
                    LoginCommand.RaiseCanExecuteChanged();
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
                    LoginCommand.RaiseCanExecuteChanged();
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

        private bool isLoggingIn;
        public bool IsLoggingIn
        {
            get { return isLoggingIn; }
            set { SetProperty(ref isLoggingIn, value); }
        }
        private ProgressDialog progressDialogPopup;

        public ProgressDialog ProgressDialogPopup
        {
            get { return progressDialogPopup; }
            set { SetProperty(ref progressDialogPopup, value); }
        }


    }
}
