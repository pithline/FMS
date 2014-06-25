using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eqstra.ServiceScheduling.UILogic
{
    public class BaseViewModel : ViewModel
    {
        INavigationService _navigationService;
        public BaseViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            this.GoHomeCommand = new DelegateCommand(() =>
            {
                _navigationService.ClearHistory();
                _navigationService.Navigate("Main", string.Empty);
            });
        }
        public DelegateCommand GoHomeCommand { get; set; }

        private Object model;
        [RestorableState]
        public Object Model
        {
            get { return model; }
            set { SetProperty(ref model, value); }
        }

        private bool isBusy;

        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }
    }
}
