using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Eqstra.DocumentDelivery.UILogic.ViewModels
{
    public class CollectionOrDeliveryDetailsPageViewModel : ViewModel
    {
        private INavigationService _navigationService;
        public CollectionOrDeliveryDetailsPageViewModel(INavigationService navigationService,SettingsFlyout addCustomerPage)
        {
            this.CollectCommand = new DelegateCommand(() =>
            {
                addCustomerPage.ShowIndependent();
            });
        }
        public override void OnNavigatedTo(object navigationParameter, Windows.UI.Xaml.Navigation.NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
        }

        public DelegateCommand CollectCommand { get; set; }
    }
}
