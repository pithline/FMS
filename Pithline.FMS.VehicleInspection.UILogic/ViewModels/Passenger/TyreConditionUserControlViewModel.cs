﻿using Eqstra.BusinessLogic.Base;
using Eqstra.BusinessLogic.Common;
using Eqstra.BusinessLogic.Helpers;
using Eqstra.BusinessLogic.Passenger;
using Microsoft.Practices.Prism.PubSubEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eqstra.VehicleInspection.UILogic.ViewModels
{
    public class TyreConditionUserControlViewModel : BaseViewModel
    {
        public TyreConditionUserControlViewModel(IEventAggregator eventAggregator):base(eventAggregator)
        {
            this.Model = new PTyreCondition();
        }

        public async override System.Threading.Tasks.Task LoadModelFromDbAsync(long vehicleInsRecID)
        {
            this.Model = await SqliteHelper.Storage.GetSingleRecordAsync<PTyreCondition>(x => x.VehicleInsRecID == vehicleInsRecID);
            if (this.Model == null)
            {
                this.Model = new PTyreCondition();
            }
            BaseModel viBaseObject = (PTyreCondition)this.Model;
            viBaseObject.VehicleInsRecID = vehicleInsRecID;
            viBaseObject.LoadSnapshotsFromDb();
            PropertyHistory.Instance.SetPropertyHistory(viBaseObject);

            viBaseObject.ShouldSave = false;

        }

    }
}
