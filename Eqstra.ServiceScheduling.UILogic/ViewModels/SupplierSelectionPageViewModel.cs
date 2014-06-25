using Eqstra.BusinessLogic;
using Eqstra.BusinessLogic.ServiceSchedule;
using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eqstra.ServiceScheduling.UILogic.ViewModels
{
    public class SupplierSelectionPageViewModel : BaseViewModel
    {
        INavigationService _navigationService;
        public SupplierSelectionPageViewModel(INavigationService navigationSerive) :base(navigationSerive)
        {
            _navigationService = navigationSerive;
            this.Model = new SupplierSelection();
            this.GoToConfirmationCommand = new DelegateCommand(() =>
            {
               string jsonCustomerDetails= JsonConvert.SerializeObject(this.CustomerDetails);
               navigationSerive.Navigate("Confirmation", jsonCustomerDetails);
            });
        }

        public override void OnNavigatedTo(object navigationParameter, Windows.UI.Xaml.Navigation.NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
            this.CustomerDetails = JsonConvert.DeserializeObject<CustomerDetails>(navigationParameter.ToString());
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
