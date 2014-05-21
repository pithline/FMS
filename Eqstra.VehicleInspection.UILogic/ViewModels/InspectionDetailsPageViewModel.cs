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
            this.PoolofTasks = new ObservableCollection<BusinessLogic.Task>();
            this.Appointments = new ScheduleAppointmentCollection();
           
            this.InspectionTask = null;
            _navigationService = navigationService;
            DrivingDirectionCommand = DelegateCommand.FromAsyncHandler(() =>
            {
                if (this.InspectionTask == null)
                {
                    return System.Threading.Tasks.Task.FromResult<object>(null);
                }
                _navigationService.Navigate("DrivingDirection", InspectionTask);
                return System.Threading.Tasks.Task.FromResult<object>(null);
            }, () =>
                { return (this.InspectionTask != null && this.InspectionTask.Status != BusinessLogic.Enums.TaskStatusEnum.AwaitingInspection && this.InspectionTask.Status != BusinessLogic.Enums.TaskStatusEnum.Completed); }
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

            this.SaveCommand = new DelegateCommand(async () =>
            {
                this.InspectionTask.Status = BusinessLogic.Enums.TaskStatusEnum.InProgress;
                await SqliteHelper.Storage.UpdateSingleRecordAsync(this.InspectionTask);
                var startTime = new DateTime(this.InspectionTask.ConfirmedDate.Year, this.InspectionTask.ConfirmedDate.Month, this.InspectionTask.ConfirmedDate.Day, this.InspectionTask.ConfirmedTime.Hour, this.InspectionTask.ConfirmedTime.Minute,
                        this.InspectionTask.ConfirmedTime.Second);
                this.Appointments.Add(new ScheduleAppointment
                {
                    Subject = "Inspection at " + this.InspectionTask.CustomerName,
                    StartTime = startTime,
                    Location = this.InspectionTask.Address,
                    EndTime = startTime.AddHours(1),
                    Status = new ScheduleAppointmentStatus { Brush = new SolidColorBrush(Colors.DarkMagenta), Status = "Free" },
                });
                this.SaveCommand.RaiseCanExecuteChanged();

                this.AwaitingConfirmationCount = this.PoolofTasks.Count(x => x.Status == BusinessLogic.Enums.TaskStatusEnum.AwaitingConfirmation);
                this.MyInspectionCount = this.PoolofTasks.Count(x => x.Status == BusinessLogic.Enums.TaskStatusEnum.InProgress);
                this.TotalCount = this.PoolofTasks.Count(x => x.ConfirmedDate.Date == DateTime.Today);
                this.IsCommandBarOpen = false;
            }
            , () =>
            {
                return (this.InspectionTask != null && this.InspectionTask.Status == BusinessLogic.Enums.TaskStatusEnum.AwaitingConfirmation);
            }
            );
            
            this.InspectionTask = new BusinessLogic.Task();
        }
        #region Overrides
        async public override void OnNavigatedTo(object navigationParameter, Windows.UI.Xaml.Navigation.NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
            IEnumerable<Eqstra.BusinessLogic.Task> list = null;
            if (navigationParameter.Equals("AwaitingInspections"))
            {
                list = (await SqliteHelper.Storage.LoadTableAsync<Eqstra.BusinessLogic.Task>()).Where(x => x.Status == BusinessLogic.Enums.TaskStatusEnum.AwaitingInspection);
            }
            if (navigationParameter.Equals("AwaitingConfirmation"))
            {
                list = (await SqliteHelper.Storage.LoadTableAsync<Eqstra.BusinessLogic.Task>()).Where(x => x.Status == BusinessLogic.Enums.TaskStatusEnum.AwaitingConfirmation);
            }
            else if (navigationParameter.Equals("Total"))
            {
                list = (await SqliteHelper.Storage.LoadTableAsync<Eqstra.BusinessLogic.Task>()).Where(x => x.ConfirmedDate.Date.Date.Equals(DateTime.Today));
            }
            else if (navigationParameter.Equals("InProgress"))
            {
                list = (await SqliteHelper.Storage.LoadTableAsync<Eqstra.BusinessLogic.Task>()).Where(x => x.Status == BusinessLogic.Enums.TaskStatusEnum.InProgress);
            }
            foreach (Eqstra.BusinessLogic.Task item in list)
            {
                var cust = await SqliteHelper.Storage.GetSingleRecordAsync<Customer>(x => x.Id == item.CustomerId);
                item.CustomerName = cust.Name;
                item.Address = cust.Address;
                this.InspectionList.Add(item);
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

      
        private bool isCommandBarOpen;

        public bool IsCommandBarOpen
        {
            get { return isCommandBarOpen; }
            set { SetProperty(ref isCommandBarOpen, value); }
        }
        private int awaitingTaskCount;

        public int AwaitingInspectionCount
        {
            get { return awaitingTaskCount; }
            set { SetProperty(ref awaitingTaskCount, value); }
        }

        private int awaitingConfirmationCount;

        public int AwaitingConfirmationCount
        {
            get { return awaitingConfirmationCount; }
            set { SetProperty(ref awaitingConfirmationCount, value); }
        }


        private int total;

        public int TotalCount
        {
            get { return total; }
            set { SetProperty(ref total, value); }
        }


        private ObservableCollection<Eqstra.BusinessLogic.Task> poolofTasks;
        public ObservableCollection<Eqstra.BusinessLogic.Task> PoolofTasks
        {
            get { return poolofTasks; }
            set
            {
                SetProperty(ref poolofTasks, value);
            }
        }

        private int myInspectionCount;

        public int MyInspectionCount
        {
            get { return myInspectionCount; }
            set { SetProperty(ref myInspectionCount, value); }
        }

        private Customer customer;

        public Customer Customer
        {
            get { return customer; }
            set { SetProperty(ref customer, value); }
        }

        private ScheduleAppointmentCollection appointments;
        public ScheduleAppointmentCollection Appointments
        {
            get { return appointments; }
            set { SetProperty(ref appointments, value); }
        }

        private CustomerDetails customerDetails;

        public CustomerDetails CustomerDetails
        {
            get { return customerDetails; }
            set { SetProperty(ref customerDetails, value); }
        }
        private Eqstra.BusinessLogic.Task task;

        public Eqstra.BusinessLogic.Task InspectionTask
        {
            get { return task; }
            set
            {
                if (SetProperty(ref task, value))
                {
                    SaveCommand.RaiseCanExecuteChanged();
                    DrivingDirectionCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public DelegateCommand DrivingDirectionCommand { get; set; }
        public DelegateCommand SaveCommand { get; set; }
        #endregion

        #region Methods
        async public System.Threading.Tasks.Task GetCustomerDetailsAsync()
        {
            try
            {
                if (this.InspectionTask != null)
                {
                    this.customer = await SqliteHelper.Storage.GetSingleRecordAsync<Customer>(c => c.Id == this.InspectionTask.CustomerId);
                    this.CustomerDetails.ContactNumber = this.customer.ContactNumber;
                    this.customerDetails.CaseNumber = this.InspectionTask.CaseNumber;
                    this.customerDetails.Status = this.InspectionTask.Status;
                    this.customerDetails.StatusDueDate = this.InspectionTask.StatusDueDate;
                    this.customerDetails.Address = this.customer.Address;
                    this.customerDetails.AllocatedTo = this.InspectionTask.AllocatedTo;
                    this.customerDetails.Name = this.customer.Name;
                    this.customerDetails.CellNumber = this.InspectionTask.CellNumber;
                    this.customerDetails.CaseType = this.InspectionTask.CaseType;
                    this.customerDetails.EmailId = this.customer.EmailId;
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
