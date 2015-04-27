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
using System.Linq;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;
using Eqstra.TechnicalInspection.UILogic;
using Windows.Storage.Pickers;
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
            Window.Current.Activated += Current_Activated;
        }

        void Current_Activated(object sender, Windows.UI.Core.WindowActivatedEventArgs e)
        {

        }

        async protected override void OnActivated(IActivatedEventArgs args)
        {
            if (args != null)
            {
                switch (args.Kind)
                {
                    case ActivationKind.PickFileContinuation:
                        var arguments = (FileOpenPickerContinuationEventArgs)args;
                        var selectedMaintenanceRepair = PersistentData.Instance.SelectedMaintenanceRepair;
                        StorageFile file = arguments.Files.FirstOrDefault();
                        if (file != null)
                        {
                            await ReadFile(file, selectedMaintenanceRepair);
                        }
                        break;
                }
            }
        }

        public async System.Threading.Tasks.Task ReadFile(StorageFile file, MaintenanceRepair selectedMaintenanceRepair)
        {
            byte[] fileBytes = null;
            using (IRandomAccessStreamWithContentType stream = await file.OpenReadAsync())
            {
                fileBytes = new byte[stream.Size];
                using (DataReader reader = new DataReader(stream))
                {
                    await reader.LoadAsync((uint)stream.Size);
                    reader.ReadBytes(fileBytes);


                    var bitmap = new BitmapImage();
                    stream.Seek(0);
                    await bitmap.SetSourceAsync(stream);

                    if (selectedMaintenanceRepair == null)
                    {
                        selectedMaintenanceRepair = new MaintenanceRepair();
                    }
                    if (selectedMaintenanceRepair.IsMajorPivot)
                    {
                        selectedMaintenanceRepair.MajorComponentImgList.Add(new Eqstra.BusinessLogic.Portable.TIModels.ImageCapture
                         {
                             ImageBitmap = bitmap,
                             ImageData = Convert.ToBase64String(fileBytes),
                             Component = selectedMaintenanceRepair.MajorComponent,
                             RepairId = selectedMaintenanceRepair.Repairid,
                             guid = Guid.NewGuid()
                         });
                    }
                    else
                    {
                        selectedMaintenanceRepair.SubComponentImgList.Add(new Eqstra.BusinessLogic.Portable.TIModels.ImageCapture
                          {
                              ImageBitmap = bitmap,
                              ImageData = Convert.ToBase64String(fileBytes),
                              Component = selectedMaintenanceRepair.MajorComponent,
                              RepairId = selectedMaintenanceRepair.Repairid,
                              guid = Guid.NewGuid()
                          });
                    }
                }


            }

            EventAggregator.GetEvent<MaintenanceRepairEvent>().Publish(selectedMaintenanceRepair);
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
            SessionStateService.RegisterKnownType(typeof(Eqstra.BusinessLogic.Portable.TIModels.ImageCapture));

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
                var viewModelTypeName = string.Format(CultureInfo.InvariantCulture, "Eqstra.TechnicalInspection.UILogic.WindowsPhone.ViewModels.{0}ViewModel,Eqstra.TechnicalInspection.UILogic.WindowsPhone, Version=1.0.0.0, Culture=neutral", viewType.Name);

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