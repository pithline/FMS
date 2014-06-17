using Eqstra.BusinessLogic;
using Eqstra.BusinessLogic.Base;
using Eqstra.BusinessLogic.Commercial;
using Eqstra.BusinessLogic.Common;
using Eqstra.BusinessLogic.Helpers;
using Eqstra.BusinessLogic.Passenger;
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
       public CommercialVehicleDetailsUserControlViewModel(INavigationService navigationService)
       {
           _navigationService = navigationService;
           this.Model = new CVehicleDetails();
           long vehicleInsRecID = long.Parse(ApplicationData.Current.LocalSettings.Values["VehicleInsRecID"].ToString());
           LoadModelFromDbAsync(vehicleInsRecID);
           this.GoToImageMarkupPageCommand = new DelegateCommand(() =>
           {
               _navigationService.Navigate("ImageMarkup", this.Model);
           });
       }

       public DelegateCommand GoToImageMarkupPageCommand { get; set; }

       public async override System.Threading.Tasks.Task LoadModelFromDbAsync(long vehicleInsRecID)
       {
           this.Model = await SqliteHelper.Storage.GetSingleRecordAsync<CVehicleDetails>(x => x.VehicleInsRecID == vehicleInsRecID);
           if (this.Model == null)
           {
               this.Model = new CVehicleDetails();
           }
           BaseModel viBaseObject = (CVehicleDetails)this.Model;
           viBaseObject.LoadSnapshotsFromDb();
           PropertyHistory.Instance.SetPropertyHistory(viBaseObject);
           viBaseObject.ShouldSave = false;
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
    }
}
