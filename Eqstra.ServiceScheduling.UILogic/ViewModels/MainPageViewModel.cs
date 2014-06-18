using Eqstra.BusinessLogic;
using Eqstra.BusinessLogic.Helpers;
using Eqstra.BusinessLogic.ServiceSchedule;
using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using Syncfusion.UI.Xaml.Schedule;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace Eqstra.ServiceScheduling.UILogic.ViewModels
{
    public class MainPageViewModel : ViewModel
    {
        INavigationService _navigationService;
        public MainPageViewModel(INavigationService navigationService)
        {
            this.PoolofTasks = new ObservableCollection<DriverTask>();
            this.Appointments = new ScheduleAppointmentCollection();
            _navigationService = navigationService;
            //this.Appointments = new ScheduleAppointmentCollection
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
            //        Subject = "Inspection at Daren May",
            //        Notes = "some noise from differential",
            //        Location = "Cape Town",
            //         ReadOnly = true,
            //        StartTime =new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,8,00,00),
            //        EndTime = new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,9,00,00),
            //        Status = new ScheduleAppointmentStatus{Brush = new SolidColorBrush(Colors.Green), Status  = "Free"},
            //    },                    
            //};

            this.BingWeatherCommand = new DelegateCommand(() =>
            {

            });

            this.StartSchedulingCommand = new DelegateCommand<object>((obj) =>
            {
                _navigationService.Navigate("ServiceScheduling", this.InspectionTask);
            });


        }

        async public override void OnNavigatedTo(object navigationParameter, Windows.UI.Xaml.Navigation.NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);

          

            var list = await SqliteHelper.Storage.LoadTableAsync<Eqstra.BusinessLogic.Task>();

            foreach (Eqstra.BusinessLogic.Task item in list)
            {
                var cust = await SqliteHelper.Storage.GetSingleRecordAsync<Customer>(x => x.Id == item.CustomerId);
                item.CustomerName = cust.CustomerName;
                if (item.Status == BusinessLogic.Helpers.TaskStatus.Completed)
                {
                    item.ConfirmedDate = DateTime.Today.AddDays(-1);
                    item.ConfirmedTime = DateTime.Now.AddHours(-2);
                }
                item.ConfirmedDate = DateTime.Now;
                if (item.Status == BusinessLogic.Helpers.TaskStatus.AwaitInspectionDetail)
                {
                    item.ConfirmedTime = DateTime.Now.AddHours(list.IndexOf(item));
                }
                item.Address = cust.Address;
                //if (item.Status != BusinessLogic.Helpers.TaskStatus.AwaitingInspection)
                //{
                //    this.Appointments.Add(new ScheduleAppointment
                //    {
                //        Subject = "Inspection at " + item.CustomerName,
                //        StartTime = item.ConfirmedTime,
                //        EndTime = item.ConfirmedTime.AddHours(1),
                //        Location = item.Address,
                //        ReadOnly = true,
                //        Status = new ScheduleAppointmentStatus { Brush = new SolidColorBrush(Colors.LightGreen), Status = "Free" },
                //    });
                //}
                var vehicleDetails = await SqliteHelper.Storage.GetSingleRecordAsync<VehicleDetails>(x => x.RegistrationNumber == item.RegistrationNumber);
                var vehicle = await SqliteHelper.Storage.GetSingleRecordAsync<Vehicle>(x => x.RegistrationNumber == item.RegistrationNumber);

                this.PoolofTasks.Add(new DriverTask
                {
                    RegistrationNumber = item.RegistrationNumber,
                    CaseNumber = item.CaseNumber,
                    VehicleInsRecId=item.VehicleInsRecId,
                    CaseType = item.CaseType,
                    Make = vehicleDetails.Make,
                    Description = vehicleDetails.Description,
                    Address = item.Address,
                    ConfirmedDate = item.ConfirmedDate,
                    ConfirmedTime = item.ConfirmedTime,
                    AllocatedTo = item.AllocatedTo,
                    CustomerId = item.CustomerId,
                    CustomerName = item.CustomerName,
                    Status = item.Status,
                    StatusDueDate = item.StatusDueDate,
                    ModelYear = vehicle.ModelYear
                });
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
