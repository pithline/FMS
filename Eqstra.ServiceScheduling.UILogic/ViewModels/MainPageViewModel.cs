﻿using Eqstra.BusinessLogic;
using Eqstra.BusinessLogic.Helpers;
using Eqstra.BusinessLogic.ServiceSchedule;
using Eqstra.ServiceScheduling.UILogic.AifServices;
using Eqstra.ServiceScheduling.UILogic.Helpers;
using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using Syncfusion.UI.Xaml.Schedule;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

                    if (this.InspectionTask.Status == DriverTaskStatus.AwaitServiceBookingConfirmation || this.InspectionTask.Status == DriverTaskStatus.AwaitJobCardCapture)
                    {
                        _navigationService.Navigate("Confirmation", string.Empty);
                    }

                    if (this.InspectionTask.Status == DriverTaskStatus.AwaitSupplierSelection)
                    {
                        _navigationService.Navigate("SupplierSelection", string.Empty);
                    }
                    if (this.InspectionTask.Status == DriverTaskStatus.AwaitServiceBookingDetail)
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
                        if (item != null)
                        {
                            this.PoolofTasks.Add(item);
                            GetAppointments(item);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                this.IsBusy = false;
                AppSettings.Instance.ErrorMessage = ex.Message;
            }
            this.IsBusy = false;
        }
        private void GetAppointments(DriverTask task)
        {
            var startTime = new DateTime(task.ConfirmationDate.Year, task.ConfirmationDate.Month, task.ConfirmationDate.Day);

            this.Appointments.Add(
            new ScheduleAppointment()
            {
                Subject = task.CaseNumber,
                Location = task.Address,
                StartTime = startTime,
                EndTime = startTime.AddHours(12),
                ReadOnly = true,
                AppointmentBackground = new SolidColorBrush(Colors.Crimson),
                AllDay = true,
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
                    this.CustomerDetails.Appointments = this.Appointments;
                }
            }
            catch (Exception ex)
            {
                AppSettings.Instance.ErrorMessage = ex.Message;
            }
        }
    }
}
