﻿using Eqstra.BusinessLogic;
using Eqstra.BusinessLogic.Helpers;
using Eqstra.BusinessLogic.ServiceSchedule;
using Eqstra.ServiceScheduling.UILogic.Services;
using Eqstra.ServiceScheduling.UILogic.ViewModels;
using Eqstra.ServiceScheduling.Views;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Reflection;
using System.Text;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Storage;
using Windows.UI.ApplicationSettings;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=234227

namespace Eqstra.ServiceScheduling
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
            this.UnhandledException += App_UnhandledException;

        }

        void App_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            e.Handled = true;
        }

        protected override void OnRegisterKnownTypesForSerialization()
        {

            base.OnRegisterKnownTypesForSerialization();
            SessionStateService.RegisterKnownType(typeof(Eqstra.BusinessLogic.UserInfo));
            SessionStateService.RegisterKnownType(typeof(Eqstra.BusinessLogic.LoggedInUser));
            SessionStateService.RegisterKnownType(typeof(Eqstra.BusinessLogic.ImageCapture));
            SessionStateService.RegisterKnownType(typeof(ObservableCollection<ImageCapture>));
            SessionStateService.RegisterKnownType(typeof(Eqstra.BusinessLogic.Enums.CaseTypeEnum));
            SessionStateService.RegisterKnownType(typeof(Eqstra.BusinessLogic.Helpers.TaskStatus));
            SessionStateService.RegisterKnownType(typeof(Syncfusion.UI.Xaml.Schedule.ScheduleAppointmentCollection));
            SessionStateService.RegisterKnownType(typeof(Eqstra.BusinessLogic.CustomerDetails));
            SessionStateService.RegisterKnownType(typeof(ObservableCollection<Eqstra.BusinessLogic.ServiceSchedule.DriverTask>));
            SessionStateService.RegisterKnownType(typeof(City));
            SessionStateService.RegisterKnownType(typeof(Country));
            SessionStateService.RegisterKnownType(typeof(Region));
            SessionStateService.RegisterKnownType(typeof(Suburb));
            SessionStateService.RegisterKnownType(typeof(Province));
            SessionStateService.RegisterKnownType(typeof(Supplier));

            SessionStateService.RegisterKnownType(typeof(List<City>));
            SessionStateService.RegisterKnownType(typeof(List<Country>));
            SessionStateService.RegisterKnownType(typeof(List<Region>));
            SessionStateService.RegisterKnownType(typeof(List<Suburb>));
            SessionStateService.RegisterKnownType(typeof(List<Province>));
            SessionStateService.RegisterKnownType(typeof(List<Supplier>));
            SessionStateService.RegisterKnownType(typeof(LogonResult));
            SessionStateService.RegisterKnownType(typeof(List<string>));

        }
        async protected override System.Threading.Tasks.Task OnLaunchApplication(LaunchActivatedEventArgs args)
        {
           // GenerateModalForHybApp();
            var db = await ApplicationData.Current.RoamingFolder.TryGetItemAsync("SQLiteDB\\eqstramobility.sqlite") as StorageFile;
            if (db == null)
            {
                var packDb = await Package.Current.InstalledLocation.GetFileAsync("SqliteDB\\eqstramobility.sqlite");
                // var packDb = await sqliteDBFolder.GetFileAsync("eqstramobility.sqlite");
                await packDb.CopyAsync(await ApplicationData.Current.RoamingFolder.CreateFolderAsync("SQLiteDB"));
            }
            SqliteHelper.Storage.ConnectionDatabaseAsync();

            var accountService = _container.Resolve<IAccountService>();
            var cred = await accountService.VerifyUserCredentialsAsync();
            if (cred != null && ApplicationData.Current.RoamingSettings.Values.ContainsKey(Constants.UserInfo))
            { 
                NavigationService.Navigate("Main", string.Empty);
            }
            else
            {
                NavigationService.Navigate("Login", args.Arguments);
            }
            Window.Current.Activate();

            
        }

         protected override void OnInitialize(IActivatedEventArgs args)
        {
            base.OnInitialize(args);
            EventAggregator = new EventAggregator();

            
            _container.RegisterInstance(NavigationService);
            _container.RegisterInstance(EventAggregator);
            _container.RegisterInstance(SessionStateService);

            _container.RegisterType<IAccountService, AccountService>(new ContainerControlledLifetimeManager());
            _container.RegisterType<ICredentialStore, RoamingCredentialStore>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IIdentityService, IdentityServiceProxy>(new ContainerControlledLifetimeManager());

            _container.RegisterType<SettingsFlyout, AddAddressFlyoutPage>(new ContainerControlledLifetimeManager());

            ViewModelLocator.Register(typeof(ServiceSchedulingPage).ToString(), () => new ServiceSchedulingPageViewModel(this.NavigationService,this.EventAggregator, new AddAddressFlyoutPage()));
            ViewModelLocator.SetDefaultViewTypeToViewModelTypeResolver((viewType) =>
            {
                var viewModelTypeName = string.Format(CultureInfo.InvariantCulture, "Eqstra.ServiceScheduling.UILogic.ViewModels.{0}ViewModel,Eqstra.ServiceScheduling.UILogic,Version 1.0.0.0, Culture=neutral", viewType.Name);
                return Type.GetType(viewModelTypeName);
            });
            //ViewModelLocator.SetDefaultViewTypeToViewModelTypeResolver((viewType) =>
            //    {
            //        var viewModelTypeName = string.Format(CultureInfo.InvariantCulture, "Eqstra.VehicleInspection.UILogic.ViewModels.Passenger.{0}ViewModel,Eqstra.VehicleInspection.UILogic,Version 1.0.0.0, Culture=neutral", viewType.Name);
            //        return Type.GetType(viewModelTypeName);
            //    });
        }

        protected override object Resolve(Type type)
        {
            return _container.Resolve(type);
        }

        protected override IList<Windows.UI.ApplicationSettings.SettingsCommand> GetSettingsCommands()
        {
            var settingsCommands = new List<SettingsCommand>();
            var accountService = _container.Resolve<IAccountService>();

            if (accountService.SignedInUser != null)
            {
                //settingsCommands.Add(new SettingsCommand("resetpassword", "Reset Password", (handler) =>
                //{
                //    ResetPasswordPage page = new ResetPasswordPage();
                //    page.Show();
                //}));
            }

            settingsCommands.Add(new SettingsCommand("privacypolicy", "Privacy Policy", (handler) =>
            {

            }));
            settingsCommands.Add(new SettingsCommand("help", "Help", (handler) =>
            {

            }));
            // args.Request.ApplicationCommands.Add(command); 

            return settingsCommands;
        }


    }
}
