using Eqstra.BusinessLogic.Enums;
using Eqstra.BusinessLogic.Helpers;
using Eqstra.DocumentDelivery.UILogic.AifServices;
using Eqstra.DocumentDelivery.UILogic.Helpers;
using Eqstra.DocumentDelivery.UILogic.Services;
using Eqstra.DocumentDelivery.UILogic.ViewModels;
using Eqstra.DocumentDelivery.Views;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.ApplicationSettings;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=234227

namespace Eqstra.DocumentDelivery
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : MvvmAppBase
    {
        private readonly IUnityContainer _container = new UnityContainer();
        public IEventAggregator EventAggregator { get; set; }
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
        }

        protected override void OnRegisterKnownTypesForSerialization()
        {
            base.OnRegisterKnownTypesForSerialization();
            SessionStateService.RegisterKnownType(typeof(Eqstra.BusinessLogic.UserInfo));
            SessionStateService.RegisterKnownType(typeof(Eqstra.BusinessLogic.Customer));
            SessionStateService.RegisterKnownType(typeof(Eqstra.BusinessLogic.LoggedInUser));
            SessionStateService.RegisterKnownType(typeof(Eqstra.BusinessLogic.Helpers.TaskStatus));
            SessionStateService.RegisterKnownType(typeof(Syncfusion.UI.Xaml.Schedule.ScheduleAppointmentCollection));
            SessionStateService.RegisterKnownType(typeof(Eqstra.BusinessLogic.CustomerDetails));
            SessionStateService.RegisterKnownType(typeof(ObservableCollection<Eqstra.BusinessLogic.CollectDeliveryTask>));
            SessionStateService.RegisterKnownType(typeof(List<string>));
        }
        async protected override System.Threading.Tasks.Task OnLaunchApplication(LaunchActivatedEventArgs args)
        {
            var accountService = _container.Resolve<IAccountService>();
            var result = await accountService.VerifyUserCredentialsAsync();

            if (result != null)
            {
                await DDServiceProxyHelper.Instance.ConnectAsync("rchivukula", "P@ssword4");
                var rs = await DDServiceProxyHelper.Instance.ValidateUser("rchivukula", "P@ssword4");
                PersistentData.RefreshInstance();//Here only setting data in new instance, and  getting data in every page.
                PersistentData.Instance.UserInfo = new BusinessLogic.DeliveryModel.CDUserInfo();
                PersistentData.Instance.UserInfo.CompanyId = rs.response.parmCompany;
                PersistentData.Instance.UserInfo.CompanyName = rs.response.parmCompanyName;
                PersistentData.Instance.UserInfo.UserId = rs.response.parmUserID;
                PersistentData.Instance.UserInfo.Name = rs.response.parmUserName;
                PersistentData.Instance.UserInfo.CDUserType = CDUserType.Driver; // it is only for testing.
                NavigationService.Navigate("Main", string.Empty);
            }
            else
            {
                NavigationService.Navigate("Login", args.Arguments);
            }
            Window.Current.Activate();
        }

        async protected override void OnInitialize(IActivatedEventArgs args)
        {
            base.OnInitialize(args);
            EventAggregator = new EventAggregator();

            var db = await ApplicationData.Current.RoamingFolder.TryGetItemAsync("SQLiteDB\\eqstramobility.sqlite") as StorageFile;
            if (db == null)
            {
                var packDb = await Package.Current.InstalledLocation.GetFileAsync("SqliteDB\\eqstramobility.sqlite");
                // var packDb = await sqliteDBFolder.GetFileAsync("eqstramobility.sqlite");
                await packDb.CopyAsync(await ApplicationData.Current.RoamingFolder.CreateFolderAsync("SQLiteDB"));
            }
            SqliteHelper.Storage.ConnectionDatabaseAsync();
            _container.RegisterInstance(NavigationService);
            _container.RegisterInstance(EventAggregator);
            _container.RegisterInstance(SessionStateService);

            _container.RegisterType<IAccountService, AccountService>(new ContainerControlledLifetimeManager());
            _container.RegisterType<ICredentialStore, RoamingCredentialStore>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IIdentityService, IdentityServiceProxy>(new ContainerControlledLifetimeManager());

            _container.RegisterType<SettingsFlyout, AddCustomerPage>(new ContainerControlledLifetimeManager());

            //ViewModelLocator.Register(typeof(CollectionOrDeliveryDetailsPage).ToString(), () => new CollectionOrDeliveryDetailsPageViewModel(this.NavigationService, this.EventAggregator, new AddCustomerPage()));
            //ViewModelLocator.Register(typeof(BriefDetailsUserControlViewModel).ToString(), () => new BriefDetailsUserControlViewModel(this.EventAggregator));

            ViewModelLocator.SetDefaultViewTypeToViewModelTypeResolver((viewType) =>
            {
                var viewModelTypeName = string.Format(CultureInfo.InvariantCulture, "Eqstra.DocumentDelivery.UILogic.ViewModels.{0}ViewModel,Eqstra.DocumentDelivery.UILogic,Version 1.0.0.0, Culture=neutral", viewType.Name);
                return Type.GetType(viewModelTypeName);
            });


        }
        protected override IList<Windows.UI.ApplicationSettings.SettingsCommand> GetSettingsCommands()
        {
            var settingsCommands = new List<SettingsCommand>();
            var accountService = _container.Resolve<IAccountService>();
            if (accountService.SignedInUser != null)
            {
                settingsCommands.Add(new SettingsCommand("resetpassword", "Reset Password", (handler) =>
                {
                    ResetPasswordPage page = new ResetPasswordPage();
                    page.Show();
                }));
            }
            settingsCommands.Add(new SettingsCommand("privacypolicy", "Privacy Policy", (handler) =>
            {

            }));
            settingsCommands.Add(new SettingsCommand("help", "Help", (handler) =>
            {

            }));

            return settingsCommands;
        }
        protected override object Resolve(Type type)
        {
            return _container.Resolve(type);
        }
    }
}
