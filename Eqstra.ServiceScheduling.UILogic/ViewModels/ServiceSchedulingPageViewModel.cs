using Eqstra.BusinessLogic;
using Eqstra.BusinessLogic.Helpers;
using Eqstra.BusinessLogic.ServiceSchedule;
using Eqstra.BusinessLogic.ServiceSchedulingModel;
using Eqstra.ServiceScheduling.UILogic.AifServices;
using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using Newtonsoft.Json;
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
    public class ServiceSchedulingPageViewModel : BaseViewModel
    {
        private DriverTask _task;
        INavigationService _navigationService;
        SettingsFlyout _settingsFlyout;
        public ServiceSchedulingPageViewModel(INavigationService navigationService, SettingsFlyout settingsFlyout) :base(navigationService)
        {
            _navigationService = navigationService;
            _settingsFlyout = settingsFlyout;

            this.CustomerDetails = new CustomerDetails();
            this.Model = new DetailServiceScheduling();
            this.GoToSupplierSelectionCommand = new DelegateCommand<object>(async (param) =>
            {
                DetailServiceScheduling detailServiceScheduling = param as DetailServiceScheduling;
                await SSProxyHelper.Instance.InsertServiceDetailsToSvcAsync(detailServiceScheduling, this._task.CaseNumber, this._task.CaseServiceRecID);
                string jsonCustomerDetails = JsonConvert.SerializeObject(this.CustomerDetails);
                _navigationService.Navigate("SupplierSelection", jsonCustomerDetails);
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
            this.IsBusy = true;
            base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
            _task = JsonConvert.DeserializeObject<DriverTask>(navigationParameter.ToString());
            this.Model = await SSProxyHelper.Instance.GetServiceDetailsFromSvcAsync(this._task.CaseNumber, this._task.CaseServiceRecID);
            await GetCustomerDetailsAsync();
            this.ODOReadingImagePath = "ms-appx:///Assets/odo_meter.png";
            this.IsBusy = false;
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
                    this.CustomerDetails.ContactNumber = this._task.CustPhone;
                    this.CustomerDetails.CaseNumber = this._task.CaseNumber;
                    this.CustomerDetails.VehicleInsRecId = this._task.VehicleInsRecId;
                    this.CustomerDetails.Status = this._task.Status;
                    this.CustomerDetails.StatusDueDate = this._task.StatusDueDate;
                    this.CustomerDetails.Address = this._task.Address;
                    this.CustomerDetails.AllocatedTo = this._task.AllocatedTo;
                    this.CustomerDetails.CustomerName = this._task.CustomerName;
                    this.CustomerDetails.ContactName = this._task.CustomerName;
                    this.CustomerDetails.CaseType = this._task.CaseType;

                }
            }
            catch (Exception)
            {
                throw;
            }

        }
        public DelegateCommand<object> GoToSupplierSelectionCommand { get; set; }

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
