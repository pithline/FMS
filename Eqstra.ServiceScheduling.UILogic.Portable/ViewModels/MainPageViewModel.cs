using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Xaml;
using Eqstra.BusinessLogic.Portable.SSModels;
using Eqstra.ServiceScheduling.UILogic.Portable.Services;
using System.Text.RegularExpressions;
using Windows.ApplicationModel.Appointments;

namespace Eqstra.ServiceScheduling.UILogic.Portable
{
    public class MainPageViewModel : ViewModel
    {
        private INavigationService _navigationService;
        private ITaskService _taskService;
        public MainPageViewModel(INavigationService navigationService, ITaskService taskService)
        {
            this._navigationService = navigationService;
            this._taskService = taskService;
            
            this.PoolofTasks = new ObservableCollection<BusinessLogic.Portable.SSModels.Task>();
            this.Tasks = new ObservableCollection<BusinessLogic.Portable.SSModels.Task>();

            this.NextPageCommand = DelegateCommand.FromAsyncHandler(
             async () =>
             {
                 try
                 {
                     if (this.InspectionTask != null && this.InspectionTask.Status == DriverTaskStatus.AwaitServiceBookingDetail)
                     {
                         navigationService.Navigate("ServiceScheduling", this.InspectionTask);
                     }
                     else
                     {
                         navigationService.Navigate("PreferredSupplier", this.InspectionTask);
                     }
                 }
                 catch (Exception ex)
                 {
                 }
                 finally
                 {

                 }
             }
);




            // this.Location = new Bing.Maps.Location();
            this.MakeIMCommand = DelegateCommand.FromAsyncHandler(async () =>
            {
                await Launcher.LaunchUriAsync(new Uri("whatsapp:" + 9290650135));
            }, () => { return(this.InspectionTask !=null && !string.IsNullOrEmpty(this.InspectionTask.CustPhone)); });

            this.MakeCallCommand = DelegateCommand.FromAsyncHandler(async () =>
            {
                await Launcher.LaunchUriAsync(new Uri("callto:" + 9290650135));
            }, () => { return (this.InspectionTask !=null && !string.IsNullOrEmpty(this.InspectionTask.CustPhone)); });

            this.MailToCommand = DelegateCommand.FromAsyncHandler(async () =>
            {
                await Launcher.LaunchUriAsync(new Uri("mailto:" + "kasif@mzkgbl.com"));
            }, () => { return (this.InspectionTask !=null && !string.IsNullOrEmpty(this.InspectionTask.CusEmailId)); });

            this.MailToCommand = DelegateCommand.FromAsyncHandler(async () =>
            {

            }, () => { return (this.InspectionTask != null && !string.IsNullOrEmpty(this.InspectionTask.CusEmailId)); });



            this.LocateCommand = DelegateCommand.FromAsyncHandler(async () =>
            {
                await Windows.System.Launcher.LaunchUriAsync(new Uri("bingmaps:?cp=40.726966~-74.006076"));
                //var stringBuilder = new StringBuilder("bingmaps:?where=" + Regex.Replace(address, "\n", ","));
                //await Launcher.LaunchUriAsync(new Uri(stringBuilder.ToString()));
            }, () =>
            {
                return (this.InspectionTask !=null && !string.IsNullOrEmpty(this.InspectionTask.Address));
            });
        }


       

        private Visibility taskProgressBar;
        public Visibility TaskProgressBar
        {
            get { return taskProgressBar; }
            set
            {
                SetProperty(ref taskProgressBar, value);
            }
        }

        private Eqstra.BusinessLogic.Portable.SSModels.Task task;
        public Eqstra.BusinessLogic.Portable.SSModels.Task InspectionTask
        {
            get { return task; }
            set
            {
                SetProperty(ref task, value);
                this.LocateCommand.RaiseCanExecuteChanged();
                this.MailToCommand.RaiseCanExecuteChanged();
                this.MakeCallCommand.RaiseCanExecuteChanged();
                this.MakeIMCommand.RaiseCanExecuteChanged();
                this.NextPageCommand.RaiseCanExecuteChanged();
            }
        }

        private ObservableCollection<Eqstra.BusinessLogic.Portable.SSModels.Task> poolofTasks;
        public ObservableCollection<Eqstra.BusinessLogic.Portable.SSModels.Task> PoolofTasks
        {
            get { return poolofTasks; }
            set
            {
                SetProperty(ref poolofTasks, value);
            }
        }

        private ObservableCollection<Eqstra.BusinessLogic.Portable.SSModels.Task> tasks;
        public ObservableCollection<Eqstra.BusinessLogic.Portable.SSModels.Task> Tasks
        {
            get { return tasks; }
            set
            {
                SetProperty(ref tasks, value);
            }
        }

        public DelegateCommand NextPageCommand { get; private set; }
        public async override void OnNavigatedTo(object navigationParameter, Windows.UI.Xaml.Navigation.NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            try
            {
                base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
                var tasksResult = await this._taskService.GetTasksAsync(new UserInfo { UserId = "axbcsvc", CompanyId = "1095" });
                foreach (var task in tasksResult)
                {
                    task.Address = Regex.Replace(task.Address, ",", "\n");
                    if (task.Status == DriverTaskStatus.AwaitServiceBookingDetail)
                    {
                        this.PoolofTasks.Add(task);
                    }
                    else
                    {
                        this.Tasks.Add(task);
                    }
                }
                this.TaskProgressBar = Visibility.Collapsed;
                PersistentData.Instance.PoolofTasks = this.PoolofTasks;
                PersistentData.Instance.Tasks = this.Tasks;
            }
            catch (Exception)
            {
                this.TaskProgressBar = Visibility.Collapsed;
            }

        }
        private void GetAppointments(Eqstra.BusinessLogic.Portable.SSModels.Task task)
        {
            try
            {

                var appointment = new Windows.ApplicationModel.Appointments.Appointment();

                // StartTime
                var date = task.AppointmentStart.Date;
                var time = task.AppointmentEnd.TimeOfDay - task.AppointmentStart.TimeOfDay;
                var timeZoneOffset = TimeZoneInfo.Local.GetUtcOffset(DateTime.Now);
                var startTime = new DateTimeOffset(date.Year, date.Month, date.Day, time.Hours,
                    time.Minutes, 0, timeZoneOffset);
                appointment.StartTime = startTime;
                appointment.Subject = task.CaseNumber;
                appointment.Location = task.Address;
                appointment.Details = task.Description;
                appointment.Duration = TimeSpan.FromHours(1);
                appointment.Reminder = TimeSpan.FromMinutes(15);

            }
            catch (Exception)
            {

                throw;
            }
        }

        public DelegateCommand MailToCommand { get; set; }

        public DelegateCommand MakeIMCommand { get; set; }

        public DelegateCommand LocateCommand { get; set; }

        public DelegateCommand MakeCallCommand { get; set; }

        //private Bing.Maps.Location location;
        //public Bing.Maps.Location Location
        //{
        //    get { return location; }
        //    set { SetProperty(ref location, value); }
        //}

    }
}
