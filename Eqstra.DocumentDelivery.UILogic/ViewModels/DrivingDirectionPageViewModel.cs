using Bing.Maps;
using Eqstra.BusinessLogic;
using Eqstra.BusinessLogic.Helpers;
using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using Newtonsoft.Json;
using Syncfusion.UI.Xaml.Schedule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage;
using Windows.System;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace Eqstra.DocumentDelivery.UILogic.ViewModels
{
    public class DrivingDirectionPageViewModel : BaseViewModel
    {
        private INavigationService _navigationService;
        private Eqstra.BusinessLogic.CollectDeliveryTask _deliveryTask;
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
                stringBuilder.Append("~adr." + this._deliveryTask.Address);
                await Launcher.LaunchUriAsync(new Uri(stringBuilder.ToString()));
            });

            this.GoToDocumentDeliveryCommand = new DelegateCommand(() =>
            {
                string jsondeliveryTask = JsonConvert.SerializeObject(this._deliveryTask);
                _navigationService.Navigate("CollectionOrDeliveryDetails", jsondeliveryTask);
            });

            this.StartDrivingCommand = new DelegateCommand(async () =>
            {
                this.IsStartDriving = false;
                this.IsArrived = true;
                await SqliteHelper.Storage.InsertSingleRecordAsync(new DrivingDuration { StartDateTime = DateTime.Now, VehicleInsRecID = long.Parse(ApplicationData.Current.LocalSettings.Values["VehicleInsRecID"].ToString()) });
            });

            this.ArrivedCommand = new DelegateCommand(async () =>
            {
                if (this._deliveryTask != null)
                {
                    var vehicleInsRecId = Int64.Parse(ApplicationData.Current.LocalSettings.Values["VehicleInsRecId"].ToString());

                    var dd = await SqliteHelper.Storage.GetSingleRecordAsync<DrivingDuration>(x => x.VehicleInsRecID.Equals(vehicleInsRecId));
                    dd.StopDateTime = DateTime.Now;
                    await SqliteHelper.Storage.UpdateSingleRecordAsync(dd);
                }
                this.IsStartDelivery = true;
                this.IsStartDriving = false;
                this.IsArrived = false;
            });

        }

        private void LoadDemoAppointments()
        {
            //this.CustomerDetails.Appointments = AppSettingData.Appointments;
            //this.CustomerDetails.Appointments = new ScheduleAppointmentCollection
            //{
            //    new ScheduleAppointment(){
            //        Subject = "Inspection at Peter Johnson",
            //        Notes = "some noise from engine",
            //        Location = "Cape Town",
            //        StartTime = DateTime.Now,
            //        EndTime = DateTime.Now.AddHours(2),
            //        ReadOnly = true,
            //       AppointmentBackground = new SolidColorBrush(Colors.Crimson),                   
            //        Status = new ScheduleAppointmentStatus{Status = "Tentative",Brush = new SolidColorBrush(Colors.Chocolate)}

            //    },
            //    new ScheduleAppointment(){
            //        Subject = "Inspection at Peter Johnson",
            //        Notes = "some noise from differential",
            //        Location = "Cape Town",
            //         ReadOnly = true,
            //        StartTime =new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,8,00,00),
            //        EndTime = new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,9,00,00),
            //        Status = new ScheduleAppointmentStatus{Brush = new SolidColorBrush(Colors.Green), Status  = "Free"},
            //    },                    
            //};
        }

        async public override void OnNavigatedTo(object navigationParameter, Windows.UI.Xaml.Navigation.NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
            _deliveryTask = JsonConvert.DeserializeObject<CollectDeliveryTask>(navigationParameter.ToString());
            await GetCustomerDetailsAsync();

            var dd = await SqliteHelper.Storage.GetSingleRecordAsync<DrivingDuration>(x => x.VehicleInsRecID == _deliveryTask.VehicleInsRecId);
            if (dd != null)
            {
                this.IsArrived = dd.StopDateTime == DateTime.MinValue;
                this.IsStartDelivery = !this.IsArrived;
            }
            else
            {
                this.IsStartDriving = true;
                this.IsArrived = false;
                this.IsStartDelivery = false;
            }
        }

        private CustomerDetails customerDetails;

        public CustomerDetails CustomerDetails
        {
            get { return customerDetails; }
            set { SetProperty(ref customerDetails, value); }
        }
        public DelegateCommand ArrivedCommand { get; set; }
        public ICommand GetDirectionsCommand { get; set; }
        public DelegateCommand GoToDocumentDeliveryCommand { get; set; }
        public DelegateCommand StartDrivingCommand { get; set; }


        private bool isStartDelivery;
        public bool IsStartDelivery
        {
            get { return isStartDelivery; }
            set { SetProperty(ref isStartDelivery, value); }
        }

        private Customer customer;

        public Customer Customer
        {
            get { return customer; }
            set { SetProperty(ref customer, value); }
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

        async private System.Threading.Tasks.Task GetCustomerDetailsAsync()
        {
            try
            {
                if (this._deliveryTask != null)
                {
                    this.Customer = await SqliteHelper.Storage.GetSingleRecordAsync<Customer>(c => c.Id == this._deliveryTask.CustomerId);
                    this.CustomerDetails.ContactNumber = this.Customer.ContactNumber;
                    this.CustomerDetails.CaseNumber = this._deliveryTask.CaseNumber;
                    this.CustomerDetails.VehicleInsRecId = this._deliveryTask.VehicleInsRecId;
                    this.CustomerDetails.Status = this._deliveryTask.Status;
                    this.CustomerDetails.StatusDueDate = this._deliveryTask.StatusDueDate;
                    this.CustomerDetails.Address = this.Customer.Address;
                    this.CustomerDetails.AllocatedTo = this._deliveryTask.AllocatedTo;
                    this.CustomerDetails.CustomerName = this.Customer.CustomerName;
                    this.CustomerDetails.ContactName = this.Customer.ContactName;
                    this.CustomerDetails.CaseType = this._deliveryTask.CaseType;
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
