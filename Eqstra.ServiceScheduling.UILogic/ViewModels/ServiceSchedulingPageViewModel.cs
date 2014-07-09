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
using Eqstra.ServiceScheduling.UILogic.Helpers;
namespace Eqstra.ServiceScheduling.UILogic.ViewModels
{
    public class ServiceSchedulingPageViewModel : BaseViewModel
    {
        private DriverTask _task;
        private INavigationService _navigationService;
        private IEventAggregator _eventAggregator;
        private SettingsFlyout _settingsFlyout;
        private Dictionary<string, object> navigationData = new Dictionary<string, object>();
        public ServiceSchedulingPageViewModel(INavigationService navigationService, IEventAggregator eventAggregator, SettingsFlyout settingsFlyout)
            : base(navigationService)
        {
            _navigationService = navigationService;
            _settingsFlyout = settingsFlyout;
            _eventAggregator = eventAggregator;
            this.IsAddFlyoutOn = Visibility.Collapsed;
            this.CustomerDetails = new CustomerDetails();
            this.CustomerDetails.Appointments = new ScheduleAppointmentCollection();
            this.Model = new ServiceSchedulingDetail();
            this.ErrorMessage = new Helpers.ObservableDictionary();
            this.GoToSupplierSelectionCommand = new DelegateCommand(() =>
            {
                if (this.Model.ValidateProperties())
                {
                    Util.WriteToDiskAsync(JsonConvert.SerializeObject(this.Model), "ServiceSchedulingDetail");
                    PersistentData.RefreshInstance();//Here only setting data in new instance, and  getting data in every page.
                    PersistentData.Instance.DriverTask = this._task;
                    PersistentData.Instance.CustomerDetails = this.CustomerDetails;
                    this._task.Status = DriverTaskStatus.AwaitSupplierSelection;
                    _navigationService.Navigate("SupplierSelection", string.Empty);
                }
                else
                {
                    foreach (var err in this.Model.Errors.Errors)
                    {
                        if (!this.ErrorMessage.ContainsKey(err.Key))
                        {
                            this.ErrorMessage.Add(err.Key, err.Value[0]);
                        }
                    }
                }
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
                    StringBuilder sb = new StringBuilder();
                    if (!String.IsNullOrEmpty(address.Street))
                    {
                        sb.Append(address.Street).Append(",").Append(Environment.NewLine);
                    }
                    if (!String.IsNullOrEmpty(address.Suburb))
                    {
                        sb.Append(address.Suburb).Append(",").Append(Environment.NewLine);
                    }
                    if (!String.IsNullOrEmpty(address.City))
                    {
                        sb.Append(address.City).Append(",").Append(Environment.NewLine);
                    }
                    if (!String.IsNullOrEmpty(address.Province))
                    {
                        sb.Append(address.Province).Append(",").Append(Environment.NewLine);
                    }

                    if (!String.IsNullOrEmpty(address.Country))
                    {
                        sb.Append(address.Country).Append(",").Append(Environment.NewLine);
                    }
                    if (!String.IsNullOrEmpty(address.Postcode))
                    {
                        sb.Append(address.Postcode);
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
                    if (param.LocType == "Driver")
                    {
                        this.Model.DestinationTypes.AddRange(await SSProxyHelper.Instance.GetDriversFromSvcAsync());
                    }
                    if (param.LocType == "Customer")
                    {
                        this.Model.DestinationTypes.AddRange(await SSProxyHelper.Instance.GetCustomersFromSvcAsync());
                    }

                    if (param.LocType == "Vendor")
                    {
                        this.Model.DestinationTypes.AddRange(await SSProxyHelper.Instance.GetVendorsFromSvcAsync());
                    }
                    this.IsAddFlyoutOn = Visibility.Collapsed;
                    if (param.LocType == "Other")
                    {
                        this.IsAddFlyoutOn = Visibility.Visible;
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
                    DestinationType destinationType = param as DestinationType;
                    this.Model.Address = destinationType.Address;
                }
                this.IsBusy = false;
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
                    this.CustomerDetails.EmailId = this._task.CusEmailId;
                    this.CustomerDetails.CategoryType = this._task.CaseCategory;

                    var startTime = new DateTime(this._task.ConfirmedTime.Year, this._task.ConfirmedTime.Month, this._task.ConfirmedTime.Day, this._task.ConfirmedTime.Hour, this._task.ConfirmedTime.Minute,
                                this._task.ConfirmedTime.Second);
                    this.CustomerDetails.Appointments.Add(
                                                        new ScheduleAppointment()
                                                        {
                                                            Subject = this._task.CaseNumber,
                                                            Location = this._task.Address,
                                                            StartTime = startTime,
                                                            EndTime = startTime.AddHours(1),
                                                            ReadOnly = true,
                                                            AppointmentBackground = new SolidColorBrush(Colors.Crimson),
                                                            Status = new ScheduleAppointmentStatus { Status = this._task.Status, Brush = new SolidColorBrush(Colors.Chocolate) }

                                                        });

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
            catch (Exception)
            {
                throw;
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
        private ObservableDictionary errors;
        public ObservableDictionary ErrorMessage
        {
            get { return errors; }
            set { SetProperty(ref errors, value); }
        }
    }
}
