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
    public class CPOIUserControlViewModel:BaseViewModel
    {
        public CPOIUserControlViewModel()
        {
            this.Model = new CPOI();
        }

        public async override System.Threading.Tasks.Task LoadModelFromDbAsync(string caseNumber)
        {
            this.Model = await SqliteHelper.Storage.GetSingleRecordAsync<CPOI>(x => x.CaseNumber == caseNumber);
            if (this.Model == null)
            {
                this.Model = new CPOI();
            }
            BaseModel viBaseObject = (CPOI)this.Model;
            viBaseObject.LoadSnapshotsFromDb();
            PropertyHistory.Instance.SetPropertyHistory(viBaseObject);
            viBaseObject.ShouldSave = false;
        }
    }
}
