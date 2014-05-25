using Eqstra.BusinessLogic.Base;
using Eqstra.BusinessLogic.Commercial;
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

       public async override System.Threading.Tasks.Task UpdateModelAsync(string caseNumber)
       {
           this.Model = await SqliteHelper.Storage.GetSingleRecordAsync<CChassisBody>(x => x.CaseNumber == caseNumber);
           if (this.Model == null)
           {
               this.Model = new CChassisBody();
           }
           VIBase viBaseObject = (CChassisBody)this.Model;
           viBaseObject.LoadSnapshotsFromDb();
       }
    }
}
