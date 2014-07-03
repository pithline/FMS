using Eqstra.BusinessLogic;
using Eqstra.BusinessLogic.Base;
using Eqstra.BusinessLogic.Commercial;
using Eqstra.BusinessLogic.Common;
using Eqstra.BusinessLogic.Helpers;
using Eqstra.BusinessLogic.Passenger;
using Eqstra.VehicleInspection.UILogic.Events;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;


namespace Eqstra.VehicleInspection.UILogic.ViewModels
{
   public class CommercialVehicleDetailsUserControlViewModel : BaseViewModel
    {
     
       INavigationService _navigationService;
       public CommercialVehicleDetailsUserControlViewModel(INavigationService navigationService,IEventAggregator eventAggregator):base(eventAggregator)
       {
           _navigationService = navigationService;
          
           long vehicleInsRecID = long.Parse(ApplicationData.Current.LocalSettings.Values["VehicleInsRecID"].ToString());
           eventAggregator.GetEvent<VehicleFetchedEvent>().Subscribe(async b =>
           {
               await LoadCommericalVehicleAsync(vehicleInsRecID);
           }, ThreadOption.UIThread);
           LoadModelFromDbAsync(vehicleInsRecID);
           LoadCommericalVehicleAsync(vehicleInsRecID);
           this.GoToImageMarkupPageCommand = new DelegateCommand(() =>
           {
               _navigationService.Navigate("ImageMarkup", this.Model);
           });
       }

       public DelegateCommand GoToImageMarkupPageCommand { get; set; }

       private async System.Threading.Tasks.Task LoadCommericalVehicleAsync(long vRecId)
       {
           this.CommercialVehicle = await SqliteHelper.Storage.GetSingleRecordAsync<CommercialVehicle>(x => x.VehicleInsRecID == vRecId);
           if (this.CommercialVehicle == null)
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
           this.Model = await SqliteHelper.Storage.GetSingleRecordAsync<CVehicleDetails>(x => x.VehicleInsRecID == vehicleInsRecID);
           if (this.Model == null)
           {
               AppSettings.Instance.IsSyncingVehDetails = 1;              
           }

           else
           {
               AppSettings.Instance.IsSyncingVehDetails = 0;              
               BaseModel viBaseObject = (CVehicleDetails)this.Model;
               viBaseObject.LoadSnapshotsFromDb();
               PropertyHistory.Instance.SetPropertyHistory(viBaseObject);
               viBaseObject.ShouldSave = false; 
           }
       }

       async public override System.Threading.Tasks.Task TakePictureAsync(ImageCapture param)
       {
           await base.TakePictureAsync(param);
           long vehicleInsRecID = long.Parse(ApplicationData.Current.LocalSettings.Values["VehicleInsRecID"].ToString());
           if (vehicleInsRecID != null)
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

       private CommercialVehicle commercialVehicle;

       public CommercialVehicle CommercialVehicle
       {
           get { return commercialVehicle; }
           set { SetProperty(ref commercialVehicle, value); }
       }

    }
}
