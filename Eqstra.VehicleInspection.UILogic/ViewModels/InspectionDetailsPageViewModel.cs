using Eqstra.BusinessLogic;
using Eqstra.BusinessLogic.Helpers;
using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using Syncfusion.UI.Xaml.Schedule;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace Eqstra.VehicleInspection.UILogic.ViewModels
{
    public class InspectionDetailsPageViewModel : ViewModel
    {
        INavigationService _navigationService;
        public InspectionDetailsPageViewModel(INavigationService navigationService)
        {
            this.inspectionList = new ObservableCollection<BusinessLogic.Task>();
            this.CustomerDetails = new CustomerDetails();
            this.inspection = null;
            _navigationService = navigationService;
            DrivingDirectionCommand = DelegateCommand.FromAsyncHandler(() =>
            {
                if (this.inspection == null)
                {
                    return System.Threading.Tasks.Task.FromResult<object>(null);
                }
                _navigationService.Navigate("DrivingDirection", inspection);
                return System.Threading.Tasks.Task.FromResult<object>(null);
            }, () =>
                { return this.inspection != null; }
            );
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
        #region Overrides
        async public override void OnNavigatedTo(object navigationParameter, Windows.UI.Xaml.Navigation.NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
            var list = await SqliteHelper.Storage.LoadTableAsync<Eqstra.BusinessLogic.Task>();
            foreach (Eqstra.BusinessLogic.Task item in list)
            {
                var cust = await SqliteHelper.Storage.GetSingleRecordAsync<Customer>(x => x.Id == item.CustomerId);
                item.CustomerName = cust.Name;
                item.Address = cust.Address;
                this.inspectionList.Add(item);
            }
        }

        public override void OnNavigatedFrom(Dictionary<string, object> viewModelState, bool suspending)
        {
            base.OnNavigatedFrom(viewModelState, suspending);
        }
        #endregion

        #region Properties
        private ObservableCollection<Eqstra.BusinessLogic.Task> inspectionList;

        public ObservableCollection<Eqstra.BusinessLogic.Task> InspectionList
        {
            get { return inspectionList; }
            set { SetProperty(ref inspectionList, value); }
        }

        private Eqstra.BusinessLogic.Task inspection;

        public Eqstra.BusinessLogic.Task Inspection
        {
            get { return inspection; }
            set
            {
                if (SetProperty(ref inspection, value))
                    DrivingDirectionCommand.RaiseCanExecuteChanged();
            }
        }


        private Customer customer;

        public Customer Customer
        {
            get { return customer; }
            set { SetProperty(ref customer, value); }
        }

        private CustomerDetails customerDetails;

        public CustomerDetails CustomerDetails
        {
            get { return customerDetails; }
            set { SetProperty(ref customerDetails, value); }
        }


        public DelegateCommand DrivingDirectionCommand { get; set; }
        #endregion

        #region Methods
        async public System.Threading.Tasks.Task GetCustomerDetailsAsync()
        {
            try
            {
                if (this.inspection != null)
                {
                    this.customer = await SqliteHelper.Storage.GetSingleRecordAsync<Customer>(c => c.Id == this.inspection.CustomerId);
                    this.CustomerDetails.ContactNumber = this.customer.ContactNumber;
                    this.customerDetails.CaseNumber = this.inspection.CaseNumber;
                    this.customerDetails.Status = this.inspection.Status;
                    this.customerDetails.StatusDueDate = this.inspection.StatusDueDate;
                    this.customerDetails.Address = this.customer.Address;
                    this.customerDetails.AllocatedTo = this.inspection.AllocatedTo;
                    this.customerDetails.Name = this.customer.Name;
                    this.customerDetails.CellNumber = this.inspection.CellNumber;
                    this.customerDetails.CaseType = this.inspection.CaseType;
                }
            }
            catch (Exception)
            {
                throw;
            }

        }
        #endregion


    }
}
