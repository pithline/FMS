using Microsoft.Practices.Prism.StoreApps;
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

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=234227

namespace Eqstra.VehicleInspection
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : MvvmAppBase
    {
        private readonly IUnityContainer _container = new UnityContainer();
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();

        }


        protected override Task OnLaunchApplication(LaunchActivatedEventArgs args)
        {
            NavigationService.Navigate("Login", args.Arguments);
            Window.Current.Activate();
            return Task.FromResult<object>(null);
        }

      async  protected override void OnInitialize(IActivatedEventArgs args)
        {
            base.OnInitialize(args);
            var db = await ApplicationData.Current.RoamingFolder.TryGetItemAsync("SQLiteDB\\eqstramobility.sqlite") as StorageFile;
            if (db ==null)
            {
                var packDb  =await Package.Current.InstalledLocation.GetFileAsync("SqliteDB\\eqstramobility.sqlite");
               // var packDb = await sqliteDBFolder.GetFileAsync("eqstramobility.sqlite");
                await packDb.CopyAsync(await ApplicationData.Current.RoamingFolder.CreateFolderAsync("SQLiteDB"));
            }
            SqliteHelper.Instance.ConnectionDatabaseAsync();
            _container.RegisterInstance(NavigationService);
            _container.RegisterInstance(SessionStateService);
            ViewModelLocator.SetDefaultViewTypeToViewModelTypeResolver((viewType) =>
            {
                var viewModelTypeName = string.Format(CultureInfo.InvariantCulture, "Eqstra.VehicleInspection.UILogic.ViewModels.{0}ViewModel,Eqstra.VehicleInspection.UILogic,Version 1.0.0.0, Culture=neutral", viewType.Name);
                return Type.GetType(viewModelTypeName);
            });           
        }

        protected override object Resolve(Type type)
        {
            return _container.Resolve(type);
        }
    }
}
