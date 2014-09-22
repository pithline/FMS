﻿using Bing.Maps;
using Eqstra.BusinessLogic;
using Eqstra.BusinessLogic.DeliveryModel;
using Eqstra.BusinessLogic.DocumentDelivery;
using Eqstra.BusinessLogic.Helpers;
using Eqstra.DocumentDelivery.UILogic.Helpers;
using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using Newtonsoft.Json;
using Syncfusion.UI.Xaml.Schedule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage;
using Windows.System;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace Eqstra.DocumentDelivery.UILogic.ViewModels
{
    public class DrivingDirectionPageViewModel : BaseViewModel
    {
        private INavigationService _navigationService;
        private Eqstra.BusinessLogic.CollectDeliveryTask _deliveryTask;
        public DrivingDirectionPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            _navigationService = navigationService;
            this._deliveryTask = PersistentData.Instance.CollectDeliveryTask;
            this.CustomerDetails = PersistentData.Instance.CustomerDetails;
            GetDirectionsCommand = DelegateCommand<Location>.FromAsyncHandler(async (location) =>
            {
                var stringBuilder = new StringBuilder("bingmaps:?rtp=pos.");
                stringBuilder.Append(location.Latitude);
                stringBuilder.Append("_");
                stringBuilder.Append(location.Longitude);
                stringBuilder.Append("~adr." + Regex.Replace(this.CustomerDetails.Address, "\n", ","));
                await Launcher.LaunchUriAsync(new Uri(stringBuilder.ToString()));
            });

            this.GoToDocumentDeliveryCommand = new DelegateCommand(() =>
            {
                _navigationService.Navigate("CollectionOrDeliveryDetails", string.Empty);
            });

            this.StartDrivingCommand = new DelegateCommand(async () =>
            {
                this.IsStartDriving = false;
                this.IsArrived = true;
                await SqliteHelper.Storage.InsertSingleRecordAsync(new DrivingDuration { StartDateTime = DateTime.Now, VehicleInsRecID = long.Parse(ApplicationData.Current.LocalSettings.Values["VehicleInsRecID"].ToString()) });
            });

            this.ArrivedCommand = new DelegateCommand(async () =>
            {
                if (this._deliveryTask != null)
                {
                    var vehicleInsRecId = Int64.Parse(ApplicationData.Current.LocalSettings.Values["VehicleInsRecId"].ToString());
                    var dd = await SqliteHelper.Storage.GetSingleRecordAsync<DrivingDuration>(x => x.VehicleInsRecID.Equals(vehicleInsRecId));
                    dd.StopDateTime = DateTime.Now;
                    this._deliveryTask.TaskType = BusinessLogic.Enums.CDTaskType.Delivery;
                    await SqliteHelper.Storage.UpdateSingleRecordAsync(this._deliveryTask);
                    await SqliteHelper.Storage.UpdateSingleRecordAsync(dd);
                }
                this.IsStartDelivery = true;
                this.IsStartDriving = false;
                this.IsArrived = false;
            });
        }
        async public override void OnNavigatedTo(object navigationParameter, Windows.UI.Xaml.Navigation.NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            try
            {
                base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
                var dd = await SqliteHelper.Storage.GetSingleRecordAsync<DrivingDuration>(x => x.VehicleInsRecID == _deliveryTask.VehicleInsRecId);
                if (dd != null)
                {
                    this.IsArrived = dd.StopDateTime == DateTime.MinValue;
                    this.IsStartDelivery = !this.IsArrived;
                }
                else
                {
                    this.IsStartDriving = true;
                    this.IsArrived = false;
                    this.IsStartDelivery = false;
                }
            }
            catch (Exception ex)
            {
                AppSettings.Instance.ErrorMessage = ex.Message;
            }
        }

        private CDCustomerDetails customerDetails;
        public CDCustomerDetails CustomerDetails
        {
            get { return customerDetails; }
            set { SetProperty(ref customerDetails, value); }
        }
        public DelegateCommand ArrivedCommand { get; set; }
        public ICommand GetDirectionsCommand { get; set; }
        public DelegateCommand GoToDocumentDeliveryCommand { get; set; }
        public DelegateCommand StartDrivingCommand { get; set; }

        private bool isStartDelivery;
        public bool IsStartDelivery
        {
            get { return isStartDelivery; }
            set { SetProperty(ref isStartDelivery, value); }
        }
        private bool isStartDriving;
        public bool IsStartDriving
        {
            get { return isStartDriving; }
            set { SetProperty(ref isStartDriving, value); }
        }
        private bool isArrived;
        public bool IsArrived
        {
            get { return isArrived; }
            set { SetProperty(ref isArrived, value); }
        }
    }
}
