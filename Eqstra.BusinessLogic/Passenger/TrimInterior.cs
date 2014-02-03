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
        
    }
}
