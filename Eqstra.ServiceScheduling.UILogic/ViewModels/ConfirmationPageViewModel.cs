﻿using Eqstra.BusinessLogic;
using Eqstra.BusinessLogic.Common;
using Eqstra.BusinessLogic.Helpers;
using Eqstra.BusinessLogic.ServiceSchedule;
using Eqstra.BusinessLogic.ServiceSchedulingModel;
using Eqstra.ServiceScheduling.UILogic.AifServices;
using Eqstra.ServiceScheduling.UILogic.Helpers;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eqstra.ServiceScheduling.UILogic.ViewModels
{
    public class ConfirmationPageViewModel : BaseViewModel
    {
        private INavigationService _navigationService;
        private IEventAggregator _eventAggregator;
        public ConfirmationPageViewModel(INavigationService navigationService, IEventAggregator eventAggregator)
            : base(navigationService)
        {
            this._navigationService = navigationService;
            this._eventAggregator = eventAggregator;
            this.Model = new ConfirmationServiceScheduling();
            SubmitCommand = new DelegateCommand(async
                () =>
            {
                if (this.DriverTask != null)
                {
                    this.DriverTask.Status = DriverTaskStatus.AwaitServiceConfirmation;
                    await SSProxyHelper.Instance.InsertConfirmationServiceSchedulingToSvcAsync(this.ServiceSchedulingDetail, this.SupplierSelection, this.DriverTask.CaseNumber, this.DriverTask.CaseServiceRecID, this.DriverTask.CollectionRecID);
                }
                navigationService.Navigate("Main", string.Empty);
            },
            () =>
            { return (this.ServiceDateOption1 == this.ServiceSchedulingDetail.ServiceDateOption1) && (this.ServiceDateOption2 == this.ServiceSchedulingDetail.ServiceDateOption2); }
            );

            this.ConfirmationDatesCommand = new DelegateCommand(async () =>
            {
                if (this.DriverTask != null)
                {
                    this.DriverTask.Status = DriverTaskStatus.AwaitJobCardCapture;
                    this.ServiceSchedulingDetail.ServiceDateOption1 = this.ServiceDateOption1;
                    this.ServiceSchedulingDetail.ServiceDateOption2 = this.ServiceDateOption2;
                    bool IsUpdated = await SSProxyHelper.Instance.UpdateConfirmationDatesToSvcAsync(this.DriverTask.CaseServiceRecID, this.ServiceSchedulingDetail);
                    if (IsUpdated)
                    {
                        this.ConfirmationDatesCommand.RaiseCanExecuteChanged();
                        this.SubmitCommand.RaiseCanExecuteChanged();
                    }
                }
            }, () => { return (this.ServiceDateOption1 != this.ServiceSchedulingDetail.ServiceDateOption1) || (this.ServiceDateOption2 != this.ServiceSchedulingDetail.ServiceDateOption2); });

        }
        public async override void OnNavigatedTo(object navigationParameter, Windows.UI.Xaml.Navigation.NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
            try
            {
                this.DriverTask = PersistentData.Instance.DriverTask;
                this.CustomerDetails = PersistentData.Instance.CustomerDetails;
                this.ServiceSchedulingDetail = await Util.DeserializeObjectAsync<ServiceSchedulingDetail>("ServiceSchedulingDetail");
                this.SupplierSelection = await Util.DeserializeObjectAsync<SupplierSelection>("SupplierSelection");
                this.ServiceDateOption1 = this.ServiceSchedulingDetail.ServiceDateOption1;
                this.ServiceDateOption2 = this.ServiceSchedulingDetail.ServiceDateOption2;
            }
            catch (Exception ex)
            {

                AppSettings.Instance.ErrorMessage = ex.Message;
            }
        }
        private CustomerDetails customerDetails;
        public CustomerDetails CustomerDetails
        {
            get { return customerDetails; }
            set { SetProperty(ref customerDetails, value); }
        }
        public DelegateCommand SubmitCommand { get; set; }

        private DriverTask driverTask;
        public DriverTask DriverTask
        {
            get { return driverTask; }
            set { SetProperty(ref driverTask, value); }
        }

        private ConfirmationServiceScheduling model;

        public new ConfirmationServiceScheduling Model
        {
            get { return model; }
            set { SetProperty(ref model, value); }
        }

        private SupplierSelection supplierSelection;

        public SupplierSelection SupplierSelection
        {
            get { return supplierSelection; }
            set { SetProperty(ref supplierSelection, value); }
        }

        private ServiceSchedulingDetail serviceSchedulingDetail;

        public ServiceSchedulingDetail ServiceSchedulingDetail
        {
            get { return serviceSchedulingDetail; }
            set { SetProperty(ref serviceSchedulingDetail, value); }
        }

        private DateTime serviceDateOption1;

        public DateTime ServiceDateOption1
        {
            get { return serviceDateOption1; }
            set
            {
                if (SetProperty(ref serviceDateOption1, value))
                {
                    ConfirmationDatesCommand.RaiseCanExecuteChanged();
                    SubmitCommand.RaiseCanExecuteChanged();
                }
            }
        }

        private DateTime serviceDateOption2;

        public DateTime ServiceDateOption2
        {
            get { return serviceDateOption2; }
            set
            {
                if (SetProperty(ref serviceDateOption2, value))
                {
                    ConfirmationDatesCommand.RaiseCanExecuteChanged();
                    SubmitCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public DelegateCommand ConfirmationDatesCommand { get; set; }
    }
}
