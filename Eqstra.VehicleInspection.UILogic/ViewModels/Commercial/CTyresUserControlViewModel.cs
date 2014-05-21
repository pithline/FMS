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
    public class CTyresUserControlViewModel:BaseViewModel
    {
        public CTyresUserControlViewModel()
        {
            this.Model = new CTyres();
        }

        public async override System.Threading.Tasks.Task UpdateModelAsync(string caseNumber)
        {
            this.Model = await SqliteHelper.Storage.GetSingleRecordAsync<CTyres>(x => x.CaseNumber == caseNumber);
            if (this.Model == null)
            {
                this.Model = new CTyres();
            }
            VIBase viBaseObject = (CTyres)this.Model;
            viBaseObject.LoadSnapshotsFromDb();
        }
    }
}
