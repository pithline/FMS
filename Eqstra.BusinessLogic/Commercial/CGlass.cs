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
    public class CGlass : BaseModel
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
       
        public async override Task<BaseModel> GetDataAsync(long vehicleInsRecID)
        {
            return await SqliteHelper.Storage.GetSingleRecordAsync<CGlass>(x => x.VehicleInsRecID == vehicleInsRecID);
        }

        private string windscreenComment;

        public string WindscreenComment
        {
            get { return windscreenComment; }

            set { SetProperty(ref  windscreenComment, value); }
        }
        private bool isWindscreenDmg;

        public bool IsWindscreenDmg
        {
            get { return isWindscreenDmg; }

            set { SetProperty(ref  isWindscreenDmg, value); }
        }
        private ObservableCollection<ImageCapture> windscreenImgList;

        [Ignore, DamageSnapshotRequired("Wind screen snapshot(s) required", "IsWindscreenDmg")]
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
        private bool isRearGlassDmg;

        public bool IsRearGlassDmg
        {
            get { return isRearGlassDmg; }

            set { SetProperty(ref  isRearGlassDmg, value); }
        }
        private ObservableCollection<ImageCapture> rearGlassImgList;

        [Ignore, DamageSnapshotRequired("Rear glass snapshot(s) required", "IsRearGlassDmg")]
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
        private bool isSideGlassDmg;

        public bool IsSideGlassDmg
        {
            get { return isSideGlassDmg; }

            set { SetProperty(ref  isSideGlassDmg, value); }
        }
        private ObservableCollection<ImageCapture> sideGlassImgList;

        [Ignore, DamageSnapshotRequired("Side glass snapshot(s) required", "IsSideGlassDmg")]
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
        private bool isHeadLightsDmg;

        public bool IsHeadLightsDmg
        {
            get { return isHeadLightsDmg; }

            set { SetProperty(ref  isHeadLightsDmg, value); }
        }
        private ObservableCollection<ImageCapture> headLightsImgList;

        [Ignore, DamageSnapshotRequired("Head lights snapshot(s) required", "IsHeadLightsDmg")]
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
        private bool isTailLightsDmg;

        public bool IsTailLightsDmg
        {
            get { return isTailLightsDmg; }

            set { SetProperty(ref  isTailLightsDmg, value); }
        }
        private ObservableCollection<ImageCapture> tailLightsImgList;

        [Ignore, DamageSnapshotRequired("Tail lights snapshot(s) required", "IsTailLightsDmg")]
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
        private bool isInductorLensesDmg;

        public bool IsInductorLensesDmg
        {
            get { return isInductorLensesDmg; }

            set { SetProperty(ref  isInductorLensesDmg, value); }
        }
        private ObservableCollection<ImageCapture> inductorLensesImgList;

        [Ignore, DamageSnapshotRequired("Inductor lenses snapshot(s) required", "IsInductorLensesDmg")]
        public ObservableCollection<ImageCapture> InductorLensesImgList
        {
            get { return inductorLensesImgList; }

            set { SetProperty(ref  inductorLensesImgList, value); }
        }

        private string extRearViewMirrorComment;

        public string ExtRearViewMirrorComment
        {
            get { return extRearViewMirrorComment; }

            set { SetProperty(ref  extRearViewMirrorComment, value); }
        }
        private bool isExtRearViewMirrorDmg;

        public bool IsExtRearViewMirrorDmg
        {
            get { return isExtRearViewMirrorDmg; }

            set { SetProperty(ref  isExtRearViewMirrorDmg, value); }
        }
        private ObservableCollection<ImageCapture> extRearViewMirrorImgList;

        [Ignore, DamageSnapshotRequired("Ext rear view mirror snapshot(s) required", "IsExtRearViewMirrorDmg")]
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
