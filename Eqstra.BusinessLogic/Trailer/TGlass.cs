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

namespace Eqstra.BusinessLogic
{
    public class TGlass : BaseModel
    {
        public TGlass()
        {
            this.GVTailLightsImgList = new ObservableCollection<ImageCapture>();
            this.GVInductorLensesImgList = new ObservableCollection<ImageCapture>();
            this.ReflectorsImgList = new ObservableCollection<ImageCapture>();
            this.ShouldSave = false;
        }

        public async override Task<BaseModel> GetDataAsync(long vehicleInsRecID)
        {
            return await SqliteHelper.Storage.GetSingleRecordAsync<TGlass>(x => x.VehicleInsRecID == vehicleInsRecID);
        }

        private ObservableCollection<ImageCapture> gVTailLightsImgList;
        [Ignore, DamageSnapshotRequired("Tail lights snapshot(s) required", "IsTailLights")]
        public ObservableCollection<ImageCapture> GVTailLightsImgList
        {
            get { return gVTailLightsImgList; }
            set { SetProperty(ref  gVTailLightsImgList, value); }
        }
        private ObservableCollection<ImageCapture> gVInductorLensesImgList;
        [Ignore, DamageSnapshotRequired("Inductor lenses snapshot(s) required", "IsInductorLenses")]
        public ObservableCollection<ImageCapture> GVInductorLensesImgList
        {
            get { return gVInductorLensesImgList; }
            set { SetProperty(ref  gVInductorLensesImgList, value); }
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

        private bool isTailLights;

        public bool IsTailLights
        {
            get { return isTailLights; }

            set { SetProperty(ref  isTailLights, value); }
        }
        private bool isInductorLenses;

        public bool IsInductorLenses
        {
            get { return isInductorLenses; }

            set { SetProperty(ref  isInductorLenses, value); }
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

        private ObservableCollection<ImageCapture> reflectorsImgList;
        [Ignore, DamageSnapshotRequired("Reflectors snapshot(s) required", "IsReflectors")]
        public ObservableCollection<ImageCapture> ReflectorsImgList
        {
            get { return reflectorsImgList; }
            set { SetProperty(ref  reflectorsImgList, value); }
        }
        private string reflectorsComment;

        public string ReflectorsComment
        {
            get { return reflectorsComment; }

            set { SetProperty(ref  reflectorsComment, value); }
        }

        private bool isReflectors;

        public bool IsReflectors
        {
            get { return isReflectors; }

            set { SetProperty(ref  isReflectors, value); }
        }


        public string reflectorsImgPathList;
        public string ReflectorsImgPathList
        {
            get { return string.Join("~", reflectorsImgList.Select(x => x.ImagePath)); }
            set { SetProperty(ref reflectorsImgPathList, value); }
        }

    }
}
