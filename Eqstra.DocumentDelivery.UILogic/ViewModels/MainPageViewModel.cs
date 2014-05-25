﻿using Eqstra.BusinessLogic;
using Eqstra.BusinessLogic.Helpers;
using Microsoft.Practices.Prism.StoreApps;

using Syncfusion.UI.Xaml.Schedule;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace Eqstra.DocumentDelivery.UILogic.ViewModels
{
    public class MainPageViewModel : ViewModel
    {

        public MainPageViewModel()
        {
            this.PoolofTasks = new ObservableCollection<BusinessLogic.CollectDeliveryTask>();
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

            this.AssignCommand = new DelegateCommand(async () =>
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
                    this.AssignCommand.RaiseCanExecuteChanged();

                    this.AwaitingInspectionCount = this.PoolofTasks.Count(x => x.Status == BusinessLogic.Enums.TaskStatusEnum.AwaitingInspection);
                    this.MyInspectionCount = this.PoolofTasks.Count(x => x.Status == BusinessLogic.Enums.TaskStatusEnum.InProgress);
                    this.TotalCount = this.PoolofTasks.Count(x => x.ConfirmedDate.Date == DateTime.Today);
                }, () =>
                {
                    return (this.InspectionTask != null && this.InspectionTask.Status == BusinessLogic.Enums.TaskStatusEnum.AwaitingInspection);
                }
            );

        }

        async public override void OnNavigatedTo(object navigationParameter, Windows.UI.Xaml.Navigation.NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);


            var weather = await SqliteHelper.Storage.LoadTableAsync<WeatherInfo>();
             this.WeatherInfo = weather.FirstOrDefault();

            var list = await SqliteHelper.Storage.LoadTableAsync<Eqstra.BusinessLogic.CollectDeliveryTask>();
            foreach (Eqstra.BusinessLogic.CollectDeliveryTask item in list)
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
                if (item.CDTaskStatus != BusinessLogic.Enums.CDTaskStatusEnum.AwaitingDelivery)
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
            this.AwaitingInspectionCount = this.PoolofTasks.Count(x => x.CDTaskStatus == BusinessLogic.Enums.CDTaskStatusEnum.AwaitingConfirmation);
            this.MyInspectionCount = this.PoolofTasks.Count(x => (x.CDTaskStatus != BusinessLogic.Enums.CDTaskStatusEnum.Complete && x.CDTaskStatus != BusinessLogic.Enums.CDTaskStatusEnum.AwaitingConfirmation));
            this.TotalCount = this.PoolofTasks.Count(x => x.ConfirmedDate.Date.Equals(DateTime.Today));
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

        private int awaitingTaskCount;

        public int AwaitingInspectionCount
        {
            get { return awaitingTaskCount; }
            set { SetProperty(ref awaitingTaskCount, value); }
        }

        private int myInspectionCount;

        public int MyInspectionCount
        {
            get { return myInspectionCount; }
            set { SetProperty(ref myInspectionCount, value); }
        }

        private Eqstra.BusinessLogic.CollectDeliveryTask task;

        public Eqstra.BusinessLogic.CollectDeliveryTask InspectionTask
        {
            get { return task; }
            set
            {
                if (SetProperty(ref task, value))
                {
                    AssignCommand.RaiseCanExecuteChanged();
                }
            }
        }


        private ObservableCollection<Eqstra.BusinessLogic.CollectDeliveryTask> poolofTasks;
        public ObservableCollection<Eqstra.BusinessLogic.CollectDeliveryTask> PoolofTasks
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