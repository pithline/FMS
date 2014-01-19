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
   public class DrivingDirectionPageViewModel:ViewModel
    {
       private INavigationService _navigationService;
       public DrivingDirectionPageViewModel(INavigationService navigationService)
       {
           _navigationService = navigationService;

           GetDirectionsCommand = new DelegateCommand(async () => 
           {
               await Launcher.LaunchUriAsync(new Uri("bingmap:"));
           });
       }

       public ICommand GetDirectionsCommand { get; set; }
    }
}
