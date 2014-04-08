using Eqstra.BusinessLogic;
using Eqstra.BusinessLogic.Helpers;
using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
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
    public class CollectionOrDeliveryDetailsPageViewModel : ViewModel
    {
        private INavigationService _navigationService;
        private CollectDeliveryTask _task;
        public CollectionOrDeliveryDetailsPageViewModel(INavigationService navigationService, SettingsFlyout addCustomerPage)
        {
            _navigationService = navigationService;
            this.CollectVisibility = Visibility.Collapsed;
            this.CompleteVisibility = Visibility.Collapsed;
            this.AddContactCommand = new DelegateCommand(() =>
            {
                addCustomerPage.ShowIndependent();
            });
            this.CollectCommand = new DelegateCommand(async () =>
            {
                _task.CDTaskStatus = BusinessLogic.Enums.CDTaskStatusEnum.AwaitingDelivery;
                await SqliteHelper.Storage.UpdateSingleRecordAsync(_task);
                _navigationService.Navigate("Main", null);

            });
            this.CompleteCommand = new DelegateCommand(async () =>
                {
                    _task.CDTaskStatus = BusinessLogic.Enums.CDTaskStatusEnum.Complete;
                    await SqliteHelper.Storage.UpdateSingleRecordAsync(_task);
                    _navigationService.Navigate("Main", null);
                });
        }
        public override void OnNavigatedTo(object navigationParameter, Windows.UI.Xaml.Navigation.NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
            _task = ((CollectDeliveryTask)navigationParameter);
            if (_task.TaskType == BusinessLogic.Enums.CDTaskTypeEnum.Collection)
            {
                this.CollectVisibility = Visibility.Visible;
                this.ProofTitle = "Proof of Collection";
            }
            else
            {
                this.CompleteVisibility = Visibility.Visible;
                this.ProofTitle = "Proof of Delivery";
            }
            this.DocumentList = new ObservableCollection<Document>
            {
                new Document{CaseNumber = "E4323",DocumentType  = "License Disc",RegistrationNumber="Registration Number", Make = "Make",Model = "Model",SerialNumber = "Serial Number"},
                new Document{CaseNumber = "E4323",DocumentType  = "License Disc",RegistrationNumber="Registration Number", Make = "Make",Model = "Model",SerialNumber = "Serial Number"},
                new Document{CaseNumber = "E4323",DocumentType  = "License Disc",RegistrationNumber="Registration Number", Make = "Make",Model = "Model",SerialNumber = "Serial Number"},
                new Document{CaseNumber = "E4323",DocumentType  = "License Disc",RegistrationNumber="Registration Number", Make = "Make",Model = "Model",SerialNumber = "Serial Number"},
                new Document{CaseNumber = "E4323",DocumentType  = "License Disc",RegistrationNumber="Registration Number", Make = "Make",Model = "Model",SerialNumber = "Serial Number"},
                new Document{CaseNumber = "E4323",DocumentType  = "License Disc",RegistrationNumber="Registration Number", Make = "Make",Model = "Model",SerialNumber = "Serial Number"},
                new Document{CaseNumber = "E4323",DocumentType  = "License Disc",RegistrationNumber="Registration Number", Make = "Make",Model = "Model",SerialNumber = "Serial Number"},
                new Document{CaseNumber = "E4323",DocumentType  = "License Disc",RegistrationNumber="Registration Number", Make = "Make",Model = "Model",SerialNumber = "Serial Number"},
                new Document{CaseNumber = "E4323",DocumentType  = "License Disc",RegistrationNumber="Registration Number", Make = "Make",Model = "Model",SerialNumber = "Serial Number"},
                new Document{CaseNumber = "E4323",DocumentType  = "License Disc",RegistrationNumber="Registration Number", Make = "Make",Model = "Model",SerialNumber = "Serial Number"},
            };
        }



        public DelegateCommand CollectCommand { get; set; }
        public DelegateCommand CompleteCommand { get; set; }
        public DelegateCommand AddContactCommand { get; set; }

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

        private ObservableCollection<Document> documentList;

        public ObservableCollection<Document> DocumentList
        {
            get { return documentList; }
            set { SetProperty(ref documentList, value); }
        }


    }
}
