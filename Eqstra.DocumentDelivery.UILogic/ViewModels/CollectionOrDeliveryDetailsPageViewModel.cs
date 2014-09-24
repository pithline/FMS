using Eqstra.BusinessLogic;
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
                if (PersistentData.Instance.UserInfo.CDUserType == CDUserType.Driver || PersistentData.Instance.UserInfo.CDUserType == CDUserType.Courier)
                {
                    _task.Status = CDTaskStatus.AwaitDeliveryConfirmation;

                }
                if (PersistentData.Instance.UserInfo.CDUserType == CDUserType.Customer)
                {
                    _task.Status = CDTaskStatus.Completed;
                    this.docuDeliveryDetails.IsDelivered = true;
                }
                _task.TaskType = BusinessLogic.Enums.CDTaskType.Delivery;
                await SqliteHelper.Storage.UpdateSingleRecordAsync(_task);
                foreach (var item in this.SelectedDocuments)
                {
                    item.IsCollected = true;
                    await SqliteHelper.Storage.UpdateSingleRecordAsync(item);
                }
                this.docuDeliveryDetails.ReceivedBy = this.SelectedContactName;
                this.docuDeliveryDetails.IsCollected = true;
                this.docuDeliveryDetails.CollectedFrom = this.SelectedCollectedFrom;
                this.docuDeliveryDetails.ShouldSave = true;
                await SqliteHelper.Storage.InsertSingleRecordAsync<DocumentDeliveryDetails>(this.docuDeliveryDetails);

                await DDServiceProxyHelper.Instance.SynchronizeAllAsync();

                _navigationService.Navigate("Main", string.Empty);
            },
            () =>
            {
                return (this.SelectedDocuments != null && this.SelectedDocuments.Any() && _task.TaskType == CDTaskType.Collect);
            }
            );

            this.CompleteCommand = new DelegateCommand(async () =>
                {
                    _task.Status =CDTaskStatus.Completed;
                    if (PersistentData.Instance.UserInfo.CDUserType == CDUserType.Courier)
                    {
                        _task.Status = CDTaskStatus.AwaitInvoice;
                    }
                    await SqliteHelper.Storage.UpdateSingleRecordAsync(_task);
                    foreach (var item in this.SelectedDocuments)
                    {
                        item.IsDelivered = true;
                        await SqliteHelper.Storage.UpdateSingleRecordAsync(item);
                    }

                    this.docuDeliveryDetails.IsDelivered = true;
                    this.docuDeliveryDetails.ReceivedBy = this.SelectedContactName;
                    this.docuDeliveryDetails.CollectedFrom = this.SelectedCollectedFrom;
                    this.docuDeliveryDetails.ShouldSave = true;
                    await SqliteHelper.Storage.InsertSingleRecordAsync<DocumentDeliveryDetails>(this.docuDeliveryDetails);

                    await DDServiceProxyHelper.Instance.SynchronizeAllAsync();

                    _navigationService.Navigate("Main", string.Empty);

                }, () =>
                {

                    return (this.SelectedDocuments != null && this.SelectedDocuments.Any() && _task.TaskType == BusinessLogic.Enums.CDTaskType.Delivery);
                });
            this.SelectedDocuments = new ObservableCollection<Document>();

            this._eventAggregator.GetEvent<ContactPersonEvent>().Subscribe(async (customerContacts) =>
           {
               this.DocuDeliveryDetails = new DocumentDeliveryDetails();
               this.DocuDeliveryDetails.ContactPersons = await SqliteHelper.Storage.LoadTableAsync<ContactPerson>();
               this._addCustomerPage.Hide();
           });

            this.DocumentsChangedCommand = new DelegateCommand<ObservableCollection<object>>((param) =>
            {
                this.SelectedDocuments.Clear();
                foreach (var item in param)
                {
                    this.SelectedDocuments.Add(((Document)item));
                }
                this.CompleteCommand.RaiseCanExecuteChanged();
                this.CollectCommand.RaiseCanExecuteChanged();
            });

        }

        public async override void OnNavigatedTo(object navigationParameter, Windows.UI.Xaml.Navigation.NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            try
            {
                base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
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
                }

                var docList = (await SqliteHelper.Storage.LoadTableAsync<Document>()).Where(d => !d.IsDelivered);
                foreach (var d in docList)
                {
                    if (d.CaseCategoryRecID.Equals(this._task.CaseCategoryRecID))
                        this.DocumentList.Add(d);
                }

                await GetProofOfCollectionAsync();
                this.DocuDeliveryDetails.DeliveredAt = this.CustomerDetails.Address;
                this.DocuDeliveryDetails.DeliveryPersonName = this.CustomerDetails.CustomerName;
            }
            catch (Exception ex)
            {
                AppSettings.Instance.ErrorMessage = ex.Message;
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

        private string selectedContactName;
        [Ignore]
        public string SelectedContactName
        {
            get { return selectedContactName; }
            set { SetProperty(ref selectedContactName, value); }
        }

        private string selectedCollectedFrom;
        [Ignore]
        public string SelectedCollectedFrom
        {
            get { return selectedCollectedFrom; }
            set { SetProperty(ref selectedCollectedFrom, value); }
        }


        private string ackDialog;
        public string AckDialog
        {
            get { return ackDialog; }
            set { SetProperty(ref ackDialog, value); }
        }

        private ObservableCollection<Document> selectedDocuments;
        public ObservableCollection<Document> SelectedDocuments
        {
            get { return selectedDocuments; }
            set
            {
                SetProperty(ref selectedDocuments, value);
            }
        }
        async public System.Threading.Tasks.Task GetProofOfCollectionAsync()
        {
            this.DocuDeliveryDetails = await SqliteHelper.Storage.GetSingleRecordAsync<DocumentDeliveryDetails>(x => x.CaseCategoryRecID == this._task.CaseCategoryRecID);
            if (this.DocuDeliveryDetails == null)
            {
                this.DocuDeliveryDetails = new DocumentDeliveryDetails();
                this.DocuDeliveryDetails.CaseCategoryRecID = this._task.CaseCategoryRecID;
            }
        }
    }
}
