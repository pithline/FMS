using Microsoft.Practices.Prism.StoreApps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eqstra.BusinessLogic.Passenger;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using Eqstra.BusinessLogic.Helpers;
using Eqstra.BusinessLogic.Base;
using Windows.Storage;
using Eqstra.BusinessLogic;
using Eqstra.BusinessLogic.Common;
using Microsoft.Practices.Prism.PubSubEvents;
using Eqstra.VehicleInspection.UILogic.Events;

namespace Eqstra.VehicleInspection.UILogic.ViewModels
{
    public class VehicleDetailsUserControlViewModel : BaseViewModel
    {
        INavigationService _navigationService;
        public VehicleDetailsUserControlViewModel(INavigationService navigationService, IEventAggregator eventAggregator)
            : base(eventAggregator)
        {
            _navigationService = navigationService;
            long vehicleInsRecID = long.Parse(ApplicationData.Current.LocalSettings.Values["vehicleInsRecID"].ToString());
            eventAggregator.GetEvent<VehicleFetchedEvent>().Subscribe(async b =>
            {
                await LoadPassengerVehicleAsync();
            }, ThreadOption.UIThread);
            LoadModelFromDbAsync(vehicleInsRecID);
            LoadPassengerVehicleAsync();
            this.GoToImageMarkupPageCommand = new DelegateCommand(() =>
            {
                _navigationService.Navigate("ImageMarkup", this.Model);
            });
        }
        public override void OnNavigatedTo(object navigationParameter, Windows.UI.Xaml.Navigation.NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
        }

        public DelegateCommand GoToImageMarkupPageCommand { get; set; }


        private async System.Threading.Tasks.Task LoadPassengerVehicleAsync()
        {
            var recId = long.Parse(ApplicationData.Current.LocalSettings.Values["vehicleInsRecID"].ToString());
            this.PassengerVehicle = await SqliteHelper.Storage.GetSingleRecordAsync<PassengerVehicle>(x => x.VehicleInsRecID.Equals(recId) );
            if (this.PassengerVehicle == null)
            {
                AppSettings.Instance.IsSyncingVehDetails = 1;
            }
            else
            {
                AppSettings.Instance.IsSyncingVehDetails = 0;
            }
        }

        public async override System.Threading.Tasks.Task LoadModelFromDbAsync(long vehicleInsRecID)
        {
            this.Model = await SqliteHelper.Storage.GetSingleRecordAsync<PVehicleDetails>(x => x.VehicleInsRecID.Equals(vehicleInsRecID));
            if (this.Model == null)
            {
                this.Model = new PVehicleDetails();
            }

            else
            {
                BaseModel viBaseObject = (PVehicleDetails)this.Model;
                viBaseObject.LoadSnapshotsFromDb();
                viBaseObject.ShouldSave = false;
                PropertyHistory.Instance.SetPropertyHistory(viBaseObject);
            }
        }

        async public override System.Threading.Tasks.Task TakePictureAsync(ImageCapture param)
        {
            await base.TakePictureAsync(param);
            long vehicleInsRecID = long.Parse(ApplicationData.Current.LocalSettings.Values["VehicleInsRecID"].ToString());
            if (vehicleInsRecID != default(long))
            {
                var viobj = await (this.Model as BaseModel).GetDataAsync(vehicleInsRecID);
                if (viobj != null)
                {
                    var successFlag = await SqliteHelper.Storage.UpdateSingleRecordAsync(this.Model);
                }
                else
                {
                    ((BaseModel)this.Model).VehicleInsRecID = vehicleInsRecID;
                    var successFlag = await SqliteHelper.Storage.InsertSingleRecordAsync(this.Model);
                }
            }
        }

        private PassengerVehicle passengerVehicle;

        public PassengerVehicle PassengerVehicle
        {
            get { return passengerVehicle; }
            set { SetProperty(ref passengerVehicle, value); }
        }

    }
}
