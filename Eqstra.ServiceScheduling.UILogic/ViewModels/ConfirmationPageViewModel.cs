using Eqstra.BusinessLogic;
using Eqstra.BusinessLogic.ServiceSchedulingModel;
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
    public class ConfirmationPageViewModel : BaseViewModel
    {
        INavigationService _navigationService;
        public ConfirmationPageViewModel(INavigationService navigationService) :base(navigationService)
        {
            _navigationService = navigationService;
            this.Model = new Confirmation();
            SubmitCommand = new DelegateCommand(() =>
            {
                navigationService.Navigate("Main", string.Empty);
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
        public DelegateCommand SubmitCommand { get; set; }

    }
}
