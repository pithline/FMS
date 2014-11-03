using Eqstra.BusinessLogic;
using Eqstra.BusinessLogic.Base;
using Eqstra.BusinessLogic.Commercial;
using Eqstra.BusinessLogic.Common;
using Eqstra.BusinessLogic.Helpers;
using Eqstra.VehicleInspection.UILogic.Events;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.StoreApps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace Eqstra.VehicleInspection.UILogic.ViewModels
{
    public class TPOIUserControlViewModel:BaseViewModel
    {
        private IEventAggregator _eventAggregator;
        public TPOIUserControlViewModel(IEventAggregator eventAggregator)
            : base(eventAggregator)
        {
            this.Model = new TPOI();
            _eventAggregator = eventAggregator;
        }

        public async override System.Threading.Tasks.Task LoadModelFromDbAsync(long vehicleInsRecID)
        {
         
            this.Model = await SqliteHelper.Storage.GetSingleRecordAsync<TPOI>(x => x.VehicleInsRecID == vehicleInsRecID);
            if (this.Model == null)
            {
                this.Model = new TPOI();
            }
            BaseModel viBaseObject = (TPOI)this.Model;
            viBaseObject.LoadSnapshotsFromDb();
            PropertyHistory.Instance.SetPropertyHistory(viBaseObject);
            viBaseObject.ShouldSave = false;
        }

        private BitmapImage custSignature;

        public BitmapImage CustSignature
        {
            get { return custSignature; }
            set
            {
                if (SetProperty(ref custSignature, value))
                {
                    ((TPOI)this.Model).CRDate = DateTime.Now;
                    _eventAggregator.GetEvent<Eqstra.VehicleInspection.UILogic.Events.SignChangedEvent>().Publish(true);
                }

            }
        }

        private BitmapImage eqstraRepSignature;

        public BitmapImage EqstraRepSignature
        {
            get { return eqstraRepSignature; }
            set
            {
                if (SetProperty(ref eqstraRepSignature, value))
                {
                    ((TPOI)this.Model).EQRDate = DateTime.Now;
                    _eventAggregator.GetEvent<SignChangedEvent>().Publish(true);
                }
            }
        }

    }
}
