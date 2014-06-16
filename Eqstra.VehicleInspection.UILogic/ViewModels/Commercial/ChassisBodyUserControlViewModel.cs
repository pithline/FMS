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
   public class ChassisBodyUserControlViewModel : BaseViewModel
    {
       public ChassisBodyUserControlViewModel()
        {
            this.Model = new CChassisBody();
        }

       public async override System.Threading.Tasks.Task LoadModelFromDbAsync(long vehicleInsRecID)
       {
           this.Model = await SqliteHelper.Storage.GetSingleRecordAsync<CChassisBody>(x => x.VehicleInsRecID == vehicleInsRecID);
           if (this.Model == null)
           {
               this.Model = new CChassisBody();
           }
           BaseModel viBaseObject = (CChassisBody)this.Model;
           viBaseObject.LoadSnapshotsFromDb();
           PropertyHistory.Instance.SetPropertyHistory(viBaseObject);
           viBaseObject.ShouldSave = false;
       }
    }
}
