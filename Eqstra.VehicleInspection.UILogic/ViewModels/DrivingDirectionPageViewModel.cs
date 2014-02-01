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
   public class DrivingDirectionPageViewModel:ViewModel
    {
       private INavigationService _navigationService;
       public DrivingDirectionPageViewModel(INavigationService navigationService)
       {
           _navigationService = navigationService;

           GetDirectionsCommand =DelegateCommand<Location>.FromAsyncHandler(async (location) =>
           {
               var stringBuilder = new StringBuilder("bingmaps:?rtp=pos.");
               stringBuilder.Append(location.Latitude);
               stringBuilder.Append("_");
               stringBuilder.Append(location.Longitude);
               stringBuilder.Append("~adr.Chanchalguda,Hyderabad");
               await Launcher.LaunchUriAsync(new Uri(stringBuilder.ToString()));
           });
       }

       public ICommand GetDirectionsCommand { get; set; }
    }
}
