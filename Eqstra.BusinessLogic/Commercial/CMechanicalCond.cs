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
    public class CMechanicalCond : VIBase
    {
        public CMechanicalCond()
        {
            this.EngineImgList = new ObservableCollection<ImageCapture>();
            this.RearSuspImgList = new ObservableCollection<ImageCapture>();
            this.FrontSuspImgList = new ObservableCollection<ImageCapture>();
            this.SteeringImgList = new ObservableCollection<ImageCapture>();
            this.ExhaustImgList = new ObservableCollection<ImageCapture>();
            this.BatteryImgList = new ObservableCollection<ImageCapture>();
            this.HandBrakeImgList = new ObservableCollection<ImageCapture>();
            this.FootBrakeImgList = new ObservableCollection<ImageCapture>();
            this.GearboxImgList = new ObservableCollection<ImageCapture>();
            this.DifferentialImgList = new ObservableCollection<ImageCapture>();
            this.AutoTransmissionImgList = new ObservableCollection<ImageCapture>();
            this.OilLeaksImgList = new ObservableCollection<ImageCapture>();
            this.HPSImgList = new ObservableCollection<ImageCapture>();
                  
        }
        public async override Task<VIBase> GetDataAsync(string caseNumber)
        {
            return await SqliteHelper.Storage.GetSingleRecordAsync<CMechanicalCond>(x => x.CaseNumber == caseNumber);
        } 

        private string engineComment;

        public string EngineComment
        {
            get { return engineComment; }

            set { SetProperty(ref  engineComment, value); }
        }
        private bool isEngine;

        public bool IsEngine
        {
            get { return isEngine; }

            set { SetProperty(ref  isEngine, value); }
        }
        private ObservableCollection<ImageCapture> engineImgList;

        [Ignore]
        public ObservableCollection<ImageCapture> EngineImgList
        {
            get { return engineImgList; }

            set { SetProperty(ref  engineImgList, value); }
        }

        private string frontSuspComment;

        public string FrontSuspComment
        {
            get { return frontSuspComment; }

            set { SetProperty(ref  frontSuspComment, value); }
        }
        private bool isFrontSusp;

        public bool IsFrontSusp
        {
            get { return isFrontSusp; }

            set { SetProperty(ref  isFrontSusp, value); }
        }
        private ObservableCollection<ImageCapture> frontSuspImgList;

        [Ignore]
        public ObservableCollection<ImageCapture> FrontSuspImgList
        {
            get { return frontSuspImgList; }

            set { SetProperty(ref  frontSuspImgList, value); }
        }

        private string rearSuspComment;

        public string RearSuspComment
        {
            get { return rearSuspComment; }

            set { SetProperty(ref  rearSuspComment, value); }
        }
        private bool isRearSusp;

        public bool IsRearSusp
        {
            get { return isRearSusp; }

            set { SetProperty(ref  isRearSusp, value); }
        }
        private ObservableCollection<ImageCapture> rearSuspImgList;

        [Ignore]
        public ObservableCollection<ImageCapture> RearSuspImgList
        {
            get { return rearSuspImgList; }

            set { SetProperty(ref  rearSuspImgList, value); }
        }

        private string steeringComment;

        public string SteeringComment
        {
            get { return steeringComment; }

            set { SetProperty(ref  steeringComment, value); }
        }
        private bool isSteering;

        public bool IsSteering
        {
            get { return isSteering; }

            set { SetProperty(ref  isSteering, value); }
        }
        private ObservableCollection<ImageCapture> steeringImgList;

        [Ignore]
        public ObservableCollection<ImageCapture> SteeringImgList
        {
            get { return steeringImgList; }

            set { SetProperty(ref  steeringImgList, value); }
        }

 
        private string exhaustComment;

        public string ExhaustComment
        {
            get { return exhaustComment; }

            set { SetProperty(ref  exhaustComment, value); }
        }
        private bool isExhaust;

        public bool IsExhaust
        {
            get { return isExhaust; }

            set { SetProperty(ref  isExhaust, value); }
        }
        private ObservableCollection<ImageCapture> exhaustImgList;

        [Ignore]
        public ObservableCollection<ImageCapture> ExhaustImgList
        {
            get { return exhaustImgList; }

            set { SetProperty(ref  exhaustImgList, value); }
        }

        private string batteryComment;

        public string BatteryComment
        {
            get { return batteryComment; }

            set { SetProperty(ref  batteryComment, value); }
        }
        private bool isBattery;

        public bool IsBattery
        {
            get { return isBattery; }

            set { SetProperty(ref  isBattery, value); }
        }
        private ObservableCollection<ImageCapture> batteryImgList;

        [Ignore]
        public ObservableCollection<ImageCapture> BatteryImgList
        {
            get { return batteryImgList; }

            set { SetProperty(ref  batteryImgList, value); }
        }

        private string handBrakeComment;

        public string HandBrakeComment
        {
            get { return handBrakeComment; }

            set { SetProperty(ref  handBrakeComment, value); }
        }
        private bool isHandBrake;

        public bool IsHandBrake
        {
            get { return isHandBrake; }

            set { SetProperty(ref  isHandBrake, value); }
        }
        private ObservableCollection<ImageCapture> handBrakeImgList;

        [Ignore]
        public ObservableCollection<ImageCapture> HandBrakeImgList
        {
            get { return handBrakeImgList; }

            set { SetProperty(ref  handBrakeImgList, value); }
        }
    
        private string footBrakeComment;

        public string FootBrakeComment
        {
            get { return footBrakeComment; }

            set { SetProperty(ref  footBrakeComment, value); }
        }
        private bool isFootBrake;

        public bool IsFootBrake
        {
            get { return isFootBrake; }

            set { SetProperty(ref  isFootBrake, value); }
        }
        private ObservableCollection<ImageCapture> footBrakeImgList;

        [Ignore]
        public ObservableCollection<ImageCapture> FootBrakeImgList
        {
            get { return footBrakeImgList; }

            set { SetProperty(ref  footBrakeImgList, value); }
        }


        private string gearboxComment;

        public string GearboxComment
        {
            get { return gearboxComment; }

            set { SetProperty(ref  gearboxComment, value); }
        }
        private bool isGearbox;

        public bool IsGearbox
        {
            get { return isGearbox; }

            set { SetProperty(ref  isGearbox, value); }
        }
        private ObservableCollection<ImageCapture> gearboxImgList;

        [Ignore]
        public ObservableCollection<ImageCapture> GearboxImgList
        {
            get { return gearboxImgList; }

            set { SetProperty(ref  gearboxImgList, value); }
        }

        private string differentialComment;

        public string DifferentialComment
        {
            get { return differentialComment; }

            set { SetProperty(ref  differentialComment, value); }
        }
        private bool isDifferential;

        public bool IsDifferential
        {
            get { return isDifferential; }

            set { SetProperty(ref  isDifferential, value); }
        }
        private ObservableCollection<ImageCapture> differentialImgList;

        [Ignore]
        public ObservableCollection<ImageCapture> DifferentialImgList
        {
            get { return differentialImgList; }

            set { SetProperty(ref  differentialImgList, value); }
        }

        private string autoTransmissionComment;

        public string AutoTransmissionComment
        {
            get { return autoTransmissionComment; }

            set { SetProperty(ref  autoTransmissionComment, value); }
        }
        private bool isAutoTransmission;

        public bool IsAutoTransmission
        {
            get { return isAutoTransmission; }

            set { SetProperty(ref  isAutoTransmission, value); }
        }
        private ObservableCollection<ImageCapture> autoTransmissionImgList;

        [Ignore]
        public ObservableCollection<ImageCapture> AutoTransmissionImgList
        {
            get { return autoTransmissionImgList; }

            set { SetProperty(ref  autoTransmissionImgList, value); }
        }

        private string oilLeaksComment;

        public string OilLeaksComment
        {
            get { return oilLeaksComment; }

            set { SetProperty(ref  oilLeaksComment, value); }
        }
        private bool isOilLeaks;

        public bool IsOilLeaks
        {
            get { return isOilLeaks; }

            set { SetProperty(ref  isOilLeaks, value); }
        }
        private ObservableCollection<ImageCapture> oilLeaksImgList;

        [Ignore]
        public ObservableCollection<ImageCapture> OilLeaksImgList
        {
            get { return oilLeaksImgList; }

            set { SetProperty(ref  oilLeaksImgList, value); }
        }

        private string hPSComment;

        public string HPSComment
        {
            get { return hPSComment; }

            set { SetProperty(ref  hPSComment, value); }
        }
        private bool isHPS;

        public bool IsHPS
        {
            get { return isHPS; }

            set { SetProperty(ref  isHPS, value); }
        }
        private ObservableCollection<ImageCapture> hPSImgList;

        [Ignore]
        public ObservableCollection<ImageCapture> HPSImgList
        {
            get { return hPSImgList; }

            set { SetProperty(ref  hPSImgList, value); }
        }

   
        private bool isRunning;

        public bool IsRunning
        {
            get { return isRunning; }

            set { SetProperty(ref  isRunning, value); }
        }

        private bool hasPetrol;

        public bool HasPetrol
        {
            get { return hasPetrol; }

            set { SetProperty(ref  hasPetrol, value); }
        }

        private bool hasDiesel;

        public bool HasDiesel
        {
            get { return hasDiesel; }

            set { SetProperty(ref  hasDiesel, value); }
        }

        private bool hasLPG;

        public bool HasLPG
        {
            get { return hasLPG; }

            set { SetProperty(ref  hasLPG, value); }
        }

        private bool hasMainBattery;

        public bool HasMainBattery
        {
            get { return hasMainBattery; }

            set { SetProperty(ref  hasMainBattery, value); }
        }

        public string engineImgPathList;
        public string EngineImgPathList
        {
            get { return string.Join("~", EngineImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref engineImgPathList, value); }
        }

        public string rearSuspImgPathList;
        public string RearSuspImgPathList
        {
            get { return string.Join("~", RearSuspImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref rearSuspImgPathList, value); }
        }

        public string frontSuspImgPathList;
        public string FrontSuspImgPathList
        {
            get { return string.Join("~", FrontSuspImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref frontSuspImgPathList, value); }
        }

        public string steeringImgPathList;
        public string SteeringImgPathList
        {
            get { return string.Join("~", SteeringImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref steeringImgPathList, value); }
        }

        public string exhaustImgPathList;
        public string ExhaustImgPathList
        {
            get { return string.Join("~", ExhaustImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref exhaustImgPathList, value); }
        }

        public string batteryImgPathList;
        public string BatteryImgPathList
        {
            get { return string.Join("~", BatteryImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref batteryImgPathList, value); }
        }

        public string handBrakeImgPathList;
        public string HandBrakeImgPathList
        {
            get { return string.Join("~", HandBrakeImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref handBrakeImgPathList, value); }
        }

        public string footBrakeImgPathList;
        public string FootBrakeImgPathList
        {
            get { return string.Join("~", FootBrakeImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref footBrakeImgPathList, value); }
        }

        public string gearboxImgPathList;
        public string GearboxImgPathList
        {
            get { return string.Join("~", GearboxImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref gearboxImgPathList, value); }
        }

        public string differentialImgPathList;
        public string DifferentialImgPathList
        {
            get { return string.Join("~", DifferentialImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref differentialImgPathList, value); }
        }

        public string autoTransmissionImgPathList;
        public string AutoTransmissionImgPathList
        {
            get { return string.Join("~", AutoTransmissionImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref autoTransmissionImgPathList, value); }
        }

        public string oilLeaksImgPathList;
        public string OilLeaksImgPathList
        {
            get { return string.Join("~", OilLeaksImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref oilLeaksImgPathList, value); }
        }

        public string hPSImgPathList;
        public string HPSImgPathList
        {
            get { return string.Join("~", HPSImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref hPSImgPathList, value); }
        }
    }
}
