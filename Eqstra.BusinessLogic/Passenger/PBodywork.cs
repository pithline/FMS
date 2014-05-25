using Eqstra.BusinessLogic.Base;
using Eqstra.BusinessLogic.Helpers;
using Microsoft.Practices.Prism.StoreApps;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eqstra.BusinessLogic.Passenger
{
    public class PBodywork : VIBase
    {
        public PBodywork()
        {
            this.HailDamageImgList = new ObservableCollection<ImageCapture>();
            this.RFDoorImgList = new ObservableCollection<ImageCapture>();
            this.RRDoorImgList = new ObservableCollection<ImageCapture>();
            this.LFDoorImgList = new ObservableCollection<ImageCapture>();
            this.LRDoorImgList = new ObservableCollection<ImageCapture>();

            this.RFBumperImgList = new ObservableCollection<ImageCapture>();
            this.LFBumperImgList = new ObservableCollection<ImageCapture>();
            this.RRBumperImgList = new ObservableCollection<ImageCapture>();
            this.LRBumperImgList = new ObservableCollection<ImageCapture>();
            this.BonnetImgList = new ObservableCollection<ImageCapture>();
            this.RoofImgList = new ObservableCollection<ImageCapture>();

            this.BootTailgateImgList = new ObservableCollection<ImageCapture>();
            this.DoorHandleImgList = new ObservableCollection<ImageCapture>();
            this.HubcapsImgList = new ObservableCollection<ImageCapture>();

            this.RightSideImgList = new ObservableCollection<ImageCapture>();
            this.LeftSideImgList = new ObservableCollection<ImageCapture>();
            this.RFWheelArchImgList = new ObservableCollection<ImageCapture>();

            this.LFWheelArchImgList = new ObservableCollection<ImageCapture>();
            this.RRWheelArchImgList = new ObservableCollection<ImageCapture>();
            this.LRWheelArchImgList = new ObservableCollection<ImageCapture>();
            this.WipersImgList = new ObservableCollection<ImageCapture>();


        }
        public async override Task<VIBase> GetDataAsync(string caseNumber)
        {
            return await SqliteHelper.Storage.GetSingleRecordAsync<PBodywork>(x => x.CaseNumber == caseNumber);
        }

        private ObservableCollection<ImageCapture> hailDamageImgList;
        [Ignore]
        public ObservableCollection<ImageCapture> HailDamageImgList
        {
            get { return hailDamageImgList; }
            set { SetProperty(ref hailDamageImgList, value); }
        }

        private ObservableCollection<ImageCapture> rFDoorImgList;
        [Ignore]
        public ObservableCollection<ImageCapture> RFDoorImgList
        {
            get { return rFDoorImgList; }
            set { SetProperty(ref rFDoorImgList, value); }
        }

        private ObservableCollection<ImageCapture> rRDoorImgList;
        [Ignore]
        public ObservableCollection<ImageCapture> RRDoorImgList
        {
            get { return rRDoorImgList; }
            set { SetProperty(ref rRDoorImgList, value); }
        }

        private ObservableCollection<ImageCapture> lFDoorImgList;
        [Ignore]
        public ObservableCollection<ImageCapture> LFDoorImgList
        {
            get { return lFDoorImgList; }
            set { SetProperty(ref lFDoorImgList, value); }
        }
        private ObservableCollection<ImageCapture> lRDoorImgList;
        [Ignore]
        public ObservableCollection<ImageCapture> LRDoorImgList
        {
            get { return lRDoorImgList; }
            set { SetProperty(ref lRDoorImgList, value); }
        }

        private ObservableCollection<ImageCapture> roofImgList;
        [Ignore]
        public ObservableCollection<ImageCapture> RoofImgList
        {
            get { return roofImgList; }
            set { SetProperty(ref roofImgList, value); }
        }

        private ObservableCollection<ImageCapture> rFBumperImgList;
        [Ignore]
        public ObservableCollection<ImageCapture> RFBumperImgList
        {
            get { return rFBumperImgList; }
            set { SetProperty(ref rFBumperImgList, value); }
        }

        private ObservableCollection<ImageCapture> lFBumperImgList;
        [Ignore]
        public ObservableCollection<ImageCapture> LFBumperImgList
        {
            get { return lFBumperImgList; }
            set { SetProperty(ref lFBumperImgList, value); }
        }

        private ObservableCollection<ImageCapture> rRBumperImgList;
        [Ignore]
        public ObservableCollection<ImageCapture> RRBumperImgList
        {
            get { return rRBumperImgList; }
            set { SetProperty(ref rRBumperImgList, value); }
        }

        private ObservableCollection<ImageCapture> lRBumperImgList;
        [Ignore]
        public ObservableCollection<ImageCapture> LRBumperImgList
        {
            get { return lRBumperImgList; }
            set { SetProperty(ref lRBumperImgList, value); }
        }

        private ObservableCollection<ImageCapture> bonnetImgList;
        [Ignore]
        public ObservableCollection<ImageCapture> BonnetImgList
        {
            get { return bonnetImgList; }
            set { SetProperty(ref bonnetImgList, value); }
        }

        private ObservableCollection<ImageCapture> bootTailgateImgList;
        [Ignore]
        public ObservableCollection<ImageCapture> BootTailgateImgList
        {
            get { return bootTailgateImgList; }
            set { SetProperty(ref bootTailgateImgList, value); }
        }
        private ObservableCollection<ImageCapture> doorHandleImgList;
        [Ignore]
        public ObservableCollection<ImageCapture> DoorHandleImgList
        {
            get { return doorHandleImgList; }
            set { SetProperty(ref doorHandleImgList, value); }
        }
        private ObservableCollection<ImageCapture> hubcapsImgList;
        [Ignore]
        public ObservableCollection<ImageCapture> HubcapsImgList
        {
            get { return hubcapsImgList; }
            set { SetProperty(ref hubcapsImgList, value); }
        }

        private ObservableCollection<ImageCapture> rightSideImgList;
           [Ignore]
        public ObservableCollection<ImageCapture> RightSideImgList
        {
            get { return rightSideImgList; }
            set { SetProperty(ref  rightSideImgList, value); }
        }
        private ObservableCollection<ImageCapture> leftSideImgList;
        [Ignore]
        public ObservableCollection<ImageCapture> LeftSideImgList
        {
            get { return leftSideImgList; }
            set { SetProperty(ref  leftSideImgList, value); }
        }
        private ObservableCollection<ImageCapture> rFWheelArchImgList;
        [Ignore]
        public ObservableCollection<ImageCapture> RFWheelArchImgList
        {
            get { return rFWheelArchImgList; }
            set { SetProperty(ref  rFWheelArchImgList, value); }
        }
        private ObservableCollection<ImageCapture> lFWheelArchImgList;
        [Ignore]
        public ObservableCollection<ImageCapture> LFWheelArchImgList
        {
            get { return lFWheelArchImgList; }
            set { SetProperty(ref  lFWheelArchImgList, value); }
        }
        private ObservableCollection<ImageCapture> rRWheelArchImgList;
        [Ignore]
        public ObservableCollection<ImageCapture> RRWheelArchImgList
        {
            get { return rRWheelArchImgList; }
            set { SetProperty(ref  rRWheelArchImgList, value); }
        }

        private ObservableCollection<ImageCapture> lRWheelArchImgList;
        [Ignore]
        public ObservableCollection<ImageCapture> LRWheelArchImgList
        {
            get { return lRWheelArchImgList; }
            set { SetProperty(ref  lRWheelArchImgList, value); }
        }
        private ObservableCollection<ImageCapture> wipersImgList;
        [Ignore]
        public ObservableCollection<ImageCapture> WipersImgList
        {
            get { return wipersImgList; }
            set { SetProperty(ref  wipersImgList, value); }
        }


        public string hailDamageImgPathList;
        public string HailDamageImgPathList
        {
            get { return string.Join("~", HailDamageImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref hailDamageImgPathList, value); }
        }

        public string rFDoorImgPathList;
        public string RFDoorImgPathList
        {
            get { return string.Join("~", RFDoorImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref rFDoorImgPathList, value); }
        }

        public string rRDoorImgPathList;
        public string RRDoorImgPathList
        {
            get { return string.Join("~", RRDoorImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref rRDoorImgPathList, value); }
        }

        public string lFDoorImgPathList;
        public string LFDoorImgPathList
        {
            get { return string.Join("~", LFDoorImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref lFDoorImgPathList, value); }
        }

        public string lRDoorImgPathList;
        public string LRDoorImgPathList
        {
            get { return string.Join("~", LRDoorImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref lRDoorImgPathList, value); }
        }

        public string rFBumperImgPathList;
        public string RFBumperImgPathList
        {
            get { return string.Join("~", RFBumperImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref rFBumperImgPathList, value); }
        }

        public string lFBumperImgPathList;
        public string LFBumperImgPathList
        {
            get { return string.Join("~", LFBumperImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref lFBumperImgPathList, value); }
        }

        public string rRBumperImgPathList;
        public string RRBumperImgPathList
        {
            get { return string.Join("~", RRBumperImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref rRBumperImgPathList, value); }
        }

        public string lRBumperImgPathList;
        public string LRBumperImgPathList
        {
            get { return string.Join("~", LRBumperImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref lRBumperImgPathList, value); }
        }

        public string bonnetImgPathList;
        public string BonnetImgPathList
        {
            get { return string.Join("~", BonnetImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref bonnetImgPathList, value); }
        }

        public string roofImgPathList;
        public string RoofImgPathList
        {
            get { return string.Join("~", RoofImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref roofImgPathList, value); }
        }

        public string bootTailgateImgPathList;
        public string BootTailgateImgPathList
        {
            get { return string.Join("~", BootTailgateImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref bootTailgateImgPathList, value); }
        }

        public string doorHandleImgPathList;
        public string DoorHandleImgPathList
        {
            get { return string.Join("~", DoorHandleImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref doorHandleImgPathList, value); }
        }

        public string hubcapsImgPathList;
        public string HubcapsImgPathList
        {
            get { return string.Join("~", HubcapsImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref hubcapsImgPathList, value); }
        }

        public string rightSideImgPathList;
        public string RightSideImgPathList
        {
            get { return string.Join("~", RightSideImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref rightSideImgPathList, value); }
        }

        public string leftSideImgPathList;
        public string LeftSideImgPathList
        {
            get { return string.Join("~", LeftSideImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref leftSideImgPathList, value); }
        }

        public string rFWheelArchImgPathList;
        public string RFWheelArchImgPathList
        {
            get { return string.Join("~", RFWheelArchImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref rFWheelArchImgPathList, value); }
        }

        public string lFWheelArchImgPathList;
        public string LFWheelArchImgPathList
        {
            get { return string.Join("~", LFWheelArchImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref lFWheelArchImgPathList, value); }
        }

        public string rRWheelArchImgPathList;
        public string RRWheelArchImgPathList
        {
            get { return string.Join("~", RRWheelArchImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref rRWheelArchImgPathList, value); }
        }

        public string lRWheelArchImgPathList;
        public string LRWheelArchImgPathList
        {
            get { return string.Join("~", LRWheelArchImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref lRWheelArchImgPathList, value); }
        }

        public string wipersImgPathList;
        public string WipersImgPathList
        {
            get { return string.Join("~", WipersImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref wipersImgPathList, value); }
        }

        private bool isHailDamageToggle;

        public bool IsHailDamageToggle
        {
            get { return isHailDamageToggle; }

            set { SetProperty(ref  isHailDamageToggle, value); }
        }
        private bool isRFDoorToggle;

        public bool IsRFDoorToggle
        {
            get { return isRFDoorToggle; }

            set { SetProperty(ref  isRFDoorToggle, value); }
        }
        private bool isLFDoorToggle;

        public bool IsLFDoorToggle
        {
            get { return isLFDoorToggle; }

            set { SetProperty(ref  isLFDoorToggle, value); }
        }
        private bool isRRDoorToggle;

        public bool IsRRDoorToggle
        {
            get { return isRRDoorToggle; }

            set { SetProperty(ref  isRRDoorToggle, value); }
        }
        private bool isLRDoorToggle;

        public bool IsLRDoorToggle
        {
            get { return isLRDoorToggle; }

            set { SetProperty(ref  isLRDoorToggle, value); }
        }
        private bool isRoofToggle;

        public bool IsRoofToggle
        {
            get { return isRoofToggle; }

            set { SetProperty(ref  isRoofToggle, value); }
        }
        private bool isRFBumperToggle;

        public bool IsRFBumperToggle
        {
            get { return isRFBumperToggle; }

            set { SetProperty(ref  isRFBumperToggle, value); }
        }
        private bool isLFBumperToggle;

        public bool IsLFBumperToggle
        {
            get { return isLFBumperToggle; }

            set { SetProperty(ref  isLFBumperToggle, value); }
        }
        private bool isRRBumperToggle;

        public bool IsRRBumperToggle
        {
            get { return isRRBumperToggle; }

            set { SetProperty(ref  isRRBumperToggle, value); }
        }
        private bool isBonnetToggle;

        public bool IsBonnetToggle
        {
            get { return isBonnetToggle; }

            set { SetProperty(ref  isBonnetToggle, value); }
        }
        private bool isBootTailgateToggle;

        public bool IsBootTailgateToggle
        {
            get { return isBootTailgateToggle; }

            set { SetProperty(ref  isBootTailgateToggle, value); }
        }
        private bool isDoorHandleToggle;

        public bool IsDoorHandleToggle
        {
            get { return isDoorHandleToggle; }

            set { SetProperty(ref  isDoorHandleToggle, value); }
        }
        private bool isHubcapsToggle;

        public bool IsHubcapsToggle
        {
            get { return isHubcapsToggle; }

            set { SetProperty(ref  isHubcapsToggle, value); }
        }
        private bool isRightSideToggle;

        public bool IsRightSideToggle
        {
            get { return isRightSideToggle; }

            set { SetProperty(ref  isRightSideToggle, value); }
        }
        private bool isLeftSideToggle;

        public bool IsLeftSideToggle
        {
            get { return isLeftSideToggle; }

            set { SetProperty(ref  isLeftSideToggle, value); }
        }
        private bool isRFWheelArchToggle;

        public bool IsRFWheelArchToggle
        {
            get { return isRFWheelArchToggle; }

            set { SetProperty(ref  isRFWheelArchToggle, value); }
        }
        private bool isLRBumperToggle;

        public bool IsLRBumperToggle
        {
            get { return isLRBumperToggle; }

            set { SetProperty(ref  isLRBumperToggle, value); }
        }
        private bool isLFWheelArchToggle;

        public bool IsLFWheelArchToggle
        {
            get { return isLFWheelArchToggle; }

            set { SetProperty(ref  isLFWheelArchToggle, value); }
        }
        private bool isRRWheelArchToggle;

        public bool IsRRWheelArchToggle
        {
            get { return isRRWheelArchToggle; }

            set { SetProperty(ref  isRRWheelArchToggle, value); }
        }
        private bool isLRWheelArchToggle;

        public bool IsLRWheelArchToggle
        {
            get { return isLRWheelArchToggle; }

            set { SetProperty(ref  isLRWheelArchToggle, value); }
        }
        private bool isWipersToggle;

        public bool IsWipersToggle
        {
            get { return isWipersToggle; }

            set
            { SetProperty(ref  isWipersToggle, value); }
        }



        private string gVHailDamageComment;

        public string GVHailDamageComment
        {
            get { return gVHailDamageComment; }

            set { SetProperty(ref  gVHailDamageComment, value); }
        }
        private string gVRFDoorComment;

        public string GVRFDoorComment
        {
            get { return gVRFDoorComment; }

            set { SetProperty(ref  gVRFDoorComment, value); }
        }
        private string gVLFDoorComment;

        public string GVLFDoorComment
        {
            get { return gVLFDoorComment; }

            set { SetProperty(ref  gVLFDoorComment, value); }
        }
        private string gVRRDoorComment;

        public string GVRRDoorComment
        {
            get { return gVRRDoorComment; }

            set { SetProperty(ref  gVRRDoorComment, value); }
        }
        private string gVLRDoorComment;

        public string GVLRDoorComment
        {
            get { return gVLRDoorComment; }

            set { SetProperty(ref  gVLRDoorComment, value); }
        }
        private string gVRoofComment;

        public string GVRoofComment
        {
            get { return gVRoofComment; }

            set { SetProperty(ref  gVRoofComment, value); }
        }
        private string gVRFBumperComment;

        public string GVRFBumperComment
        {
            get { return gVRFBumperComment; }

            set { SetProperty(ref  gVRFBumperComment, value); }
        }
        private string gVLFBumperComment;

        public string GVLFBumperComment
        {
            get { return gVLFBumperComment; }

            set { SetProperty(ref  gVLFBumperComment, value); }
        }
        private string gVRRBumperComment;

        public string GVRRBumperComment
        {
            get { return gVRRBumperComment; }

            set { SetProperty(ref  gVRRBumperComment, value); }
        }
        private string gVBonnetComment;

        public string GVBonnetComment
        {
            get { return gVBonnetComment; }

            set { SetProperty(ref  gVBonnetComment, value); }
        }
        private string gVBootTailgateComment;

        public string GVBootTailgateComment
        {
            get { return gVBootTailgateComment; }

            set { SetProperty(ref  gVBootTailgateComment, value); }
        }
        private string gVLFDoorHandleComment;

        public string GVLFDoorHandleComment
        {
            get { return gVLFDoorHandleComment; }

            set { SetProperty(ref  gVLFDoorHandleComment, value); }
        }
        private string gVHubcapsComment;

        public string GVHubcapsComment
        {
            get { return gVHubcapsComment; }

            set { SetProperty(ref  gVHubcapsComment, value); }
        }
        private string gVRightSideComment;

        public string GVRightSideComment
        {
            get { return gVRightSideComment; }

            set { SetProperty(ref  gVRightSideComment, value); }
        }
        private string gVLeftSideComment;

        public string GVLeftSideComment
        {
            get { return gVLeftSideComment; }

            set { SetProperty(ref  gVLeftSideComment, value); }
        }
        private string gVRFWheelArchComment;

        public string GVRFWheelArchComment
        {
            get { return gVRFWheelArchComment; }

            set { SetProperty(ref  gVRFWheelArchComment, value); }
        }
        private string gVLFWheelArchToggleComment;

        public string GVLFWheelArchToggleComment
        {
            get { return gVLFWheelArchToggleComment; }

            set { SetProperty(ref  gVLFWheelArchToggleComment, value); }
        }
        private string gVRRWheelArchComment;

        public string GVRRWheelArchComment
        {
            get { return gVRRWheelArchComment; }

            set { SetProperty(ref  gVRRWheelArchComment, value); }
        }
        private string gVLRWheelArchComment;

        public string GVLRWheelArchComment
        {
            get { return gVLRWheelArchComment; }

            set { SetProperty(ref  gVLRWheelArchComment, value); }
        }
        private string gVLRBumperComment;

        public string GVLRBumperComment
        {
            get { return gVLRBumperComment; }

            set { SetProperty(ref  gVLRBumperComment, value); }
        }
        private string gVWipersComment;

        public string GVWipersComment
        {
            get { return gVWipersComment; }

            set { SetProperty(ref  gVWipersComment, value); }
        }
    }
}

