using Eqstra.BusinessLogic;
using Eqstra.BusinessLogic.Base;
using Eqstra.BusinessLogic.Common;
using Eqstra.BusinessLogic.Helpers;
using Eqstra.BusinessLogic.TI;
using Eqstra.TechnicalInspection.UILogic;
using Eqstra.TechnicalInspection.UILogic.AifServices;
using Eqstra.TechnicalInspection.UILogic.Events;
using Eqstra.TechnicalInspection.Views;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using Newtonsoft.Json;
using Syncfusion.UI.Xaml.Schedule;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Eqstra.TechnicalInspection.ViewModels
{
    public class TechnicalInspectionPageViewModel : BaseViewModel
    {
        private Eqstra.BusinessLogic.Task _task;
        private INavigationService _navigationService;
        private IEventAggregator _eventAggregator;

        public TechnicalInspectionPageViewModel(INavigationService navigationService, IEventAggregator eventAggregator)
            : base(navigationService)
        {
            try
            {
                _navigationService = navigationService;
                this._eventAggregator = eventAggregator;
                this.InspectionUserControls = new ObservableCollection<UserControl>();
                this.CustomerDetails = new CustomerDetails();
                this.TechnicalInspList = new ObservableCollection<MaintenanceRepair>();
                this.Model = new TechnicalInsp();

                this.PrevViewStack = new Stack<UserControl>();


                this.CompleteCommand = new DelegateCommand(async () =>
                {
                    this.IsBusy = true;
                    this._task.ProcessStep = ProcessStep.AcceptInspection;
                    this._task.Status = Eqstra.BusinessLogic.Helpers.TaskStatus.AwaitDamageConfirmation;
                    await SqliteHelper.Storage.UpdateSingleRecordAsync(this._task);
                    var currentModel = ((BaseViewModel)this.NextViewStack.Peek().DataContext).Model;

                    this.SaveCurrentUIDataAsync(currentModel);
                    _navigationService.Navigate("Main", null);

                    // await TIServiceHelper.Instance.UpdateTaskStatusAsync();
                    this.IsBusy = false;
                }, () =>
                {
                    return (this.NextViewStack.Count == 1);
                });

                this._eventAggregator.GetEvent<SignChangedEvent>().Subscribe(p =>
                {
                    CompleteCommand.RaiseCanExecuteChanged();
                });

                this._eventAggregator.GetEvent<ErrorsRaisedEvent>().Subscribe((errors) =>
                {
                    Errors = errors;
                    OnPropertyChanged("Errors");
                    ShowValidationSummary = true;
                    OnPropertyChanged("ShowValidationSummary");
                }, ThreadOption.UIThread);

                this.PreviousCommand = new DelegateCommand(async () =>
                {
                    this.IsCommandBarOpen = false;
                    ShowValidationSummary = false;
                    var currentModel = ((BaseViewModel)this.NextViewStack.Peek().DataContext).Model as BaseModel;
                    if (currentModel.ValidateModel())
                    {
                        SetFrameContent();
                        this.SaveCurrentUIDataAsync(currentModel);
                        if (this.PrevViewStack.FirstOrDefault() != null)
                        {
                            BaseViewModel nextViewModel = this.PrevViewStack.FirstOrDefault().DataContext as BaseViewModel;
                            await nextViewModel.LoadModelFromDbAsync(this._task.VehicleInsRecId);
                        }
                    }
                    else
                    {
                        Errors = currentModel.Errors;
                        OnPropertyChanged("Errors");
                        ShowValidationSummary = true;
                    }


                }, () =>
                {
                    return this.PrevViewStack.Count > 0;
                });

            }
            catch (Exception)
            {
                throw;
            }

        }

        private void SetFrameContent()
        {
            var item = this.PrevViewStack.Pop();
            this.FrameContent = item;
            this.NextViewStack.Push(item);
            CompleteCommand.RaiseCanExecuteChanged();
            PreviousCommand.RaiseCanExecuteChanged();
        }

        private void LoadAppointments()
        {
            var startTime = new DateTime(this._task.ConfirmedDate.Year, this._task.ConfirmedDate.Month, this._task.ConfirmedDate.Day, this._task.ConfirmedTime.Hour, this._task.ConfirmedTime.Minute,
                                this._task.ConfirmedTime.Second);
            this.CustomerDetails.Appointments = new ScheduleAppointmentCollection
            {
                new ScheduleAppointment(){
                    Subject = this._task.CaseNumber,                    
                    Location =this._task.Address,
                    StartTime = startTime,
                    EndTime = startTime.AddHours(1),
                    ReadOnly = true,
                    AppointmentBackground = new SolidColorBrush(Colors.Crimson),                   
                    Status = new ScheduleAppointmentStatus{Status = this._task.Status,Brush = new SolidColorBrush(Colors.Chocolate)}

                },
                               
            };
        }


        private void loadDataFromDb(long CaseServiceRecId)
        {
            try
            {
                var maintenanceRepairdata = (SqliteHelper.Storage.LoadTableAsync<MaintenanceRepair>()).Result;

                if (maintenanceRepairdata != null && maintenanceRepairdata.Any())
                {
                    foreach (var item in maintenanceRepairdata)
                    {
                        this.TechnicalInspList.Add(item);
                    }
                }
                TechnicalInsp viBaseObject = (TechnicalInsp)this.Model;
                //viBaseObject.LoadSnapshotsFromDb();
                PropertyHistory.Instance.SetPropertyHistory(viBaseObject);
                viBaseObject.ShouldSave = false;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async override System.Threading.Tasks.Task LoadModelFromDbAsync(long CaseServiceRecId)
        {

            this.Model = await SqliteHelper.Storage.GetSingleRecordAsync<TechnicalInsp>(x => x.CaseServiceRecID == CaseServiceRecId);
            if (this.Model == null)
            {
                this.Model = new TechnicalInsp();
            }
            loadDataFromDb(CaseServiceRecId);
        }

        async public override void OnNavigatedTo(object navigationParameter, Windows.UI.Xaml.Navigation.NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            try
            {
                base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
                this.InspectionHistList = new ObservableCollection<InspectionHistory>{
                new InspectionHistory{InspectionResult=new List<string>{"Engine and brake oil replacement","Wheel alignment"},CustomerId="1",InspectedBy="Jon Tabor",InspectedOn = DateTime.Now},
                new InspectionHistory{InspectionResult=new List<string>{"Vehicle coolant replacement","Few dent repairs"},CustomerId="1",InspectedBy="Robert Green",InspectedOn = DateTime.Now},
                new InspectionHistory{InspectionResult=new List<string>{"Vehicle is in perfect condition"},CustomerId="1",InspectedBy="Christopher",InspectedOn = DateTime.Now},
            };
                _task = JsonConvert.DeserializeObject<Eqstra.BusinessLogic.Task>(navigationParameter.ToString());
                App.Task = _task;

                ApplicationData.Current.LocalSettings.Values["CaseNumber"] = _task.CaseNumber;
                LoadAppointments();
                await GetCustomerDetailsAsync();

                _eventAggregator.GetEvent<CustFetchedEvent>().Subscribe(async b =>
                {
                    await GetCustomerDetailsAsync();
                });

                await this.LoadModelFromDbAsync(this._task.CaseServiceRecID);
                this.CustomerDetails = PersistentData.Instance.CustomerDetails;

            }
            catch (Exception)
            {

                throw;
            }
        }

        private UserControl frameContent;

        public UserControl FrameContent
        {
            get { return frameContent; }
            set { SetProperty(ref frameContent, value); }
        }

        private DelegateCommand completeCommand;

        public DelegateCommand CompleteCommand
        {
            get { return completeCommand; }
            set { SetProperty(ref completeCommand, value); }
        }
        private bool isCommandBarOpen;
        [RestorableState]
        public bool IsCommandBarOpen
        {
            get { return isCommandBarOpen; }
            set { SetProperty(ref isCommandBarOpen, value); }
        }


        private ObservableCollection<MaintenanceRepair> technicalInspList;
        public ObservableCollection<MaintenanceRepair> TechnicalInspList
        {
            get { return technicalInspList; }
            set { SetProperty(ref technicalInspList, value); }
        }


        private DelegateCommand previousCommand;
        public DelegateCommand PreviousCommand
        {
            get { return previousCommand; }
            set { SetProperty(ref previousCommand, value); }
        }

        private Stack<UserControl> nextViewStack;
        public Stack<UserControl> NextViewStack
        {
            get { return nextViewStack; }
            set
            {
                SetProperty(ref nextViewStack, value);
                //NextCommand.RaiseCanExecuteChanged();
            }
        }

        private Stack<UserControl> prevViewStack;
        public Stack<UserControl> PrevViewStack
        {
            get { return prevViewStack; }
            set
            {
                SetProperty(ref prevViewStack, value);
                //PreviousCommand.RaiseCanExecuteChanged();
            }
        }
        private bool isBusy;

        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        private ObservableCollection<UserControl> inpectionUserControls;
        public ObservableCollection<UserControl> InspectionUserControls
        {
            get { return inpectionUserControls; }
            set { SetProperty(ref inpectionUserControls, value); }
        }

        private ObservableCollection<InspectionHistory> inspectionHistList;
        public ObservableCollection<InspectionHistory> InspectionHistList
        {
            get { return inspectionHistList; }
            set { SetProperty(ref inspectionHistList, value); }
        }

        private CustomerDetails customerDetails;

        public CustomerDetails CustomerDetails
        {
            get { return customerDetails; }
            set { SetProperty(ref customerDetails, value); }
        }

        private Customer customer;
        public Customer Customer
        {
            get { return customer; }
            set { SetProperty(ref customer, value); }
        }

        private bool showValidationSummary;
        [RestorableState]
        public bool ShowValidationSummary
        {
            get { return showValidationSummary; }
            set { SetProperty(ref showValidationSummary, value); }
        }

        private ObservableCollection<ValidationError> errors;
        public ObservableCollection<ValidationError> Errors
        {
            get { return errors; }
            set { SetProperty(ref errors, value); }
        }

        async private System.Threading.Tasks.Task GetCustomerDetailsAsync()
        {
            try
            {
                if (this._task != null)
                {
                    this.Customer = await SqliteHelper.Storage.GetSingleRecordAsync<Customer>(c => c.Id == this._task.CustomerId);
                    if (this.Customer == null)
                    {
                        AppSettings.Instance.IsSyncingCustDetails = 1;

                    }
                    else
                    {
                        AppSettings.Instance.IsSyncingCustDetails = 0;
                        this.CustomerDetails.ContactNumber = this.Customer.ContactNumber;
                        this.CustomerDetails.CaseNumber = this._task.CaseNumber;
                        this.CustomerDetails.Status = this._task.Status;
                        this.CustomerDetails.StatusDueDate = this._task.StatusDueDate;
                        this.CustomerDetails.Address = this.Customer.Address;
                        this.CustomerDetails.AllocatedTo = this._task.AllocatedTo;
                        this.CustomerDetails.CustomerName = this.Customer.CustomerName;
                        this.CustomerDetails.ContactName = this.Customer.ContactName;
                        this.CustomerDetails.CategoryType = this._task.CategoryType;
                        this.CustomerDetails.EmailId = this.Customer.EmailId;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        async private void SaveCurrentUIDataAsync(Object model)
        {
            try
            {
                if (this._task != null)
                {
                    var m = (BaseModel)model;
                    var successFlag = 0;
                    if (m.ShouldSave)
                    {
                        var baseModel = await (model as BaseModel).GetDataAsync(this._task.VehicleInsRecId);


                        if (baseModel != null)
                        {
                            successFlag = await SqliteHelper.Storage.UpdateSingleRecordAsync(m);
                        }
                        else
                        {
                            m.VehicleInsRecID = this._task.VehicleInsRecId;
                            successFlag = await SqliteHelper.Storage.InsertSingleRecordAsync(m);
                        }
                    }

                    if (successFlag != 0)
                    {
                        m.ShouldSave = false;
                        await TIServiceHelper.Instance.SyncFromSvcAsync(m);
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// This function is to create table Only it should run only once.
        /// </summary>

    }
}
