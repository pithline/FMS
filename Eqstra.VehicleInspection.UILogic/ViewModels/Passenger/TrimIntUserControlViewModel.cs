using Eqstra.BusinessLogic;
using Eqstra.BusinessLogic.Base;
using Eqstra.BusinessLogic.Common;
using Eqstra.BusinessLogic.Helpers;
using Eqstra.BusinessLogic.Passenger;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.StoreApps;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Capture;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
namespace Eqstra.VehicleInspection.UILogic.ViewModels
{
    public class TrimIntUserControlViewModel : BaseViewModel
    {

        public TrimIntUserControlViewModel(IEventAggregator eventAggregator)
            : base(eventAggregator)
        {
            this.Model = new PTrimInterior();
        }
        public async override System.Threading.Tasks.Task LoadModelFromDbAsync(long vehicleInsRecID)
        {
            this.Model = await SqliteHelper.Storage.GetSingleRecordAsync<PTrimInterior>(x => x.VehicleInsRecID == vehicleInsRecID);            
            if (this.Model == null)
            {
                this.Model = new PTrimInterior();
            }
            BaseModel viBaseObject = (PTrimInterior)this.Model;
            viBaseObject.ShouldSave = false;
            viBaseObject.LoadSnapshotsFromDb();
            PropertyHistory.Instance.SetPropertyHistory(viBaseObject);
        }
    }
}
