using Microsoft.Practices.Prism.StoreApps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eqstra.BusinessLogic.Passenger;
using Microsoft.Practices.Prism.StoreApps.Interfaces;

namespace Eqstra.VehicleInspection.UILogic.ViewModels
{
    public class VehicleDetailsUserControlViewModel : BaseViewModel
    {
        INavigationService _navigationService;
        public VehicleDetailsUserControlViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            this.Model = new PVehicleDetails();
            this.GoToImageMarkupPageCommand = new DelegateCommand(() =>
            {
                _navigationService.Navigate("ImageMarkup", this.Model);
            });
        }
        public override void OnNavigatedTo(object navigationParameter, Windows.UI.Xaml.Navigation.NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
        }

        public DelegateCommand GoToImageMarkupPageCommand { get; set; }

    }
}
