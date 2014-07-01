using Eqstra.BusinessLogic;
using Eqstra.BusinessLogic.Helpers;
using Eqstra.BusinessLogic.ServiceSchedule;
using Eqstra.ServiceScheduling.UILogic.Helpers;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using Newtonsoft.Json;
using Syncfusion.UI.Xaml.Schedule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace Eqstra.ServiceScheduling.UILogic.ViewModels
{
    public class SupplierSelectionPageViewModel : BaseViewModel
    {
        INavigationService _navigationService;
        IEventAggregator _eventAggregator;
        Dictionary<string, object> navigationData = new Dictionary<string, object>();
        public SupplierSelectionPageViewModel(INavigationService navigationSerive, IEventAggregator eventAggregator)
            : base(navigationSerive)
        {
            _navigationService = navigationSerive;
            _eventAggregator = eventAggregator;
            this.Model = new SupplierSelection();

            this.GoToConfirmationCommand = new DelegateCommand(async () =>
            {
                    SupplierSelection supplierSelection = this.Model as SupplierSelection;
                    await Util.WriteToDiskAsync(JsonConvert.SerializeObject(supplierSelection), "SupplierSelection");
             
                _navigationService.Navigate("Confirmation", string.Empty);
            });
        }
        public override void OnNavigatedTo(object navigationParameter, Windows.UI.Xaml.Navigation.NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
            this.DriverTask = PersistentData.Instance.DriverTask;
            this.CustomerDetails = PersistentData.Instance.CustomerDetails;

        }

        private CustomerDetails customerDetails;
        public CustomerDetails CustomerDetails
        {
            get { return customerDetails; }
            set { SetProperty(ref customerDetails, value); }
        }

        private DriverTask driverTask;

        public DriverTask DriverTask
        {
            get { return driverTask; }
            set { SetProperty(ref driverTask, value); }
        }
        public DelegateCommand GoToConfirmationCommand { get; set; }

    }
}
