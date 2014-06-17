using Eqstra.BusinessLogic;
using Eqstra.BusinessLogic.Base;
using Eqstra.BusinessLogic.Commercial;
using Eqstra.BusinessLogic.Helpers;
using Eqstra.BusinessLogic.Passenger;
using Eqstra.VehicleInspection.Common;
using Eqstra.VehicleInspection.UILogic;
using Eqstra.VehicleInspection.UILogic.AifServices;
using Eqstra.VehicleInspection.UILogic.ViewModels;
using Eqstra.VehicleInspection.Views;
using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using SQLite;
using Syncfusion.UI.Xaml.Schedule;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;
using Windows.System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;

namespace Eqstra.VehicleInspection.ViewModels
{
    public class VehicleInspectionPageViewModel : BaseViewModel
    {
        private Eqstra.BusinessLogic.Task _task;
        private INavigationService _navigationService;

        public VehicleInspectionPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            try
            {
                _navigationService = navigationService;
                this.InspectionUserControls = new ObservableCollection<UserControl>();
                this.CustomerDetails = new CustomerDetails();

                this.PrevViewStack = new Stack<UserControl>();
                // LoadDemoAppointments();

                this.CompleteCommand = new DelegateCommand(async () =>
                {
                    this._task.Status = BusinessLogic.Enums.TaskStatus.AwaitDamageConfirmation;
                    await SqliteHelper.Storage.UpdateSingleRecordAsync(this._task);
                    var currentModel = ((BaseViewModel)this.prevViewStack.FirstOrDefault().DataContext).Model;
                    this.SaveCurrentUIDataAsync(currentModel);
                    _navigationService.Navigate("Main", null);
                    this.IsCommandBarOpen = false;
                   await VIServiceHelper.Instance.UpdateTaskStatusAsync();
                }, () => { return this.NextViewStack.Count == 1; });

                this.NextCommand = new DelegateCommand(async () =>
                {
                    //this.IsCommandBarOpen = false;
                    //this.IsFlyoutOpen = true;
                    ShowValidationSummary = false;
                    var currentModel = ((BaseViewModel)this.NextViewStack.Peek().DataContext).Model as BaseModel;

                    if (currentModel.ValidateModel())
                    {

                        this.PrevViewStack.Push(this.NextViewStack.Pop());
                        this.SaveCurrentUIDataAsync(currentModel);
                        this.FrameContent = this.NextViewStack.Peek();
                        CompleteCommand.RaiseCanExecuteChanged();
                        NextCommand.RaiseCanExecuteChanged();
                        PreviousCommand.RaiseCanExecuteChanged();
                        if (this.NextViewStack.FirstOrDefault() != null)
                        {
                            BaseViewModel nextViewModel = this.NextViewStack.FirstOrDefault().DataContext as BaseViewModel;
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
                    return this.NextViewStack.Count > 1;
                });


                this.PreviousCommand = new DelegateCommand(async () =>
                {
                    this.IsCommandBarOpen = false;
                    ShowValidationSummary = false;
                    var currentModel = ((BaseViewModel)this.NextViewStack.Peek().DataContext).Model as BaseModel;
                    if (currentModel.ValidateModel())
                    {
                        var item = this.PrevViewStack.Pop();
                        this.FrameContent = item;
                        this.NextViewStack.Push(item);
                        CompleteCommand.RaiseCanExecuteChanged();
                        PreviousCommand.RaiseCanExecuteChanged();
                        NextCommand.RaiseCanExecuteChanged();
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

        private void LoadDemoAppointments()
        {
            this.CustomerDetails.Appointments = AppSettingData.Appointments;


            //  Eqstra.BusinessLogic.DrivingDuration durationData = SqliteHelper.Storage.GetSingleRecordAsync<Eqstra.BusinessLogic.DrivingDuration>(w => w.CaseNumber == this._task.CaseNumber);


            this.CustomerDetails.Appointments = new ScheduleAppointmentCollection
            {
                new ScheduleAppointment(){
                    Subject = this._task.CaseNumber,
                    
                    Location =this._task.Address,
                    StartTime = DateTime.Parse(  this._task.ConfirmedDate.ToString("MM/dd/yyyy") + this._task.ConfirmedTime.ToString("hh:mm:ss")),
                    EndTime = DateTime.Now.AddHours(2),
                    ReadOnly = true,
                    AppointmentBackground = new SolidColorBrush(Colors.Crimson),                   
                    Status = new ScheduleAppointmentStatus{Status = this._task.Status,Brush = new SolidColorBrush(Colors.Chocolate)}

                },
                //new ScheduleAppointment(){
                //    Subject = "Inspection at Peter Johnson",
                //    Notes = "some noise from differential",
                //    Location = this._task.Address,
                //     ReadOnly = true,
                //    StartTime =new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,8,00,00),
                //    EndTime = new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,9,00,00),
                //    Status = new ScheduleAppointmentStatus{Brush = new SolidColorBrush(Colors.Green), Status = this._task.Status},
                //},                    
            };
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
                //var vt = await SqliteHelper.Storage.LoadTableAsync<Vehicle>();
                ApplicationData.Current.LocalSettings.Values["CaseNumber"] = _task.CaseNumber;

                await GetCustomerDetailsAsync();
                if (_task.VehicleType == BusinessLogic.Enums.VehicleTypeEnum.Passenger)
                {
                    this.InspectionUserControls.Add(new VehicleDetailsUserControl());
                    this.InspectionUserControls.Add(new TrimIntUserControl());
                    this.InspectionUserControls.Add(new BodyworkUserControl());
                    this.InspectionUserControls.Add(new GlassUserControl());
                    this.InspectionUserControls.Add(new AccessoriesUserControl());
                    this.InspectionUserControls.Add(new TyreConditionUserControl());
                    this.InspectionUserControls.Add(new MechanicalCondUserControl());
                    this.InspectionUserControls.Add(new InspectionProofUserControl());
                }
                else
                {
                    this.InspectionUserControls.Add(new CommercialVehicleDetailsUserControl());
                    this.InspectionUserControls.Add(new CabTrimInterUserControl());
                    this.InspectionUserControls.Add(new ChassisBodyUserControl());
                    this.InspectionUserControls.Add(new CGlassUserControl());
                    this.InspectionUserControls.Add(new CAccessoriesUserControl());
                    this.InspectionUserControls.Add(new CTyresUserControl());
                    this.InspectionUserControls.Add(new CMechanicalCondUserControl());
                    this.InspectionUserControls.Add(new CPOIUserControl());
                }
                NextViewStack = new Stack<UserControl>(this.InspectionUserControls.Reverse());
                this.FrameContent = this.inpectionUserControls[0];
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
        private DelegateCommand nextCommand;

        public DelegateCommand NextCommand
        {
            get { return nextCommand; }
            set
            {
                SetProperty(ref nextCommand, value);
            }
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


        async private System.Threading.Tasks.Task GetCustomerDetailsAsync()
        {
            try
            {
                if (this._task != null)
                {
                    this.Customer = await SqliteHelper.Storage.GetSingleRecordAsync<Customer>(c => c.Id == this._task.CustomerId);
                    this.CustomerDetails.ContactNumber = this.Customer.ContactNumber;
                    this.CustomerDetails.CaseNumber = this._task.CaseNumber;
                    this.CustomerDetails.Status = this._task.Status;
                    this.CustomerDetails.StatusDueDate = this._task.StatusDueDate;
                    this.CustomerDetails.Address = this.Customer.Address;
                    this.CustomerDetails.AllocatedTo = this._task.AllocatedTo;
                    this.CustomerDetails.CustomerName = this.Customer.CustomerName;
                    this.CustomerDetails.ContactName = this.Customer.ContactName;
                    this.CustomerDetails.CaseType = this._task.CaseType;
                    this.CustomerDetails.EmailId = this.Customer.EmailId;

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
                        await VIServiceHelper.Instance.SyncFromSvcAsync(m);
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
