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
    public class CCabTrimInter : VIBase
    {
        public CCabTrimInter()
        {
            this.LeftDoorImgList = new ObservableCollection<ImageCapture>();
            this.RightDoorImgList = new ObservableCollection<ImageCapture>();
            this.LFQuaterPanelImgList = new ObservableCollection<ImageCapture>();
            this.RFQuaterPanelImgList = new ObservableCollection<ImageCapture>();
            this.LRQuaterPanelImgList = new ObservableCollection<ImageCapture>();
            this.RRQuaterPanelImgList = new ObservableCollection<ImageCapture>();
            this.FrontViewImgList = new ObservableCollection<ImageCapture>();
            this.BumperImgList = new ObservableCollection<ImageCapture>();
            this.GrillImgList = new ObservableCollection<ImageCapture>();
            this.RearMirrorImgList = new ObservableCollection<ImageCapture>();
            this.WheelArchLeftImgList = new ObservableCollection<ImageCapture>();
            this.RoofImgList = new ObservableCollection<ImageCapture>();
            this.DoorHandleLeftImgList = new ObservableCollection<ImageCapture>();
            this.WheelArchRightImgList = new ObservableCollection<ImageCapture>();
            this.DoorHandleRightImgList = new ObservableCollection<ImageCapture>();
            this.InternalTrimImgList = new ObservableCollection<ImageCapture>();
            this.WipersImgList = new ObservableCollection<ImageCapture>();
            this.MatImgList = new ObservableCollection<ImageCapture>();
            this.DriverSeatImgList = new ObservableCollection<ImageCapture>();
            this.PassengerSeatImgList = new ObservableCollection<ImageCapture>();
        }

        private ObservableCollection<ImageCapture> leftDoorImgList;
        [Ignore]
        public ObservableCollection<ImageCapture> LeftDoorImgList
        {
            get { return leftDoorImgList; }

            set { SetProperty(ref  leftDoorImgList, value); }
        }
        private ObservableCollection<ImageCapture> rightDoorImgList;
        [Ignore]
        public ObservableCollection<ImageCapture> RightDoorImgList
        {
            get { return rightDoorImgList; }

            set { SetProperty(ref  rightDoorImgList, value); }
        }
        private ObservableCollection<ImageCapture> lfQuaterPanelImgList;
        [Ignore]
        public ObservableCollection<ImageCapture> LFQuaterPanelImgList
        {
            get { return lfQuaterPanelImgList; }

            set { SetProperty(ref  lfQuaterPanelImgList, value); }
        }
        private ObservableCollection<ImageCapture> rfQuaterPanelImgList;
        [Ignore]
        public ObservableCollection<ImageCapture> RFQuaterPanelImgList
        {
            get { return rfQuaterPanelImgList; }

            set { SetProperty(ref  rfQuaterPanelImgList, value); }
        }
        private ObservableCollection<ImageCapture> lrQuaterPanelImgList;
        [Ignore]
        public ObservableCollection<ImageCapture> LRQuaterPanelImgList
        {
            get { return lrQuaterPanelImgList; }

            set { SetProperty(ref  lrQuaterPanelImgList, value); }
        }
        private ObservableCollection<ImageCapture> rrQuaterPanelImgList;
        [Ignore]
        public ObservableCollection<ImageCapture> RRQuaterPanelImgList
        {
            get { return rrQuaterPanelImgList; }

            set { SetProperty(ref  rrQuaterPanelImgList, value); }
        }
        private ObservableCollection<ImageCapture> frontViewImgList;
        [Ignore]
        public ObservableCollection<ImageCapture> FrontViewImgList
        {
            get { return frontViewImgList; }

            set { SetProperty(ref  frontViewImgList, value); }
        }
        private ObservableCollection<ImageCapture> bumperImgList;
        [Ignore]
        public ObservableCollection<ImageCapture> BumperImgList
        {
            get { return bumperImgList; }

            set { SetProperty(ref  bumperImgList, value); }
        }
        private ObservableCollection<ImageCapture> grillImgList;
        [Ignore]
        public ObservableCollection<ImageCapture> GrillImgList
        {
            get { return grillImgList; }

            set { SetProperty(ref  grillImgList, value); }
        }
        private ObservableCollection<ImageCapture> rearMirrorImgList;
        [Ignore]
        public ObservableCollection<ImageCapture> RearMirrorImgList
        {
            get { return rearMirrorImgList; }

            set { SetProperty(ref  rearMirrorImgList, value); }
        }
        private ObservableCollection<ImageCapture> wheelArchLeftImgList;
        [Ignore]
        public ObservableCollection<ImageCapture> WheelArchLeftImgList
        {
            get { return wheelArchLeftImgList; }

            set { SetProperty(ref  wheelArchLeftImgList, value); }
        }
        private ObservableCollection<ImageCapture> wheelArchRightImgList;
        [Ignore]
        public ObservableCollection<ImageCapture> WheelArchRightImgList
        {
            get { return wheelArchRightImgList; }

            set { SetProperty(ref  wheelArchRightImgList, value); }
        }
        private ObservableCollection<ImageCapture> roofImgList;
        [Ignore]
        public ObservableCollection<ImageCapture> RoofImgList
        {
            get { return roofImgList; }

            set { SetProperty(ref  roofImgList, value); }
        }
        private ObservableCollection<ImageCapture> doorHandleLeftImgList;
        [Ignore]
        public ObservableCollection<ImageCapture> DoorHandleLeftImgList
        {
            get { return doorHandleLeftImgList; }

            set { SetProperty(ref  doorHandleLeftImgList, value); }
        }
        private ObservableCollection<ImageCapture> doorHandleRightImgList;
        [Ignore]
        public ObservableCollection<ImageCapture> DoorHandleRightImgList
        {
            get { return doorHandleRightImgList; }

            set { SetProperty(ref  doorHandleRightImgList, value); }
        }
        private ObservableCollection<ImageCapture> wipersImgList;
        [Ignore]
        public ObservableCollection<ImageCapture> WipersImgList
        {
            get { return wipersImgList; }

            set { SetProperty(ref  wipersImgList, value); }
        }
        private ObservableCollection<ImageCapture> internalTrimImgList;
        [Ignore]
        public ObservableCollection<ImageCapture> InternalTrimImgList
        {
            get { return internalTrimImgList; }

            set { SetProperty(ref  internalTrimImgList, value); }
        }
        private ObservableCollection<ImageCapture> matImgList;
        [Ignore]
        public ObservableCollection<ImageCapture> MatImgList
        {
            get { return matImgList; }

            set { SetProperty(ref  matImgList, value); }
        }
        private ObservableCollection<ImageCapture> driverSeatImgList;
        [Ignore]
        public ObservableCollection<ImageCapture> DriverSeatImgList
        {
            get { return driverSeatImgList; }

            set { SetProperty(ref  driverSeatImgList, value); }
        }
        private ObservableCollection<ImageCapture> passengerSeatImgList;
        [Ignore]
        public ObservableCollection<ImageCapture> PassengerSeatImgList
        {
            get { return passengerSeatImgList; }

            set { SetProperty(ref  passengerSeatImgList, value); }
        }

        public string leftDoorImgPathList;
        public string LeftDoorImgPathList
        {
            get { return string.Join("~", LeftDoorImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref leftDoorImgPathList, value); }
        }

        public string rightDoorImgPathList;
        public string RightDoorImgPathList
        {
            get { return string.Join("~", RightDoorImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref rightDoorImgPathList, value); }
        }

        public string lFQuaterPanelImgPathList;
        public string LFQuaterPanelImgPathList
        {
            get { return string.Join("~", LFQuaterPanelImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref lFQuaterPanelImgPathList, value); }
        }

        public string rFQuaterPanelImgPathList;
        public string RFQuaterPanelImgPathList
        {
            get { return string.Join("~", RFQuaterPanelImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref rFQuaterPanelImgPathList, value); }
        }

        public string lRQuaterPanelImgPathList;
        public string LRQuaterPanelImgPathList
        {
            get { return string.Join("~", LRQuaterPanelImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref lRQuaterPanelImgPathList, value); }
        }

        public string rRQuaterPanelImgPathList;
        public string RRQuaterPanelImgPathList
        {
            get { return string.Join("~", RRQuaterPanelImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref rRQuaterPanelImgPathList, value); }
        }

        public string frontViewImgPathList;
        public string FrontViewImgPathList
        {
            get { return string.Join("~", FrontViewImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref frontViewImgPathList, value); }
        }

        public string bumperImgPathList;
        public string BumperImgPathList
        {
            get { return string.Join("~", BumperImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref bumperImgPathList, value); }
        }

        public string grillImgPathList;
        public string GrillImgPathList
        {
            get { return string.Join("~", GrillImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref grillImgPathList, value); }
        }

        public string rearMirrorImgPathList;
        public string RearMirrorImgPathList
        {
            get { return string.Join("~", RearMirrorImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref rearMirrorImgPathList, value); }
        }

        public string wheelArchLeftImgPathList;
        public string WheelArchLeftImgPathList
        {
            get { return string.Join("~", WheelArchLeftImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref wheelArchLeftImgPathList, value); }
        }

        public string roofImgPathList;
        public string RoofImgPathList
        {
            get { return string.Join("~", RoofImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref roofImgPathList, value); }
        }

        public string doorHandleLeftImgPathList;
        public string DoorHandleLeftImgPathList
        {
            get { return string.Join("~", DoorHandleLeftImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref doorHandleLeftImgPathList, value); }
        }

        public string wheelArchRightImgPathList;
        public string WheelArchRightImgPathList
        {
            get { return string.Join("~", WheelArchRightImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref wheelArchRightImgPathList, value); }
        }

        public string doorHandleRightImgPathList;
        public string DoorHandleRightImgPathList
        {
            get { return string.Join("~", DoorHandleRightImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref doorHandleRightImgPathList, value); }
        }

        public string internalTrimImgPathList;
        public string InternalTrimImgPathList
        {
            get { return string.Join("~", InternalTrimImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref internalTrimImgPathList, value); }
        }

        public string wipersImgPathList;
        public string WipersImgPathList
        {
            get { return string.Join("~", WipersImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref wipersImgPathList, value); }
        }

        public string matImgPathList;
        public string MatImgPathList
        {
            get { return string.Join("~", MatImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref matImgPathList, value); }
        }

        public string driverSeatImgPathList;
        public string DriverSeatImgPathList
        {
            get { return string.Join("~", DriverSeatImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref driverSeatImgPathList, value); }
        }

        public string passengerSeatImgPathList;
        public string PassengerSeatImgPathList
        {
            get { return string.Join("~", PassengerSeatImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref passengerSeatImgPathList, value); }
        }
        private string leftDoorComment;

        public string LeftDoorComment
        {
            get { return leftDoorComment; }

            set { SetProperty(ref  leftDoorComment, value); }
        }
        private string rightDoorComment;

        public string RightDoorComment
        {
            get { return rightDoorComment; }

            set { SetProperty(ref  rightDoorComment, value); }
        }
        private string lFQuatPanelComment;

        public string LFQuatPanelComment
        {
            get { return lFQuatPanelComment; }

            set { SetProperty(ref  lFQuatPanelComment, value); }
        }
        private string rFQuatPanelComment;

        public string RFQuatPanelComment
        {
            get { return rFQuatPanelComment; }

            set { SetProperty(ref  rFQuatPanelComment, value); }
        }
        private string lRQuatPanelComment;

        public string LRQuatPanelComment
        {
            get { return lRQuatPanelComment; }

            set { SetProperty(ref  lRQuatPanelComment, value); }
        }
        private string rRQuatPanelComment;

        public string RRQuatPanelComment
        {
            get { return rRQuatPanelComment; }

            set { SetProperty(ref  rRQuatPanelComment, value); }
        }
        private string frontViewComment;

        public string FrontViewComment
        {
            get { return frontViewComment; }

            set { SetProperty(ref  frontViewComment, value); }
        }
        private string bumperComment;

        public string BumperComment
        {
            get { return bumperComment; }

            set { SetProperty(ref  bumperComment, value); }
        }
        private string grillComment;

        public string GrillComment
        {
            get { return grillComment; }

            set { SetProperty(ref  grillComment, value); }
        }
        private string rearMirrorComment;

        public string RearMirrorComment
        {
            get { return rearMirrorComment; }

            set { SetProperty(ref  rearMirrorComment, value); }
        }
        private string wheelArchLeftComment;

        public string WheelArchLeftComment
        {
            get { return wheelArchLeftComment; }

            set { SetProperty(ref  wheelArchLeftComment, value); }
        }
        private string wheelArchRightComment;

        public string WheelArchRightComment
        {
            get { return wheelArchRightComment; }

            set { SetProperty(ref  wheelArchRightComment, value); }
        }
        private string roofComment;

        public string RoofComment
        {
            get { return roofComment; }

            set { SetProperty(ref  roofComment, value); }
        }
        private string doorHandleLeftComment;

        public string DoorHandleLeftComment
        {
            get { return doorHandleLeftComment; }

            set { SetProperty(ref  doorHandleLeftComment, value); }
        }
        private string doorHandleRightComment;

        public string DoorHandleRightComment
        {
            get { return doorHandleRightComment; }

            set { SetProperty(ref  doorHandleRightComment, value); }
        }
        private string wipersComment;

        public string WipersComment
        {
            get { return wipersComment; }

            set { SetProperty(ref  wipersComment, value); }
        }
        private string internalTrimComment;

        public string InternalTrimComment
        {
            get { return internalTrimComment; }

            set { SetProperty(ref  internalTrimComment, value); }
        }
        private string matsComment;

        public string MatsComment
        {
            get { return matsComment; }

            set { SetProperty(ref  matsComment, value); }
        }
        private string driverSeatComment;

        public string DriverSeatComment
        {
            get { return driverSeatComment; }

            set { SetProperty(ref  driverSeatComment, value); }
        }
        private string passengerSeatComment;

        public string PassengerSeatComment
        {
            get { return passengerSeatComment; }

            set { SetProperty(ref  passengerSeatComment, value); }
        }

        private bool isLeftDoor;

        public bool IsLeftDoor
        {
            get { return isLeftDoor; }

            set { SetProperty(ref  isLeftDoor, value); }
        }
        private bool isRightDoor;

        public bool IsRightDoor
        {
            get { return isRightDoor; }

            set { SetProperty(ref  isRightDoor, value); }
        }
        private bool isLFQuatPanel;

        public bool IsLFQuatPanel
        {
            get { return isLFQuatPanel; }

            set { SetProperty(ref  isLFQuatPanel, value); }
        }
        private bool isRFQuatPanel;

        public bool IsRFQuatPanel
        {
            get { return isRFQuatPanel; }

            set { SetProperty(ref  isRFQuatPanel, value); }
        }
        private bool isLRQuatPanel;

        public bool IsLRQuatPanel
        {
            get { return isLRQuatPanel; }

            set { SetProperty(ref  isLRQuatPanel, value); }
        }
        private bool isRRQuatPanel;

        public bool IsRRQuatPanel
        {
            get { return isRRQuatPanel; }

            set { SetProperty(ref  isRRQuatPanel, value); }
        }
        private bool isFrontView;

        public bool IsFrontView
        {
            get { return isFrontView; }

            set { SetProperty(ref  isFrontView, value); }
        }
        private bool isBumper;

        public bool IsBumper
        {
            get { return isBumper; }

            set { SetProperty(ref  isBumper, value); }
        }
        private bool isGrill;

        public bool IsGrill
        {
            get { return isGrill; }

            set { SetProperty(ref  isGrill, value); }
        }
        private bool isRearMirror;

        public bool IsRearMirror
        {
            get { return isRearMirror; }

            set { SetProperty(ref  isRearMirror, value); }
        }
        private bool isWheelArchLeft;

        public bool IsWheelArchLeft
        {
            get { return isWheelArchLeft; }

            set { SetProperty(ref  isWheelArchLeft, value); }
        }
        private bool isWheelArchRight;

        public bool IsWheelArchRight
        {
            get { return isWheelArchRight; }

            set { SetProperty(ref  isWheelArchRight, value); }
        }
        private bool isRoof;

        public bool IsRoof
        {
            get { return isRoof; }

            set { SetProperty(ref  isRoof, value); }
        }
        private bool isDoorHandleLeft;

        public bool IsDoorHandleLeft
        {
            get { return isDoorHandleLeft; }

            set { SetProperty(ref  isDoorHandleLeft, value); }
        }
        private bool isDoorHandleRight;

        public bool IsDoorHandleRight
        {
            get { return isDoorHandleRight; }

            set { SetProperty(ref  isDoorHandleRight, value); }
        }
        private bool isWipers;

        public bool IsWipers
        {
            get { return isWipers; }

            set { SetProperty(ref  isWipers, value); }
        }
        private bool isInternalTrim;

        public bool IsInternalTrim
        {
            get { return isInternalTrim; }

            set { SetProperty(ref  isInternalTrim, value); }
        }
        private bool isMats;

        public bool IsMats
        {
            get { return isMats; }

            set { SetProperty(ref  isMats, value); }
        }
        private bool isDriverSeat;

        public bool IsDriverSeat
        {
            get { return isDriverSeat; }

            set { SetProperty(ref  isDriverSeat, value); }
        }
        private bool isPassengerSeat;

        public bool IsPassengerSeat
        {
            get { return isPassengerSeat; }

            set { SetProperty(ref  isPassengerSeat, value); }
        }

        public async override Task<VIBase> GetDataAsync(string caseNumber)
        {
            return await SqliteHelper.Storage.GetSingleRecordAsync<CCabTrimInter>(x => x.CaseNumber == caseNumber);
        }
    }
}
