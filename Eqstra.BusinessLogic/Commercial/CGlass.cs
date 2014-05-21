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
    public class CGlass : VIBase
    {
        public CGlass()
        {
            this.WindscreenImgList = new ObservableCollection<ImageCapture>();
            this.RearGlassImgList = new ObservableCollection<ImageCapture>();
            this.SideGlassImgList = new ObservableCollection<ImageCapture>();
            this.HeadLightsImgList = new ObservableCollection<ImageCapture>();
            this.TailLightsImgList = new ObservableCollection<ImageCapture>();
            this.InductorLensesImgList = new ObservableCollection<ImageCapture>();
            this.ExtRearViewMirrorImgList = new ObservableCollection<ImageCapture>();
        }
        public async override Task<VIBase> GetDataAsync(string caseNumber)
        {
            return await SqliteHelper.Storage.GetSingleRecordAsync<CGlass>(x => x.CaseNumber == caseNumber);
        } 

        private string windscreenComment;

        public string WindscreenComment
        {
            get { return windscreenComment; }

            set { SetProperty(ref  windscreenComment, value); }
        }
        private bool isWindscreen;

        public bool IsWindscreen
        {
            get { return isWindscreen; }

            set { SetProperty(ref  isWindscreen, value); }
        }
        private ObservableCollection<ImageCapture> windscreenImgList;

        [Ignore]
        public ObservableCollection<ImageCapture> WindscreenImgList
        {
            get { return windscreenImgList; }

            set { SetProperty(ref  windscreenImgList, value); }
        }
     
        private string rearGlassComment;

        public string RearGlassComment
        {
            get { return rearGlassComment; }

            set { SetProperty(ref  rearGlassComment, value); }
        }
        private bool isRearGlass;

        public bool IsRearGlass
        {
            get { return isRearGlass; }

            set { SetProperty(ref  isRearGlass, value); }
        }
        private ObservableCollection<ImageCapture> rearGlassImgList;

        [Ignore]
        public ObservableCollection<ImageCapture> RearGlassImgList
        {
            get { return rearGlassImgList; }

            set { SetProperty(ref  rearGlassImgList, value); }
        }

        private string sideGlassComment;

        public string SideGlassComment
        {
            get { return sideGlassComment; }

            set { SetProperty(ref  sideGlassComment, value); }
        }
        private bool isSideGlass;

        public bool IsSideGlass
        {
            get { return isSideGlass; }

            set { SetProperty(ref  isSideGlass, value); }
        }
        private ObservableCollection<ImageCapture> sideGlassImgList;

        [Ignore]
        public ObservableCollection<ImageCapture> SideGlassImgList
        {
            get { return sideGlassImgList; }

            set { SetProperty(ref  sideGlassImgList, value); }
        }

        private string headLightsComment;

        public string HeadLightsComment
        {
            get { return headLightsComment; }

            set { SetProperty(ref  headLightsComment, value); }
        }
        private bool isHeadLights;

        public bool IsHeadLights
        {
            get { return isHeadLights; }

            set { SetProperty(ref  isHeadLights, value); }
        }
        private ObservableCollection<ImageCapture> headLightsImgList;

        [Ignore]
        public ObservableCollection<ImageCapture> HeadLightsImgList
        {
            get { return headLightsImgList; }

            set { SetProperty(ref  headLightsImgList, value); }
        }

    
        private string tailLightsComment;

        public string TailLightsComment
        {
            get { return tailLightsComment; }

            set { SetProperty(ref  tailLightsComment, value); }
        }
        private bool isTailLights;

        public bool IsTailLights
        {
            get { return isTailLights; }

            set { SetProperty(ref  isTailLights, value); }
        }
        private ObservableCollection<ImageCapture> tailLightsImgList;

        [Ignore]
        public ObservableCollection<ImageCapture> TailLightsImgList
        {
            get { return tailLightsImgList; }

            set { SetProperty(ref  tailLightsImgList, value); }
        }

        private string inductorLensesComment;

        public string InductorLensesComment
        {
            get { return inductorLensesComment; }

            set { SetProperty(ref  inductorLensesComment, value); }
        }
        private bool isInductorLenses;

        public bool IsInductorLenses
        {
            get { return isInductorLenses; }

            set { SetProperty(ref  isInductorLenses, value); }
        }
        private ObservableCollection<ImageCapture> inductorLensesImgList;

        [Ignore]
        public ObservableCollection<ImageCapture> InductorLensesImgList 
        {
            get { return inductorLensesImgList; }

            set { SetProperty(ref  inductorLensesImgList, value); }
        }

        private string extRearViewMirrorToggleComment;

        public string ExtRearViewMirrorToggleComment
        {
            get { return extRearViewMirrorToggleComment; }

            set { SetProperty(ref  extRearViewMirrorToggleComment, value); }
        }
        private bool isExtRearViewMirrorToggle;

        public bool IsExtRearViewMirrorToggle
        {
            get { return isExtRearViewMirrorToggle; }

            set { SetProperty(ref  isExtRearViewMirrorToggle, value); }
        }
        private ObservableCollection<ImageCapture> extRearViewMirrorImgList;

        [Ignore]
        public ObservableCollection<ImageCapture> ExtRearViewMirrorImgList 
        {
            get { return extRearViewMirrorImgList; }

            set { SetProperty(ref  extRearViewMirrorImgList, value); }
        }


        public string windscreenImgPathList;
        public string WindscreenImgPathList
        {
            get { return string.Join("~", WindscreenImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref windscreenImgPathList, value); }
        }

        public string rearGlassImgPathList;
        public string RearGlassImgPathList
        {
            get { return string.Join("~", RearGlassImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref rearGlassImgPathList, value); }
        }

        public string sideGlassImgPathList;
        public string SideGlassImgPathList
        {
            get { return string.Join("~", SideGlassImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref sideGlassImgPathList, value); }
        }

        public string headLightsImgPathList;
        public string HeadLightsImgPathList
        {
            get { return string.Join("~", HeadLightsImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref headLightsImgPathList, value); }
        }

        public string tailLightsImgPathList;
        public string TailLightsImgPathList
        {
            get { return string.Join("~", TailLightsImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref tailLightsImgPathList, value); }
        }

        public string inductorLensesImgPathList;
        public string InductorLensesImgPathList
        {
            get { return string.Join("~", InductorLensesImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref inductorLensesImgPathList, value); }
        }

        public string extRearViewMirrorImgPathList;
        public string ExtRearViewMirrorImgPathList
        {
            get { return string.Join("~", ExtRearViewMirrorImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref extRearViewMirrorImgPathList, value); }
        }
      
    }
}
