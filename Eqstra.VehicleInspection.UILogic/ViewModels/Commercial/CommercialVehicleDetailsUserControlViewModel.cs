using Eqstra.BusinessLogic.Commercial;
using Eqstra.BusinessLogic.Passenger;
using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Eqstra.VehicleInspection.UILogic.ViewModels
{
   public class CommercialVehicleDetailsUserControlViewModel : BaseViewModel
    {
       INavigationService _navigationService;
       public CommercialVehicleDetailsUserControlViewModel(INavigationService navigationService)
       {
           _navigationService = navigationService;
           this.Model = new CVehicleDetails();
           this.GoToImageMarkupPageCommand = new DelegateCommand(() =>
           {
               _navigationService.Navigate("ImageMarkup", this.Model);
           });
       }

       public DelegateCommand GoToImageMarkupPageCommand { get; set; }
    }
}
