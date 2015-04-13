using Eqstra.BusinessLogic.Portable;
using Eqstra.BusinessLogic.Portable.TIModels;
using Eqstra.TechnicalInspection.UILogic.WindowsPhone.Factories;
using Eqstra.TechnicalInspection.UILogic.WindowsPhone.Services;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using System;
using System.Globalization;
using Windows.ApplicationModel.Activation;
using Windows.Storage;
using Windows.UI.Xaml;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace Eqstra.TechnicalInspection.WindowsPhone
{
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

            SessionStateService.RegisterKnownType(typeof(Task));
            SessionStateService.RegisterKnownType(typeof(UserInfo));
            SessionStateService.RegisterKnownType(typeof(UserInfo));
            SessionStateService.RegisterKnownType(typeof(ImageCapture));

            EventAggregator = new EventAggregator();

            _container.RegisterInstance(NavigationService);
            _container.RegisterInstance(EventAggregator);
            _container.RegisterInstance(SessionStateService);


            //Register Services

            _container.RegisterType<ITaskService, TaskService>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IUserService, UserService>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IHttpFactory, HttpFactory>(new ContainerControlledLifetimeManager());


            ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver((viewType) =>
            {
                var viewModelTypeName = string.Format(CultureInfo.InvariantCulture, "Eqstra.TechnicalInspection.UILogic.WindowsPhone.ViewModels.{0}ViewModel,Eqstra.TechnicalInspection.UILogic.WindowsPhone.ViewModels, Version=1.0.0.0, Culture=neutral", viewType.Name);

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

            if (ApplicationData.Current.RoamingSettings.Values.ContainsKey(Constants.ACCESSTOKEN))
            {
                var accessToken = JsonConvert.DeserializeObject<AccessToken>(ApplicationData.Current.RoamingSettings.Values[Constants.ACCESSTOKEN].ToString());
                if (accessToken.ExpirationDate > DateTime.Now)
                {
                    NavigationService.Navigate("Main", string.Empty);
                }
                else
                {
                    NavigationService.Navigate("Login", args.Arguments);
                }
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