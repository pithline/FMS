using Bing.Maps;
using Eqstra.BusinessLogic;
using Eqstra.BusinessLogic.Helpers;
using Eqstra.VehicleInspection.UILogic.AifServices;
using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using Newtonsoft.Json;
using Syncfusion.UI.Xaml.Schedule;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage;
using Windows.System;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace Eqstra.VehicleInspection.UILogic.ViewModels
{
    public class DrivingDirectionPageViewModel : BaseViewModel
    {
        private INavigationService _navigationService;
        private Eqstra.BusinessLogic.Task _inspection;


        public DrivingDirectionPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            _navigationService = navigationService;
            this.CustomerDetails = new BusinessLogic.CustomerDetails();
            LoadDemoAppointments();

            GetDirectionsCommand = DelegateCommand<Location>.FromAsyncHandler(async (location) =>
            {
                var stringBuilder = new StringBuilder("bingmaps:?rtp=pos.");
                stringBuilder.Append(location.Latitude);
                stringBuilder.Append("_");
                stringBuilder.Append(location.Longitude);
                stringBuilder.Append("~adr." + Regex.Replace(this.CustomerDetails.Address, "\n", ","));
                await Launcher.LaunchUriAsync(new Uri(stringBuilder.ToString()));
            });

            this.GoToVehicleInspectionCommand = new DelegateCommand(async () =>
            {
                this._inspection.ProcessStep = ProcessStep.CaptureInspectionData;
                await SqliteHelper.Storage.UpdateSingleRecordAsync(this._inspection);
                string JsoninspectionTask = JsonConvert.SerializeObject(this._inspection);
                await VIServiceHelper.Instance.UpdateTaskStatusAsync(ProcessStep.CaptureInspectionData);
                _navigationService.Navigate("VehicleInspection", JsoninspectionTask);

            });

            this.StartDrivingCommand = new DelegateCommand(async () =>
            {
                this.IsStartDriving = false;
                this.IsArrived = true;
                await SqliteHelper.Storage.InsertSingleRecordAsync(new DrivingDuration { StartDateTime = DateTime.Now, VehicleInsRecID = long.Parse(ApplicationData.Current.LocalSettings.Values["VehicleInsRecID"].ToString()) });
            });
            this.ArrivedCommand = new DelegateCommand(async () =>
            {
                if (this._inspection != null)
                {
                    var vehicleInsRecId = Int64.Parse(ApplicationData.Current.LocalSettings.Values["VehicleInsRecId"].ToString());

                    var dd = await SqliteHelper.Storage.GetSingleRecordAsync<DrivingDuration>(x => x.VehicleInsRecID.Equals(vehicleInsRecId));
                    dd.StopDateTime = DateTime.Now;
                    await SqliteHelper.Storage.UpdateSingleRecordAsync(dd);
                }
                this.IsStartInspection = true;
                this.IsStartDriving = false;
                this.IsArrived = false;
            });


        }

        private void LoadDemoAppointments()
        {
            this.CustomerDetails.Appointments = AppSettingData.Appointments;
            this.CustomerDetails.Appointments = new ScheduleAppointmentCollection
            {
                new ScheduleAppointment(){
                    Subject = "Inspection at Peter Johnson",
                    Notes = "some noise from engine",
                    Location = "Cape Town",
                    StartTime = DateTime.Now,
                    EndTime = DateTime.Now.AddHours(2),
                    ReadOnly = true,
                   AppointmentBackground = new SolidColorBrush(Colors.Crimson),                   
                    Status = new ScheduleAppointmentStatus{Status = "Tentative",Brush = new SolidColorBrush(Colors.Chocolate)}

                },
                new ScheduleAppointment(){
                    Subject = "Inspection at Peter Johnson",
                    Notes = "some noise from differential",
                    Location = "Cape Town",
                     ReadOnly = true,
                    StartTime =new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,8,00,00),
                    EndTime = new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,9,00,00),
                    Status = new ScheduleAppointmentStatus{Brush = new SolidColorBrush(Colors.Green), Status  = "Free"},
                },                    
            };
        }

        async public override void OnNavigatedTo(object navigationParameter, Windows.UI.Xaml.Navigation.NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
            _inspection = JsonConvert.DeserializeObject<Eqstra.BusinessLogic.Task>(navigationParameter.ToString());
            await GetCustomerDetailsAsync();

            var dd = await SqliteHelper.Storage.GetSingleRecordAsync<DrivingDuration>(x => x.VehicleInsRecID == _inspection.VehicleInsRecId);
            if (dd != null)
            {
                this.IsArrived = dd.StopDateTime == DateTime.MinValue;
                this.IsStartInspection = !this.IsArrived;
            }
            else
            {
                this.IsStartDriving = true;
                this.IsArrived = false;
                this.IsStartInspection = false;
            }

        }

        private CustomerDetails customerDetails;

        public CustomerDetails CustomerDetails
        {
            get { return customerDetails; }
            set { SetProperty(ref customerDetails, value); }
        }

        public ICommand GetDirectionsCommand { get; set; }
        public DelegateCommand GoToVehicleInspectionCommand { get; set; }
        public DelegateCommand StartDrivingCommand { get; set; }
        public DelegateCommand ArrivedCommand { get; set; }

        private bool isStartInspection;
        public bool IsStartInspection
        {
            get { return isStartInspection; }
            set { SetProperty(ref isStartInspection, value); }
        }

        private bool isStartDriving;
        public bool IsStartDriving
        {
            get { return isStartDriving; }
            set { SetProperty(ref isStartDriving, value); }
        }

        private bool isArrived;
        public bool IsArrived
        {
            get { return isArrived; }
            set { SetProperty(ref isArrived, value); }
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
                if (this._inspection != null)
                {
                    this.Customer = await SqliteHelper.Storage.GetSingleRecordAsync<Customer>(c => c.Id == this._inspection.CustomerId);
                    this.CustomerDetails.ContactNumber = this.Customer.ContactNumber;
                    this.CustomerDetails.CaseNumber = this._inspection.CaseNumber;
                    this.CustomerDetails.VehicleInsRecId = this._inspection.VehicleInsRecId;
                    this.CustomerDetails.Status = this._inspection.Status;
                    this.CustomerDetails.StatusDueDate = this._inspection.StatusDueDate;
                    this.CustomerDetails.Address = this.Customer.Address;
                    this.CustomerDetails.AllocatedTo = this._inspection.AllocatedTo;
                    this.CustomerDetails.CustomerName = this.Customer.CustomerName;
                    this.CustomerDetails.ContactName = this.Customer.ContactName;
                    this.CustomerDetails.CaseType = this._inspection.CaseType;
                    this.CustomerDetails.EmailId = this.Customer.EmailId;
                }
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}
