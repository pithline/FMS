using Bing.Maps;
using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.System;

namespace Eqstra.VehicleInspection.UILogic.ViewModels
{
    public class DrivingDirectionPageViewModel : ViewModel
    {
        private INavigationService _navigationService;
        private Eqstra.BusinessLogic.Task _task;
        public DrivingDirectionPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            GetDirectionsCommand = DelegateCommand<Location>.FromAsyncHandler(async (location) =>
            {
                var stringBuilder = new StringBuilder("bingmaps:?rtp=pos.");
                stringBuilder.Append(location.Latitude);
                stringBuilder.Append("_");
                stringBuilder.Append(location.Longitude);
                stringBuilder.Append("~adr.Chanchalguda,Hyderabad");
                await Launcher.LaunchUriAsync(new Uri(stringBuilder.ToString()));
            });

            this.GoToVehicleInspectionCommand = new DelegateCommand(() =>
            {
                _navigationService.Navigate("VehicleInspection", _task);
            });


        }
        public override void OnNavigatedTo(object navigationParameter, Windows.UI.Xaml.Navigation.NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
            _task = (Eqstra.BusinessLogic.Task)navigationParameter;
        }



        public ICommand GetDirectionsCommand { get; set; }
        public DelegateCommand GoToVehicleInspectionCommand { get; set; }
    }
}
