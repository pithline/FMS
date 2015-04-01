using Eqstra.BusinessLogic.Portable.SSModels;

using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eqstra.ServiceScheduling.UILogic.Portable
{
    public class ServiceSchedulingPageViewModel : ViewModel
    {
        private INavigationService _navigationService;
        public ServiceSchedulingPageViewModel(INavigationService navigationService)
        {
            this._navigationService = navigationService;
            this.Model = new ServiceSchedulingDetail();
        }

        private ServiceSchedulingDetail model;
        public ServiceSchedulingDetail Model
        {
            get { return model; }
            set { SetProperty(ref model, value); }
        }

    }
}
