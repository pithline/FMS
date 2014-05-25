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
    public class PTrimInterior : VIBase
    {
        public PTrimInterior()
        {
            this.InternalTrimImgList = new ObservableCollection<ImageCapture>();
            this.RRDoorTrimImgList = new ObservableCollection<ImageCapture>();
            this.LRDoorTrimImgList = new ObservableCollection<ImageCapture>();
            this.RFDoorTrimImgList = new ObservableCollection<ImageCapture>();
            this.LFDoorTrimImgList = new ObservableCollection<ImageCapture>();
            this.DriverSeatImgList = new ObservableCollection<ImageCapture>();
            this.PassengerSeatImgList = new ObservableCollection<ImageCapture>();
            this.RearSeatImgList = new ObservableCollection<ImageCapture>();
            this.DashImgList = new ObservableCollection<ImageCapture>();
            this.CarpetImgList = new ObservableCollection<ImageCapture>();
            // this.CarpetImgList.Add(new ImageCapture() { ImagePath = "ms-appx:///Images/images.png" });
        }

        public async override Task<VIBase> GetDataAsync(string caseNumber)
        {
            return await SqliteHelper.Storage.GetSingleRecordAsync<PTrimInterior>(x => x.CaseNumber == caseNumber);
        }

        private ObservableCollection<ImageCapture> internalTrimImgList;
        [Ignore]
        public ObservableCollection<ImageCapture> InternalTrimImgList
        {
            get { return internalTrimImgList; }
            set { SetProperty(ref internalTrimImgList, value); }
        }

        private ObservableCollection<ImageCapture> rrDoorTrimiImgList;
        [Ignore]

        public ObservableCollection<ImageCapture> RRDoorTrimImgList
        {
            get { return rrDoorTrimiImgList; }
            set { SetProperty(ref rrDoorTrimiImgList, value); }
        }

        private ObservableCollection<ImageCapture> lrDoorTrimImgList;
        [Ignore]

        public ObservableCollection<ImageCapture> LRDoorTrimImgList
        {
            get { return lrDoorTrimImgList; }
            set { SetProperty(ref lrDoorTrimImgList, value); }
        }

        private ObservableCollection<ImageCapture> rfDoorTrimImgList;
        [Ignore]

        public ObservableCollection<ImageCapture> RFDoorTrimImgList
        {
            get { return rfDoorTrimImgList; }
            set { SetProperty(ref rfDoorTrimImgList, value); }
        }

        private ObservableCollection<ImageCapture> lfDoorTrimImgList;
        [Ignore]

        public ObservableCollection<ImageCapture> LFDoorTrimImgList
        {
            get { return lfDoorTrimImgList; }
            set { SetProperty(ref lfDoorTrimImgList, value); }
        }
        private ObservableCollection<ImageCapture> driverSeatImgList;
        [Ignore]

        public ObservableCollection<ImageCapture> DriverSeatImgList
        {
            get { return driverSeatImgList; }
            set { SetProperty(ref driverSeatImgList, value); }
        }

        private ObservableCollection<ImageCapture> passengerSeatImgList;
        [Ignore]

        public ObservableCollection<ImageCapture> PassengerSeatImgList
        {
            get { return passengerSeatImgList; }
            set { SetProperty(ref passengerSeatImgList, value); }
        }
        private ObservableCollection<ImageCapture> rearSeatImgList;
        [Ignore]

        public ObservableCollection<ImageCapture> RearSeatImgList
        {
            get { return rearSeatImgList; }
            set { SetProperty(ref rearSeatImgList, value); }
        }

        private ObservableCollection<ImageCapture> dashImgList;
        [Ignore]

        public ObservableCollection<ImageCapture> DashImgList
        {
            get { return dashImgList; }
            set { SetProperty(ref dashImgList, value); }
        }

        private ObservableCollection<ImageCapture> carpetImgList;
        [Ignore]
        public ObservableCollection<ImageCapture> CarpetImgList
        {
            get { return carpetImgList; }

            set { SetProperty(ref carpetImgList, value); }
        }


        public string internalTrimImgPathList;
        public string InternalTrimImgPathList
        {
            get { return string.Join("~", InternalTrimImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref internalTrimImgPathList, value); }
        }

        public string rRDoorTrimImgPathList;
        public string RRDoorTrimImgPathList
        {
            get { return string.Join("~", RRDoorTrimImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref rRDoorTrimImgPathList, value); }
        }

        public string lRDoorTrimImgPathList;
        public string LRDoorTrimImgPathList
        {
            get { return string.Join("~", LRDoorTrimImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref lRDoorTrimImgPathList, value); }
        }

        public string rFDoorTrimImgPathList;
        public string RFDoorTrimImgPathList
        {
            get { return string.Join("~", RFDoorTrimImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref rFDoorTrimImgPathList, value); }
        }

        public string lFDoorTrimImgPathList;
        public string LFDoorTrimImgPathList
        {
            get { return string.Join("~", LFDoorTrimImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref lFDoorTrimImgPathList, value); }
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

        public string rearSeatImgPathList;
        public string RearSeatImgPathList
        {
            get { return string.Join("~", RearSeatImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref rearSeatImgPathList, value); }
        }


        public string dashImgPathList;
        public string DashImgPathList
        {
            get { return string.Join("~", DashImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref dashImgPathList, value); }
        }


        public string carpetImgPathList;
        public string CarpetImgPathList
        {
            get { return string.Join("~", CarpetImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref carpetImgPathList, value); }
        }


        private string internalTrimComment;

        public string InternalTrimComment
        {
            get { return internalTrimComment; }
            set { SetProperty(ref internalTrimComment, value); }
        }

        private bool isInternalTrim;

        public bool IsInternalTrim
        {
            get { return isInternalTrim; }
            set { SetProperty(ref isInternalTrim, value); }
        }

        private bool isRRDoorTrim;

        public bool IsRRDoorTrim
        {
            get { return isRRDoorTrim; }
            set { SetProperty(ref isRRDoorTrim, value); }
        }

        private string rrDoorTrimComment;

        public string RRDoorTrimComment
        {
            get { return rrDoorTrimComment; }
            set { SetProperty(ref rrDoorTrimComment, value); }
        }

        private bool isLFDoorTrim;

        public bool IsLFDoorTrim
        {
            get { return isLFDoorTrim; }
            set { SetProperty(ref isLFDoorTrim, value); }
        }

        private string lfDoorTrimComment;

        public string LFDoorTrimComment
        {
            get { return lfDoorTrimComment; }
            set { SetProperty(ref lfDoorTrimComment, value); }
        }

        private string rfDoorTrimComment;

        public string RFDoorTrimComment
        {
            get { return rfDoorTrimComment; }
            set { SetProperty(ref rfDoorTrimComment, value); }
        }

        private bool isRFDoorTrim;

        public bool IsRFDoorTrim
        {
            get { return isRFDoorTrim; }
            set { SetProperty(ref isRFDoorTrim, value); }
        }

        private string lrDoorTrimComment;

        public string LRDoorTrimComment
        {
            get { return lrDoorTrimComment; }
            set { SetProperty(ref lrDoorTrimComment, value); }
        }

        private bool isLRDoorTrim;

        public bool IsLRDoorTrim
        {
            get { return isLRDoorTrim; }
            set { SetProperty(ref isLRDoorTrim, value); }
        }

        private bool isDriverSeat;

        public bool IsDriverSeat
        {
            get { return isDriverSeat; }
            set { SetProperty(ref isDriverSeat, value); }
        }

        private string driverSeatComment;

        public string DriverSeatComment
        {
            get { return driverSeatComment; }
            set { SetProperty(ref driverSeatComment, value); }
        }

        private bool isPassengerSeat;

        public bool IsPassengerSeat
        {
            get { return isPassengerSeat; }
            set { SetProperty(ref isPassengerSeat, value); }
        }

        private string passengerSeatComment;

        public string PassengerSeatComment
        {
            get { return passengerSeatComment; }
            set { SetProperty(ref passengerSeatComment, value); }
        }

        private bool isRearSeat;

        public bool IsRearSeat
        {
            get { return isRearSeat; }
            set { SetProperty(ref isRearSeat, value); }
        }

        private string rearSeatComment;

        public string RearSeatComment
        {
            get { return rearSeatComment; }
            set { SetProperty(ref rearSeatComment, value); }
        }

        private bool isDash;

        public bool IsDash
        {
            get { return isDash; }
            set { SetProperty(ref isDash, value); }
        }

        private string dashComment;

        public string DashComment
        {
            get { return dashComment; }
            set { SetProperty(ref dashComment, value); }
        }

        private bool isCarpet;

        public bool IsCarpet
        {
            get { return isCarpet; }
            set { SetProperty(ref isCarpet, value); }
        }
        private string carpetComment;

        public string CarpetComment
        {
            get { return carpetComment; }
            set { SetProperty(ref carpetComment, value); }
        }


    }
}
