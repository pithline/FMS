using Microsoft.Practices.Prism.StoreApps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System;

namespace Eqstra.VehicleInspection.UILogic.ViewModels
{
   public class CustomerDetailUserControlViewModel : ViewModel
    {
       public CustomerDetailUserControlViewModel()
       {
           this.MakeIMCommand = DelegateCommand<string>.FromAsyncHandler(async(emailId)=>
               {
                   await Launcher.LaunchUriAsync(new Uri("skype:shoaibrafi?chat"));
               }, (emailId) => { return !string.IsNullOrEmpty(emailId); });

           this.MakeSkypeCallCommand = DelegateCommand<string>.FromAsyncHandler(async (number) =>
               {
                   await Launcher.LaunchUriAsync(new Uri("skype:" + number + "?call"));
               }, (number) => { return !string.IsNullOrEmpty(number); });
          
       }

       public DelegateCommand<string> MakeIMCommand { get; set; }

       public DelegateCommand<string> MakeSkypeCallCommand { get; set; }
    }
}
