using Eqstra.BusinessLogic;
using Eqstra.BusinessLogic.Helpers;
using Eqstra.BusinessLogic.ServiceSchedule;
using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Capture;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace Eqstra.ServiceScheduling.UILogic.ViewModels
{
    public class ServiceSchedulingPageViewModel : ViewModel
    {
        private DriverTask _task;
        INavigationService _navigationService;
        SettingsFlyout _settingsFlyout;
        public ServiceSchedulingPageViewModel(INavigationService navigationService, SettingsFlyout settingsFlyout)
        {
            _navigationService = navigationService;
            _settingsFlyout = settingsFlyout;

            this.CustomerDetails = new CustomerDetails();

            this.GoToSupplierSelectionCommand = new DelegateCommand(() =>
            {
                _navigationService.Navigate("SupplierSelection", this.CustomerDetails);
            });

            this.AddAddressCommand = new DelegateCommand<string>((x) =>
            {
                settingsFlyout.Title = x + " Address";
                settingsFlyout.ShowIndependent();
            });

            this.ODOReadingPictureCommand = new DelegateCommand(async () =>
            {
                CameraCaptureUI cam = new CameraCaptureUI();
                var file = await cam.CaptureFileAsync(CameraCaptureUIMode.Photo);
                if (file != null)
                {
                    ODOReadingImagePath = file.Path;
                }
            });

        }

        async public override void OnNavigatedTo(object navigationParameter, Windows.UI.Xaml.Navigation.NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
            _task = navigationParameter as DriverTask;
            await GetCustomerDetailsAsync();
            this.ODOReadingImagePath = "ms-appx:///Assets/odo_meter.png";
        }

        private CustomerDetails customerDetails;

        public CustomerDetails CustomerDetails
        {
            get { return customerDetails; }
            set { SetProperty(ref customerDetails, value); }
        }

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
                if (this._task != null)
                {
                    this.Customer = await SqliteHelper.Storage.GetSingleRecordAsync<Customer>(c => c.Id == this._task.CustomerId);
                    this.CustomerDetails.ContactNumber = this.Customer.ContactNumber;
                    this.CustomerDetails.CaseNumber = this._task.CaseNumber;
                    this.CustomerDetails.VehicleInsRecId = this._task.VehicleInsRecId;
                    this.CustomerDetails.Status = this._task.Status;
                    this.CustomerDetails.StatusDueDate = this._task.StatusDueDate;
                    this.CustomerDetails.Address = this.Customer.Address;
                    this.CustomerDetails.AllocatedTo = this._task.AllocatedTo;
                    this.CustomerDetails.CustomerName = this.Customer.CustomerName;
                    this.CustomerDetails.ContactName = this.Customer.ContactName;
                    this.CustomerDetails.CaseType = this._task.CaseType;
                    this.CustomerDetails.EmailId = this.Customer.EmailId;
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        public DelegateCommand GoToSupplierSelectionCommand { get; set; }

        public DelegateCommand ODOReadingPictureCommand { get; set; }

        public DelegateCommand<string> AddAddressCommand { get; set; }

        private string odoReadingImagePath;

        public string ODOReadingImagePath
        {
            get { return odoReadingImagePath; }
            set { SetProperty(ref odoReadingImagePath, value); }
        }

    }
}
