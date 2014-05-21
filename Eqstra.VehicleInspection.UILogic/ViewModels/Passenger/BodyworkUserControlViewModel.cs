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
    public class BodyworkUserControlViewModel : BaseViewModel
    {
        public BodyworkUserControlViewModel()
        {
            this.Model = new PBodywork();
        }

        public async override Task UpdateModelAsync(string caseNumber)
        {
            this.Model = await SqliteHelper.Storage.GetSingleRecordAsync<PBodywork>(x => x.CaseNumber == caseNumber);
            if (this.Model == null)
            {
                this.Model = new PBodywork();
            }
            VIBase viBaseObject = (PBodywork)this.Model;
            viBaseObject.LoadSnapshotsFromDb();
        }
    }
}
