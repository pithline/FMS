using Eqstra.BusinessLogic;
using Eqstra.BusinessLogic.Base;
using Eqstra.BusinessLogic.Commercial;
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
           string CaseNumber = (string)ApplicationData.Current.LocalSettings.Values["CaseNumber"];
           UpdateModelAsync(CaseNumber);
           this.GoToImageMarkupPageCommand = new DelegateCommand(() =>
           {
               _navigationService.Navigate("ImageMarkup", this.Model);
           });
       }

       public DelegateCommand GoToImageMarkupPageCommand { get; set; }

       public async override System.Threading.Tasks.Task UpdateModelAsync(string caseNumber)
       {
           this.Model = await SqliteHelper.Storage.GetSingleRecordAsync<CVehicleDetails>(x => x.CaseNumber == caseNumber);
           if (this.Model == null)
           {
               this.Model = new CVehicleDetails();
           }
           VIBase viBaseObject = (CVehicleDetails)this.Model;
           viBaseObject.LoadSnapshotsFromDb();
       }

       async public override System.Threading.Tasks.Task TakePictureAsync(ImageCapture param)
       {
           await base.TakePictureAsync(param);
           string caseNumber = (string)ApplicationData.Current.LocalSettings.Values["CaseNumber"];
           if (caseNumber != null)
           {
               var viobj = await (this.Model as VIBase).GetDataAsync(caseNumber);
               if (viobj != null)
               {
                   var successFlag = await SqliteHelper.Storage.UpdateSingleRecordAsync(this.Model);
               }
               else
               {
                   ((VIBase)this.Model).CaseNumber = caseNumber;
                   var successFlag = await SqliteHelper.Storage.InsertSingleRecordAsync(this.Model);
               }
           }
       }
    }
}
