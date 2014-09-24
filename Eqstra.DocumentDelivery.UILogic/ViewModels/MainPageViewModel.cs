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
            this.PoolofTasks = new ObservableCollection<CollectDeliveryTask>();
            this.Appointments = new ScheduleAppointmentCollection();
            _eventAggregator = eventAggregator;
            CreateTableAsync();
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
                            GetAppointments();
                            AppSettings.Instance.IsSynchronizing = 0;
                            AppSettings.Instance.Synced = true;
                        }
                         );

                        PersistentData.Instance.Appointments = this.Appointments;
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

                PersistentData.Instance.Appointments = this.Appointments;
            }
            catch (Exception ex)
            {
                AppSettings.Instance.ErrorMessage = ex.Message;
            }
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

        private int myTaskCount;
        public int MyTaskCount
        {
            get { return myTaskCount; }
            set { SetProperty(ref myTaskCount, value); }
        }
        public DelegateCommand SyncCommand { get; set; }

        private CollectDeliveryTask task;
        public CollectDeliveryTask _cdTask
        {
            get { return task; }
            set
            {
                SetProperty(ref task, value);
            }
        }
        private ObservableCollection<CollectDeliveryTask> poolofTasks;
        public ObservableCollection<CollectDeliveryTask> PoolofTasks
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

            //await SqliteHelper.Storage.DropTableAsync<Document>();
            //await SqliteHelper.Storage.CreateTableAsync<Document>();
            //var d = new ObservableCollection<Document>
            //  {
            //      new Document{CaseCategoryRecID=123, CaseNumber = "Case000454",DocumentType  = "License Disc",RegistrationNumber="Registration Number", Make = "Make",Model = "Model",SerialNumber = "Serial Number"},
            //      new Document{CaseCategoryRecID=234, CaseNumber = "Case000454",DocumentType  = "License Disc",RegistrationNumber="Registration Number", Make = "Make",Model = "Model",SerialNumber = "Serial Number"},
            //      new Document{CaseCategoryRecID=345, CaseNumber = "Case000454",DocumentType  = "License Disc",RegistrationNumber="Registration Number", Make = "Make",Model = "Model",SerialNumber = "Serial Number"},
            //      new Document{CaseCategoryRecID=456, CaseNumber = "Case000454",DocumentType  = "License Disc",RegistrationNumber="Registration Number", Make = "Make",Model = "Model",SerialNumber = "Serial Number"},
            //      new Document{CaseCategoryRecID=789, CaseNumber = "Case000454",DocumentType  = "License Disc",RegistrationNumber="Registration Number", Make = "Make",Model = "Model",SerialNumber = "Serial Number"},
            //      new Document{CaseCategoryRecID=985, CaseNumber = "Case000454",DocumentType  = "License Disc",RegistrationNumber="Registration Number", Make = "Make",Model = "Model",SerialNumber = "Serial Number"},
            //      new Document{CaseCategoryRecID=741, CaseNumber = "Case000454",DocumentType  = "License Disc",RegistrationNumber="Registration Number", Make = "Make",Model = "Model",SerialNumber = "Serial Number"},
            //      new Document{CaseCategoryRecID=852, CaseNumber = "Case000454",DocumentType  = "License Disc",RegistrationNumber="Registration Number", Make = "Make",Model = "Model",SerialNumber = "Serial Number"},
            //      new Document{CaseCategoryRecID=145, CaseNumber = "Case000454",DocumentType  = "License Disc",RegistrationNumber="Registration Number", Make = "Make",Model = "Model",SerialNumber = "Serial Number"},
            //      new Document{CaseCategoryRecID=963, CaseNumber = "Case000454",DocumentType  = "License Disc",RegistrationNumber="Registration Number", Make = "Make",Model = "Model",SerialNumber = "Serial Number"},
            //  };
           // await SqliteHelper.Storage.InsertAllAsync<Document>(d);


            //await SqliteHelper.Storage.DropTableAsync<CDDrivingDuration>();
            //await SqliteHelper.Storage.DropTableAsync<ContactPerson>();
            //await SqliteHelper.Storage.DropTableAsync<Document>();
            //await SqliteHelper.Storage.DropTableAsync<CollectDeliveryTask>();
            //await SqliteHelper.Storage.DropTableAsync<CDCustomerDetails>();
            //await SqliteHelper.Storage.DropTableAsync<DocumentDeliveryDetails>();


            //await SqliteHelper.Storage.CreateTableAsync<CDDrivingDuration>();
            //await SqliteHelper.Storage.CreateTableAsync<ContactPerson>();
            //await SqliteHelper.Storage.CreateTableAsync<Document>();
            //await SqliteHelper.Storage.CreateTableAsync<CollectDeliveryTask>();
            //await SqliteHelper.Storage.CreateTableAsync<CDCustomerDetails>();
            //await SqliteHelper.Storage.CreateTableAsync<DocumentDeliveryDetails>();

            

        }

        private async System.Threading.Tasks.Task GetTasksFromDbAsync()
        {
            var list = (await SqliteHelper.Storage.LoadTableAsync<CollectDeliveryTask>()).Where(w => w.Status != CDTaskStatus.Completed);
            foreach (var item in list)
            {
                if (item != null)
                {
                    this.PoolofTasks.Add(item);
                }
            }
        }


        private void GetAllCount()
        {
            this.AwaitingConfirmationCount = this.PoolofTasks.Count(x => x.Status == CDTaskStatus.AwaitCollectionDetail || x.Status == CDTaskStatus.AwaitCourierCollection || x.Status ==CDTaskStatus.AwaitDriverCollection);
            this.MyTaskCount = this.PoolofTasks.Count(x => x.IsAssignTask && x.Status != CDTaskStatus.Completed);
            this.TotalCount = this.PoolofTasks.Count(x => x.DeliveryDate.Date.Equals(DateTime.Today));
        }
        private void GetAppointments()
        {

            foreach (var item in this.PoolofTasks.Where(x => !x.Status.Equals(CDTaskStatus.Completed)))
            {
                var startTime = new DateTime(item.DeliveryDate.Year, item.DeliveryDate.Month, item.DeliveryDate.Day, item.DeliveryDate.Hour, item.DeliveryDate.Minute,
                           item.DeliveryDate.Second);
              
                this.Appointments.Add(

                              new ScheduleAppointment()
                              {
                                  Subject = item.CaseNumber + Environment.NewLine + item.CustomerName,
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
