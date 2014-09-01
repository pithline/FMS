using Eqstra.BusinessLogic;
using Eqstra.BusinessLogic.Base;
using Eqstra.BusinessLogic.Helpers;
using Eqstra.BusinessLogic.ServiceSchedule;
using Eqstra.BusinessLogic.ServiceSchedulingModel;
using Eqstra.ServiceScheduling.UILogic.AifServices;
using Eqstra.ServiceScheduling.UILogic.Helpers;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using Newtonsoft.Json;
using Syncfusion.UI.Xaml.Schedule;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Capture;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Eqstra.ServiceScheduling.UILogic.ViewModels
{
    public class ServiceSchedulingPageViewModel : BaseViewModel
    {
        private DriverTask _task;
        private INavigationService _navigationService;
        private IEventAggregator _eventAggregator;
        private SettingsFlyout _settingsFlyout;
        public ServiceSchedulingPageViewModel(INavigationService navigationService, IEventAggregator eventAggregator, SettingsFlyout settingsFlyout)
            : base(navigationService)
        {
            _navigationService = navigationService;
            _settingsFlyout = settingsFlyout;
            _eventAggregator = eventAggregator;
            this.Address = new Address();
            this.IsAddFlyoutOn = Visibility.Collapsed;
            this.Model = new ServiceSchedulingDetail();
            this.GoToSupplierSelectionCommand = new DelegateCommand(async () =>
            {
                this.IsBusy = true;
                if (this.Model.ValidateProperties())
                {
                    bool isInserted = await SSProxyHelper.Instance.InsertServiceDetailsAsyncToSvcAsync(this.Model, this.Address, this._task.CaseNumber, this._task.CaseServiceRecID, this.Address.EntityRecId);
                    if (isInserted)
                    {
                        this._task.Status = DriverTaskStatus.AwaitSupplierSelection;
                       // PersistentData.Instance.CustomerDetails.Status = await SSProxyHelper.Instance.UpdateStatusListToSvcAsync(this._task);

                        _navigationService.Navigate("SupplierSelection", string.Empty);
                    }
                }
                this.IsBusy = false;
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

            this._eventAggregator.GetEvent<AddressEvent>().Subscribe((address) =>
            {
                if (address != null)
                {
                    this.Address = address;
                    StringBuilder sb = new StringBuilder();
                    if (!String.IsNullOrEmpty(address.Street))
                    {
                        sb.Append(address.Street).Append(",").Append(Environment.NewLine);
                    }
                    if ((address.SelectedSuburb != null) && !String.IsNullOrEmpty(address.SelectedSuburb.Name))
                    {
                        sb.Append(address.SelectedSuburb.Name).Append(",").Append(Environment.NewLine);
                    }
                    if ((address.SelectedCity != null) && !String.IsNullOrEmpty(address.SelectedCity.Name))
                    {
                        sb.Append(address.SelectedCity.Name).Append(",").Append(Environment.NewLine);
                    }
                    if ((address.Selectedprovince != null) && !String.IsNullOrEmpty(address.Selectedprovince.Name))
                    {
                        sb.Append(address.Selectedprovince.Name).Append(",").Append(Environment.NewLine);
                    }

                    if ((address.SelectedCountry != null) && !String.IsNullOrEmpty(address.SelectedCountry.Name))
                    {
                        sb.Append(address.SelectedCountry.Name).Append(",").Append(Environment.NewLine);
                    }
                    if (!String.IsNullOrEmpty(address.SelectedZip))
                    {
                        sb.Append(address.SelectedZip);
                    }
                    this.Model.Address = sb.ToString();
                }
                settingsFlyout.Hide();
            });

            this.TakePictureCommand = DelegateCommand<ImageCapture>.FromAsyncHandler(async (param) =>
            {
                await TakePictureAsync(param);
            });

            this.LocTypeChangedCommand = new DelegateCommand<LocationType>(async (param) =>
            {
                try
                {
                    this.IsBusy = true;
                    if (this.Model.DestinationTypes != null)
                    {
                        this.Model.DestinationTypes.Clear();
                    }
                    if (param.LocType == LocationTypeConstants.Driver)
                    {

                        this.Model.DestinationTypes.AddRange(await SSProxyHelper.Instance.GetDriversFromSvcAsync());
                    }
                    if (param.LocType == LocationTypeConstants.Customer)
                    {
                        this.Model.DestinationTypes.AddRange(await SSProxyHelper.Instance.GetCustomersFromSvcAsync());
                    }

                    if (param.LocType == LocationTypeConstants.Vendor)
                    {
                        this.Model.DestinationTypes.AddRange(await SSProxyHelper.Instance.GetVendorsFromSvcAsync());
                    }
                    this.IsAddFlyoutOn = Visibility.Collapsed;
                    this.IsAlternative = Visibility.Visible;
                    if (param.LocType == LocationTypeConstants.Other)
                    {
                        this.Model.DestinationTypes = new ObservableCollection<DestinationType>();
                        this.Model.SelectedDestinationType = new DestinationType();
                        this.IsAddFlyoutOn = Visibility.Visible;
                        this.IsAlternative = Visibility.Collapsed;
                    }

                    this.IsBusy = false;
                }
                catch (Exception ex)
                {
                    AppSettings.Instance.ErrorMessage = ex.Message;
                }

            });

            this.DestiTypeChangedCommand = new DelegateCommand<DestinationType>(async (param) =>
            {
                this.IsBusy = true;
                if (param != null)
                {
                    this.Address.EntityRecId = param.RecID;
                    DestinationType destinationType = param as DestinationType;
                    this.Model.Address = destinationType.Address;
                }
                this.IsBusy = false;
            });
        }
        async public override void OnNavigatedTo(object navigationParameter, Windows.UI.Xaml.Navigation.NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            try
            {
                this.IsBusy = true;
                base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
                this._task = PersistentData.Instance.DriverTask;
                this.CustomerDetails = PersistentData.Instance.CustomerDetails;
                this.Model = await SSProxyHelper.Instance.GetServiceDetailsFromSvcAsync(this._task.CaseNumber, this._task.CaseServiceRecID);
                this.Model.IsValidationEnabled =true;
                this.ODOReadingImagePath = "ms-appx:///Assets/odo_meter.png";
                this.IsBusy = false;
            }
            catch (Exception ex)
            {
                AppSettings.Instance.ErrorMessage = ex.Message;
            }
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
        public DelegateCommand GoToSupplierSelectionCommand { get; set; }
        public DelegateCommand ODOReadingPictureCommand { get; set; }
        public DelegateCommand<string> AddAddressCommand { get; set; }
        public DelegateCommand<LocationType> LocTypeChangedCommand { get; set; }
        public DelegateCommand<DestinationType> DestiTypeChangedCommand { get; set; }

        private string odoReadingImagePath;
        public string ODOReadingImagePath
        {
            get { return odoReadingImagePath; }
            set { SetProperty(ref odoReadingImagePath, value); }
        }
        public DelegateCommand<ImageCapture> TakePictureCommand { get; set; }
        async public virtual System.Threading.Tasks.Task TakePictureAsync(ImageCapture param)
        {
            try
            {
                CameraCaptureUI cam = new CameraCaptureUI();
                var file = await cam.CaptureFileAsync(CameraCaptureUIMode.Photo);
                if (file != null)
                {
                    param.ImagePath = file.Path;
                }
            }
            catch (Exception ex)
            {
                AppSettings.Instance.ErrorMessage = ex.Message;
            }
        }
        private Visibility isAddFlyoutOn;
        public Visibility IsAddFlyoutOn
        {
            get { return isAddFlyoutOn; }
            set { SetProperty(ref isAddFlyoutOn, value); }
        }
        private ServiceSchedulingDetail model;
        public ServiceSchedulingDetail Model
        {
            get { return model; }
            set { SetProperty(ref model, value); }
        }

        private Visibility isAlternative;
        public Visibility IsAlternative
        {
            get { return isAlternative; }
            set { SetProperty(ref isAlternative, value); }
        }

        private Address address;
        public Address Address
        {
            get { return address; }
            set { SetProperty(ref address, value); }
        }
    }
}
