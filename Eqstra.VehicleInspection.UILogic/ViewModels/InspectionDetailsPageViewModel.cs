using Eqstra.BusinessLogic;
using Eqstra.BusinessLogic.Helpers;
using Eqstra.VehicleInspection.UILogic.AifServices;
using Eqstra.VehicleInspection.UILogic.Events;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using Newtonsoft.Json;
using Syncfusion.UI.Xaml.Grid;
using Syncfusion.UI.Xaml.Schedule;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage;
using Windows.System;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Notifications;
using Windows.UI.Xaml.Media;

namespace Eqstra.VehicleInspection.UILogic.ViewModels
{
    public class InspectionDetailsPageViewModel : BaseViewModel
    {
        INavigationService _navigationService;
        IEventAggregator _eventAggregator;
        public InspectionDetailsPageViewModel(INavigationService navigationService, IEventAggregator eventAggregator)
            : base(navigationService)
        {
            this.InspectionList = new ObservableCollection<BusinessLogic.Task>();
            this.CustomerDetails = new CustomerDetails();
            this.PoolofTasks = new ObservableCollection<BusinessLogic.Task>();
            this.Appointments = new ScheduleAppointmentCollection();
            _navigationService = navigationService;
            _eventAggregator = eventAggregator;
            DrivingDirectionCommand = DelegateCommand.FromAsyncHandler(async () =>
            {
                ApplicationData.Current.LocalSettings.Values["CaseNumber"] = this.InspectionTask.CaseNumber;
                ApplicationData.Current.LocalSettings.Values["VehicleInsRecId"] = this.InspectionTask.VehicleInsRecId;

                string jsonInspectionTask = JsonConvert.SerializeObject(this.InspectionTask);
                var dd = await SqliteHelper.Storage.GetSingleRecordAsync<DrivingDuration>(x => x.VehicleInsRecID == this.InspectionTask.VehicleInsRecId);

                if (dd != null && !dd.StopDateTime.Equals(DateTime.MinValue))
                {
                    _navigationService.Navigate("VehicleInspection", jsonInspectionTask);
                }
                else
                {
                    navigationService.Navigate("DrivingDirection", jsonInspectionTask);
                }
                
            }, () =>
            {
                return (this.InspectionTask != null);
            }
            );




            this.SaveCommand = new DelegateCommand(async () =>
            {
                this.IsBusy = true;
                if (this.InspectionTask.ConfirmedDate < DateTime.Today)
                {
                    Util.ShowToast("Confirmed Date should not be less than today's date");
                }
                else
                {
                    this.InspectionTask.ProcessStep = ProcessStep.ConfirmInspectionDetails;
                    this.InspectionTask.Status = Eqstra.BusinessLogic.Helpers.TaskStatus.AwaitInspectionDataCapture;
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
                    this.IsCommandBarOpen = false;
                    await VIServiceHelper.Instance.UpdateTaskStatusAsync();
                    IsBusy = false;
                    navigationService.GoBack();
                }
            }
            , () =>
            {
                return (this.InspectionTask != null);
            }
            );
        }
        #region Overrides
        async public override void OnNavigatedTo(object navigationParameter, Windows.UI.Xaml.Navigation.NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            try
            {
                base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
                
                //foreach (var t in tasks)
                //{
                //    var cust = await SqliteHelper.Storage.GetSingleRecordAsync<Customer>(x => x.Id == t.CustomerId);
                //    if (cust != null)
                //    {
                //        t.CustomerName = cust.CustomerName;
                //        t.Address = cust.Address;
                //    }
                //}

                _eventAggregator.GetEvent<TasksFetchedEvent>().Subscribe(async o =>
                    {

                        await ShowTasksAsync(navigationParameter);

                    }, ThreadOption.UIThread);

                
                 await ShowTasksAsync(navigationParameter);
            }
            catch (SQLite.SQLiteException)
            {


            }
        }

        private async System.Threading.Tasks.Task ShowTasksAsync(object navigationParameter)
        {
            var list = EnumerateTasks(navigationParameter, await SqliteHelper.Storage.LoadTableAsync<Eqstra.BusinessLogic.Task>());
            this.InspectionList.Clear();
            foreach (var item in list)
            {
                this.InspectionList.Add(item);
            }

            this.InspectionTask = list.FirstOrDefault();
           
            _eventAggregator.GetEvent<CustFetchedEvent>().Subscribe(async b =>
            {
                await GetCustomerDetailsAsync(b);
            });
        }

        private IEnumerable<BusinessLogic.Task> EnumerateTasks(object navigationParameter, IEnumerable<BusinessLogic.Task> tasks)
        {
            try
            {
                IEnumerable<Eqstra.BusinessLogic.Task> list = null;
                if (navigationParameter.Equals("AwaitInspectionDetail"))
                {
                    NavigationMode = Syncfusion.UI.Xaml.Grid.NavigationMode.Cell;
                    list = (tasks).Where(x => x.Status.Equals(BusinessLogic.Helpers.TaskStatus.AwaitingConfirmation)
                        || x.Status.Equals(BusinessLogic.Helpers.TaskStatus.AwaitCollectionConfirmation));
                    list.AsParallel().ForAll(x => x.AllowEditing = true);
                        //|| x.Status.Equals(BusinessLogic.Helpers.TaskStatus.AwaitCollectionDetail));
                }
                if (navigationParameter.Equals("Total"))
                {
                    NavigationMode = Syncfusion.UI.Xaml.Grid.NavigationMode.Row;
                    list = (tasks).Where(x => DateTime.Equals(x.ConfirmedDate, DateTime.Today) && (x.Status.Equals(BusinessLogic.Helpers.TaskStatus.AwaitInspectionDataCapture)));
                    list.AsParallel().ForAll(x=>x.AllowEditing = false);
                }
                if (navigationParameter.Equals("MyTasks"))
                {
                    NavigationMode = Syncfusion.UI.Xaml.Grid.NavigationMode.Row;
                    list = (tasks).Where(x => x.Status.Equals(BusinessLogic.Helpers.TaskStatus.AwaitInspectionDataCapture));
                    list.AsParallel().ForAll(x => x.AllowEditing = false);
                }
                this.CustomerDetails.Appointments = new ScheduleAppointmentCollection();
                foreach (var item in tasks.Where(x => x.Status.Equals(BusinessLogic.Helpers.TaskStatus.AwaitInspectionDataCapture) || x.Status.Equals(BusinessLogic.Helpers.TaskStatus.AwaitInspectionAcceptance)))
                {
                    var startTime = new DateTime(item.ConfirmedDate.Year, item.ConfirmedDate.Month, item.ConfirmedDate.Day, item.ConfirmedTime.Hour, item.ConfirmedTime.Minute,
                               item.ConfirmedTime.Second);
                    this.CustomerDetails.Appointments.Add(

                                  new ScheduleAppointment()
                                  {
                                      Subject = item.CaseNumber,
                                      Location = item.Address,
                                      StartTime = startTime,
                                      EndTime = startTime.AddHours(1),
                                      ReadOnly = true,
                                      AppointmentBackground = new SolidColorBrush(Colors.Crimson),
                                      Status = new ScheduleAppointmentStatus { Status = item.Status, Brush = new SolidColorBrush(Colors.Chocolate) }

                                  }
                             );
                }
                return list;
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        #region Properties
        private bool allowEditing;
        [RestorableState]
        public bool AllowEditing
        {
            get { return allowEditing; }
            set { SetProperty(ref allowEditing, value); }
        }

        private ObservableCollection<Eqstra.BusinessLogic.Task> inspectionList;

        public ObservableCollection<Eqstra.BusinessLogic.Task> InspectionList
        {
            get { return inspectionList; }
            set { SetProperty(ref inspectionList, value); }
        }

        private bool isSave;
        [RestorableState]
        public bool IsSave
        {
            get { return isSave; }
            set { SetProperty(ref isSave, value); }
        }

        private bool isBusy;

        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }


        private bool isNext;
        [RestorableState]
        public bool IsNext
        {
            get { return isNext; }
            set { SetProperty(ref isNext, value); }
        }

        private NavigationMode navigationMode;

        public NavigationMode NavigationMode
        {
            get { return navigationMode; }
            set { SetProperty(ref navigationMode, value); }
        }


        private bool isCommandBarOpen;
        [RestorableState]
        public bool IsCommandBarOpen
        {
            get { return isCommandBarOpen; }
            set { SetProperty(ref isCommandBarOpen, value); }
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
        async public System.Threading.Tasks.Task GetCustomerDetailsAsync(bool isAppBarOpen)
        {
            try
            {
                if (this.InspectionTask != null)
                {
                    this.Customer = await SqliteHelper.Storage.GetSingleRecordAsync<Customer>(c => c.Id == this.InspectionTask.CustomerId);
                    if (this.Customer == null)
                    {
                            AppSettings.Instance.IsSyncingCustDetails = 1;
                        
                    }
                    else
                    {
                            AppSettings.Instance.IsSyncingCustDetails = 0;
                            this.CustomerDetails.ContactNumber = this.customer.ContactNumber;
                            this.CustomerDetails.CaseNumber = this.InspectionTask.CaseNumber;
                            this.CustomerDetails.VehicleInsRecId = this.InspectionTask.VehicleInsRecId;
                            this.CustomerDetails.Status = this.InspectionTask.Status;
                            this.CustomerDetails.StatusDueDate = this.InspectionTask.StatusDueDate;
                            this.CustomerDetails.Address = this.customer.Address;
                            this.CustomerDetails.AllocatedTo = this.InspectionTask.AllocatedTo;
                            this.CustomerDetails.CustomerName = this.customer.CustomerName;
                            this.CustomerDetails.ContactName = this.customer.ContactName;
                            this.CustomerDetails.CategoryType = this.InspectionTask.CategoryType;

                            this.CustomerDetails.EmailId = this.customer.EmailId;
                            
                    }
                   
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
