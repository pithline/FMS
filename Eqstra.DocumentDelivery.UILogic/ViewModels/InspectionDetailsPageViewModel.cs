using Eqstra.BusinessLogic;
using Eqstra.BusinessLogic.Enums;
using Eqstra.BusinessLogic.Helpers;
using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using Syncfusion.UI.Xaml.Schedule;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Eqstra.DocumentDelivery.UILogic.ViewModels
{
    public class InspectionDetailsPageViewModel : ViewModel
    {
        INavigationService _navigationService;
        public InspectionDetailsPageViewModel(INavigationService navigationService)
        {
            this.InspectionList = new ObservableCollection<BusinessLogic.CollectDeliveryTask>();
            this.SaveVisibility = Visibility.Collapsed;
            this.NextStepVisibility = Visibility.Collapsed;
            this.CustomerDetails = new CustomerDetails();
            this.Inspection = null;
            _navigationService = navigationService;
            DrivingDirectionCommand = DelegateCommand.FromAsyncHandler(() =>
            {
                if (this.Inspection == null)
                {
                    return System.Threading.Tasks.Task.FromResult<object>(null);
                }
                _navigationService.Navigate("DrivingDirection", Inspection);
                return System.Threading.Tasks.Task.FromResult<object>(null);
            }, 
            () =>
                { return (this.Inspection != null && this.Inspection.Status != BusinessLogic.Enums.TaskStatusEnum.AwaitingInspection && this.Inspection.Status != BusinessLogic.Enums.TaskStatusEnum.Completed); }
            );

            this.SaveTaskCommand = new DelegateCommand(async () =>
            {
                foreach (var item in this.SelectedTaskList)
                {
                    //this.InspectionList.Remove(item);
                    if (item.TaskType == CDTaskTypeEnum.Delivery)
                    {
                        item.CDTaskStatus = CDTaskStatusEnum.AwaitingDelivery; 
                    }
                    else
                    {
                        item.CDTaskStatus = CDTaskStatusEnum.AwaitingDriverCollection; 
                    }
                    await SqliteHelper.Storage.UpdateSingleRecordAsync<Eqstra.BusinessLogic.CollectDeliveryTask>(item);
                    this._navigationService.Navigate("Main",null);
                }
            }, 
            () =>
            {
                return this.SelectedTaskList.Count > 0;
            });
            this.NextStepCommand = new DelegateCommand(() =>
            {

                if (this.Inspection.TaskType == CDTaskTypeEnum.Collection)
                {
                    this._navigationService.Navigate("CollectionOrDeliveryDetails", this.Inspection); 
                }
                else
                {
                    _navigationService.Navigate("DrivingDirection", Inspection);
                }
            },
            () =>
            {
                return (this.Inspection != null);
            }
            );
            this.CustomerDetails.Appointments = new ScheduleAppointmentCollection
            {
                new ScheduleAppointment(){
                    Subject = "Inspection at Peter Johnson",
                    Notes = "some noise from engine",
                    Location = "Cape Town",
                    StartTime = DateTime.Now,
                    EndTime = DateTime.Now.AddHours(2),
                    ReadOnly = true,
                   AppointmentBackground = new SolidColorBrush(Colors.Crimson),                   
                    Status = new ScheduleAppointmentStatus{Status = "Tentative",Brush = new SolidColorBrush(Colors.Chocolate)}

                },
                new ScheduleAppointment(){
                    Subject = "Inspection at Peter Johnson",
                    Notes = "some noise from differential",
                    Location = "Cape Town",
                     ReadOnly = true,
                    StartTime =new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,8,00,00),
                    EndTime = new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,9,00,00),
                    Status = new ScheduleAppointmentStatus{Brush = new SolidColorBrush(Colors.Green), Status  = "Free"},
                },                    
            };
        }
        #region Overrides
        async public override void OnNavigatedTo(object navigationParameter, Windows.UI.Xaml.Navigation.NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
            this.SelectedTaskList = new ObservableCollection<BusinessLogic.CollectDeliveryTask>();
            IEnumerable<Eqstra.BusinessLogic.CollectDeliveryTask> list = null;
            if (navigationParameter.Equals("AwaitingInspections"))
            {
                this.SaveVisibility = Visibility.Visible;
                this.NextStepVisibility = Visibility.Collapsed;
                list = (await SqliteHelper.Storage.LoadTableAsync<Eqstra.BusinessLogic.CollectDeliveryTask>()).Where(x => x.CDTaskStatus == CDTaskStatusEnum.AwaitingConfirmation);
            }
            else if (navigationParameter.Equals("Total"))
            {
                this.SaveVisibility = Visibility.Collapsed;
                this.NextStepVisibility = Visibility.Collapsed;
                list = (await SqliteHelper.Storage.LoadTableAsync<Eqstra.BusinessLogic.CollectDeliveryTask>()).Where(x => x.ConfirmedDate.Date.Date.Equals(DateTime.Today));
            }
            else if (navigationParameter.Equals("InProgress"))
            {
                this.SaveVisibility = Visibility.Collapsed;
                this.NextStepVisibility = Visibility.Visible;
                list = (await SqliteHelper.Storage.LoadTableAsync<Eqstra.BusinessLogic.CollectDeliveryTask>()).Where(x => (x.CDTaskStatus != CDTaskStatusEnum.AwaitingConfirmation && x.CDTaskStatus != CDTaskStatusEnum.Complete));
            }
            foreach (Eqstra.BusinessLogic.CollectDeliveryTask item in list)
            {
                var cust = await SqliteHelper.Storage.GetSingleRecordAsync<Customer>(x => x.Id == item.CustomerId);
                item.CustomerName = cust.CustomerName;
                item.Address = cust.Address;
                this.InspectionList.Add(item);
            }
        }

        public override void OnNavigatedFrom(Dictionary<string, object> viewModelState, bool suspending)
        {
            base.OnNavigatedFrom(viewModelState, suspending);
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


        private ObservableCollection<Eqstra.BusinessLogic.CollectDeliveryTask> inspectionList;

        public ObservableCollection<Eqstra.BusinessLogic.CollectDeliveryTask> InspectionList
        {
            get { return inspectionList; }
            set { SetProperty(ref inspectionList, value); }
        }

        private ObservableCollection<Eqstra.BusinessLogic.CollectDeliveryTask> selectedTaskList;

        public ObservableCollection<Eqstra.BusinessLogic.CollectDeliveryTask> SelectedTaskList
        {
            get { return selectedTaskList; }
            set { SetProperty(ref selectedTaskList, value); }
        }


        private Eqstra.BusinessLogic.CollectDeliveryTask inspection;

        public Eqstra.BusinessLogic.CollectDeliveryTask Inspection
        {
            get { return inspection; }
            set
            {
                if (SetProperty(ref inspection, value))
                    DrivingDirectionCommand.RaiseCanExecuteChanged();
            }
        }


        private Customer customer;

        public Customer Customer
        {
            get { return customer; }
            set { SetProperty(ref customer, value); }
        }

        private CustomerDetails customerDetails;

        public CustomerDetails CustomerDetails
        {
            get { return customerDetails; }
            set { SetProperty(ref customerDetails, value); }
        }


        public DelegateCommand DrivingDirectionCommand { get; set; }
        public DelegateCommand SaveTaskCommand { get; set; }
        public DelegateCommand NextStepCommand { get; set; }
        #endregion

        #region Methods
        async public System.Threading.Tasks.Task GetCustomerDetailsAsync()
        {
            try
            {
                if (this.inspection != null)
                {
                    this.customer = await SqliteHelper.Storage.GetSingleRecordAsync<Customer>(c => c.Id == this.inspection.CustomerId);
                    this.CustomerDetails.ContactNumber = this.customer.ContactNumber;
                    this.customerDetails.CaseNumber = this.inspection.CaseNumber;
                    this.customerDetails.Status = this.inspection.Status;
                    this.customerDetails.StatusDueDate = this.inspection.StatusDueDate;
                    this.customerDetails.Address = this.customer.Address;
                    this.customerDetails.AllocatedTo = this.inspection.AllocatedTo;
                    this.customerDetails.CustomerName = this.customer.CustomerName;
                    this.customerDetails.CaseType = this.inspection.CaseType;
                    this.customerDetails.EmailId = this.customer.EmailId;
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
