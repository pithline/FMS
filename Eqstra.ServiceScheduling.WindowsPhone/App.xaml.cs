using Eqstra.BusinessLogic;
using Eqstra.BusinessLogic.Portable.SSModels;
using Eqstra.ServiceScheduling.UILogic.Portable.Factories;
using Eqstra.ServiceScheduling.UILogic.Portable.Services;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Globalization;
using Windows.ApplicationModel.Activation;
using Windows.Storage;
using Windows.UI.Xaml;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace Eqstra.ServiceScheduling.WindowsPhone
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public sealed partial class App : MvvmAppBase
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
        }

        protected override System.Threading.Tasks.Task OnInitializeAsync(IActivatedEventArgs args)
        {
            SessionStateService.RegisterKnownType(typeof(Country));
            SessionStateService.RegisterKnownType(typeof(Province));
            SessionStateService.RegisterKnownType(typeof(City));
            SessionStateService.RegisterKnownType(typeof(Suburb));
            SessionStateService.RegisterKnownType(typeof(Region));

            SessionStateService.RegisterKnownType(typeof(Task));
            SessionStateService.RegisterKnownType(typeof(ServiceSchedulingDetail));
            SessionStateService.RegisterKnownType(typeof(Supplier));
            SessionStateService.RegisterKnownType(typeof(DestinationType));
            SessionStateService.RegisterKnownType(typeof(LocationType));
            SessionStateService.RegisterKnownType(typeof(SupplierFilter));

            SessionStateService.RegisterKnownType(typeof(UserInfo));
            SessionStateService.RegisterKnownType(typeof(ImageCapture));

            EventAggregator = new EventAggregator();


            _container.RegisterInstance(NavigationService);
            _container.RegisterInstance(EventAggregator);
            _container.RegisterInstance(SessionStateService);


            //Register Services

            _container.RegisterType<ITaskService, TaskService>(new ContainerControlledLifetimeManager());
            _container.RegisterType<ISupplierService, SupplierService>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IServiceDetailService, ServiceDetailService>(new ContainerControlledLifetimeManager());
            _container.RegisterType<ILocationService, LocationService>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IUserService, UserService>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IHttpFactory, HttpFactory>(new ContainerControlledLifetimeManager());


            ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver((viewType) =>
            {
                var viewModelTypeName = string.Format(CultureInfo.InvariantCulture, "Eqstra.ServiceScheduling.UILogic.Portable.{0}ViewModel,Eqstra.ServiceScheduling.UILogic.Portable, Version=1.0.0.0, Culture=neutral", viewType.Name);

                return Type.GetType(viewModelTypeName);
            });
            return base.OnInitializeAsync(args);
        }


        protected override object Resolve(Type type)
        {
            return _container.Resolve(type);
        }


        protected override System.Threading.Tasks.Task OnLaunchApplicationAsync(LaunchActivatedEventArgs args)
        {
            if (ApplicationData.Current.RoamingSettings.Values.ContainsKey(Constants.UserInfo))
            {
                NavigationService.Navigate("Main", string.Empty);
            }
            else
            {
                NavigationService.Navigate("Login", args.Arguments);
            }
            Window.Current.Activate();
            return System.Threading.Tasks.Task.FromResult<object>(null);
        }
    }
}