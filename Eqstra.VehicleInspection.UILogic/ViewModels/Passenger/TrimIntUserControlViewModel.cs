using Eqstra.BusinessLogic;
using Eqstra.BusinessLogic.Passenger;
using Microsoft.Practices.Prism.StoreApps;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Capture;

namespace Eqstra.VehicleInspection.UILogic.ViewModels
{
    public class TrimIntUserControlViewModel : BaseViewModel
    {
        public TrimIntUserControlViewModel()
        {
            this.Model = new TrimInterior();            
        }

       




    }
}
