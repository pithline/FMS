using Eqstra.BusinessLogic.ServiceSchedule;
using Microsoft.Practices.Prism.StoreApps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eqstra.ServiceScheduling.UILogic.ViewModels
{
    public class AddAddressFlyoutPageViewModel : ViewModel
    {
        public AddAddressFlyoutPageViewModel()
        {
            this.Model = new Address();
        }

        private object model;

        public object Model
        {
            get { return model; }
            set { SetProperty(ref model, value); }
        }
    }
}
