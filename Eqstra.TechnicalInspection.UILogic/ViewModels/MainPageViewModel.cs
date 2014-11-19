using Eqstra.BusinessLogic;
using Eqstra.BusinessLogic.DocumentDelivery;
using Eqstra.BusinessLogic.Helpers;
using Eqstra.TechnicalInspection.UILogic.AifServices;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using Newtonsoft.Json;
using Syncfusion.UI.Xaml.Schedule;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml.Media;

namespace Eqstra.TechnicalInspection.UILogic.ViewModels
{

    public class MainPageViewModel : BaseViewModel
    {
        INavigationService _navigationService;
        IEventAggregator _eventAggregator;

        public MainPageViewModel(INavigationService navigationService, IEventAggregator eventAggregator)
            : base(eventAggregator)
        {
            _eventAggregator = eventAggregator;
            this.PoolofTasks = new ObservableCollection<BusinessLogic.Task>();
            this.Appointments = new ScheduleAppointmentCollection();
            _navigationService = navigationService;

            DrivingDirectionCommand = DelegateCommand.FromAsyncHandler(async () =>
            {
                ApplicationData.Current.LocalSettings.Values["CaseNumber"] = this.InspectionTask.CaseNumber;
                ApplicationData.Current.LocalSettings.Values["CaseServiceRecID"] = this.InspectionTask.CaseServiceRecID;

                string jsonInspectionTask = JsonConvert.SerializeObject(this.InspectionTask);
                var dd = await SqliteHelper.Storage.GetSingleRecordAsync<DrivingDuration>(x => x.CaseNumber == this.InspectionTask.CaseNumber);

                if (dd != null && !dd.StopDateTime.Equals(DateTime.MinValue))
                {
                    _navigationService.Navigate("TechnicalInspection", jsonInspectionTask);
                }
                else
                {
                    navigationService.Navigate("DrivingDirection", jsonInspectionTask);
                }

            }, () =>
            {
                return (this.InspectionTask != null );
            }
            );




            this.SyncCommand = new DelegateCommand(() =>
            {
                if (AppSettings.Instance.IsSynchronizing == 0)
                {
                    TIServiceHelper.Instance.Synchronize(async () =>
                    {
                        await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {

                            AppSettings.Instance.IsSynchronizing = 1;
                        });

                        await TIServiceHelper.Instance.GetTasksAsync();
                        _eventAggregator.GetEvent<Eqstra.BusinessLogic.TasksFetchedEvent>().Publish(this.task);
                        await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                        {
                            this.PoolofTasks.Clear();
                            await GetTasksFromDbAsync();
                            GetAllCount();
                            GetAppointments();
                            AppSettings.Instance.IsSynchronizing = 0;
                            AppSettings.Instance.Synced = true;
                        }
                              );

                    });
                }
            });

        }

        async public override void OnNavigatedTo(object navigationParameter, Windows.UI.Xaml.Navigation.NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {

            try
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(ApplicationData.Current.RoamingSettings.Values[Constants.UserInfo].ToString());

                base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
        
                //SyncData();


                await GetTasksFromDbAsync();
                GetAllCount();
                GetAppointments();

                if (AppSettings.Instance.IsSynchronizing == 0 && !AppSettings.Instance.Synced)
                {
                    TIServiceHelper.Instance.Synchronize(async () =>
                    {
                        await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {

                            AppSettings.Instance.IsSynchronizing = 1;
                        });

                        await TIServiceHelper.Instance.GetTasksAsync();
                        _eventAggregator.GetEvent<Eqstra.BusinessLogic.TasksFetchedEvent>().Publish(this.task);
                        await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                        {
                            this.PoolofTasks.Clear();
                            await GetTasksFromDbAsync();
                            GetAllCount();
                            GetAppointments();

                            AppSettings.Instance.IsSynchronizing = 0;
                            AppSettings.Instance.Synced = true;
                        });

                    });
                }

            }
            catch (Exception ex)
            {
                AppSettings.Instance.ErrorMessage = ex.Message;
            }
        }

        private void GetAllCount()
        {
            this.TotalCount = this.PoolofTasks.Count();
        }

        private void GetAppointments()
        {
            foreach (var item in this.PoolofTasks)
            {
                var startTime = new DateTime(item.ConfirmedDate.Year, item.ConfirmedDate.Month, item.ConfirmedDate.Day, item.ConfirmedTime.Hour, item.ConfirmedTime.Minute,
                           item.ConfirmedTime.Second);
                this.Appointments.Add(

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
        }

        private async System.Threading.Tasks.Task GetTasksFromDbAsync()
        {
            var list = (await SqliteHelper.Storage.LoadTableAsync<Eqstra.BusinessLogic.Task>()).Where(w => w.Status != Eqstra.BusinessLogic.Helpers.TaskStatus.AwaitDamageConfirmation);
            foreach (Eqstra.BusinessLogic.Task item in list)
            {
                this.PoolofTasks.Add(item);
                this.InspectionTask = this.PoolofTasks.FirstOrDefault();
            }
        }


        private Eqstra.BusinessLogic.Task task;
        public Eqstra.BusinessLogic.Task InspectionTask
        {
            get { return task; }
            set
            {
                if (SetProperty(ref task, value))
                {
                    DrivingDirectionCommand.RaiseCanExecuteChanged();
                }
            }
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


        public DelegateCommand SyncCommand { get; set; }

        public DelegateCommand DrivingDirectionCommand { get; set; }
        /// <summary>
        ///  this is  Temporary method for create tables in DB
        /// </summary>

      
    }
}

