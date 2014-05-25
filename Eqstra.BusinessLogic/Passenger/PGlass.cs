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
    public class PGlass : VIBase
    {
        public PGlass()
        {
            this.GVWindscreenImgList = new ObservableCollection<ImageCapture>();
            this.GVRearGlassImgList = new ObservableCollection<ImageCapture>();
            this.GVHeadLightsImgList = new ObservableCollection<ImageCapture>();
            this.GVSideGlassImgList = new ObservableCollection<ImageCapture>();
            this.GVHeadLightsImgList = new ObservableCollection<ImageCapture>();
            this.GVTailLightsImgList = new ObservableCollection<ImageCapture>();
            this.GVInductorLensesImgList = new ObservableCollection<ImageCapture>();
            this.GVExtRearViewMirrorImgList = new ObservableCollection<ImageCapture>();
        }

        public async override Task<VIBase> GetDataAsync(string caseNumber)
        {
            return await SqliteHelper.Storage.GetSingleRecordAsync<PGlass>(x => x.CaseNumber == caseNumber);
        }

        private ObservableCollection<ImageCapture> gVWindscreenImgList;
        [Ignore]
        public ObservableCollection<ImageCapture> GVWindscreenImgList
        {
            get { return gVWindscreenImgList; }
            set { SetProperty(ref  gVWindscreenImgList, value); }
        }

        private ObservableCollection<ImageCapture> gVRearGlassImgList;
        [Ignore]
        public ObservableCollection<ImageCapture> GVRearGlassImgList
        {
            get { return gVRearGlassImgList; }
            set { SetProperty(ref  gVRearGlassImgList, value); }
        }

        private ObservableCollection<ImageCapture> gVSideGlassImgList;
        [Ignore]
        public ObservableCollection<ImageCapture> GVSideGlassImgList
        {
            get { return gVSideGlassImgList; }
            set { SetProperty(ref  gVSideGlassImgList, value); }
        }
        private ObservableCollection<ImageCapture> gVHeadLightsImgList;
        [Ignore]
        public ObservableCollection<ImageCapture> GVHeadLightsImgList
        {
            get { return gVHeadLightsImgList; }
            set { SetProperty(ref  gVHeadLightsImgList, value); }
        }
        private ObservableCollection<ImageCapture> gVTailLightsImgList;
        [Ignore]
        public ObservableCollection<ImageCapture> GVTailLightsImgList
        {
            get { return gVTailLightsImgList; }
            set { SetProperty(ref  gVTailLightsImgList, value); }
        }
        private ObservableCollection<ImageCapture> gVInductorLensesImgList;
        [Ignore]
        public ObservableCollection<ImageCapture> GVInductorLensesImgList
        {
            get { return gVInductorLensesImgList; }
            set { SetProperty(ref  gVInductorLensesImgList, value); }
        }
        private ObservableCollection<ImageCapture> gVExtRearViewMirrorImgList;
        [Ignore]
        public ObservableCollection<ImageCapture> GVExtRearViewMirrorImgList
        {
            get { return gVExtRearViewMirrorImgList; }
            set { SetProperty(ref  gVExtRearViewMirrorImgList, value); }
        }

        private string gVWindscreenComment;

        public string GVWindscreenComment
        {
            get { return gVWindscreenComment; }

            set { SetProperty(ref  gVWindscreenComment, value); }
        }
        private string gVRearGlassComment;

        public string GVRearGlassComment
        {
            get { return gVRearGlassComment; }

            set { SetProperty(ref  gVRearGlassComment, value); }
        }
        private string gVSideGlassComment;

        public string GVSideGlassComment
        {
            get { return gVSideGlassComment; }

            set { SetProperty(ref  gVSideGlassComment, value); }
        }
        private string gVHeadLightsComment;

        public string GVHeadLightsComment
        {
            get { return gVHeadLightsComment; }

            set { SetProperty(ref  gVHeadLightsComment, value); }
        }
        private string gVTailLightsComment;

        public string GVTailLightsComment
        {
            get { return gVTailLightsComment; }

            set { SetProperty(ref  gVTailLightsComment, value); }
        }
        private string gVInductorLensesComment;

        public string GVInductorLensesComment
        {
            get { return gVInductorLensesComment; }

            set { SetProperty(ref  gVInductorLensesComment, value); }
        }
        private string gVExtRearViewMirrorToggleComment;

        public string GVExtRearViewMirrorToggleComment
        {
            get { return gVExtRearViewMirrorToggleComment; }

            set { SetProperty(ref  gVExtRearViewMirrorToggleComment, value); }
        }


        private bool isWindscreen;

        public bool IsWindscreen
        {
            get { return isWindscreen; }

            set { SetProperty(ref  isWindscreen, value); }
        }
        private bool isRearGlass;

        public bool IsRearGlass
        {
            get { return isRearGlass; }

            set { SetProperty(ref  isRearGlass, value); }
        }
        private bool isSideGlassToggle;

        public bool IsSideGlassToggle
        {
            get { return isSideGlassToggle; }

            set { SetProperty(ref  isSideGlassToggle, value); }
        }
        private bool isHeadLightsToggle;

        public bool IsHeadLightsToggle
        {
            get { return isHeadLightsToggle; }

            set { SetProperty(ref  isHeadLightsToggle, value); }
        }
        private bool isTailLightsToggle;

        public bool IsTailLightsToggle
        {
            get { return isTailLightsToggle; }

            set { SetProperty(ref  isTailLightsToggle, value); }
        }
        private bool isInductorLensesToggle;

        public bool IsInductorLensesToggle
        {
            get { return isInductorLensesToggle; }

            set { SetProperty(ref  isInductorLensesToggle, value); }
        }
        private bool isExtRearViewMirrorToggle;

        public bool IsExtRearViewMirrorToggle
        {
            get { return isExtRearViewMirrorToggle; }

            set { SetProperty(ref  isExtRearViewMirrorToggle, value); }
        }


        public string gVWindscreenImgPathList;
        public string GVWindscreenImgPathList
        {
            get { return string.Join("~", GVWindscreenImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref gVWindscreenImgPathList, value); }
        }

        public string gVRearGlassImgPathList;
        public string GVRearGlassImgPathList
        {
            get { return string.Join("~", GVRearGlassImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref gVRearGlassImgPathList, value); }
        }


        public string gVSideGlassImgPathList;
        public string GVSideGlassImgPathList
        {
            get { return string.Join("~", GVSideGlassImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref gVSideGlassImgPathList, value); }
        }

        public string gVHeadLightsImgPathList;
        public string GVHeadLightsImgPathList
        {
            get { return string.Join("~", GVHeadLightsImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref gVHeadLightsImgPathList, value); }
        }

        public string gVTailLightsImgPathList;
        public string GVTailLightsImgPathList
        {
            get { return string.Join("~", GVTailLightsImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref gVTailLightsImgPathList, value); }
        }

        public string gVInductorLensesImgPathList;
        public string GVInductorLensesImgPathList
        {
            get { return string.Join("~", GVInductorLensesImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref gVInductorLensesImgPathList, value); }
        }

        public string gVExtRearViewMirrorImgPathList;
        public string GVExtRearViewMirrorImgPathList
        {
            get { return string.Join("~", GVExtRearViewMirrorImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref gVExtRearViewMirrorImgPathList, value); }
        }

    }
}
