﻿using Eqstra.BusinessLogic;
using Eqstra.BusinessLogic.Commercial;
using Eqstra.BusinessLogic.Helpers;
using Eqstra.BusinessLogic.Passenger;
using Eqstra.VehicleInspection.UILogic.AifServices;
using Eqstra.VehicleInspection.UILogic.VIService;
using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
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
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
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

            try
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(ApplicationData.Current.RoamingSettings.Values[Constants.UserInfo].ToString());
                userInfo.CompanyId = "1000";
                base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
                 await CreateTableAsync();
                //SyncData();

                var weather = await SqliteHelper.Storage.LoadTableAsync<WeatherInfo>();
                this.WeatherInfo = weather.FirstOrDefault();

                var list = (await SqliteHelper.Storage.LoadTableAsync<Eqstra.BusinessLogic.Task>()).TakeWhile(w => w.Status != Eqstra.BusinessLogic.Enums.TaskStatus.AwaitDamageConfirmation).ToList();
                foreach (Eqstra.BusinessLogic.Task item in list)
                {
                    var cust = await SqliteHelper.Storage.GetSingleRecordAsync<Customer>(x => x.Id.Equals(item.CustomerId));
                    item.CustomerName = cust.CustomerName;
                    if (item.Status == BusinessLogic.Enums.TaskStatus.Completed)
                    {
                        item.ConfirmedDate = DateTime.Today.AddDays(-1);
                        item.ConfirmedTime = DateTime.Now.AddHours(-2);
                    }
                    item.ConfirmedDate = DateTime.Now;
                    if (item.Status == BusinessLogic.Enums.TaskStatus.AwaitInspectionDetail)
                    {
                        item.ConfirmedTime = DateTime.Now.AddHours(list.IndexOf(item));
                    }
                    item.Address = cust.Address;
                    //if (item.Status != BusinessLogic.Enums.TaskStatus.AwaitingInspection)
                    //{
                    //    this.Appointments.Add(new ScheduleAppointment
                    //           {
                    //               Subject = "Inspection at " + item.CustomerName,
                    //               StartTime = item.ConfirmedTime,
                    //               EndTime = item.ConfirmedTime.AddHours(1),
                    //               Location = item.Address,
                    //               ReadOnly = true,
                    //               Status = new ScheduleAppointmentStatus { Brush = new SolidColorBrush(Colors.LightGreen), Status = "Free" },
                    //           });
                    //}
                    AppSettingData.Appointments = this.Appointments;
                    this.PoolofTasks.Add(item);
                }
                this.AwaitingConfirmationCount = this.PoolofTasks.Count(x => x.Status == BusinessLogic.Enums.TaskStatus.AwaitInspectionDetail);
                this.MyTasksCount = this.PoolofTasks.Count(x => x.Status == BusinessLogic.Enums.TaskStatus.AwaitInspectionAcceptance || x.Status == BusinessLogic.Enums.TaskStatus.AwaitInspectionDataCapture);

                this.TotalCount = this.PoolofTasks.Count(x => x.ConfirmedDate.Date.Equals(DateTime.Today) && (x.Status == BusinessLogic.Enums.TaskStatus.AwaitInspectionDataCapture || x.Status == BusinessLogic.Enums.TaskStatus.AwaitInspectionAcceptance));
                if (AppSettings.Instance.IsSynchronizing == 0)
                {
                    VIServiceHelper.Instance.Synchronize(async () =>
                       {
                           await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                           {

                               AppSettings.Instance.IsSynchronizing = 1;
                           }
                                );

                          await VIServiceHelper.Instance.SyncTasksFromSvcAsync();
                           await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                                 {

                                     AppSettings.Instance.IsSynchronizing = 0;
                                 }
                                 );

                       });
                }
            }
            catch (Exception)
            {
                

            }
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
        async private void SyncTaskFromService()
        {
            await BackgroundExecutionManager.RequestAccessAsync();
            BackgroundTaskBuilder builder = new BackgroundTaskBuilder();
            builder.TaskEntryPoint = "Eqstra.VehicleInspection.UILogic.ServiceBackgroundTask";
            builder.SetTrigger(new TimeTrigger(15, false));
            builder.AddCondition(new SystemCondition(SystemConditionType.InternetAvailable));
            builder.Name = "ServiceBackgroundTask";
            var taskfromService = builder.Register();
            taskfromService.Completed += taskfromService_Completed;
        }
        void taskfromService_Completed(BackgroundTaskRegistration sender, BackgroundTaskCompletedEventArgs args)
        {
            throw new NotImplementedException();
        }



        private WeatherInfo weatherInfo;

        public WeatherInfo WeatherInfo
        {
            get { return weatherInfo; }
            set { SetProperty(ref weatherInfo, value); }
        }

        private int total;
        [RestorableState]
        public int TotalCount
        {
            get { return total; }
            set { SetProperty(ref total, value); }
        }

        private int awaitingConfirmationCount;
        [RestorableState]
        public int AwaitingConfirmationCount
        {
            get { return awaitingConfirmationCount; }
            set { SetProperty(ref awaitingConfirmationCount, value); }
        }
        private int myTasksCount;
        [RestorableState]
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




        /// <summary>
        ///  this is  Temporary method for create tables in DB
        /// </summary>

        private async System.Threading.Tasks.Task CreateTableAsync()
        {

            ////Drop Existing tables

            await SqliteHelper.Storage.DropTableAsync<Eqstra.BusinessLogic.Task>();
            await SqliteHelper.Storage.CreateTableAsync<Eqstra.BusinessLogic.Task>();

            await SqliteHelper.Storage.DropTableAsync<PVehicleDetails>();
            await SqliteHelper.Storage.DropTableAsync<PTyreCondition>();
            await SqliteHelper.Storage.DropTableAsync<PMechanicalCond>();
            await SqliteHelper.Storage.DropTableAsync<PInspectionProof>();
            await SqliteHelper.Storage.DropTableAsync<PGlass>();
            await SqliteHelper.Storage.DropTableAsync<PBodywork>();
            await SqliteHelper.Storage.DropTableAsync<PTrimInterior>();
            await SqliteHelper.Storage.DropTableAsync<PAccessories>();

            await SqliteHelper.Storage.DropTableAsync<CVehicleDetails>();
            await SqliteHelper.Storage.DropTableAsync<CTyres>();
            await SqliteHelper.Storage.DropTableAsync<CAccessories>();
            await SqliteHelper.Storage.DropTableAsync<CChassisBody>();
            await SqliteHelper.Storage.DropTableAsync<CGlass>();
            await SqliteHelper.Storage.DropTableAsync<CMechanicalCond>();
            await SqliteHelper.Storage.DropTableAsync<CPOI>();
            await SqliteHelper.Storage.DropTableAsync<CCabTrimInter>();
            await SqliteHelper.Storage.DropTableAsync<DrivingDuration>();

            ////create new  tables

            await SqliteHelper.Storage.CreateTableAsync<PVehicleDetails>();
            await SqliteHelper.Storage.CreateTableAsync<PTyreCondition>();
            await SqliteHelper.Storage.CreateTableAsync<PMechanicalCond>();
            await SqliteHelper.Storage.CreateTableAsync<PInspectionProof>();
            await SqliteHelper.Storage.CreateTableAsync<PGlass>();
            await SqliteHelper.Storage.CreateTableAsync<PBodywork>();
            await SqliteHelper.Storage.CreateTableAsync<PTrimInterior>();
            await SqliteHelper.Storage.CreateTableAsync<PAccessories>();

            await SqliteHelper.Storage.CreateTableAsync<CVehicleDetails>();
            await SqliteHelper.Storage.CreateTableAsync<CTyres>();
            await SqliteHelper.Storage.CreateTableAsync<CAccessories>();
            await SqliteHelper.Storage.CreateTableAsync<CChassisBody>();
            await SqliteHelper.Storage.CreateTableAsync<CGlass>();
            await SqliteHelper.Storage.CreateTableAsync<CMechanicalCond>();
            await SqliteHelper.Storage.CreateTableAsync<CPOI>();
            await SqliteHelper.Storage.CreateTableAsync<CCabTrimInter>();
            await SqliteHelper.Storage.CreateTableAsync<DrivingDuration>();
            await SqliteHelper.Storage.CreateTableAsync<DrivingDuration>();
        }

    }
}
