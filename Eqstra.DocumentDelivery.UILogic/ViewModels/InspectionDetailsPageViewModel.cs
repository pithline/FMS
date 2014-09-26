using Eqstra.BusinessLogic;
using Eqstra.BusinessLogic.DeliveryModel;
using Eqstra.BusinessLogic.DocumentDelivery;
using Eqstra.BusinessLogic.Enums;
using Eqstra.BusinessLogic.Helpers;
using Eqstra.DocumentDelivery.UILogic.AifServices;
using Eqstra.DocumentDelivery.UILogic.Helpers;
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
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Eqstra.DocumentDelivery.UILogic.ViewModels
{
    public class InspectionDetailsPageViewModel : BaseViewModel
    {
        private INavigationService _navigationService;
        IEventAggregator _eventAggregator;
        public InspectionDetailsPageViewModel(INavigationService navigationService, IEventAggregator eventAggregator)
            : base(navigationService)
        {
            this.CDTaskList = new ObservableCollection<CollectDeliveryTask>();
            this.SelectedTaskList = new ObservableCollection<CollectDeliveryTask>();
            this.BriefDetailsUserControlViewModel = new BriefDetailsUserControlViewModel();
            this.CDUserInfo = PersistentData.Instance.UserInfo;
            this.SaveVisibility = Visibility.Collapsed;
            this.NextStepVisibility = Visibility.Collapsed;
            this.CustomerDetails = new CDCustomerDetails();
            this.CustomerDetails.Appointments = new ScheduleAppointmentCollection();
            this.CDTask = null;
            _navigationService = navigationService;
            _eventAggregator = eventAggregator;
            this.SaveTaskCommand = new DelegateCommand(async () =>
            {
                try
                {
                    this.isBusy = true;
                    foreach (var item in this.SelectedTaskList)
                    {
                        item.IsAssignTask = true;
                        await SqliteHelper.Storage.UpdateSingleRecordAsync<CollectDeliveryTask>(item);

                        //await DDServiceProxyHelper.Instance.UpdateTaskStatusAsync();

                        this._navigationService.Navigate("Main", string.Empty);
                    }
                    this.IsBusy = false;
                }
                catch (Exception ex)
                {
                    this.IsBusy = false;
                    AppSettings.Instance.ErrorMessage = ex.Message;
                }
            },
            () =>
            {
                return (this.CDTask != null);
            });
            this.NextStepCommand = new DelegateCommand(() =>
            {
                try
                {
                    this.IsBusy = true;
                    PersistentData.Instance.CollectDeliveryTask = this.CDTask;
                    ApplicationData.Current.LocalSettings.Values["CaseNumber"] = this.CDTask.CaseNumber;
                    if (this.CDTask.TaskType == CDTaskType.Collect)
                    {
                        this._navigationService.Navigate("CollectionOrDeliveryDetails", string.Empty);
                    }
                    else
                    {
                        _navigationService.Navigate("DrivingDirection", string.Empty);
                    }
                    this.IsBusy = false;
                }
                catch (Exception ex)
                {
                    this.IsBusy = false;
                    AppSettings.Instance.ErrorMessage = ex.Message;
                }
            },
            () =>
            {
                return (this.CDTask != null);
            }
            );

            this.TasksChangedCommand = new DelegateCommand<ObservableCollection<object>>((param) =>
            {
                try
                {
                    this.SelectedTaskList.Clear();
                    foreach (var item in param)
                    {
                        this.SelectedTaskList.Add((CollectDeliveryTask)item);
                    }

                    this.SaveTaskCommand.RaiseCanExecuteChanged();
                    this.NextStepCommand.RaiseCanExecuteChanged();
                    this.GetCustomerDetailsAsync();
                    PersistentData.Instance.CustomerDetails = this.CustomerDetails;
                    this.BriefDetailsUserControlViewModel.CustomerDetails = this.CustomerDetails;
                    _eventAggregator.GetEvent<CustomerDetailsEvent>().Publish(this.CustomerDetails);
                }
                catch (Exception ex)
                {
                    AppSettings.Instance.ErrorMessage = ex.Message;
                }

            });
        }
        #region Overrides
        async public override void OnNavigatedTo(object navigationParameter, Windows.UI.Xaml.Navigation.NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            try
            {
                this.IsBusy = true;
                base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
                _eventAggregator.GetEvent<TasksFetchedEvent>().Subscribe(async o =>
                {
                    await ShowTasksAsync(navigationParameter);
                }, ThreadOption.UIThread);
                await ShowTasksAsync(navigationParameter);
                this.IsBusy = false;
            }
            catch (Exception ex)
            {
                this.IsBusy = false;
                AppSettings.Instance.ErrorMessage = ex.Message;
            }
        }

        private async System.Threading.Tasks.Task ShowTasksAsync(object navigationParameter)
        {
            try
            {
                this.IsBusy = true;
                var list = EnumerateTasks(navigationParameter, await SqliteHelper.Storage.LoadTableAsync<CollectDeliveryTask>()).GroupBy(g => g.CustomerId).Select(f => f.First()).Where(w => w.Status != CDTaskStatus.Completed);
                this.CDTaskList.Clear();
                foreach (CollectDeliveryTask item in list)
                {
                    if (item != null)
                    {
                        this.CDTaskList.Add(item);
                        GetAppointments(item);
                    }
                }
                if (this.CDTaskList.Any())
                    this.CDTask = this.CDTaskList.FirstOrDefault();

                await GetAllDocumentFromDbByCusomer();
                this.IsBusy = false;
            }
            catch (Exception ex)
            {
                AppSettings.Instance.ErrorMessage = ex.Message;
                this.IsBusy = false;
            }

        }

        private IEnumerable<CollectDeliveryTask> EnumerateTasks(object navigationParameter, IEnumerable<CollectDeliveryTask> tasks)
        {
            try
            {
                PersistentData.Instance.CustomerDetails = this.CustomerDetails;
                IEnumerable<CollectDeliveryTask> list = null;
                if (navigationParameter.Equals("TasksAwaitingConfirmation"))
                {
                    this.DetailTitle = "Awaiting Tasks";
                    this.SaveVisibility = Visibility.Visible;
                    this.NextStepVisibility = Visibility.Collapsed;
                    list = (tasks).Where(x => !x.IsAssignTask).Where(x => x.Status == CDTaskStatus.AwaitCollectionDetail || x.Status == CDTaskStatus.AwaitCourierCollection || x.Status == CDTaskStatus.AwaitDriverCollection);
                }
                if (navigationParameter.Equals("Total"))
                {
                    this.DetailTitle = "Todays's Tasks";
                    this.SaveVisibility = Visibility.Collapsed;
                    this.NextStepVisibility = Visibility.Visible;
                    list = (tasks).Where(x => x.IsAssignTask && x.DeliveryDate.Date.Equals(DateTime.Today));
                }
                if (navigationParameter.Equals("MyTasks"))
                {
                    this.DetailTitle = "My Tasks";
                    this.SaveVisibility = Visibility.Collapsed;
                    this.NextStepVisibility = Visibility.Visible;
                    list = (tasks).Where(x => x.IsAssignTask && x.Status != CDTaskStatus.Completed);
                }

                return list;
            }
            catch (Exception ex)
            {
                AppSettings.Instance.ErrorMessage = ex.Message;
                return null;
            }
        }

        #endregion

        #region Properties

        private Visibility saveVisibility;
        public Visibility SaveVisibility
        {
            get { return saveVisibility; }
            set { SetProperty(ref saveVisibility, value); }
        }

        private Visibility nextStepVisibility;
        public Visibility NextStepVisibility
        {
            get { return nextStepVisibility; }
            set { SetProperty(ref nextStepVisibility, value); }
        }

        private ObservableCollection<CollectDeliveryTask> cdTaskList;
        public ObservableCollection<CollectDeliveryTask> CDTaskList
        {
            get { return cdTaskList; }
            set { SetProperty(ref cdTaskList, value); }
        }
        private ObservableCollection<CollectDeliveryTask> selectedTaskList;
        public ObservableCollection<CollectDeliveryTask> SelectedTaskList
        {
            get { return selectedTaskList; }
            set { SetProperty(ref selectedTaskList, value); }
        }
        private CollectDeliveryTask cdTask;
        public CollectDeliveryTask CDTask
        {
            get { return cdTask; }
            set
            {
                if (SetProperty(ref cdTask, value))
                    this.NextStepCommand.RaiseCanExecuteChanged();
            }
        }

        private string detailTitle;
        public string DetailTitle
        {
            get { return detailTitle; }
            set { SetProperty(ref detailTitle, value); }
        }

        private CDUserInfo cdUserInfo;
        public CDUserInfo CDUserInfo
        {
            get { return cdUserInfo; }
            set { SetProperty(ref cdUserInfo, value); }
        }

        private Customer customer;

        public Customer Customer
        {
            get { return customer; }
            set { SetProperty(ref customer, value); }
        }

        private bool isBusy;

        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        private CDCustomerDetails customerDetails;
        public CDCustomerDetails CustomerDetails
        {
            get { return customerDetails; }
            set { SetProperty(ref customerDetails, value); }
        }

        private BriefDetailsUserControlViewModel briefDetailsUserControlViewModel;

        public BriefDetailsUserControlViewModel BriefDetailsUserControlViewModel
        {
            get { return briefDetailsUserControlViewModel; }
            set { SetProperty(ref briefDetailsUserControlViewModel, value); }
        }

        public DelegateCommand SaveTaskCommand { get; set; }
        public DelegateCommand NextStepCommand { get; set; }
        public DelegateCommand<ObservableCollection<Object>> TasksChangedCommand { get; set; }


        #endregion

        #region Methods

        public async System.Threading.Tasks.Task GetAllDocumentFromDbByCusomer()
        {
            if (this.CDTask != null)
            {
                var docList = (await SqliteHelper.Storage.LoadTableAsync<Document>()).Where(d => !d.IsDelivered); ;
                var allTaskOfCustomer = (await SqliteHelper.Storage.LoadTableAsync<CollectDeliveryTask>()).Where(d => d.CustomerId == this.CDTask.CustomerId).ToList();
                this.BriefDetailsUserControlViewModel.DocumentsBriefs = new ObservableCollection<Document>();

                foreach (var d in docList)
                {
                    if (allTaskOfCustomer.Any(t => t.CaseCategoryRecID == d.CaseCategoryRecID))
                        this.BriefDetailsUserControlViewModel.DocumentsBriefs.Add(new Document { SerialNumber = d.SerialNumber, DocumentType = d.DocumentType });
                }
                GetCustomerDetailsAsync();
                this.BriefDetailsUserControlViewModel.CustomerDetails = this.CustomerDetails;
            }

        }
        private void GetAppointments(CollectDeliveryTask task)
        {

            var startTime = new DateTime(task.ConfirmedTime.Year, task.ConfirmedTime.Month, task.ConfirmedTime.Day, task.ConfirmedTime.Hour, task.ConfirmedTime.Minute,
                       task.ConfirmedTime.Second);
            startTime = DateTime.Now;//for testing only
            this.CustomerDetails.Appointments.Add(
            new ScheduleAppointment()
            {
                Subject = task.CaseNumber + Environment.NewLine + task.CustomerName + Environment.NewLine + task.Address,
                Location = task.Address,
                StartTime = startTime,
                EndTime = startTime.AddHours(1),
                ReadOnly = true,
                AppointmentBackground = new SolidColorBrush(Colors.Crimson),
                Status = new ScheduleAppointmentStatus { Status = task.Status, Brush = new SolidColorBrush(Colors.Chocolate) }

            });
        }
        public void GetCustomerDetailsAsync()
        {
            try
            {
                if (this.CDTask != null)
                {
                    this.CustomerDetails.CustomerNumber = this.CDTask.CustomerNumber;
                    this.CustomerDetails.CaseNumber = this.CDTask.CaseNumber;
                    this.CustomerDetails.CaseCategoryRecID = this.CDTask.CaseCategoryRecID;
                    this.CustomerDetails.Address = this.CDTask.Address.Replace(",", Environment.NewLine);
                    this.CustomerDetails.CustomerName = this.CDTask.CustomerName;
                    this.CustomerDetails.EmailId = this.CDTask.EmailId;
                    this.CustomerDetails.DeliveryDate = this.CDTask.DeliveryDate;
                    this.CustomerDetails.DeliveryTime = this.CDTask.DeliveryTime;
                    this.CustomerDetails.RegistrationNumber = this.CDTask.RegistrationNumber;
                    this.CustomerDetails.MakeModel = this.CDTask.Make + Environment.NewLine + this.CDTask.Model;
                    this.CustomerDetails.CaseType = this.CDTask.CaseType;
                    this.CustomerDetails.ContactName = this.CDTask.ContactName;

                    this.CustomerDetails.Appointments = PersistentData.Instance.Appointments;
                }
            }
            catch (Exception)
            {
                throw;
            }

        }
        #endregion
    }
}
