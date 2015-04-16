using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace Eqstra.BusinessLogic.Portable.TIModels
{
    public class ImageCapture : BindableBase
    {
        public long VehicleInspecId { get; set; }

        public string CaseNumber { get; set; }

        public long RepairId { get; set; }

        public string Component { get; set; }

        public string ImagePath { get; set; }
   
        public string FileName { get; set; }

        public string ImageData { get; set; }

        private BitmapImage imageBitmap;
        public BitmapImage ImageBitmap
        {
            get { return imageBitmap; }
            set { SetProperty(ref imageBitmap, value); }
        }
    }
}
