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
    public class BodyworkUserControlViewModel : BaseViewModel
    {
        public BodyworkUserControlViewModel(IEventAggregator eventAggregator)
            : base(eventAggregator)
        {
            this.Model = new PBodywork();
        }

        public async override Task LoadModelFromDbAsync(long vehicleInsRecID)
        {
            this.Model = await SqliteHelper.Storage.GetSingleRecordAsync<PBodywork>(x => x.VehicleInsRecID == vehicleInsRecID);
            if (this.Model == null)
            {
                this.Model = new PBodywork();
            }
            BaseModel viBaseObject = (PBodywork)this.Model;
            viBaseObject.LoadSnapshotsFromDb();
            viBaseObject.ShouldSave = false;
            PropertyHistory.Instance.SetPropertyHistory(viBaseObject);
        }
    }
}
