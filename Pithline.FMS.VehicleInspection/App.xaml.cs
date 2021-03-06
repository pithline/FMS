﻿using Microsoft.Practices.Prism.StoreApps;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Microsoft.Practices.Unity;

using Windows.UI.Xaml.Navigation;
using Windows.Storage;
using Eqstra.BusinessLogic.Helpers;
using Eqstra.VehicleInspection.Views;
using Eqstra.VehicleInspection.ViewModels;
using Windows.UI.ApplicationSettings;
using Microsoft.Practices.Prism.PubSubEvents;
using Eqstra.VehicleInspection.UILogic.Services;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using Windows.UI.Popups;
using System.Collections.ObjectModel;
using Eqstra.BusinessLogic;
using Eqstra.VehicleInspection.UILogic.ViewModels;
using Eqstra.VehicleInspection.Common;
using Eqstra.VehicleInspection.UILogic;
using Newtonsoft.Json;
using Eqstra.VehicleInspection.UILogic.AifServices;
using Windows.Networking.Connectivity;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=234227

namespace Eqstra.VehicleInspection
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : MvvmAppBase, IDisposable
    {
        public static Eqstra.BusinessLogic.Task Task { get; set; }
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
            SessionStateService.RegisterKnownType(typeof(Eqstra.BusinessLogic.Task));
            SessionStateService.RegisterKnownType(typeof(Eqstra.BusinessLogic.Customer));
            SessionStateService.RegisterKnownType(typeof(Eqstra.BusinessLogic.LoggedInUser));
            SessionStateService.RegisterKnownType(typeof(Eqstra.BusinessLogic.Passenger.PAccessories));
            SessionStateService.RegisterKnownType(typeof(Eqstra.BusinessLogic.Passenger.PBodywork));
            SessionStateService.RegisterKnownType(typeof(Eqstra.BusinessLogic.ImageCapture));
            SessionStateService.RegisterKnownType(typeof(ObservableCollection<ImageCapture>));
            SessionStateService.RegisterKnownType(typeof(Eqstra.BusinessLogic.Passenger.PGlass));
            SessionStateService.RegisterKnownType(typeof(Eqstra.BusinessLogic.Passenger.PInspectionProof));
            SessionStateService.RegisterKnownType(typeof(Eqstra.BusinessLogic.Enums.CaseTypeEnum));
            SessionStateService.RegisterKnownType(typeof(Eqstra.BusinessLogic.Helpers.TaskStatus));
            SessionStateService.RegisterKnownType(typeof(Syncfusion.UI.Xaml.Schedule.ScheduleAppointmentCollection));
            SessionStateService.RegisterKnownType(typeof(Eqstra.BusinessLogic.Enums.VehicleTypeEnum));
            SessionStateService.RegisterKnownType(typeof(Eqstra.BusinessLogic.CustomerDetails));
            SessionStateService.RegisterKnownType(typeof(ObservableCollection<Eqstra.BusinessLogic.Task>));
            SessionStateService.RegisterKnownType(typeof(LogonResult));
            SessionStateService.RegisterKnownType(typeof(List<string>));
            
        }

        async protected override System.Threading.Tasks.Task OnLaunchApplication(LaunchActivatedEventArgs args)
        {

            var db = await ApplicationData.Current.RoamingFolder.TryGetItemAsync("SQLiteDB\\eqstramobility.sqlite") as StorageFile;
            if (db == null)
            {
                var packDb = await Package.Current.InstalledLocation.GetFileAsync("SqliteDB\\eqstramobility.sqlite");
                // var packDb = await sqliteDBFolder.GetFileAsync("eqstramobility.sqlite");
                var destinationFolder = await ApplicationData.Current.RoamingFolder.CreateFolderAsync("SQLiteDB", CreationCollisionOption.ReplaceExisting);
                await packDb.CopyAsync(destinationFolder);
            }
            SqliteHelper.Storage.ConnectionDatabaseAsync();
            var accountService = _container.Resolve<IAccountService>();
            
            var cred =  accountService.VerifyUserCredentialsAsync();
            if (cred != null && ApplicationData.Current.RoamingSettings.Values.ContainsKey(Constants.UserInfo))
            {
                //string jsonUserInfo = JsonConvert.SerializeObject(userInfo);
                //ApplicationData.Current.RoamingSettings.Values[Constants.UserInfo] = jsonUserInfo;
                VIServiceHelper.Instance.ConnectAsync(cred.Item1,cred.Item2,EventAggregator);
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
            try
            {
                base.OnInitialize(args);
                EventAggregator = new EventAggregator();

                _container.RegisterInstance(NavigationService);
                _container.RegisterInstance(EventAggregator);

                _container.RegisterInstance(SessionStateService);

                _container.RegisterType<IAccountService, AccountService>(new ContainerControlledLifetimeManager());
                _container.RegisterType<ICredentialStore, RoamingCredentialStore>(new ContainerControlledLifetimeManager());
                _container.RegisterType<IIdentityService, IdentityServiceProxy>(new ContainerControlledLifetimeManager());

                ViewModelLocator.Register(typeof(VehicleInspectionPage).ToString(), () => new VehicleInspectionPageViewModel(NavigationService,EventAggregator));

                ViewModelLocator.SetDefaultViewTypeToViewModelTypeResolver((viewType) =>
                {
                    var viewModelTypeName = string.Format(CultureInfo.InvariantCulture, "Eqstra.VehicleInspection.UILogic.ViewModels.{0}ViewModel,Eqstra.VehicleInspection.UILogic,Version 1.0.0.0, Culture=neutral", viewType.Name);
                    return Type.GetType(viewModelTypeName);
                });
                //ViewModelLocator.SetDefaultViewTypeToViewModelTypeResolver((viewType) =>
                //    {
                //        var viewModelTypeName = string.Format(CultureInfo.InvariantCulture, "Eqstra.VehicleInspection.UILogic.ViewModels.Passenger.{0}ViewModel,Eqstra.VehicleInspection.UILogic,Version 1.0.0.0, Culture=neutral", viewType.Name);
                //        return Type.GetType(viewModelTypeName);
                //    });
            }
            catch (Exception ex)
            {
                new MessageDialog(ex.Message).ShowAsync();
            }

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
            // args.Request.ApplicationCommands.Add(command); 

            return settingsCommands;
        }

        public void Dispose()
        {
            this._container.Dispose();
        }
    }
}
