using Eqstra.BusinessLogic.Base;
using Eqstra.BusinessLogic.Commercial;
using Eqstra.BusinessLogic.Common;
using Eqstra.BusinessLogic.Helpers;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.StoreApps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eqstra.VehicleInspection.UILogic.ViewModels
{
   public class CGlassUserControlViewModel : BaseViewModel
    {
       public CGlassUserControlViewModel(IEventAggregator eventAggregator)
           : base(eventAggregator)
       {
           this.Model = new CGlass();
       }

       public async override System.Threading.Tasks.Task LoadModelFromDbAsync(long vehicleInsRecID)
       {
           this.Model = await SqliteHelper.Storage.GetSingleRecordAsync<CGlass>(x => x.VehicleInsRecID == vehicleInsRecID);
           if (this.Model == null)
           {
               this.Model = new CGlass();
           }
           BaseModel viBaseObject = (CGlass)this.Model;
           viBaseObject.LoadSnapshotsFromDb();
           PropertyHistory.Instance.SetPropertyHistory(viBaseObject);
           viBaseObject.ShouldSave = false;
       }
    }
}
