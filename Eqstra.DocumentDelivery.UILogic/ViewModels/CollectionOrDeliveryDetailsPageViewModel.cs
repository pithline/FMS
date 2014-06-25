using Eqstra.BusinessLogic;
using Eqstra.BusinessLogic.DeliveryModel;
using Eqstra.BusinessLogic.Helpers;
using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using Newtonsoft.Json;
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
        private CollectDeliveryTask _task;
        public CollectionOrDeliveryDetailsPageViewModel(INavigationService navigationService, SettingsFlyout addCustomerPage)
            : base(navigationService)
        {
            _navigationService = navigationService;
            this.CollectVisibility = Visibility.Collapsed;
            this.CompleteVisibility = Visibility.Collapsed;
            this.SaveVisibility = Visibility.Visible;
            this.AddContactCommand = new DelegateCommand(() =>
            {
                addCustomerPage.ShowIndependent();
            });
            this.CollectCommand = new DelegateCommand(async () =>
            {
                _task.CDTaskStatus = BusinessLogic.Enums.CDTaskStatus.AwaitingDelivery;
                await SqliteHelper.Storage.UpdateSingleRecordAsync(_task);
                _navigationService.Navigate("Main", string.Empty);

            });
            this.SaveCommand = new DelegateCommand<object>(async (param) =>
            {
                ProofOfCollection proofOfCollection = param as ProofOfCollection;
                var result=await SqliteHelper.Storage.LoadTableAsync<ProofOfCollection>();
                if (result!=null)
                {
                    if (result.Any(a => a.VehicleInsRecID == proofOfCollection.VehicleInsRecID))
                    {
                        await SqliteHelper.Storage.UpdateSingleRecordAsync(proofOfCollection);
                    }
                    else
                    {
                        await SqliteHelper.Storage.InsertSingleRecordAsync(proofOfCollection);
                    } 
                }
                this.SaveVisibility = Visibility.Collapsed;
                this.CollectVisibility = Visibility.Visible;
            }
            );
            this.CompleteCommand = new DelegateCommand(async () =>
                {
                    _task.CDTaskStatus = BusinessLogic.Enums.CDTaskStatus.Complete;
                    await SqliteHelper.Storage.UpdateSingleRecordAsync(_task);
                    _navigationService.Navigate("Main", string.Empty);
                });
        }
        public override void OnNavigatedTo(object navigationParameter, Windows.UI.Xaml.Navigation.NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
            _task = JsonConvert.DeserializeObject<CollectDeliveryTask>(navigationParameter.ToString());
            if (_task.TaskType == BusinessLogic.Enums.CDTaskTypeEnum.Collection)
            {
                this.SaveVisibility = Visibility.Visible;
                this.ProofTitle = "Proof of Collection";
            }
            else
            {
                this.CompleteVisibility = Visibility.Visible;
                this.ProofTitle = "Proof of Delivery";
            }
            this.DocumentList = new ObservableCollection<Document>
            {
                new Document{VehicleInsRecID=123, CaseNumber = "E4323",DocumentType  = "License Disc",RegistrationNumber="Registration Number", Make = "Make",Model = "Model",SerialNumber = "Serial Number"},
                new Document{VehicleInsRecID=234, CaseNumber = "E4323",DocumentType  = "License Disc",RegistrationNumber="Registration Number", Make = "Make",Model = "Model",SerialNumber = "Serial Number"},
                new Document{VehicleInsRecID=345, CaseNumber = "E4323",DocumentType  = "License Disc",RegistrationNumber="Registration Number", Make = "Make",Model = "Model",SerialNumber = "Serial Number"},
                new Document{VehicleInsRecID=456, CaseNumber = "E4323",DocumentType  = "License Disc",RegistrationNumber="Registration Number", Make = "Make",Model = "Model",SerialNumber = "Serial Number"},
                new Document{VehicleInsRecID=789, CaseNumber = "E4323",DocumentType  = "License Disc",RegistrationNumber="Registration Number", Make = "Make",Model = "Model",SerialNumber = "Serial Number"},
                new Document{VehicleInsRecID=985, CaseNumber = "E4323",DocumentType  = "License Disc",RegistrationNumber="Registration Number", Make = "Make",Model = "Model",SerialNumber = "Serial Number"},
                new Document{VehicleInsRecID=741, CaseNumber = "E4323",DocumentType  = "License Disc",RegistrationNumber="Registration Number", Make = "Make",Model = "Model",SerialNumber = "Serial Number"},
                new Document{VehicleInsRecID=852, CaseNumber = "E4323",DocumentType  = "License Disc",RegistrationNumber="Registration Number", Make = "Make",Model = "Model",SerialNumber = "Serial Number"},
                new Document{VehicleInsRecID=145, CaseNumber = "E4323",DocumentType  = "License Disc",RegistrationNumber="Registration Number", Make = "Make",Model = "Model",SerialNumber = "Serial Number"},
                new Document{VehicleInsRecID=963, CaseNumber = "E4323",DocumentType  = "License Disc",RegistrationNumber="Registration Number", Make = "Make",Model = "Model",SerialNumber = "Serial Number"},
            };
        }

        public DelegateCommand CollectCommand { get; set; }
        public DelegateCommand<object> SaveCommand { get; set; }
        public DelegateCommand CompleteCommand { get; set; }
        public DelegateCommand AddContactCommand { get; set; }

        private Visibility completeVisibility;

        public Visibility CompleteVisibility
        {
            get { return completeVisibility; }
            set { SetProperty(ref completeVisibility, value); }
        }
        private Visibility saveVisibility;

        public Visibility SaveVisibility
        {
            get { return saveVisibility; }
            set { SetProperty(ref saveVisibility, value); }
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
        private ProofOfCollection proofOfCollection;

        public ProofOfCollection ProofOfCollection
        {
            get { return proofOfCollection; }
            set { SetProperty(ref proofOfCollection, value); }
        }
        private ObservableCollection<Document> documentList;
        public ObservableCollection<Document> DocumentList
        {
            get { return documentList; }
            set { SetProperty(ref documentList, value); }
        }
        async public System.Threading.Tasks.Task GetProofOfCollectionAsync()
        {
            this.ProofOfCollection = await SqliteHelper.Storage.GetSingleRecordAsync<ProofOfCollection>(x => x.VehicleInsRecID == this._task.VehicleInsRecId);
            if (this.ProofOfCollection == null)
            {
                this.ProofOfCollection = new ProofOfCollection();
                this.ProofOfCollection.VehicleInsRecID = this._task.VehicleInsRecId;
            }
        }
    }
}
