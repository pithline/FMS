using Eqstra.BusinessLogic;
using Eqstra.BusinessLogic.Helpers;
using Eqstra.BusinessLogic.ServiceSchedule;
using Eqstra.ServiceScheduling.UILogic.AifServices;
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

            _navigationService = navigationService;

            this.BingWeatherCommand = new DelegateCommand(() =>
            {
            });

            this.StartSchedulingCommand = new DelegateCommand<object>((obj) =>
            {
                string jsonInspectionTask = JsonConvert.SerializeObject(this.InspectionTask);
                _navigationService.Navigate("ServiceScheduling", jsonInspectionTask);
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
                        //if (item.Status == BusinessLogic.Helpers.TaskStatus.Completed)
                        //{
                        //    item.ConfirmedDate = DateTime.Today.AddDays(-1);
                        //    item.ConfirmedTime = DateTime.Now.AddHours(-2);
                        //}
                        //item.ConfirmedDate = DateTime.Now;
                        //if (item.Status == BusinessLogic.Helpers.TaskStatus.AwaitInspectionDetail)
                        //{
                        //    item.ConfirmedTime = DateTime.Now.AddHours(list.IndexOf(item));
                        //}

                        this.PoolofTasks.Add(item);


                        //var vehicleDetails = await SqliteHelper.Storage.GetSingleRecordAsync<VehicleDetails>(x => x.RegistrationNumber == item.RegistrationNumber);
                        //var vehicle = await SqliteHelper.Storage.GetSingleRecordAsync<Vehicle>(x => x.RegistrationNumber == item.RegistrationNumber);

                        //this.PoolofTasks.Add(new DriverTask
                        //{
                        //    RegistrationNumber = item.RegistrationNumber,
                        //    CaseNumber = item.CaseNumber,
                        //    VehicleInsRecId=item.VehicleInsRecId,
                        //    CaseType = item.CaseType,
                        //    Make = vehicleDetails.Make,
                        //    Description = vehicleDetails.Description,
                        //    Address = item.Address,
                        //    ConfirmedDate = item.ConfirmedDate,
                        //    ConfirmedTime = item.ConfirmedTime,
                        //    AllocatedTo = item.AllocatedTo,
                        //    CustomerId = item.CustomerId,
                        //    CustomerName = item.CustomerName,
                        //    Status = item.Status,
                        //    StatusDueDate = item.StatusDueDate,
                        //    ModelYear = vehicle.ModelYear
                        //});
                    }

                    GetAppointments();

                }

            }
            catch (Exception)
            {

                throw;
            }
            this.IsBusy = false;

        }



        private void GetAppointments()
        {
            foreach (var item in this.PoolofTasks)
            {

                var startTime = new DateTime(item.ConfirmedTime.Year, item.ConfirmedTime.Month, item.ConfirmedTime.Day, item.ConfirmedTime.Hour, item.ConfirmedTime.Minute,
                           item.ConfirmedTime.Second);
                this.Appointments = new ScheduleAppointmentCollection
            {
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
                         
                         };
            }

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

    }
}
