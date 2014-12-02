using Eqstra.BusinessLogic.Base;
using Eqstra.BusinessLogic.Helpers;
using Microsoft.Practices.Prism.StoreApps;
using SQLite;
using System.Collections.ObjectModel;

namespace Eqstra.BusinessLogic.TI
{
    public class MaintenanceRepair : BaseModel
    {
        private long repairid;
        [PrimaryKey]
        public long Repairid
        {
            get { return repairid; }
            set { SetProperty(ref repairid, value); }
        }

        private long vehicleInsRecID;
        public new long VehicleInsRecID
        {
            get { return vehicleInsRecID; }
            set { SetProperty(ref vehicleInsRecID, value); }
        }

        private long caseServiceRecId;

        public long CaseServiceRecId
        {
            get { return caseServiceRecId; }
            set { SetProperty(ref caseServiceRecId, value); }
        }


        private string majorComponent;

        public string MajorComponent
        {
            get { return majorComponent; }
            set { SetProperty(ref majorComponent, value); }
        }


        private string subComponent;

        public string SubComponent
        {
            get { return subComponent; }
            set { SetProperty(ref subComponent, value); }
        }

        private string cause;
        public string Cause
        {
            get { return cause; }
            set { SetProperty(ref cause, value); }
        }

        private string action;

        public string Action
        {
            get { return action; }
            set { SetProperty(ref action, value); }
        }

        private ObservableCollection<ImageCapture> majorComponentImgList;
        [Ignore]
        public ObservableCollection<ImageCapture> MajorComponentImgList
        {
            get { return majorComponentImgList; }
            set { SetProperty(ref majorComponentImgList, value); }
        }

        private ObservableCollection<ImageCapture> subComponentImgList;
        [Ignore]
        public ObservableCollection<ImageCapture> SubComponentImgList
        {
            get { return subComponentImgList; }
            set { SetProperty(ref subComponentImgList, value); }
        }

        public async override System.Threading.Tasks.Task<BaseModel> GetDataAsync(long vehicleInsRecID)
        {
            return await SqliteHelper.Storage.GetSingleRecordAsync<MaintenanceRepair>(x => x.VehicleInsRecID == vehicleInsRecID);
        }
    }
}
