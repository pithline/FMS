using Eqstra.BusinessLogic;
using Eqstra.BusinessLogic.DeliveryModel;
using Eqstra.BusinessLogic.DocumentDelivery;
using Eqstra.BusinessLogic.Enums;
using Eqstra.BusinessLogic.Helpers;
using Eqstra.DocumentDelivery.UILogic.AifServices;
using Eqstra.DocumentDelivery.UILogic.Helpers;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.StoreApps;
using Newtonsoft.Json;
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
using Windows.UI.Core;
using Windows.UI.Xaml.Media;

namespace Eqstra.DocumentDelivery.UILogic.ViewModels
{
    public class MainPageViewModel : ViewModel
    {
        IEventAggregator _eventAggregator;
        public MainPageViewModel(IEventAggregator eventAggregator)
        {
            this.PoolofTasks = new ObservableCollection<BusinessLogic.CollectDeliveryTask>();
            this.Appointments = new ScheduleAppointmentCollection();
            _eventAggregator = eventAggregator;
            this.SyncCommand = new DelegateCommand(() =>
            {
                if (AppSettings.Instance.IsSynchronizing == 0)
                {

                    DDServiceProxyHelper.Instance.Synchronize(async () =>
                    {
                        await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {

                            AppSettings.Instance.IsSynchronizing = 1;
                        });

                        await DDServiceProxyHelper.Instance.SyncTasksFromSvcAsync();
                        _eventAggregator.GetEvent<TasksFetchedEvent>().Publish(this.task);
                        await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                        {
                            this.PoolofTasks.Clear();
                            await GetTasksFromDbAsync();
                            GetAllCount();

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
                base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
                this.UserInfo = PersistentData.Instance.UserInfo;
                await GetTasksFromDbAsync();
                GetAllCount();
                GetAppointments();

                if (AppSettings.Instance.IsSynchronizing == 0 && !AppSettings.Instance.Synced)
                {
                    DDServiceProxyHelper.Instance.Synchronize(async () =>
                    {
                        await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {
                            AppSettings.Instance.IsSynchronizing = 1;
                        });

                        await DDServiceProxyHelper.Instance.SyncTasksFromSvcAsync();
                        _eventAggregator.GetEvent<TasksFetchedEvent>().Publish(this.task);
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
            }
            catch (Exception ex)
            {
                AppSettings.Instance.ErrorMessage = ex.Message;
            }
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
        public int AwaitingConfirmationCount
        {
            get { return awaitingTaskCount; }
            set { SetProperty(ref awaitingTaskCount, value); }
        }

        private int myTaskCount;
        public int MyTaskCount
        {
            get { return myTaskCount; }
            set { SetProperty(ref myTaskCount, value); }
        }
        public DelegateCommand SyncCommand { get; set; }

        private Eqstra.BusinessLogic.CollectDeliveryTask task;
        public Eqstra.BusinessLogic.CollectDeliveryTask _cdTask
        {
            get { return task; }
            set
            {
                SetProperty(ref task, value);
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

        private CDUserInfo userInfo;

        public CDUserInfo UserInfo
        {
            get { return userInfo; }
            set { SetProperty(ref userInfo, value); }
        }

        /// <summary>
        /// / testing temporary code.
        /// </summary>
        /// <returns></returns>
        private async System.Threading.Tasks.Task CreateTableAsync()
        {


            //var d = new ObservableCollection<Document>
            //  {
            //      new Document{VehicleInsRecID=123, CaseNumber = "E4323",DocumentType  = "License Disc",RegistrationNumber="Registration Number", Make = "Make",Model = "Model",SerialNumber = "Serial Number"},
            //      new Document{VehicleInsRecID=234, CaseNumber = "E4323",DocumentType  = "License Disc",RegistrationNumber="Registration Number", Make = "Make",Model = "Model",SerialNumber = "Serial Number"},
            //      new Document{VehicleInsRecID=345, CaseNumber = "E4323",DocumentType  = "License Disc",RegistrationNumber="Registration Number", Make = "Make",Model = "Model",SerialNumber = "Serial Number"},
            //      new Document{VehicleInsRecID=456, CaseNumber = "E4323",DocumentType  = "License Disc",RegistrationNumber="Registration Number", Make = "Make",Model = "Model",SerialNumber = "Serial Number"},
            //      new Document{VehicleInsRecID=789, CaseNumber = "E4323",DocumentType  = "License Disc",RegistrationNumber="Registration Number", Make = "Make",Model = "Model",SerialNumber = "Serial Number"},
            //      new Document{VehicleInsRecID=985, CaseNumber = "E4323",DocumentType  = "License Disc",RegistrationNumber="Registration Number", Make = "Make",Model = "Model",SerialNumber = "Serial Number"},
            //      new Document{VehicleInsRecID=741, CaseNumber = "E4323",DocumentType  = "License Disc",RegistrationNumber="Registration Number", Make = "Make",Model = "Model",SerialNumber = "Serial Number"},
            //      new Document{VehicleInsRecID=852, CaseNumber = "E4323",DocumentType  = "License Disc",RegistrationNumber="Registration Number", Make = "Make",Model = "Model",SerialNumber = "Serial Number"},
            //      new Document{VehicleInsRecID=145, CaseNumber = "E4323",DocumentType  = "License Disc",RegistrationNumber="Registration Number", Make = "Make",Model = "Model",SerialNumber = "Serial Number"},
            //      new Document{VehicleInsRecID=963, CaseNumber = "E4323",DocumentType  = "License Disc",RegistrationNumber="Registration Number", Make = "Make",Model = "Model",SerialNumber = "Serial Number"},
            //  };
            //  await SqliteHelper.Storage.InsertAllAsync<Document>(d);

            //await SqliteHelper.Storage.DropTableAsync<DrivingDuration>();

            //await SqliteHelper.Storage.CreateTableAsync<DrivingDuration>();
            //await SqliteHelper.Storage.CreateTableAsync<DestinationContacts>();
            //await SqliteHelper.Storage.CreateTableAsync<Document>();
            //await SqliteHelper.Storage.CreateTableAsync<CollectDeliveryTask>();
            //await SqliteHelper.Storage.CreateTableAsync<CDCustomerDetails>();
            //await SqliteHelper.Storage.CreateTableAsync<CDProof>();

        }

        private async System.Threading.Tasks.Task GetTasksFromDbAsync()
        {
            var list = (await SqliteHelper.Storage.LoadTableAsync<Eqstra.BusinessLogic.CollectDeliveryTask>()).Where(w => w.Status != Eqstra.BusinessLogic.Enums.CDTaskStatus.Complete);
            foreach (var item in list)
            {
                this.PoolofTasks.Add(item);
            }
        }


        private void GetAllCount()
        {
            string _cdtaskStatus = CDTaskStatus.AwaitingDelivery;
            if (this.UserInfo.CDUserType == CDUserType.Driver)
            {
                _cdtaskStatus = CDTaskStatus.AwaitingDriverCollection;
            }
            if (this.UserInfo.CDUserType == CDUserType.Customer)
            {
                _cdtaskStatus = CDTaskStatus.AwaitingCustomerCollection;
            }
            if (this.UserInfo.CDUserType == CDUserType.Courier)
            {
                _cdtaskStatus = CDTaskStatus.AwaitingCourierCollection;
            }

            this.AwaitingConfirmationCount = this.PoolofTasks.Count(x => x.Status == BusinessLogic.Enums.CDTaskStatus.AwaitingConfirmation || x.Status == _cdtaskStatus || x.Status == BusinessLogic.Enums.CDTaskStatus.AwaitingDelivery);
            this.MyTaskCount = this.PoolofTasks.Count(x => (x.CDTaskStatus != BusinessLogic.Enums.CDTaskStatus.Complete && x.CDTaskStatus != BusinessLogic.Enums.CDTaskStatus.AwaitingConfirmation));
            this.TotalCount = this.PoolofTasks.Count(x => x.DeliveryDate.Date.Equals(DateTime.Today));
        }
        private void GetAppointments()
        {

            foreach (var item in this.PoolofTasks.Where(x => x.Status.Equals(BusinessLogic.Enums.CDTaskStatus.AwaitingDelivery) || x.Status.Equals(BusinessLogic.Enums.CDTaskStatus.AwaitingDriverCollection)))
            {
                var startTime = new DateTime(item.DeliveryDate.Year, item.DeliveryDate.Month, item.DeliveryDate.Day, item.DeliveryDate.Hour, item.DeliveryDate.Minute,
                           item.DeliveryDate.Second);
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
    }
}
