using Eqstra.BusinessLogic.Base;
using Eqstra.BusinessLogic.Helpers;
using Eqstra.BusinessLogic.Passenger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eqstra.VehicleInspection.UILogic.ViewModels
{
   public class MechanicalCondUserControlViewModel : BaseViewModel
    {
       public MechanicalCondUserControlViewModel()
       {
           this.Model = new PMechanicalCond();
       }

       public async override System.Threading.Tasks.Task UpdateModelAsync(string caseNumber)
       {
           this.Model = await SqliteHelper.Storage.GetSingleRecordAsync<PMechanicalCond>(x => x.CaseNumber == caseNumber);
           if (this.Model == null)
           {
               this.Model = new PMechanicalCond();
           }
           VIBase viBaseObject = (PMechanicalCond)this.Model;
           viBaseObject.LoadSnapshotsFromDb();
       }
    }
}
