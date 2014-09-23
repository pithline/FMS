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
            this.CDTaskList = new ObservableCollection<BusinessLogic.CollectDeliveryTask>();
            this.SelectedTaskList = new ObservableCollection<BusinessLogic.CollectDeliveryTask>();
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
                foreach (var item in this.SelectedTaskList)
                {
                    if (item.TaskType == CDTaskType.Delivery)
                    {
                        item.Status = CDTaskStatus.AwaitingDelivery;
                    }
                    else
                    {
                        if (this.CDUserInfo.CDUserType == CDUserType.Driver)
                        {
                            item.Status = CDTaskStatus.AwaitingDriverCollection;
                        }
                        if (this.CDUserInfo.CDUserType == CDUserType.Customer)
                        {
                            item.Status = CDTaskStatus.AwaitingCustomerCollection;
                        }
                        if (this.CDUserInfo.CDUserType == CDUserType.Courier)
                        {
                            item.Status = CDTaskStatus.AwaitingCourierCollection;
                        }
                    }
                    await SqliteHelper.Storage.UpdateSingleRecordAsync<Eqstra.BusinessLogic.CollectDeliveryTask>(item);

                    //await DDServiceProxyHelper.Instance.UpdateTaskStatusAsync();

                    this._navigationService.Navigate("Main", string.Empty);
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
                    PersistentData.Instance.CollectDeliveryTask = this.CDTask;
                    ApplicationData.Current.LocalSettings.Values["VehicleInsRecID"] = this.CDTask.VehicleInsRecId;
                    if (this.CDTask.TaskType == CDTaskType.Collect)
                    {
                        this._navigationService.Navigate("CollectionOrDeliveryDetails", string.Empty);
                    }
                    else
                    {
                        _navigationService.Navigate("DrivingDirection", string.Empty);
                    }
                }
                catch (Exception ex)
                {
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
                
            });
        }
        #region Overrides
        async public override void OnNavigatedTo(object navigationParameter, Windows.UI.Xaml.Navigation.NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            try
            {
                base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
                _eventAggregator.GetEvent<TasksFetchedEvent>().Subscribe(async o =>
                {

                    await ShowTasksAsync(navigationParameter);

                }, ThreadOption.UIThread);
                await ShowTasksAsync(navigationParameter);
            }
            catch (Exception ex)
            {

                AppSettings.Instance.ErrorMessage = ex.Message;
            }
        }

        private async System.Threading.Tasks.Task ShowTasksAsync(object navigationParameter)
        {
            var list = EnumerateTasks(navigationParameter, await SqliteHelper.Storage.LoadTableAsync<Eqstra.BusinessLogic.CollectDeliveryTask>()).Where(w => w.Status != Eqstra.BusinessLogic.Enums.CDTaskStatus.Complete);
            this.CDTaskList.Clear();
            foreach (Eqstra.BusinessLogic.CollectDeliveryTask item in list.OrderBy(o => o.CustomerName).ThenBy(o => o.Address))
            {
                this.CDTaskList.Add(item);
                GetAppointments(item);
            }
            if (this.CDTaskList.Any())
                this.CDTask = this.CDTaskList.FirstOrDefault();

            await GetDocumentsFromDbByCaseNumber();

        }

        private IEnumerable<Eqstra.BusinessLogic.CollectDeliveryTask> EnumerateTasks(object navigationParameter, IEnumerable<BusinessLogic.CollectDeliveryTask> tasks)
        {
            try
            {
                PersistentData.Instance.CustomerDetails = this.CustomerDetails;
                IEnumerable<Eqstra.BusinessLogic.CollectDeliveryTask> list = null;
                string _cdtaskStatus = string.Empty;
                if (this.CDUserInfo.CDUserType == CDUserType.Driver)
                {
                    _cdtaskStatus = CDTaskStatus.AwaitingDriverCollection;
                }
                if (this.CDUserInfo.CDUserType == CDUserType.Customer)
                {
                    _cdtaskStatus = CDTaskStatus.AwaitingCustomerCollection;
                }
                if (this.CDUserInfo.CDUserType == CDUserType.Courier)
                {
                    _cdtaskStatus = CDTaskStatus.AwaitingCourierCollection;
                }
                if (navigationParameter.Equals("TasksAwaitingConfirmation"))
                {
                    this.DetailTitle = "Awaiting Tasks";
                    this.SaveVisibility = Visibility.Visible;
                    this.NextStepVisibility = Visibility.Collapsed;
                    list = (tasks).Where(x => x.Status == CDTaskStatus.AwaitingDelivery || x.Status == CDTaskStatus.AwaitingConfirmation || x.Status == _cdtaskStatus);
                }
                if (navigationParameter.Equals("Total"))
                {
                    this.DetailTitle = "Todays's Tasks";
                    this.SaveVisibility = Visibility.Collapsed;
                    this.NextStepVisibility = Visibility.Visible;
                    list = (tasks).Where(x => x.DeliveryDate.Date.Date.Equals(DateTime.Today));
                }
                if (navigationParameter.Equals("InProgress"))
                {
                    this.DetailTitle = "My Tasks";
                    this.SaveVisibility = Visibility.Collapsed;
                    this.NextStepVisibility = Visibility.Visible;
                    list = (tasks).Where(x => (x.Status != CDTaskStatus.AwaitingConfirmation && x.CDTaskStatus != CDTaskStatus.Complete));
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

        private ObservableCollection<Eqstra.BusinessLogic.CollectDeliveryTask> cdTaskList;
        public ObservableCollection<Eqstra.BusinessLogic.CollectDeliveryTask> CDTaskList
        {
            get { return cdTaskList; }
            set { SetProperty(ref cdTaskList, value); }
        }
        private ObservableCollection<Eqstra.BusinessLogic.CollectDeliveryTask> selectedTaskList;
        public ObservableCollection<Eqstra.BusinessLogic.CollectDeliveryTask> SelectedTaskList
        {
            get { return selectedTaskList; }
            set { SetProperty(ref selectedTaskList, value); }
        }
        private Eqstra.BusinessLogic.CollectDeliveryTask cdTask;
        public Eqstra.BusinessLogic.CollectDeliveryTask CDTask
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

        public async System.Threading.Tasks.Task GetDocumentsFromDbByCaseNumber()
        {
            var docs = await SqliteHelper.Storage.LoadTableAsync<Eqstra.BusinessLogic.Document>();

            this.BriefDetailsUserControlViewModel.DocumentsBriefs = new ObservableCollection<Document>();
            if (this.CDTask != null)
            {
                foreach (var d in docs.Where(w => w.CaseNumber == this.CDTask.CaseNumber))
                {
                    this.BriefDetailsUserControlViewModel.DocumentsBriefs.Add(new Document { CaseNumber = d.CaseNumber, DocumentType = d.DocumentType });


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
                    this.CustomerDetails.VehicleInsRecId = this.CDTask.VehicleInsRecId;
                    this.CustomerDetails.Address = this.CDTask.Address.Replace(",", Environment.NewLine);
                    this.CustomerDetails.CustomerName = this.CDTask.CustomerName;
                    this.CustomerDetails.EmailId = this.CDTask.EmailId;
                    this.CustomerDetails.DeliveryDate = this.CDTask.DeliveryDate;
                    this.CustomerDetails.DeliveryTime = this.CDTask.DeliveryTime;
                    this.CustomerDetails.RegistrationNumber = this.CDTask.RegistrationNumber;
                    this.CustomerDetails.MakeModel = this.CDTask.Make + Environment.NewLine + this.CDTask.Model;
                    this.CustomerDetails.CaseType = this.CDTask.CaseType;
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
