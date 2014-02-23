using Bing.Maps;
using Eqstra.BusinessLogic;
using Eqstra.BusinessLogic.Helpers;
using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.System;

namespace Eqstra.VehicleInspection.UILogic.ViewModels
{
    public class DrivingDirectionPageViewModel : ViewModel
    {
        private INavigationService _navigationService;
        private Eqstra.BusinessLogic.Task _inspection;

        public DrivingDirectionPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            this.customerDetails = new BusinessLogic.CustomerDetails();

            GetDirectionsCommand = DelegateCommand<Location>.FromAsyncHandler(async (location) =>
            {
                var stringBuilder = new StringBuilder("bingmaps:?rtp=pos.");
                stringBuilder.Append(location.Latitude);
                stringBuilder.Append("_");
                stringBuilder.Append(location.Longitude);
                stringBuilder.Append("~adr.Chanchalguda,Hyderabad");
                await Launcher.LaunchUriAsync(new Uri(stringBuilder.ToString()));
            });

            this.GoToVehicleInspectionCommand = new DelegateCommand(() =>
            {
                _navigationService.Navigate("VehicleInspection", _inspection);
            });


        }
        async public override void OnNavigatedTo(object navigationParameter, Windows.UI.Xaml.Navigation.NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
            _inspection = (Eqstra.BusinessLogic.Task)navigationParameter;
            await GetCustomerDetailsAsync();
        }

        private CustomerDetails customerDetails;

        public CustomerDetails CustomerDetails
        {
            get { return customerDetails; }
            set { SetProperty(ref customerDetails, value); }
        }

        public ICommand GetDirectionsCommand { get; set; }
        public DelegateCommand GoToVehicleInspectionCommand { get; set; }




        private Customer customer;

        public Customer Customer
        {
            get { return customer; }
            set { SetProperty(ref customer, value); }
        }

        async private System.Threading.Tasks.Task GetCustomerDetailsAsync()
        {
            try
            {
                if (this._inspection != null)
                {
                    this.customer = await SqliteHelper.Storage.GetSingleRecordAsync<Customer>(c => c.Id == this._inspection.CustomerId);
                    this.CustomerDetails.ContactNumber = this.customer.ContactNumber;
                    this.customerDetails.CaseNumber = this._inspection.CaseNumber;
                    this.customerDetails.Status = this._inspection.Status;
                    this.customerDetails.StatusDueDate = this._inspection.StatusDueDate;
                    this.customerDetails.Address = this.customer.Address;
                    this.customerDetails.AllocatedTo = this._inspection.AllocatedTo;
                    this.customerDetails.Name = this.customer.Name;
                    this.customerDetails.CellNumber = this._inspection.CellNumber;
                    this.customerDetails.CaseType = this._inspection.CaseType;
                }
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}
