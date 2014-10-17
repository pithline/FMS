﻿using Eqstra.BusinessLogic;
using Eqstra.BusinessLogic.DocumentDelivery;
using Eqstra.BusinessLogic.Enums;
using Eqstra.BusinessLogic.Helpers;
using Eqstra.DocumentDelivery.UILogic.AifServices;
using Eqstra.DocumentDelivery.UILogic.Helpers;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Eqstra.DocumentDelivery.UILogic.ViewModels
{
    public class CollectionOrDeliveryDetailsPageViewModel : BaseViewModel
    {
        private INavigationService _navigationService;
        private IEventAggregator _eventAggregator;
        private CollectDeliveryTask _task;
        private SettingsFlyout _addCustomerPage;
        public CollectionOrDeliveryDetailsPageViewModel(INavigationService navigationService, IEventAggregator eventAggregator, SettingsFlyout addCustomerPage)
            : base(navigationService)
        {
            _navigationService = navigationService;
            this._eventAggregator = eventAggregator;
            this._addCustomerPage = addCustomerPage;
            this._task = PersistentData.Instance.CollectDeliveryTask;
            this.CustomerDetails = PersistentData.Instance.CustomerDetails;
            this.DocumentList = new ObservableCollection<Document>();
            this.CollectVisibility = Visibility.Collapsed;
            this.CompleteVisibility = Visibility.Collapsed;
            this.AddContactCommand = new DelegateCommand(() =>
            {
                addCustomerPage.ShowIndependent();
            });
            this.CollectCommand = new DelegateCommand(async () =>
            {
                try
                {
                    this.IsBusy = true;
                    if (PersistentData.Instance.UserInfo.CDUserType == CDUserType.Driver || PersistentData.Instance.UserInfo.CDUserType == CDUserType.Courier)
                    {
                        foreach (var task in this.SelectedTaskBucket)
                        {
                            if (this.DocumentList.Any(a => a.CaseNumber == task.CaseNumber && a.IsMarked))
                            {
                                task.Status = CDTaskStatus.AwaitDeliveryConfirmation;
                                task.TaskType = BusinessLogic.Enums.CDTaskType.Delivery;
                                await SqliteHelper.Storage.UpdateSingleRecordAsync(task);
                            }
                        }

                    }
                    if (PersistentData.Instance.UserInfo.CDUserType == CDUserType.Customer)
                    {
                        foreach (var task in this.SelectedTaskBucket)
                        {
                            if (this.DocumentList.Any(a => a.CaseNumber == task.CaseNumber && a.IsMarked))
                            {
                                task.Status = CDTaskStatus.Completed;
                                task.TaskType = BusinessLogic.Enums.CDTaskType.Delivery;
                                await SqliteHelper.Storage.UpdateSingleRecordAsync(task);
                            }
                        }
                        this.DocuDeliveryDetails.IsDelivered = true;
                        this.DocuDeliveryDetails.IsColletedByCustomer = true;
                    }

                    this.DocuDeliveryDetails.ReceivedBy = this.SelectedContact.UserName;
                    this.DocuDeliveryDetails.IsCollected = true;
                    this.DocuDeliveryDetails.CollectedAt = this.SelectedCollectedFrom.Address;

                    this.DocuDeliveryDetails.SelectedCollectedFrom = this.SelectedCollectedFrom.UserName;


                    if (this.IsAlternateOn)
                    {
                        this.DocuDeliveryDetails.ReceivedBy = this.SelectedAlternateContact.FullName;
                        this.DocuDeliveryDetails.DeliveryPersonName = this.SelectedAlternateContact.FullName;

                        this.DocuDeliveryDetails.Email = this.SelectedAlternateContact.Email;
                        this.DocuDeliveryDetails.Position = this.SelectedAlternateContact.Position;
                        this.DocuDeliveryDetails.Phone = this.SelectedAlternateContact.CellPhone;

                    }
                    var markedDoc = await SqliteHelper.Storage.LoadTableAsync<DocumentDeliveryDetails>();
                    if (markedDoc.Any(a => a.CaseNumber == this.DocuDeliveryDetails.CaseNumber))
                    {
                        await SqliteHelper.Storage.UpdateSingleRecordAsync<DocumentDeliveryDetails>(this.DocuDeliveryDetails);
                    }
                    else
                    {
                        await SqliteHelper.Storage.InsertSingleRecordAsync<DocumentDeliveryDetails>(this.DocuDeliveryDetails);
                    }
                    await DDServiceProxyHelper.Instance.SynchronizeAllAsync();
                    this.IsBusy = false;
                    _navigationService.Navigate("InspectionDetails", string.Empty);
                }
                catch (Exception ex)
                {
                    AppSettings.Instance.ErrorMessage = ex.Message;
                    this.IsBusy = false;
                }
            },
            () =>
            {
                return (this.DocumentList.Any(a => a.IsMarked) && ((this.SelectedContact != null && !this.IsAlternateOn) || (this.SelectedAlternateContact != null && this.IsAlternateOn))
                    && this.CRSignature != null && _task.TaskType == CDTaskType.Collect);
            }
            );

            this.CompleteCommand = new DelegateCommand(async () =>
                {
                    try
                    {
                        this.IsBusy = true;
                        if (PersistentData.Instance.UserInfo.CDUserType == CDUserType.Courier)
                        {
                            foreach (var task in this.SelectedTaskBucket)
                            {
                                if (this.DocumentList.Any(a => a.CaseNumber == task.CaseNumber & a.IsMarked))
                                {
                                    task.Status = CDTaskStatus.AwaitInvoice;
                                    await SqliteHelper.Storage.UpdateSingleRecordAsync(task);
                                }
                            }
                        }
                        else
                        {
                            foreach (var task in this.SelectedTaskBucket)
                            {
                                if (this.DocumentList.Any(a => a.CaseNumber == task.CaseNumber && a.IsMarked))
                                {
                                    task.Status = CDTaskStatus.Completed;
                                    await SqliteHelper.Storage.UpdateSingleRecordAsync(task);
                                }
                            }
                        }

                        this.DocuDeliveryDetails.IsDelivered = true;
                        this.DocuDeliveryDetails.ReceivedBy = this.SelectedContact.UserName;
                        this.DocuDeliveryDetails.DeliveryPersonName = this.SelectedContact.UserName;

                        if (this.IsAlternateOn)
                        {
                            this.DocuDeliveryDetails.ReceivedBy = this.SelectedAlternateContact.FullName;
                            this.DocuDeliveryDetails.DeliveryPersonName = this.SelectedAlternateContact.FullName;

                            this.DocuDeliveryDetails.Email = this.SelectedAlternateContact.Email;
                            this.DocuDeliveryDetails.Position = this.SelectedAlternateContact.Position;
                            this.DocuDeliveryDetails.Phone = this.SelectedAlternateContact.CellPhone;

                        }
                        var markedDoc = await SqliteHelper.Storage.LoadTableAsync<DocumentDeliveryDetails>();
                        if (markedDoc.Any(a => a.CaseNumber == this.DocuDeliveryDetails.CaseNumber))
                        {
                            await SqliteHelper.Storage.UpdateSingleRecordAsync<DocumentDeliveryDetails>(this.DocuDeliveryDetails);
                        }
                        else
                        {
                            await SqliteHelper.Storage.InsertSingleRecordAsync<DocumentDeliveryDetails>(this.DocuDeliveryDetails);
                        }

                        await DDServiceProxyHelper.Instance.SynchronizeAllAsync();
                        this.IsBusy = false;
                        _navigationService.Navigate("InspectionDetails", string.Empty);

                    }
                    catch (Exception ex)
                    {
                        AppSettings.Instance.ErrorMessage = ex.Message;
                        this.IsBusy = false;
                    }

                }, () =>
                {

                    return (this.DocumentList.Any(a => a.IsMarked) && ((this.SelectedContact != null && !this.IsAlternateOn) || (this.SelectedAlternateContact != null && this.IsAlternateOn))
                        && this.CRSignature != null && _task.TaskType == CDTaskType.Delivery);
                });
            //this.SelectedDocuments = new ObservableCollection<Document>();

            this._eventAggregator.GetEvent<AlternateContactPersonEvent>().Subscribe(async (customerContacts) =>
           {
               this.DocuDeliveryDetails = new DocumentDeliveryDetails();
               this.AlternateContactPersons = await SqliteHelper.Storage.LoadTableAsync<AlternateContactPerson>();
               this._addCustomerPage.Hide();
           });

            this.DocumentsChangedCommand = new DelegateCommand<ObservableCollection<object>>((param) =>
            {
                foreach (var item in this.DocumentList)
                {
                    item.IsMarked = false;
                }
                foreach (var item in param)
                {
                    ((Document)item).IsMarked = true;
                }
                this.CompleteCommand.RaiseCanExecuteChanged();
                this.CollectCommand.RaiseCanExecuteChanged();
            });

        }

        public async override void OnNavigatedTo(object navigationParameter, Windows.UI.Xaml.Navigation.NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            try
            {
                this.IsBusy = true;
                this.IsDelSignatureDate = Visibility.Collapsed;
                this.IsCollSignatureDate = Visibility.Collapsed;
                this.ContactNameBorderBrush = new Windows.UI.Xaml.Media.SolidColorBrush(Windows.UI.Colors.Red);
                base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
                await GetProofOfCDAsync();
                if (_task.TaskType == BusinessLogic.Enums.CDTaskType.Collect)
                {
                    this.ProofTitle = "Proof of Collection";
                    this.AckDialog = string.Empty;
                    this.CDTitle = "Tasks Details";
                    this.CollectVisibility = Visibility.Visible;
                    this.CompleteVisibility = Visibility.Collapsed;
                }
                else
                {
                    this.CompleteVisibility = Visibility.Visible;
                    this.CollectVisibility = Visibility.Collapsed;
                    this.ProofTitle = "Proof of Delivery";
                    this.AckDialog = "I hereby confirm that i have received the following documents";
                    this.CDTitle = "Acknowledgement";
                    this.DocuDeliveryDetails.DeliveredAt = this.CustomerDetails.Address;
                }

                this.SelectedTaskBucket = (await SqliteHelper.Storage.LoadTableAsync<CollectDeliveryTask>()).Where(d => d.Status != CDTaskStatus.Completed && d.CustomerId == this._task.CustomerId &&
                    d.Address == this._task.Address && d.TaskType == this._task.TaskType && d.UserID == PersistentData.Instance.UserInfo.UserId).ToList();
                foreach (var d in this.SelectedTaskBucket)
                {
                    this.DocumentList.Add(new Document
                    {
                        CaseNumber = d.CaseNumber,
                        SerialNumber = d.SerialNumber,
                        CaseCategoryRecID = d.CaseCategoryRecID,
                        DocumentType = d.DocumentType,
                        MakeModel = d.MakeModel,
                        RegistrationNumber = d.RegistrationNumber,
                        DocumentName = d.DocumentName
                    });
                }

                switch (PersistentData.Instance.UserInfo.CDUserType)
                {
                    case CDUserType.Courier:
                        this.ContactPersons = await SqliteHelper.Storage.LoadTableAsync<Courier>();
                        break;

                    case CDUserType.Driver:
                        this.ContactPersons = await SqliteHelper.Storage.LoadTableAsync<Driver>();

                        break;
                    case CDUserType.Customer:
                        var contactPersonsData = await SqliteHelper.Storage.LoadTableAsync<CDCustomer>();
                        this.ContactPersons = contactPersonsData;
                        this.SelectedContact = contactPersonsData.Where(s => s.Isprimary).First();
                        break;
                }

                this.CollectedFrom = await SqliteHelper.Storage.LoadTableAsync<CollectedFromData>();
                this.AlternateContactPersons = await SqliteHelper.Storage.LoadTableAsync<AlternateContactPerson>();
                this.IsBusy = false;
            }
            catch (Exception ex)
            {
                AppSettings.Instance.ErrorMessage = ex.Message;
                this.IsBusy = false;
            }
        }
        public DelegateCommand CollectCommand { get; set; }
        public DelegateCommand CompleteCommand { get; set; }
        public DelegateCommand AddContactCommand { get; set; }

        public DelegateCommand<ObservableCollection<object>> DocumentsChangedCommand { get; set; }

        private Visibility completeVisibility;

        public Visibility CompleteVisibility
        {
            get { return completeVisibility; }
            set { SetProperty(ref completeVisibility, value); }
        }

        private Visibility collectVisibility;
        public Visibility CollectVisibility
        {
            get { return collectVisibility; }
            set { SetProperty(ref collectVisibility, value); }
        }

        private Visibility isCollSignatureDate;

        public Visibility IsCollSignatureDate
        {
            get { return isCollSignatureDate; }
            set { SetProperty(ref isCollSignatureDate, value); }
        }

        private Visibility isDelSignatureDate;

        public Visibility IsDelSignatureDate
        {
            get { return isDelSignatureDate; }
            set { SetProperty(ref isDelSignatureDate, value); }
        }

        private bool isAlternateOn;
        public bool IsAlternateOn
        {
            get { return isAlternateOn; }
            set
            {
                if (SetProperty(ref isAlternateOn, value))
                {
                    this.CompleteCommand.RaiseCanExecuteChanged();
                    this.CollectCommand.RaiseCanExecuteChanged();
                }
            }
        }

        private bool masterIsChecked;
        [Ignore]
        public bool MasterIsChecked
        {
            get { return masterIsChecked; }
            set
            {
                if (SetProperty(ref masterIsChecked, value))
                {
                    foreach (var item in this.DocumentList)
                    {
                        item.IsMarked = masterIsChecked;
                    }
                    this.CompleteCommand.RaiseCanExecuteChanged();
                    this.CollectCommand.RaiseCanExecuteChanged();
                }
            }
        }


        private string proofTitle;
        public string ProofTitle
        {
            get { return proofTitle; }
            set { SetProperty(ref proofTitle, value); }
        }
        private DocumentDeliveryDetails docuDeliveryDetails;
        public DocumentDeliveryDetails DocuDeliveryDetails
        {
            get { return docuDeliveryDetails; }
            set { SetProperty(ref docuDeliveryDetails, value); }
        }
        private ObservableCollection<Document> documentList;
        public ObservableCollection<Document> DocumentList
        {
            get { return documentList; }
            set { SetProperty(ref documentList, value); }
        }

        private CDCustomerDetails customerDetails;

        public CDCustomerDetails CustomerDetails
        {
            get { return customerDetails; }
            set { SetProperty(ref customerDetails, value); }
        }

        private string title;
        public string CDTitle
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        private DocumentsReceiver selectedContact;
        [Ignore]
        public DocumentsReceiver SelectedContact
        {
            get { return selectedContact; }
            set
            {
                if (SetProperty(ref selectedContact, value))
                {
                    this.ContactNameBorderBrush = new Windows.UI.Xaml.Media.SolidColorBrush(Windows.UI.Colors.SlateBlue);
                    this.CompleteCommand.RaiseCanExecuteChanged();
                    this.CollectCommand.RaiseCanExecuteChanged();
                }
            }
        }


        private AlternateContactPerson selectedAlternateContact;
        [Ignore]
        public AlternateContactPerson SelectedAlternateContact
        {
            get { return selectedAlternateContact; }
            set
            {
                if (SetProperty(ref selectedAlternateContact, value))
                {
                    this.ContactNameBorderBrush = new Windows.UI.Xaml.Media.SolidColorBrush(Windows.UI.Colors.SlateBlue);
                    this.CompleteCommand.RaiseCanExecuteChanged();
                    this.CollectCommand.RaiseCanExecuteChanged();
                }
            }
        }

        private CollectedFromData selectedCollectedFrom;
        [Ignore]
        public CollectedFromData SelectedCollectedFrom
        {
            get { return selectedCollectedFrom; }
            set { SetProperty(ref selectedCollectedFrom, value); }
        }


        private IEnumerable<DocumentsReceiver> contactPersons;
        [Ignore]
        public IEnumerable<DocumentsReceiver> ContactPersons
        {
            get { return contactPersons; }
            set
            {
                SetProperty(ref contactPersons, value);
            }
        }

        private IEnumerable<AlternateContactPerson> alternateContactPersons;
        [Ignore]
        public IEnumerable<AlternateContactPerson> AlternateContactPersons
        {
            get { return alternateContactPersons; }
            set
            {
                SetProperty(ref alternateContactPersons, value);
            }
        }

        private BitmapImage cRSignature;
        [Ignore]
        public BitmapImage CRSignature
        {
            get { return cRSignature; }
            set
            {
                if (SetProperty(ref cRSignature, value))
                {
                    if (this.CollectVisibility == Visibility.Visible)
                    {
                        this.IsCollSignatureDate = Visibility.Visible;
                        this.DocuDeliveryDetails.ReceivedDate = DateTime.Now;
                    }

                    if (this.CompleteVisibility == Visibility.Visible)
                    {
                        this.IsDelSignatureDate = Visibility.Visible;
                        this.DocuDeliveryDetails.DeliveryDate = DateTime.Now;
                    }
                    this.CompleteCommand.RaiseCanExecuteChanged();
                    this.CollectCommand.RaiseCanExecuteChanged();
                }
            }
        }

        private List<CollectedFromData> collectedFrom;
        [Ignore]
        public List<CollectedFromData> CollectedFrom
        {
            get { return collectedFrom; }
            set { SetProperty(ref collectedFrom, value); }
        }

        private string ackDialog;
        public string AckDialog
        {
            get { return ackDialog; }
            set { SetProperty(ref ackDialog, value); }
        }

        private List<CollectDeliveryTask> allTaskOfCustomer;

        public List<CollectDeliveryTask> SelectedTaskBucket
        {
            get { return allTaskOfCustomer; }
            set { SetProperty(ref allTaskOfCustomer, value); }
        }
        private bool isBusy;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        private bool isDeliverType;

        public bool IsDeliverType
        {
            get { return isDeliverType; }
            set { SetProperty(ref isDeliverType, value); }
        }

        private Brush contactNameBorderBrush;
        public Brush ContactNameBorderBrush
        {
            get { return contactNameBorderBrush; }
            set { SetProperty(ref contactNameBorderBrush, value); }
        }

        //private ObservableCollection<Document> selectedDocuments;
        //public ObservableCollection<Document> SelectedDocuments
        //{
        //    get { return selectedDocuments; }
        //    set
        //    {
        //        SetProperty(ref selectedDocuments, value);
        //    }
        //}
        async public System.Threading.Tasks.Task GetProofOfCDAsync()
        {
            try
            {

                this.DocuDeliveryDetails = await SqliteHelper.Storage.GetSingleRecordAsync<DocumentDeliveryDetails>(x => x.CaseNumber == this._task.CaseNumber);
                if (this.DocuDeliveryDetails == null)
                {
                    this.DocuDeliveryDetails = new DocumentDeliveryDetails();
                    this.DocuDeliveryDetails.CaseNumber = this._task.CaseNumber;
                    this.DocuDeliveryDetails.CaseServiceRecId = this._task.CaseServiceRecID;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
