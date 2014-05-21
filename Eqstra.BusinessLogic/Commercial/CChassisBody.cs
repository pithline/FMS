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
    public class CChassisBody : VIBase
    {
        public CChassisBody()
        {
            this.DoorsImgList = new ObservableCollection<ImageCapture>();
            this.ChassisImgList = new ObservableCollection<ImageCapture>();
            this.FloorImgList = new ObservableCollection<ImageCapture>();
            this.HeadboardImgList = new ObservableCollection<ImageCapture>();
            this.DropSideLeftImgList = new ObservableCollection<ImageCapture>();
            this.DropSideRightImgList = new ObservableCollection<ImageCapture>();
            this.DropSideFrontImgList = new ObservableCollection<ImageCapture>();
            this.DropSideRearImgList = new ObservableCollection<ImageCapture>();
            this.FuelTankImgList = new ObservableCollection<ImageCapture>();
            this.SpareWheelCarrierImgList = new ObservableCollection<ImageCapture>();
            this.UnderRunBumperImgList = new ObservableCollection<ImageCapture>();
            this.ChevronImgList = new ObservableCollection<ImageCapture>();
            this.LandingLegsImgList = new ObservableCollection<ImageCapture>();

        }
        public async override Task<VIBase> GetDataAsync(string caseNumber)
        {
            return await SqliteHelper.Storage.GetSingleRecordAsync<CChassisBody>(x => x.CaseNumber == caseNumber);
        }

        private string doorsComment;

        public string DoorsComment
        {
            get { return doorsComment; }

            set { SetProperty(ref  doorsComment, value); }
        }
        private bool isDoors;

        public bool IsDoors
        {
            get { return isDoors; }

            set { SetProperty(ref  isDoors, value); }
        }
        private ObservableCollection<ImageCapture> doorsImgList;

        [Ignore]
        public ObservableCollection<ImageCapture> DoorsImgList
        {
            get { return doorsImgList; }

            set { SetProperty(ref  doorsImgList, value); }
        }

        private string chassisComment;

        public string ChassisComment
        {
            get { return chassisComment; }

            set { SetProperty(ref  chassisComment, value); }
        }
        private bool isChassis;

        public bool IsChassis
        {
            get { return isChassis; }

            set { SetProperty(ref  isChassis, value); }
        }
        private ObservableCollection<ImageCapture> chassisImgList;

        [Ignore]
        public ObservableCollection<ImageCapture> ChassisImgList
        {
            get { return chassisImgList; }

            set { SetProperty(ref  chassisImgList, value); }
        }


        private string floorComment;

        public string FloorComment
        {
            get { return floorComment; }

            set { SetProperty(ref  floorComment, value); }
        }
        private bool isFloor;

        public bool IsFloor
        {
            get { return isFloor; }

            set { SetProperty(ref  isFloor, value); }
        }
        private ObservableCollection<ImageCapture> floorImgList;

        [Ignore]
        public ObservableCollection<ImageCapture> FloorImgList
        {
            get { return floorImgList; }

            set { SetProperty(ref  floorImgList, value); }
        }

        private string headboardComment;

        public string HeadboardComment
        {
            get { return headboardComment; }

            set { SetProperty(ref  headboardComment, value); }
        }
        private bool isHeadboard;

        public bool IsHeadboard
        {
            get { return isHeadboard; }

            set { SetProperty(ref  isHeadboard, value); }
        }
        private ObservableCollection<ImageCapture> headboardImgList;

        [Ignore]
        public ObservableCollection<ImageCapture> HeadboardImgList
        {
            get { return headboardImgList; }

            set { SetProperty(ref  headboardImgList, value); }
        }

        private string dropSideLeftComment;
        public string DropSideLeftComment
        {
            get { return dropSideLeftComment; }

            set { SetProperty(ref  dropSideLeftComment, value); }
        }
        private bool isDropSideLeft;

        public bool IsDropSideLeft
        {
            get { return isDropSideLeft; }

            set { SetProperty(ref  isDropSideLeft, value); }
        }
        private ObservableCollection<ImageCapture> dropSideLeftImgList;

        [Ignore]
        public ObservableCollection<ImageCapture> DropSideLeftImgList
        {
            get { return dropSideLeftImgList; }

            set { SetProperty(ref  dropSideLeftImgList, value); }
        }

        private string dropSideRightComment;

        public string DropSideRightComment
        {
            get { return dropSideRightComment; }

            set { SetProperty(ref  dropSideRightComment, value); }
        }
        private bool isDropSideRight;

        public bool IsDropSideRight
        {
            get { return isDropSideRight; }

            set { SetProperty(ref  isDropSideRight, value); }
        }
        private ObservableCollection<ImageCapture> dropSideRightImgList;

        [Ignore]
        public ObservableCollection<ImageCapture> DropSideRightImgList
        {
            get { return dropSideRightImgList; }

            set { SetProperty(ref  dropSideRightImgList, value); }
        }

        private string dropSideFrontComment;

        public string DropSideFrontComment
        {
            get { return dropSideFrontComment; }

            set { SetProperty(ref  dropSideFrontComment, value); }
        }
        private bool isDropSideFront;

        public bool IsDropSideFront
        {
            get { return isDropSideFront; }

            set { SetProperty(ref  isDropSideFront, value); }
        }
        private ObservableCollection<ImageCapture> dropSideFrontImgList;

        [Ignore]
        public ObservableCollection<ImageCapture> DropSideFrontImgList
        {
            get { return dropSideFrontImgList; }

            set { SetProperty(ref  dropSideFrontImgList, value); }
        }

        private string dropSideRearComment;

        public string DropSideRearComment
        {
            get { return dropSideRearComment; }

            set { SetProperty(ref  dropSideRearComment, value); }
        }
        private bool isDropSideRear;

        public bool IsDropSideRear
        {
            get { return isDropSideRear; }

            set { SetProperty(ref  isDropSideRear, value); }
        }
        private ObservableCollection<ImageCapture> dropSideRearImgList;

        [Ignore]
        public ObservableCollection<ImageCapture> DropSideRearImgList
        {
            get { return dropSideRearImgList; }

            set { SetProperty(ref  dropSideRearImgList, value); }
        }

        private string fuelTankComment;

        public string FuelTankComment
        {
            get { return fuelTankComment; }

            set { SetProperty(ref  fuelTankComment, value); }
        }
        private bool isFuelTank;

        public bool IsFuelTank
        {
            get { return isFuelTank; }

            set { SetProperty(ref  isFuelTank, value); }
        }
        private ObservableCollection<ImageCapture> fuelTankImgList;

        [Ignore]
        public ObservableCollection<ImageCapture> FuelTankImgList
        {
            get { return fuelTankImgList; }

            set { SetProperty(ref  fuelTankImgList, value); }
        }

        private string spareWheelCarrierComment;

        public string SpareWheelCarrierComment
        {
            get { return spareWheelCarrierComment; }

            set { SetProperty(ref  spareWheelCarrierComment, value); }
        }
        private bool isSpareWheelCarrier;

        public bool IsSpareWheelCarrier
        {
            get { return isSpareWheelCarrier; }

            set { SetProperty(ref  isSpareWheelCarrier, value); }
        }
        private ObservableCollection<ImageCapture> spareWheelCarrierImgList;

        [Ignore]
        public ObservableCollection<ImageCapture> SpareWheelCarrierImgList
        {
            get { return spareWheelCarrierImgList; }

            set { SetProperty(ref  spareWheelCarrierImgList, value); }
        }

        private string underRunBumperComment;

        public string UnderRunBumperComment
        {
            get { return underRunBumperComment; }

            set { SetProperty(ref  underRunBumperComment, value); }
        }
        private bool isUnderRunBumper;

        public bool IsUnderRunBumper
        {
            get { return isUnderRunBumper; }

            set { SetProperty(ref  isUnderRunBumper, value); }
        }
        private ObservableCollection<ImageCapture> underRunBumperImgList;

        [Ignore]
        public ObservableCollection<ImageCapture> UnderRunBumperImgList
        {
            get { return underRunBumperImgList; }

            set { SetProperty(ref  underRunBumperImgList, value); }
        }

        private string chevronComment;

        public string ChevronComment
        {
            get { return chevronComment; }

            set { SetProperty(ref  chevronComment, value); }
        }
        private bool isChevron;

        public bool IsChevron
        {
            get { return isChevron; }

            set { SetProperty(ref  isChevron, value); }
        }
        private ObservableCollection<ImageCapture> chevronImgList;

        [Ignore]
        public ObservableCollection<ImageCapture> ChevronImgList
        {
            get { return chevronImgList; }

            set { SetProperty(ref  chevronImgList, value); }
        }

        private string landingLegsComment;

        public string LandingLegsComment
        {
            get { return landingLegsComment; }

            set { SetProperty(ref  landingLegsComment, value); }
        }
        private bool isLandingLegs;

        public bool IsLandingLegs
        {
            get { return isLandingLegs; }

            set { SetProperty(ref  isLandingLegs, value); }
        }
        private ObservableCollection<ImageCapture> landingLegsImgList;

        [Ignore]
        public ObservableCollection<ImageCapture> LandingLegsImgList
        {
            get { return landingLegsImgList; }

            set { SetProperty(ref  landingLegsImgList, value); }
        }


        public string doorsImgPathList;
        public string DoorsImgPathList
        {
            get { return string.Join("~", DoorsImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref doorsImgPathList, value); }
        }

        public string chassisImgPathList;
        public string ChassisImgPathList
        {
            get { return string.Join("~", ChassisImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref chassisImgPathList, value); }
        }

        public string floorImgPathList;
        public string FloorImgPathList
        {
            get { return string.Join("~", FloorImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref floorImgPathList, value); }
        }

        public string headboardImgPathList;
        public string HeadboardImgPathList
        {
            get { return string.Join("~", HeadboardImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref headboardImgPathList, value); }
        }

        public string dropSideLeftImgPathList;
        public string DropSideLeftImgPathList
        {
            get { return string.Join("~", DropSideLeftImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref dropSideLeftImgPathList, value); }
        }

        public string dropSideRightImgPathList;
        public string DropSideRightImgPathList
        {
            get { return string.Join("~", DropSideRightImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref dropSideRightImgPathList, value); }
        }

        public string dropSideFrontImgPathList;
        public string DropSideFrontImgPathList
        {
            get { return string.Join("~", DropSideFrontImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref dropSideFrontImgPathList, value); }
        }

        public string dropSideRearImgPathList;
        public string DropSideRearImgPathList
        {
            get { return string.Join("~", DropSideRearImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref dropSideRearImgPathList, value); }
        }

        public string fuelTankImgPathList;
        public string FuelTankImgPathList
        {
            get { return string.Join("~", FuelTankImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref fuelTankImgPathList, value); }
        }

        public string spareWheelCarrierImgPathList;
        public string SpareWheelCarrierImgPathList
        {
            get { return string.Join("~", SpareWheelCarrierImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref spareWheelCarrierImgPathList, value); }
        }

        public string underRunBumperImgPathList;
        public string UnderRunBumperImgPathList
        {
            get { return string.Join("~", UnderRunBumperImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref underRunBumperImgPathList, value); }
        }

        public string chevronImgPathList;
        public string ChevronImgPathList
        {
            get { return string.Join("~", ChevronImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref chevronImgPathList, value); }
        }

        public string landingLegsImgPathList;
        public string LandingLegsImgPathList
        {
            get { return string.Join("~", LandingLegsImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref landingLegsImgPathList, value); }
        }
    }
}
