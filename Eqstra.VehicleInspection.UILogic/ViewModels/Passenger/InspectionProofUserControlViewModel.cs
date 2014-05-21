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
    public class InspectionProofUserControlViewModel : BaseViewModel
    {
        public InspectionProofUserControlViewModel()
        {
            this.Model = new PInspectionProof();
        }

        public async override System.Threading.Tasks.Task UpdateModelAsync(string caseNumber)
        {
            this.Model = await SqliteHelper.Storage.GetSingleRecordAsync<PInspectionProof>(x => x.CaseNumber == caseNumber);
            if (this.Model == null)
            {
                this.Model = new PInspectionProof();
            }
            VIBase viBaseObject = (PInspectionProof)this.Model;
            viBaseObject.LoadSnapshotsFromDb();
        }
    }
}
