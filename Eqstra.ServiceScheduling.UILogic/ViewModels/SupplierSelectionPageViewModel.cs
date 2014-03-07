using Eqstra.BusinessLogic;
using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eqstra.ServiceScheduling.UILogic.ViewModels
{
    public class SupplierSelectionPageViewModel : ViewModel
    {
        INavigationService _navigationService;
        public SupplierSelectionPageViewModel(INavigationService navigationSerive)
        {
            _navigationService = navigationSerive;
            this.GoToConfirmationCommand = new DelegateCommand(() =>
            {
                navigationSerive.Navigate("Confirmation", this.CustomerDetails);
            });
        }

        public override void OnNavigatedTo(object navigationParameter, Windows.UI.Xaml.Navigation.NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
            this.CustomerDetails = navigationParameter as CustomerDetails;

        }

        private CustomerDetails customerDetails;

        public CustomerDetails CustomerDetails
        {
            get { return customerDetails; }
            set { SetProperty(ref customerDetails, value); }
        }

        public DelegateCommand GoToConfirmationCommand { get; set; }
    }
}
