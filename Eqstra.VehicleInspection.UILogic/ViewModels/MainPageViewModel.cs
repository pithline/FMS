using Eqstra.BusinessLogic;
using Eqstra.BusinessLogic.Helpers;
using Eqstra.VehicleInspection.UILogic.VIService;
using Microsoft.Practices.Prism.StoreApps;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Syncfusion.UI.Xaml.Schedule;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml.Media;

namespace Eqstra.VehicleInspection.UILogic.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {

        public MainPageViewModel()
        {
            this.PoolofTasks = new ObservableCollection<BusinessLogic.Task>();
            this.Appointments = new ScheduleAppointmentCollection();
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

        }

        async public override void OnNavigatedTo(object navigationParameter, Windows.UI.Xaml.Navigation.NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);

            Synchronize(async () =>
            {
                //this.IsSynchronizing = true;
                //VIService.MzkVehicleInspectionServiceClient client = new VIService.MzkVehicleInspectionServiceClient();
                //client.ClientCredentials.Windows.ClientCredential = new NetworkCredential("rchivukula", "Password3", "lfmd");
                //var res = await client.getTasksAsync("rchivukula");
                //if (res != null && res.response.Count > 0)
                //{
                //    await SqliteHelper.Storage.DropTableAsync<Eqstra.BusinessLogic.Task>();

                //    foreach (var item in res.response)
                //    {
                //        await SqliteHelper.Storage.InsertSingleRecordAsync<Eqstra.BusinessLogic.Task>(new Eqstra.BusinessLogic.Task
                //        {
                //            Address = item.parmCustAddress,
                //            CaseNumber = item.parmCaseID,
                //            CaseCategory = item.parmCaseCategory,
                //            StatusDueDate = item.parmStatusDueDate,
                //            ConfirmedDate = item.parmConfirmedDueDate,
                //            CustomerName = item.parmCustName
                //        });
                //    }
                //}
                this.IsSynchronizing = false;
          
            });
            //SyncData();

            var weather = await SqliteHelper.Storage.LoadTableAsync<WeatherInfo>();
            this.WeatherInfo = weather.FirstOrDefault();

            var list = await SqliteHelper.Storage.LoadTableAsync<Eqstra.BusinessLogic.Task>();
            foreach (Eqstra.BusinessLogic.Task item in list)
            {
                var cust = await SqliteHelper.Storage.GetSingleRecordAsync<Customer>(x => x.Id == item.CustomerId);
                item.CustomerName = cust.CustomerName;
                if (item.Status == BusinessLogic.Enums.TaskStatusEnum.Completed)
                {
                    item.ConfirmedDate = DateTime.Today.AddDays(-1);
                    item.ConfirmedTime = DateTime.Now.AddHours(-2);
                }
                item.ConfirmedDate = DateTime.Now;
                if (item.Status == BusinessLogic.Enums.TaskStatusEnum.InProgress)
                {
                    item.ConfirmedTime = DateTime.Now.AddHours(list.IndexOf(item));
                }
                item.Address = cust.Address;
                if (item.Status != BusinessLogic.Enums.TaskStatusEnum.AwaitingInspection)
                {
                    this.Appointments.Add(new ScheduleAppointment
                           {
                               Subject = "Inspection at " + item.CustomerName,
                               StartTime = item.ConfirmedTime,
                               EndTime = item.ConfirmedTime.AddHours(1),
                               Location = item.Address,
                               ReadOnly = true,
                               Status = new ScheduleAppointmentStatus { Brush = new SolidColorBrush(Colors.LightGreen), Status = "Free" },
                           });
                }
                AppSettingData.Appointments = this.Appointments;
                this.PoolofTasks.Add(item);
            }
            this.AwaitingConfirmationCount= this.PoolofTasks.Count(x => x.Status == BusinessLogic.Enums.TaskStatusEnum.AwaitingConfirmation);
            this.MyTasksCount = this.PoolofTasks.Count(x => x.Status == BusinessLogic.Enums.TaskStatusEnum.AwaitInspectionAcceptance || x.Status == BusinessLogic.Enums.TaskStatusEnum.AwaitInspectionDataCapture);
            
            this.TotalCount = this.PoolofTasks.Count(x => x.ConfirmedDate.Date.Equals(DateTime.Today));
        }
        async private void SyncData()
        {
            await BackgroundExecutionManager.RequestAccessAsync();
            BackgroundTaskBuilder builder = new BackgroundTaskBuilder();
            builder.TaskEntryPoint = "Eqstra.VehicleInspection.BackgroundTask.SilentSync";
            builder.SetTrigger(new TimeTrigger(15, false));
            builder.AddCondition(new SystemCondition(SystemConditionType.InternetAvailable));
            builder.Name = "SilentSync";
            var task = builder.Register();
            task.Completed += task_Completed;

        }

        void task_Completed(BackgroundTaskRegistration sender, BackgroundTaskCompletedEventArgs args)
        {

        }

        private WeatherInfo weatherInfo;

        public WeatherInfo WeatherInfo
        {
            get { return weatherInfo; }
            set { SetProperty(ref weatherInfo, value); }
        }

        private int total;

        public int TotalCount
        {
            get { return total; }
            set { SetProperty(ref total, value); }
        }

        private int awaitingConfirmationCount;

        public int AwaitingConfirmationCount
        {
            get { return awaitingConfirmationCount; }
            set { SetProperty(ref awaitingConfirmationCount, value); }
        }
        private int myTasksCount;

        public int MyTasksCount
        {
            get { return myTasksCount; }
            set { SetProperty(ref myTasksCount, value); }
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

        private ScheduleAppointmentCollection appointments;
        public ScheduleAppointmentCollection Appointments
        {
            get { return appointments; }
            set { SetProperty(ref appointments, value); }
        }
        public DelegateCommand BingWeatherCommand { get; set; }

        public DelegateCommand AssignCommand { get; set; }

    }
}
