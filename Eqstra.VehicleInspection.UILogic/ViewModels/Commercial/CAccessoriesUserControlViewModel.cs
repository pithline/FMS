using Eqstra.BusinessLogic.Base;
using Eqstra.BusinessLogic.Commercial;
using Eqstra.BusinessLogic.Common;
using Eqstra.BusinessLogic.Helpers;
using Microsoft.Practices.Prism.StoreApps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eqstra.VehicleInspection.UILogic.ViewModels
{
   public class CAccessoriesUserControlViewModel : BaseViewModel
    {
       public CAccessoriesUserControlViewModel()
       {
           this.Model = new CAccessories();
       }

       public async override System.Threading.Tasks.Task LoadModelFromDbAsync(long vehicleInsRecID)
       {
           this.Model = await SqliteHelper.Storage.GetSingleRecordAsync<CAccessories>(x => x.VehicleInsRecID == vehicleInsRecID);
           if (this.Model == null)
           {
               this.Model = new CAccessories();
           }
           BaseModel viBaseObject = (CAccessories)this.Model;
           viBaseObject.LoadSnapshotsFromDb();
           PropertyHistory.Instance.SetPropertyHistory(viBaseObject);
           viBaseObject.ShouldSave = false;
       }
    }
}
