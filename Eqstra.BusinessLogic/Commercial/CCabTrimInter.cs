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
    public class CCabTrimInter : BaseModel
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
        [Ignore, DamageSnapshotRequired("Left door snapshot(s) required", "IsLeftDoorDmg")]
        public ObservableCollection<ImageCapture> LeftDoorImgList
        {
            get { return leftDoorImgList; }

            set { SetProperty(ref  leftDoorImgList, value); }
        }
        private ObservableCollection<ImageCapture> rightDoorImgList;
        [Ignore, DamageSnapshotRequired("Right door snapshot(s) required", "IsRightDoorDmg")]
        public ObservableCollection<ImageCapture> RightDoorImgList
        {
            get { return rightDoorImgList; }

            set { SetProperty(ref  rightDoorImgList, value); }
        }
        private ObservableCollection<ImageCapture> lfQuaterPanelImgList;
        [Ignore, DamageSnapshotRequired("LF quater panel snapshot(s) required", "IsLFQuatPanelDmg")]
        public ObservableCollection<ImageCapture> LFQuaterPanelImgList
        {
            get { return lfQuaterPanelImgList; }

            set { SetProperty(ref  lfQuaterPanelImgList, value); }
        }
        private ObservableCollection<ImageCapture> rfQuaterPanelImgList;
        [Ignore, DamageSnapshotRequired("RF quater panel snapshot(s) required", "IsRFQuatPanelDmg")]
        public ObservableCollection<ImageCapture> RFQuaterPanelImgList
        {
            get { return rfQuaterPanelImgList; }

            set { SetProperty(ref  rfQuaterPanelImgList, value); }
        }
        private ObservableCollection<ImageCapture> lrQuaterPanelImgList;
        [Ignore, DamageSnapshotRequired("LR quater panel  snapshot(s) required", "IsLRQuatPanelDmg")]
        public ObservableCollection<ImageCapture> LRQuaterPanelImgList
        {
            get { return lrQuaterPanelImgList; }

            set { SetProperty(ref  lrQuaterPanelImgList, value); }
        }
        private ObservableCollection<ImageCapture> rrQuaterPanelImgList;
        [Ignore, DamageSnapshotRequired("RR quater panel snapshot(s) required", "IsRRQuatPanelDmg")]
        public ObservableCollection<ImageCapture> RRQuaterPanelImgList
        {
            get { return rrQuaterPanelImgList; }

            set { SetProperty(ref  rrQuaterPanelImgList, value); }
        }
        private ObservableCollection<ImageCapture> frontViewImgList;
        [Ignore, DamageSnapshotRequired("Front view  snapshot(s) required", "IsFrontViewDmg")]
        public ObservableCollection<ImageCapture> FrontViewImgList
        {
            get { return frontViewImgList; }

            set { SetProperty(ref  frontViewImgList, value); }
        }
        private ObservableCollection<ImageCapture> bumperImgList;
        [Ignore, DamageSnapshotRequired("Bumper  snapshot(s) required", "IsBumperDmg")]
        public ObservableCollection<ImageCapture> BumperImgList
        {
            get { return bumperImgList; }

            set { SetProperty(ref  bumperImgList, value); }
        }
        private ObservableCollection<ImageCapture> grillImgList;
        [Ignore, DamageSnapshotRequired("Grill snapshot(s) required", "IsGrillDmg")]
        public ObservableCollection<ImageCapture> GrillImgList
        {
            get { return grillImgList; }

            set { SetProperty(ref  grillImgList, value); }
        }
        private ObservableCollection<ImageCapture> rearMirrorImgList;
        [Ignore, DamageSnapshotRequired("Rear mirror snapshot(s) required", "IsRearMirrorDmg")]
        public ObservableCollection<ImageCapture> RearMirrorImgList
        {
            get { return rearMirrorImgList; }

            set { SetProperty(ref  rearMirrorImgList, value); }
        }
        private ObservableCollection<ImageCapture> wheelArchLeftImgList;
        [Ignore, DamageSnapshotRequired("Wheel arch left snapshot(s) required", "IsWheelArchLeftDmg")]
        public ObservableCollection<ImageCapture> WheelArchLeftImgList
        {
            get { return wheelArchLeftImgList; }

            set { SetProperty(ref  wheelArchLeftImgList, value); }
        }
        private ObservableCollection<ImageCapture> wheelArchRightImgList;
        [Ignore, DamageSnapshotRequired("Wheel arch right snapshot(s) required", "IsWheelArchRightDmg")]
        public ObservableCollection<ImageCapture> WheelArchRightImgList
        {
            get { return wheelArchRightImgList; }

            set { SetProperty(ref  wheelArchRightImgList, value); }
        }
        private ObservableCollection<ImageCapture> roofImgList;
        [Ignore, DamageSnapshotRequired("Roof  snapshot(s) required", "IsRoofDmg")]
        public ObservableCollection<ImageCapture> RoofImgList
        {
            get { return roofImgList; }

            set { SetProperty(ref  roofImgList, value); }
        }
        private ObservableCollection<ImageCapture> doorHandleLeftImgList;
        [Ignore, DamageSnapshotRequired("Door handle left snapshot(s) required", "IsDoorHandleLeftDmg")]
        public ObservableCollection<ImageCapture> DoorHandleLeftImgList
        {
            get { return doorHandleLeftImgList; }

            set { SetProperty(ref  doorHandleLeftImgList, value); }
        }
        private ObservableCollection<ImageCapture> doorHandleRightImgList;
        [Ignore, DamageSnapshotRequired("Door handle right snapshot(s) required", "IsDoorHandleRightDmg")]
        public ObservableCollection<ImageCapture> DoorHandleRightImgList
        {
            get { return doorHandleRightImgList; }

            set { SetProperty(ref  doorHandleRightImgList, value); }
        }
        private ObservableCollection<ImageCapture> wipersImgList;
        [Ignore, DamageSnapshotRequired("Wipers  snapshot(s) required", "IsWipersDmg")]
        public ObservableCollection<ImageCapture> WipersImgList
        {
            get { return wipersImgList; }

            set { SetProperty(ref  wipersImgList, value); }
        }
        private ObservableCollection<ImageCapture> internalTrimImgList;
        [Ignore, DamageSnapshotRequired("Internal snapshot(s) required", "IsInternalTrimDmg")]
        public ObservableCollection<ImageCapture> InternalTrimImgList
        {
            get { return internalTrimImgList; }

            set { SetProperty(ref  internalTrimImgList, value); }
        }
        private ObservableCollection<ImageCapture> matImgList;
        [Ignore, DamageSnapshotRequired("Mat snapshot(s) required", "IsMatsDmg")]
        public ObservableCollection<ImageCapture> MatImgList
        {
            get { return matImgList; }

            set { SetProperty(ref  matImgList, value); }
        }
        private ObservableCollection<ImageCapture> driverSeatImgList;
        [Ignore, DamageSnapshotRequired("Driver seat  snapshot(s) required", "IsDriverSeatDmg")]
        public ObservableCollection<ImageCapture> DriverSeatImgList
        {
            get { return driverSeatImgList; }

            set { SetProperty(ref  driverSeatImgList, value); }
        }
        private ObservableCollection<ImageCapture> passengerSeatImgList;
        [Ignore, DamageSnapshotRequired("Passenger seat snapshot(s) required", "IsPassengerSeatDmg")]
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

        private bool isLeftDoorDmg;

        public bool IsLeftDoorDmg
        {
            get { return isLeftDoorDmg; }

            set { SetProperty(ref  isLeftDoorDmg, value); }
        }
        private bool isRightDoorDmg;

        public bool IsRightDoorDmg
        {
            get { return isRightDoorDmg; }

            set { SetProperty(ref  isRightDoorDmg, value); }
        }
        private bool isLFQuatPanelDmg;

        public bool IsLFQuatPanelDmg
        {
            get { return isLFQuatPanelDmg; }

            set { SetProperty(ref  isLFQuatPanelDmg, value); }
        }
        private bool isRFQuatPanelDmg;

        public bool IsRFQuatPanelDmg
        {
            get { return isRFQuatPanelDmg; }

            set { SetProperty(ref  isRFQuatPanelDmg, value); }
        }
        private bool isLRQuatPanelDmg;

        public bool IsLRQuatPanelDmg
        {
            get { return isLRQuatPanelDmg; }

            set { SetProperty(ref  isLRQuatPanelDmg, value); }
        }
        private bool isRRQuatPanelDmg;

        public bool IsRRQuatPanelDmg
        {
            get { return isRRQuatPanelDmg; }

            set { SetProperty(ref  isRRQuatPanelDmg, value); }
        }
        private bool isFrontViewDmg;

        public bool IsFrontViewDmg
        {
            get { return isFrontViewDmg; }

            set { SetProperty(ref  isFrontViewDmg, value); }
        }
        private bool isBumperDmg;

        public bool IsBumperDmg
        {
            get { return isBumperDmg; }

            set { SetProperty(ref  isBumperDmg, value); }
        }
        private bool isGrillDmg;

        public bool IsGrillDmg
        {
            get { return isGrillDmg; }

            set { SetProperty(ref  isGrillDmg, value); }
        }
        private bool isRearMirrorDmg;

        public bool IsRearMirrorDmg
        {
            get { return isRearMirrorDmg; }

            set { SetProperty(ref  isRearMirrorDmg, value); }
        }
        private bool isWheelArchLeftDmg;

        public bool IsWheelArchLeftDmg
        {
            get { return isWheelArchLeftDmg; }

            set { SetProperty(ref isWheelArchLeftDmg, value); }
        }
        private bool isWheelArchRightDmg;

        public bool IsWheelArchRightDmg
        {
            get { return isWheelArchRightDmg; }

            set { SetProperty(ref  isWheelArchRightDmg, value); }
        }
        private bool isRoofDmg;

        public bool IsRoofDmg
        {
            get { return isRoofDmg; }

            set { SetProperty(ref  isRoofDmg, value); }
        }
        private bool isDoorHandleLeftDmg;

        public bool IsDoorHandleLeftDmg
        {
            get { return isDoorHandleLeftDmg; }

            set { SetProperty(ref  isDoorHandleLeftDmg, value); }
        }
        private bool isDoorHandleRightDmg;

        public bool IsDoorHandleRightDmg
        {
            get { return isDoorHandleRightDmg; }

            set { SetProperty(ref  isDoorHandleRightDmg, value); }
        }
        private bool isWipersDmg;

        public bool IsWipersDmg
        {
            get { return isWipersDmg; }

            set { SetProperty(ref  isWipersDmg, value); }
        }
        private bool isInternalTrimDmg;

        public bool IsInternalTrimDmg
        {
            get { return isInternalTrimDmg; }

            set { SetProperty(ref  isInternalTrimDmg, value); }
        }
        private bool isMatsDmg;

        public bool IsMatsDmg
        {
            get { return isMatsDmg; }

            set { SetProperty(ref  isMatsDmg, value); }
        }
        private bool isDriverSeatDmg;

        public bool IsDriverSeatDmg
        {
            get { return isDriverSeatDmg; }

            set { SetProperty(ref  isDriverSeatDmg, value); }
        }
        private bool isPassengerSeatDmg;

        public bool IsPassengerSeatDmg
        {
            get { return isPassengerSeatDmg; }

            set { SetProperty(ref  isPassengerSeatDmg, value); }
        }

        public async override Task<BaseModel> GetDataAsync(long vehicleInsRecID)
        {
            return await SqliteHelper.Storage.GetSingleRecordAsync<CCabTrimInter>(x => x.VehicleInsRecID == vehicleInsRecID);
        }
    }
}
