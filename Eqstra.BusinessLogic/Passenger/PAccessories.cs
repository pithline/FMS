﻿using Eqstra.BusinessLogic.Base;
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
    public class PAccessories : VIBase
    {
        public PAccessories()
        {

            this.RadioImgList = new ObservableCollection<ImageCapture>();
            this.CDShuffleImgList = new ObservableCollection<ImageCapture>();
            this.KeyImgList = new ObservableCollection<ImageCapture>();
            this.NavigationImgList = new ObservableCollection<ImageCapture>();
            this.AirconImgList = new ObservableCollection<ImageCapture>();
            this.AlarmImgList = new ObservableCollection<ImageCapture>();

            this.MagsImgList = new ObservableCollection<ImageCapture>();
            this.TrackingUnitImgList = new ObservableCollection<ImageCapture>();
            this.CanopyImgList = new ObservableCollection<ImageCapture>();
            this.JackImgList = new ObservableCollection<ImageCapture>();
            this.ToolsImgList = new ObservableCollection<ImageCapture>();

            this.SpareKeysImgList = new ObservableCollection<ImageCapture>();
            this.ServicesBookImgList = new ObservableCollection<ImageCapture>();
            this.SpareTyreImgList = new ObservableCollection<ImageCapture>();
        }

        public async override Task<VIBase> GetDataAsync(string caseNumber)
        {
            return await SqliteHelper.Storage.GetSingleRecordAsync<PAccessories>(x => x.CaseNumber == caseNumber);
        }
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
        [Ignore, DamageSnapshotRequired("Radio snapshot(s) required", "IsRadioDmg")]
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
        [Ignore, DamageSnapshotRequired("CD shuffle snapshot(s) required", "IsCDShuffleDmg")]
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
        [Ignore, DamageSnapshotRequired("Navigation snapshot(s) required", "IsNavigationDmg")]
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
        [Ignore, DamageSnapshotRequired("Aircon snapshot(s) required", "IsAirconDmg")]
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
        [Ignore, DamageSnapshotRequired("Alarm snapshot(s) required", "IsAlarmDmg")]
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
        [Ignore, DamageSnapshotRequired("Key snapshot(s) required", "IsKeyDmg")]
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
        [Ignore, DamageSnapshotRequired("Spare keys snapshot(s) required", "IsSpareKeysDmg")]
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
        [Ignore, DamageSnapshotRequired("Services book snapshot(s) required", "IsServicesBookDmg")]
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
        [Ignore, DamageSnapshotRequired("Spare tyre snapshot(s) required", "IsSpareTyreDmg")]
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
        [Ignore, DamageSnapshotRequired("Tools snapshot(s) required", "IsToolsDmg")]
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
        [Ignore, DamageSnapshotRequired("Jack snapshot(s) required", "IsJackDmg")]
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
        [Ignore, DamageSnapshotRequired("Canopy snapshot(s) required", "IsCanopyDmg")]
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
        [Ignore, DamageSnapshotRequired("TrackingUnit snapshot(s) required", "IsTrackingUnitDmg")]
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
        [Ignore, DamageSnapshotRequired("Mags snapshot(s) required", "IsMagsDmg")]
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

        public string radioImgPathList;
        public string RadioImgPathList
        {
            get { return string.Join("~", RadioImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref radioImgPathList, value); }
        }

        public string cDShuffleImgPathList;
        public string CDShuffleImgPathList
        {
            get { return string.Join("~", CDShuffleImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref cDShuffleImgPathList, value); }
        }

        public string keyImgPathList;
        public string KeyImgPathList
        {
            get { return string.Join("~", KeyImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref keyImgPathList, value); }
        }

        public string navigationImgPathList;
        public string NavigationImgPathList
        {
            get { return string.Join("~", NavigationImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref navigationImgPathList, value); }
        }

        public string airconImgPathList;
        public string AirconImgPathList
        {
            get { return string.Join("~", AirconImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref airconImgPathList, value); }
        }

        public string alarmImgPathList;
        public string AlarmImgPathList
        {
            get { return string.Join("~", AlarmImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref alarmImgPathList, value); }
        }

        public string magsImgPathList;
        public string MagsImgPathList
        {
            get { return string.Join("~", MagsImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref magsImgPathList, value); }
        }

        public string trackingUnitImgPathList;
        public string TrackingUnitImgPathList
        {
            get { return string.Join("~", TrackingUnitImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref trackingUnitImgPathList, value); }
        }

        public string canopyImgPathList;
        public string CanopyImgPathList
        {
            get { return string.Join("~", CanopyImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref canopyImgPathList, value); }
        }

        public string jackImgPathList;
        public string JackImgPathList
        {
            get { return string.Join("~", JackImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref jackImgPathList, value); }
        }

        public string toolsImgPathList;
        public string ToolsImgPathList
        {
            get { return string.Join("~", ToolsImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref toolsImgPathList, value); }
        }

        public string spareKeysImgPathList;
        public string SpareKeysImgPathList
        {
            get { return string.Join("~", SpareKeysImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref spareKeysImgPathList, value); }
        }

        public string servicesBookImgPathList;
        public string ServicesBookImgPathList
        {
            get { return string.Join("~", ServicesBookImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref servicesBookImgPathList, value); }
        }

        public string spareTyreImgPathList;
        public string SpareTyreImgPathList
        {
            get { return string.Join("~", SpareTyreImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref spareTyreImgPathList, value); }
        }
    }
}
