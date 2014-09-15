using Eqstra.BusinessLogic.Base;
using Eqstra.BusinessLogic.Common;
using Eqstra.BusinessLogic.Helpers;
using Eqstra.BusinessLogic.Passenger;
using Eqstra.VehicleInspection.UILogic.Events;
using Microsoft.Practices.Prism.PubSubEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace Eqstra.VehicleInspection.UILogic.ViewModels
{
    public class InspectionProofUserControlViewModel : BaseViewModel
    {
        private IEventAggregator _eventAggregator;
        public InspectionProofUserControlViewModel(IEventAggregator eventAggregator)
            : base(eventAggregator)
        {
            this.Model = new PInspectionProof();
            _eventAggregator = eventAggregator;
        }

        public async override System.Threading.Tasks.Task LoadModelFromDbAsync(long vehicleInsRecID)
        {
            this.Model = await SqliteHelper.Storage.GetSingleRecordAsync<PInspectionProof>(x => x.VehicleInsRecID == vehicleInsRecID);
            if (this.Model == null)
            {
                this.Model = new PInspectionProof();
            }
            BaseModel viBaseObject = (PInspectionProof)this.Model;
            viBaseObject.LoadSnapshotsFromDb();

            viBaseObject.ShouldSave = false;
            PropertyHistory.Instance.SetPropertyHistory(viBaseObject);
       
        }

        private BitmapImage custSignature;
        
        public BitmapImage CustSignature
        {
            get { return custSignature; }
            set
            {
                if (SetProperty(ref custSignature, value))
                {
                    ((PInspectionProof)this.Model).CRDate = DateTime.Now;
                    _eventAggregator.GetEvent<SignChangedEvent>().Publish(true);
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
                   ((PInspectionProof)this.Model).EQRDate = DateTime.Now;
                   _eventAggregator.GetEvent<SignChangedEvent>().Publish(true);
                }
            }
        }


    }
}
