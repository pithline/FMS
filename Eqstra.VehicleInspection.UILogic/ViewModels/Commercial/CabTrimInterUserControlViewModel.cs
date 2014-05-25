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
    public class CabTrimInterUserControlViewModel : BaseViewModel
    {
        public CabTrimInterUserControlViewModel()
       {
           this.Model = new CCabTrimInter();
       }

        public async override System.Threading.Tasks.Task UpdateModelAsync(string caseNumber)
        {
            this.Model = await SqliteHelper.Storage.GetSingleRecordAsync<CCabTrimInter>(x => x.CaseNumber == caseNumber);
            if (this.Model == null)
            {
                this.Model = new CCabTrimInter();
            }
            VIBase viBaseObject = (CCabTrimInter)this.Model;
            viBaseObject.LoadSnapshotsFromDb();
        }
    }
}
