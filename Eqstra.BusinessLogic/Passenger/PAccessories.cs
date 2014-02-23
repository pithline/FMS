using Microsoft.Practices.Prism.StoreApps;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eqstra.BusinessLogic.Passenger
{
   public class PAccessories : ValidatableBindableBase
    {
        private bool hasRadio;

        public bool HasRadio
        {
            get { return hasRadio; }
            set { SetProperty(ref hasRadio, value); }
        }

        private bool isRadioDmg;

        public bool IsRadioDmg
        {
            get { return isRadioDmg; }
            set { SetProperty(ref isRadioDmg, value); }
        }

        private string radioComment;

        public string RadioComment
        {
            get { return radioComment; }
            set { SetProperty(ref radioComment, value); }
        }

        private ObservableCollection<ImageCapture> radioImgList;

        public ObservableCollection<ImageCapture> RadioImgList
        {
            get { return radioImgList; }
            set { SetProperty(ref radioImgList, value); }
        }

        private bool hasCDShuffle;

        public bool HasCDShuffle
        {
            get { return hasCDShuffle; }
            set { SetProperty(ref hasCDShuffle, value); }
        }

        private bool isCDShuffleDmg;

        public bool IsCDShuffleDmg
        {
            get { return isCDShuffleDmg; }
            set { SetProperty(ref isCDShuffleDmg, value); }
        }

        private string cdShuffleComment;

        public string CDShuffleComment
        {
            get { return cdShuffleComment; }
            set { SetProperty(ref cdShuffleComment, value); }
        }
        private ObservableCollection<ImageCapture> cdShuffleImgList;

        public ObservableCollection<ImageCapture> CDShuffleImgList
        {
            get { return cdShuffleImgList; }
            set { SetProperty(ref cdShuffleImgList, value); }
        }

        private bool hasNavigation;

        public bool HasNavigation
        {
            get { return hasNavigation; }
            set { SetProperty(ref hasNavigation, value); }
        }

        private bool isNavigationDmg;

        public bool IsNavigationDmg
        {
            get { return isNavigationDmg; }
            set { SetProperty(ref isNavigationDmg, value); }
        }

        private string navigationComment;

        public string NavigationComment
        {
            get { return navigationComment; }
            set { SetProperty(ref navigationComment, value); }
        }

        private ObservableCollection<ImageCapture> navigationImgList;

        public ObservableCollection<ImageCapture> NavigationImgList
        {
            get { return navigationImgList; }
            set { SetProperty(ref navigationImgList, value); }
        }

        private bool hasAircon;

        public bool HasAircon
        {
            get { return hasAircon; }
            set { SetProperty(ref hasAircon, value); }
        }

        private bool isAirconDmg;

        public bool IsAirconDmg
        {
            get { return isAirconDmg; }
            set { SetProperty(ref isAirconDmg, value); }
        }

        private string airconComment;

        public string AirconComment
        {
            get { return airconComment; }
            set { SetProperty(ref airconComment, value); }
        }

        private ObservableCollection<ImageCapture> airconImgList;

        public ObservableCollection<ImageCapture> AirconImgList
        {
            get { return airconImgList; }
            set { SetProperty(ref airconImgList, value); }
        }

        private bool hasAlarm;

        public bool HasAlarm
        {
            get { return hasAlarm; }
            set { SetProperty(ref hasAlarm, value); }
        }

        private bool isAlarmDmg;

        public bool IsAlarmDmg
        {
            get { return isAlarmDmg; }
            set { SetProperty(ref isAlarmDmg, value); }
        }

        private string alarmComment;

        public string AlarmComment
        {
            get { return alarmComment; }
            set { SetProperty(ref alarmComment, value); }
        }
        private ObservableCollection<ImageCapture> alarmImgList;

        public ObservableCollection<ImageCapture> AlarmImgList
        {
            get { return alarmImgList; }
            set { SetProperty(ref alarmImgList, value); }
        }

        private bool hasKey;

        public bool HasKey
        {
            get { return hasKey; }
            set { SetProperty(ref hasKey, value); }
        }

        private bool isKeyDmg;

        public bool IsKeyDmg
        {
            get { return isKeyDmg; }
            set { SetProperty(ref isKeyDmg, value); }
        }

        private string keyComment;

        public string KeyComment
        {
            get { return keyComment; }
            set { SetProperty(ref keyComment, value); }
        }
        private ObservableCollection<ImageCapture> keyImgList;

        public ObservableCollection<ImageCapture> KeyImgList
        {
            get { return keyImgList; }
            set { SetProperty(ref keyImgList, value); }
        }

        private bool hasSpareKeys;

        public bool HasSpareKeys
        {
            get { return hasSpareKeys; }
            set { SetProperty(ref hasSpareKeys, value); }
        }

        private bool isSpareKeysDmg;

        public bool IsSpareKeysDmg
        {
            get { return isSpareKeysDmg; }
            set { SetProperty(ref isSpareKeysDmg, value); }
        }
        private string spareKeysComment;

        public string SpareKeysComment
        {
            get { return spareKeysComment; }
            set { SetProperty(ref spareKeysComment, value); }
        }

        private ObservableCollection<ImageCapture> spareKeysImgList;

        public ObservableCollection<ImageCapture> SpareKeysImgList
        {
            get { return spareKeysImgList; }
            set { SetProperty(ref spareKeysImgList, value); }
        }

        private bool hasServicesBook;

        public bool HasServicesBook
        {
            get { return hasServicesBook; }
            set { SetProperty(ref hasServicesBook, value); }
        }
        private bool isServicesBookDmg;

        public bool IsServicesBookDmg
        {
            get { return isServicesBookDmg; }
            set { SetProperty(ref isServicesBookDmg, value); }
        }
        private string servicesBookComment;

        public string ServicesBookComment
        {
            get { return servicesBookComment; }
            set { SetProperty(ref servicesBookComment, value); }
        }
        private ObservableCollection<ImageCapture> servicesBookImgList;

        public ObservableCollection<ImageCapture> ServicesBookImgList
        {
            get { return servicesBookImgList; }
            set { SetProperty(ref servicesBookImgList, value); }
        }

        private bool hasSpareTyre;

        public bool HasSpareTyre
        {
            get { return hasSpareTyre; }
            set { SetProperty(ref hasSpareTyre, value); }
        }

        private bool isSpareTyreDmg;

        public bool IsSpareTyreDmg
        {
            get { return isSpareTyreDmg; }
            set { SetProperty(ref isSpareTyreDmg, value); }
        }

        private string spareTyreComment;

        public string SpareTyreComment
        {
            get { return spareTyreComment; }
            set { SetProperty(ref spareTyreComment, value); }
        }

        private ObservableCollection<ImageCapture> spareTyreImgList;

        public ObservableCollection<ImageCapture> SpareTyreImgList
        {
            get { return spareTyreImgList; }
            set { SetProperty(ref spareTyreImgList, value); }
        }

        private bool hasTools;

        public bool HasTools
        {
            get { return hasTools; }
            set { SetProperty(ref hasTools, value); }
        }

        private bool isToolsDmg;

        public bool IsToolsDmg
        {
            get { return isToolsDmg; }
            set { SetProperty(ref isToolsDmg, value); }
        }

        private string toolsComment;

        public string ToolsComment
        {
            get { return toolsComment; }
            set { SetProperty(ref toolsComment, value); }
        }

        private ObservableCollection<ImageCapture> toolsImgList;

        public ObservableCollection<ImageCapture> ToolsImgList
        {
            get { return toolsImgList; }
            set { SetProperty(ref toolsImgList, value); }
        }

        private bool hasJack;

        public bool HasJack
        {
            get { return hasJack; }
            set { SetProperty(ref hasJack, value); }
        }

        private bool isJackDmg;

        public bool IsJackDmg
        {
            get { return isJackDmg; }
            set { SetProperty(ref isJackDmg, value); }
        }

        private string jackComment;

        public string JackComment
        {
            get { return jackComment; }
            set { SetProperty(ref jackComment, value); }
        }

        private ObservableCollection<ImageCapture> jackImgList;

        public ObservableCollection<ImageCapture> JackImgList
        {
            get { return jackImgList; }
            set { SetProperty(ref jackImgList, value); }
        }

        private bool hasCanopy;

        public bool HasCanopy
        {
            get { return hasCanopy; }
            set { SetProperty(ref hasCanopy, value); }
        }

        private bool isCanopyDmg;

        public bool IsCanopyDmg
        {
            get { return isCanopyDmg; }
            set { SetProperty(ref isCanopyDmg, value); }
        }

        private string canopyComment;

        public string CanopyComment
        {
            get { return canopyComment; }
            set { SetProperty(ref canopyComment, value); }
        }

        private ObservableCollection<ImageCapture> canopyImgList;

        public ObservableCollection<ImageCapture> CanopyImgList
        {
            get { return canopyImgList; }
            set { SetProperty(ref canopyImgList, value); }
        }

        private bool hasTrackingUnit;

        public bool HasTrackingUnit
        {
            get { return hasTrackingUnit; }
            set { SetProperty(ref hasTrackingUnit, value); }
        }

        private bool isTrackingUnitDmg;

        public bool IsTrackingUnitDmg
        {
            get { return isTrackingUnitDmg; }
            set { SetProperty(ref isTrackingUnitDmg, value); }
        }

        private string trackingUnitComment;

        public string TrackingUnitComment
        {
            get { return trackingUnitComment; }
            set { SetProperty(ref trackingUnitComment, value); }
        }

        private ObservableCollection<ImageCapture> trackingUnitImgList;

        public ObservableCollection<ImageCapture> TrackingUnitImgList
        {
            get { return trackingUnitImgList; }
            set { SetProperty(ref trackingUnitImgList, value); }
        }

        private bool hasMags;

        public bool HasMags
        {
            get { return hasMags; }
            set { SetProperty(ref hasMags, value); }
        }

        private bool isMagsDmg;

        public bool IsMagsDmg
        {
            get { return isMagsDmg; }
            set { SetProperty(ref isMagsDmg, value); }
        }

        private string magsComment;

        public string MagsComment
        {
            get { return magsComment; }
            set { SetProperty(ref magsComment, value); }
        }

        private ObservableCollection<ImageCapture> magsImgList;

        public ObservableCollection<ImageCapture> MagsImgList
        {
            get { return magsImgList; }
            set { SetProperty(ref magsImgList, value); }
        }

        private bool isOthers;

        public bool IsOthers
        {
            get { return isOthers; }
            set { SetProperty(ref isOthers, value); }
        }

        private string othersComment;

        public string OthersComment
        {
            get { return othersComment; }
            set { SetProperty(ref othersComment, value); }
        }


    }
}
