using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eqstra.BusinessLogic.Passenger;
using Eqstra.BusinessLogic.Helpers;
using Eqstra.BusinessLogic.Base;
using Eqstra.BusinessLogic.Common;

namespace Eqstra.VehicleInspection.UILogic.ViewModels
{
    public class AccessoriesUserControlViewModel : BaseViewModel
    {
        public AccessoriesUserControlViewModel()
        {
            this.Model =new PAccessories();
        }

        public async override System.Threading.Tasks.Task LoadModelFromDbAsync(string caseNumber)
        {
            this.Model = await SqliteHelper.Storage.GetSingleRecordAsync<PAccessories>(x => x.CaseNumber == caseNumber);
            if (this.Model == null)
            {
                this.Model = new PAccessories();
            }
            BaseModel viBaseObject = (PAccessories)this.Model;
            viBaseObject.LoadSnapshotsFromDb();
            PropertyHistory.Instance.SetPropertyHistory(viBaseObject);
            viBaseObject.ShouldSave = false;
        }

    }
}
