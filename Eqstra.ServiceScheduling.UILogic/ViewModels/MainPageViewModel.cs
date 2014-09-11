using Eqstra.BusinessLogic;
using Eqstra.BusinessLogic.Helpers;
using Eqstra.BusinessLogic.ServiceSchedule;
using Eqstra.ServiceScheduling.UILogic.AifServices;
using Eqstra.ServiceScheduling.UILogic.Helpers;
using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using Newtonsoft.Json;
using Syncfusion.UI.Xaml.Schedule;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace Eqstra.ServiceScheduling.UILogic.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        INavigationService _navigationService;
        public MainPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            this.PoolofTasks = new ObservableCollection<DriverTask>();
            this.Appointments = new ScheduleAppointmentCollection();
            this.CustomerDetails = new CustomerDetails();
            this.CustomerDetails.Appointments = new ScheduleAppointmentCollection();
            _navigationService = navigationService;
            this.BingWeatherCommand = new DelegateCommand(() =>
            {
            });

            this.StartSchedulingCommand = new DelegateCommand<object>((obj) =>
            {
                try
                {
                    GetCustomerDetailsAsync();
                    PersistentData.RefreshInstance();//Here only setting data in new instance, and  getting data in every page.
                    PersistentData.Instance.DriverTask = this.InspectionTask;
                    PersistentData.Instance.CustomerDetails = this.CustomerDetails;

                    if (this.InspectionTask.Status == DriverTaskStatus.AwaitServiceConfirmation || this.InspectionTask.Status == DriverTaskStatus.AwaitJobCardCapture)
                    {
                        _navigationService.Navigate("Confirmation", string.Empty);
                    }

                    if (this.InspectionTask.Status == DriverTaskStatus.AwaitSupplierSelection)
                    {
                        _navigationService.Navigate("SupplierSelection", string.Empty);
                    }
                    if (this.InspectionTask.Status == DriverTaskStatus.AwaitServiceDetail || this.InspectionTask.Status == DriverTaskStatus.AwaitServiceBookingDetail)
                    {
                        _navigationService.Navigate("ServiceScheduling", string.Empty);
                    }
                }
                catch (Exception ex)
                {
                    AppSettings.Instance.ErrorMessage = ex.Message;
                }
            });
        }

        async public override void OnNavigatedTo(object navigationParameter, Windows.UI.Xaml.Navigation.NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            this.IsBusy = true;
            base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
            var list = await SSProxyHelper.Instance.GetTasksFromSvcAsync();
            try
            {
                if (list != null)
                {
                    foreach (Eqstra.BusinessLogic.ServiceSchedule.DriverTask item in list)
                    {
                        this.PoolofTasks.Add(item);
                        GetAppointments(item);
                    }
                }

            }
            catch (Exception ex)
            {
                AppSettings.Instance.ErrorMessage = ex.Message;
            }
            this.IsBusy = false;
        }
        private void GetAppointments(DriverTask task)
        {
            var startTime = new DateTime(task.ConfirmedTime.Year, task.ConfirmedTime.Month, task.ConfirmedTime.Day, task.ConfirmedTime.Hour, task.ConfirmedTime.Minute,
                       task.ConfirmedTime.Second);
            this.Appointments.Add(
            new ScheduleAppointment()
            {
                Subject = task.CaseNumber,
                Location = task.Address,
                StartTime = startTime,
                EndTime = startTime.AddHours(1),
                ReadOnly = true,
                AppointmentBackground = new SolidColorBrush(Colors.Crimson),
                Status = new ScheduleAppointmentStatus { Status = task.Status, Brush = new SolidColorBrush(Colors.Chocolate) }

            });
        }

        private DriverTask task;
        public DriverTask InspectionTask
        {
            get { return task; }
            set
            {
                SetProperty(ref task, value);
            }
        }

        private ObservableCollection<DriverTask> poolofTasks;
        public ObservableCollection<DriverTask> PoolofTasks
        {
            get { return poolofTasks; }
            set
            {
                SetProperty(ref poolofTasks, value);
            }
        }

        private ScheduleAppointmentCollection appointments;
        public ScheduleAppointmentCollection Appointments
        {
            get { return appointments; }
            set { SetProperty(ref appointments, value); }
        }
        public DelegateCommand BingWeatherCommand { get; set; }
        public DelegateCommand<object> StartSchedulingCommand { get; set; }

        private CustomerDetails customerDetails;
        public CustomerDetails CustomerDetails
        {
            get { return customerDetails; }
            set { SetProperty(ref customerDetails, value); }
        }
        private void GetCustomerDetailsAsync()
        {
            try
            {
                if (this.InspectionTask != null)
                {
                    this.CustomerDetails.ContactNumber = this.InspectionTask.CustPhone;
                    this.CustomerDetails.CaseNumber = this.InspectionTask.CaseNumber;
                    this.CustomerDetails.VehicleInsRecId = this.InspectionTask.VehicleInsRecId;
                    this.CustomerDetails.Status = this.InspectionTask.Status;
                    this.CustomerDetails.StatusDueDate = this.InspectionTask.StatusDueDate;
                    this.CustomerDetails.Address = this.InspectionTask.Address;
                    this.CustomerDetails.AllocatedTo = this.InspectionTask.AllocatedTo;
                    this.CustomerDetails.CustomerName = this.InspectionTask.CustomerName;
                    this.CustomerDetails.ContactName = this.InspectionTask.CustomerName;
                    this.CustomerDetails.EmailId = this.InspectionTask.CusEmailId;
                    this.CustomerDetails.CategoryType = this.InspectionTask.CaseCategory;

                    var startTime = new DateTime(this.InspectionTask.ConfirmedTime.Year, this.InspectionTask.ConfirmedTime.Month, this.InspectionTask.ConfirmedTime.Day, this.InspectionTask.ConfirmedTime.Hour, this.InspectionTask.ConfirmedTime.Minute,
                                this.InspectionTask.ConfirmedTime.Second);
                    this.CustomerDetails.Appointments.Add(
                                                        new ScheduleAppointment()
                                                        {
                                                            Subject = this.InspectionTask.CaseNumber,
                                                            Location = this.InspectionTask.Address,
                                                            StartTime = startTime,
                                                            EndTime = startTime.AddHours(1),
                                                            ReadOnly = true,
                                                            AppointmentBackground = new SolidColorBrush(Colors.Crimson),
                                                            Status = new ScheduleAppointmentStatus { Status = this.InspectionTask.Status, Brush = new SolidColorBrush(Colors.Chocolate) }

                                                        });

                }
            }
            catch (Exception ex)
            {
                AppSettings.Instance.ErrorMessage = ex.Message;
            }
        }
    }
}
