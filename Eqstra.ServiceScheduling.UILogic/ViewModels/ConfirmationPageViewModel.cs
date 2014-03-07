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
    public class ConfirmationPageViewModel : ViewModel
    {
        INavigationService _navigationService;
        public ConfirmationPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            SubmitCommand = new DelegateCommand(() =>
            {
                navigationService.Navigate("Main", null);
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

        public DelegateCommand SubmitCommand { get; set; }
    }
}
