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
  public class CMechanicalCondUserControlViewModel : BaseViewModel
    {
      public CMechanicalCondUserControlViewModel()
        {
            this.Model = new CMechanicalCond();
        }

      public async override System.Threading.Tasks.Task UpdateModelAsync(string caseNumber)
      {
          this.Model = await SqliteHelper.Storage.GetSingleRecordAsync<CMechanicalCond>(x => x.CaseNumber == caseNumber);
          if (this.Model == null)
          {
              this.Model = new CMechanicalCond();
          }
          VIBase viBaseObject = (CMechanicalCond)this.Model;
          viBaseObject.LoadSnapshotsFromDb();
      }
    }
}
