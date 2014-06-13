using Eqstra.BusinessLogic.Base;
using Eqstra.BusinessLogic.Common;
using Eqstra.BusinessLogic.Helpers;
using Eqstra.BusinessLogic.Passenger;
using Microsoft.Practices.Prism.StoreApps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eqstra.VehicleInspection.UILogic.ViewModels
{
    public class GlassUserControlViewModel : BaseViewModel
    {
        public GlassUserControlViewModel()
        {
            this.Model = new PGlass();
            ((BaseModel)this.Model).ShouldSave = false;
        }
        public async override System.Threading.Tasks.Task LoadModelFromDbAsync(string caseNumber)
        {
            this.Model = await SqliteHelper.Storage.GetSingleRecordAsync<PGlass>(x => x.CaseNumber == caseNumber);
            if (this.Model == null)
            {
                this.Model = new PGlass();
            }
            BaseModel viBaseObject = (PGlass)this.Model;
            viBaseObject.LoadSnapshotsFromDb(); 

            viBaseObject.ShouldSave = false;
            PropertyHistory.Instance.SetPropertyHistory(viBaseObject);
        }
    }
}
