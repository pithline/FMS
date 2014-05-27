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
        private bool isServiceBlockDmg;

        public bool IsServiceBlockDmg
        {
            get { return isServiceBlockDmg; }

            set { SetProperty(ref  isServiceBlockDmg, value); }
        }
        private ObservableCollection<ImageCapture> serviceBlockImgList;

        [Ignore, DamageSnapshotRequired("Service block snapshot(s) required", "IsServiceBlockDmg")]
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
        private bool isToolsDmg;

        public bool IsToolsDmg
        {
            get { return isToolsDmg; }

            set { SetProperty(ref  isToolsDmg, value); }
        }
        private ObservableCollection<ImageCapture> toolsImgList;

        [Ignore, DamageSnapshotRequired("Tools snapshot(s) required", "IsToolsDmg")]
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
        private bool isJackDmg;

        public bool IsJackDmg
        {
            get { return isJackDmg; }

            set { SetProperty(ref  isJackDmg, value); }
        }
        private ObservableCollection<ImageCapture> jackImgList;

        [Ignore, DamageSnapshotRequired("Jack snapshot(s) required", "IsJackDmg")]
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
        private bool isBullBarDmg;

        public bool IsBullBarDmg
        {
            get { return isBullBarDmg; }

            set { SetProperty(ref  isBullBarDmg, value); }
        }
        private ObservableCollection<ImageCapture> bullBarImgList;

        [Ignore, DamageSnapshotRequired("BullBar snapshot(s) required", "IsBullBarDmg")]
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
        private bool isTrackingDeviceDmg;

        public bool IsTrackingDeviceDmg
        {
            get { return isTrackingDeviceDmg; }

            set { SetProperty(ref  isTrackingDeviceDmg, value); }
        }
        private ObservableCollection<ImageCapture> trackingDeviceImgList;

        [Ignore, DamageSnapshotRequired("Tracking device snapshot(s) required", "IsTrackingDeviceDmg")]
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
        private bool isEngineProtectionUnitDmg;

        public bool IsEngineProtectionUnitDmg
        {
            get { return isEngineProtectionUnitDmg; }

            set { SetProperty(ref  isEngineProtectionUnitDmg, value); }
        }
        private ObservableCollection<ImageCapture> engineProtectionUnitImgList;

        [Ignore, DamageSnapshotRequired("Engine protection unit snapshot(s) required", "IsEngineProtectionUnitDmg")]
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
        private bool isDecalSignWritingDmg;

        public bool IsDecalSignWritingDmg
        {
            get { return isDecalSignWritingDmg; }

            set { SetProperty(ref  isDecalSignWritingDmg, value); }
        }
        private ObservableCollection<ImageCapture> decalSignWritingImgList;

        [Ignore, DamageSnapshotRequired("Decal sign writing snapshot(s) required", "IsDecalSignWritingDmg")]
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
        private bool isReflectiveTapeDmg;

        public bool IsReflectiveTapeDmg
        {
            get { return isReflectiveTapeDmg; }

            set { SetProperty(ref  isReflectiveTapeDmg, value); }
        }
        private ObservableCollection<ImageCapture> reflectiveTapeImgList;

        [Ignore, DamageSnapshotRequired("Reflective tape snapshot(s) required", "IsReflectiveTapeDmg")]
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

        private bool hasServiceBlockDmg;
        public bool HasServiceBlockDmg
        {
            get { return hasServiceBlockDmg; }

            set { SetProperty(ref  hasServiceBlockDmg, value); }
        }

        private bool hasToolsDmg;
        public bool HasToolsDmg
        {
            get { return hasToolsDmg; }

            set { SetProperty(ref  hasToolsDmg, value); }
        }

        private bool hasJackDmg;
        public bool HasJackDmg
        {
            get { return hasJackDmg; }

            set { SetProperty(ref  hasJackDmg, value); }
        }

        private bool hasBullBarDmg;
        public bool HasBullBarDmg
        {
            get { return hasBullBarDmg; }

            set { SetProperty(ref  hasBullBarDmg, value); }
        }

        private bool hasTrackingDeviceDmg;
        public bool HasTrackingDeviceDmg
        {
            get { return hasTrackingDeviceDmg; }

            set { SetProperty(ref  hasTrackingDeviceDmg, value); }
        }

        private bool hasEngineProtectionUnitDmg;

        public bool HasEngineProtectionUnitDmg
        {
            get { return hasEngineProtectionUnitDmg; }

            set { SetProperty(ref  hasEngineProtectionUnitDmg, value); }
        }

        private bool hasDecalSignWritingDmg;
        public bool HasDecalSignWritingDmg
        {
            get { return hasDecalSignWritingDmg; }

            set { SetProperty(ref  hasDecalSignWritingDmg, value); }
        }

        private bool hasReflectiveTapeDmg;
        public bool HasReflectiveTapeDmg
        {
            get { return hasReflectiveTapeDmg; }

            set { SetProperty(ref  hasReflectiveTapeDmg, value); }
        }
        public async override Task<VIBase> GetDataAsync(string caseNumber)
        {
            return await SqliteHelper.Storage.GetSingleRecordAsync<CAccessories>(x => x.CaseNumber == caseNumber);
        }
    }
}
