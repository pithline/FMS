using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eqstra.BusinessLogic.Passenger;
using Eqstra.BusinessLogic.Helpers;
using Eqstra.BusinessLogic.Base;
using Eqstra.BusinessLogic.Common;
using Microsoft.Practices.Prism.PubSubEvents;

namespace Eqstra.VehicleInspection.UILogic.ViewModels
{
    public class AccessoriesUserControlViewModel : BaseViewModel
    {
        public AccessoriesUserControlViewModel(IEventAggregator eventAggregator)
            : base(eventAggregator)
        {
            this.Model =new PAccessories();
        }

        public async override System.Threading.Tasks.Task LoadModelFromDbAsync(long vehicleInsRecID)
        {
            this.Model = await SqliteHelper.Storage.GetSingleRecordAsync<PAccessories>(x => x.VehicleInsRecID == vehicleInsRecID);
            if (this.Model == null)
            {
                this.Model = new PAccessories();
            }
            BaseModel viBaseObject = (PAccessories)this.Model;
            viBaseObject.VehicleInsRecID = vehicleInsRecID;
            viBaseObject.LoadSnapshotsFromDb();
            PropertyHistory.Instance.SetPropertyHistory(viBaseObject);
            viBaseObject.ShouldSave = false;
        }

    }
}
