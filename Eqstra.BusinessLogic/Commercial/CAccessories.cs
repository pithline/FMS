using Eqstra.BusinessLogic.Base;
using Eqstra.BusinessLogic.Helpers;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eqstra.BusinessLogic.Commercial
{
    public class CAccessories : VIBase
    {
        public CAccessories()
        {
            this.ServiceBlockImgList = new ObservableCollection<ImageCapture>();
            this.ToolsImgList = new ObservableCollection<ImageCapture>();
            this.JackImgList = new ObservableCollection<ImageCapture>();
            this.BullBarImgList = new ObservableCollection<ImageCapture>();
            this.TrackingDeviceImgList = new ObservableCollection<ImageCapture>();
            this.EngineProtectionUnitImgList = new ObservableCollection<ImageCapture>();
            this.DecalSignWritingImgList = new ObservableCollection<ImageCapture>();
            this.ReflectiveTapeImgList = new ObservableCollection<ImageCapture>();
        }
        private string serviceBlockComment;

        public string ServiceBlockComment
        {
            get { return serviceBlockComment; }

            set { SetProperty(ref  serviceBlockComment, value); }
        }
        private bool isServiceBlock;

        public bool IsServiceBlock
        {
            get { return isServiceBlock; }

            set { SetProperty(ref  isServiceBlock, value); }
        }
        private ObservableCollection<ImageCapture> serviceBlockImgList;

        [Ignore]
        public ObservableCollection<ImageCapture> ServiceBlockImgList
        {
            get { return serviceBlockImgList; }

            set { SetProperty(ref  serviceBlockImgList, value); }
        }

        private string toolsComment;

        public string ToolsComment
        {
            get { return toolsComment; }

            set { SetProperty(ref  toolsComment, value); }
        }
        private bool isTools;

        public bool IsTools
        {
            get { return isTools; }

            set { SetProperty(ref  isTools, value); }
        }
        private ObservableCollection<ImageCapture> toolsImgList;

        [Ignore]
        public ObservableCollection<ImageCapture> ToolsImgList
        {
            get { return toolsImgList; }

            set { SetProperty(ref  toolsImgList, value); }
        }

    
        private string jackComment;

        public string JackComment
        {
            get { return jackComment; }

            set { SetProperty(ref  jackComment, value); }
        }
        private bool isJack;

        public bool IsJack
        {
            get { return isJack; }

            set { SetProperty(ref  isJack, value); }
        }
        private ObservableCollection<ImageCapture> jackImgList;

        [Ignore]
        public ObservableCollection<ImageCapture> JackImgList
        {
            get { return jackImgList; }

            set { SetProperty(ref  jackImgList, value); }
        }

        private string bullBarComment;

        public string BullBarComment
        {
            get { return bullBarComment; }

            set { SetProperty(ref  bullBarComment, value); }
        }
        private bool isBullBar;

        public bool IsBullBar
        {
            get { return isBullBar; }

            set { SetProperty(ref  isBullBar, value); }
        }
        private ObservableCollection<ImageCapture> bullBarImgList;

        [Ignore]
        public ObservableCollection<ImageCapture> BullBarImgList
        {
            get { return bullBarImgList; }

            set { SetProperty(ref  bullBarImgList, value); }
        }

        private string trackingDeviceComment;

        public string TrackingDeviceComment
        {
            get { return trackingDeviceComment; }

            set { SetProperty(ref  trackingDeviceComment, value); }
        }
        private bool isTrackingDevice;

        public bool IsTrackingDevice
        {
            get { return isTrackingDevice; }

            set { SetProperty(ref  isTrackingDevice, value); }
        }
        private ObservableCollection<ImageCapture> trackingDeviceImgList;

        [Ignore]
        public ObservableCollection<ImageCapture> TrackingDeviceImgList
        {
            get { return trackingDeviceImgList; }

            set { SetProperty(ref  trackingDeviceImgList, value); }
        }

        private string engineProtectionUnitComment;
        public string EngineProtectionUnitComment
        {
            get { return engineProtectionUnitComment; }

            set { SetProperty(ref  engineProtectionUnitComment, value); }
        }
        private bool isEngineProtectionUnit;

        public bool IsEngineProtectionUnit
        {
            get { return isEngineProtectionUnit; }

            set { SetProperty(ref  isEngineProtectionUnit, value); }
        }
        private ObservableCollection<ImageCapture> engineProtectionUnitImgList;

        [Ignore]
        public ObservableCollection<ImageCapture> EngineProtectionUnitImgList 
        {
            get { return engineProtectionUnitImgList; }

            set { SetProperty(ref  engineProtectionUnitImgList, value); }
        }
      
        private string decalSignWritingComment;

        public string DecalSignWritingComment
        {
            get { return decalSignWritingComment; }

            set { SetProperty(ref  decalSignWritingComment, value); }
        }
        private bool isDecalSignWriting;

        public bool IsDecalSignWriting
        {
            get { return isDecalSignWriting; }

            set { SetProperty(ref  isDecalSignWriting, value); }
        }
        private ObservableCollection<ImageCapture> decalSignWritingImgList;

        [Ignore]
        public ObservableCollection<ImageCapture> DecalSignWritingImgList
        {
            get { return decalSignWritingImgList; }

            set { SetProperty(ref  decalSignWritingImgList, value); }
        }
     
        private string reflectiveTapeComment;

        public string ReflectiveTapeComment
        {
            get { return reflectiveTapeComment; }

            set { SetProperty(ref  reflectiveTapeComment, value); }
        }
        private bool isReflectiveTape;

        public bool IsReflectiveTape
        {
            get { return isReflectiveTape; }

            set { SetProperty(ref  isReflectiveTape, value); }
        }
        private ObservableCollection<ImageCapture> reflectiveTapeImgList;

        [Ignore]
        public ObservableCollection<ImageCapture> ReflectiveTapeImgList
        {
            get { return reflectiveTapeImgList; }

            set { SetProperty(ref  reflectiveTapeImgList, value); }
        }

        public string serviceBlockImgPathList;
        public string ServiceBlockImgPathList
        {
            get { return string.Join("~", ServiceBlockImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref serviceBlockImgPathList, value); }
        }

        public string toolsImgPathList;
        public string ToolsImgPathList
        {
            get { return string.Join("~", ToolsImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref toolsImgPathList, value); }
        }

        public string jackImgPathList;
        public string JackImgPathList
        {
            get { return string.Join("~", JackImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref jackImgPathList, value); }
        }

        public string bullBarImgPathList;
        public string BullBarImgPathList
        {
            get { return string.Join("~", BullBarImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref bullBarImgPathList, value); }
        }

        public string trackingDeviceImgPathList;
        public string TrackingDeviceImgPathList
        {
            get { return string.Join("~", TrackingDeviceImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref trackingDeviceImgPathList, value); }
        }

        public string engineProtectionUnitImgPathList;
        public string EngineProtectionUnitImgPathList
        {
            get { return string.Join("~", EngineProtectionUnitImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref engineProtectionUnitImgPathList, value); }
        }

        public string decalSignWritingImgPathList;
        public string DecalSignWritingImgPathList
        {
            get { return string.Join("~", DecalSignWritingImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref decalSignWritingImgPathList, value); }
        }

        public string reflectiveTapeImgPathList;
        public string ReflectiveTapeImgPathList
        {
            get { return string.Join("~", ReflectiveTapeImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref reflectiveTapeImgPathList, value); }
        }

        private bool hasServiceBlock;
        public bool HasServiceBlock
        {
            get { return hasServiceBlock; }

            set { SetProperty(ref  hasServiceBlock, value); }
        }

        private bool hasTools;
        public bool HasTools
        {
            get { return hasTools; }

            set { SetProperty(ref  hasTools, value); }
        }

        private bool hasJack;
        public bool HasJack
        {
            get { return hasJack; }

            set { SetProperty(ref  hasJack, value); }
        }

        private bool hasBullBar;
        public bool HasBullBar
        {
            get { return hasBullBar; }

            set { SetProperty(ref  hasBullBar, value); }
        }

        private bool hasTrackingDevice;
        public bool HasTrackingDevice
        {
            get { return hasTrackingDevice; }

            set { SetProperty(ref  hasTrackingDevice, value); }
        }

        private bool hasEngineProtectionUnit;

        public bool HasEngineProtectionUnit
        {
            get { return hasEngineProtectionUnit; }

            set { SetProperty(ref  hasEngineProtectionUnit, value); }
        }

        private bool hasDecalSignWriting;
        public bool HasDecalSignWriting
        {
            get { return hasDecalSignWriting; }

            set { SetProperty(ref  hasDecalSignWriting, value); }
        }

        private bool hasReflectiveTape;
        public bool HasReflectiveTape
        {
            get { return hasReflectiveTape; }

            set { SetProperty(ref  hasReflectiveTape, value); }
        }
        public async override Task<VIBase> GetDataAsync(string caseNumber)
        {
            return await SqliteHelper.Storage.GetSingleRecordAsync<CAccessories>(x => x.CaseNumber == caseNumber);
        } 
    }
}
