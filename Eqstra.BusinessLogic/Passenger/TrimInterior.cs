using Microsoft.Practices.Prism.StoreApps;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eqstra.BusinessLogic.Passenger
{
    public class TrimInterior : ValidatableBindableBase
    {
        private ObservableCollection<ImageCapture> internalTrimImgList;
        public ObservableCollection<ImageCapture> InternalTrimImgList
        {
            get { return internalTrimImgList; }
            set { SetProperty(ref internalTrimImgList, value); }
        }

        private ObservableCollection<ImageCapture> rrDoorTrimiImgList;

        public ObservableCollection<ImageCapture> RRDoorTrimImgList
        {
            get { return rrDoorTrimiImgList; }
            set { SetProperty(ref rrDoorTrimiImgList, value); }
        }

        private ObservableCollection<ImageCapture> lrDoorTrimImgList;

        public ObservableCollection<ImageCapture> LRDoorTrimImgList
        {
            get { return lrDoorTrimImgList; }
            set { SetProperty(ref lrDoorTrimImgList, value); }
        }

        private ObservableCollection<ImageCapture> rfDoorTrimImgList;

        public ObservableCollection<ImageCapture> RFDoorTrimImgList
        {
            get { return rfDoorTrimImgList; }
            set { SetProperty(ref rfDoorTrimImgList, value); }
        }

        private ObservableCollection<ImageCapture> lfDoorTrimImgList;

        public ObservableCollection<ImageCapture> LFDoorTrimImgList
        {
            get { return lfDoorTrimImgList; }
            set { SetProperty(ref lfDoorTrimImgList, value); }
        }
        private ObservableCollection<ImageCapture> driverSeatImgList;

        public ObservableCollection<ImageCapture> DriverSeatImgList
        {
            get { return driverSeatImgList; }
            set { SetProperty(ref driverSeatImgList, value); }
        }

        private ObservableCollection<ImageCapture> passengerSeatImgList;

        public ObservableCollection<ImageCapture> PassengerSeatImgList
        {
            get { return passengerSeatImgList; }
            set { SetProperty(ref passengerSeatImgList, value); }
        }
        private ObservableCollection<ImageCapture> rearSeatImgList;

        public ObservableCollection<ImageCapture> RearSeatImgList
        {
            get { return rearSeatImgList; }
            set { SetProperty(ref rearSeatImgList, value); }
        }

        private ObservableCollection<ImageCapture> dashImgList;

        public ObservableCollection<ImageCapture> DashImgList
        {
            get { return dashImgList; }
            set { SetProperty(ref dashImgList, value); }
        }

        private ObservableCollection<ImageCapture> carpetImgList;

        public ObservableCollection<ImageCapture> CarpetImgList
        {
            get { return carpetImgList; }
            set { SetProperty(ref carpetImgList, value); }
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
