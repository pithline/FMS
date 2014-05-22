using Bing.Maps;
using Eqstra.BusinessLogic;
using Eqstra.BusinessLogic.Helpers;
using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using Syncfusion.UI.Xaml.Schedule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.System;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace Eqstra.DocumentDelivery.UILogic.ViewModels
{
    public class DrivingDirectionPageViewModel : ViewModel
    {
        private INavigationService _navigationService;
        private Eqstra.BusinessLogic.CollectDeliveryTask _inspection;

        public DrivingDirectionPageViewModel(INavigationService navigationService)
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
                stringBuilder.Append("~adr.Chanchalguda,Hyderabad");
                await Launcher.LaunchUriAsync(new Uri(stringBuilder.ToString()));
            });

            this.GoToDocumentDeliveryCommand = new DelegateCommand(() =>
            {
                _navigationService.Navigate("CollectionOrDeliveryDetails", _inspection);
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
            _inspection = (Eqstra.BusinessLogic.CollectDeliveryTask)navigationParameter;
            await GetCustomerDetailsAsync();
        }

        private CustomerDetails customerDetails;

        public CustomerDetails CustomerDetails
        {
            get { return customerDetails; }
            set { SetProperty(ref customerDetails, value); }
        }

        public ICommand GetDirectionsCommand { get; set; }
        public DelegateCommand GoToDocumentDeliveryCommand { get; set; }




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
