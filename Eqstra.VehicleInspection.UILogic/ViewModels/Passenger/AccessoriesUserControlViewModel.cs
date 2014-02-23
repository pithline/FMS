using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eqstra.BusinessLogic.Passenger;

namespace Eqstra.VehicleInspection.UILogic.ViewModels
{
    public class AccessoriesUserControlViewModel : BaseViewModel
    {
        public AccessoriesUserControlViewModel()
        {
            this.Model =new PAccessories();
        }
    }
}
