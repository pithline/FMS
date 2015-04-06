using Eqstra.BusinessLogic;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eqstra.BusinessLogic.Portable.SSModels
{
    public class ServiceSchedulingDetail : BindableBase
    {
        public ServiceSchedulingDetail()
        {
            this.OdoReadingImageCapture = new ImageCapture { ImagePath = "ms-appx:///Assets/ODO_meter.png", ImageBitmap = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri("ms-appx:///Assets/ODO_meter.png")) };
        }
        
        public String CaseNumber { get; set; }

        public Int64 CaseServiceRecID { get; set; }

        public string ODOReadingSnapshot { get; set; }

        private ImageCapture odoReadingImageCapture;
        public ImageCapture OdoReadingImageCapture
        {
            get { return odoReadingImageCapture; }
            set { SetProperty(ref odoReadingImageCapture, value); }
        }

        public Int64 ODOReading { get; set; }

        public string ODOReadingDate { get; set; }

        public List<string> ServiceType { get; set; }

        public String ContactPersonName { get; set; }

        public String ContactPersonPhone { get; set; }

        public String SupplierName { get; set; }

        public String EventDesc { get; set; }

        public List<DestinationType> DestinationTypes { get; set; }

        public List<LocationType> LocationTypes { get; set; }

        public String Address { get; set; }

        public String AdditionalWork { get; set; }

        public string ServiceDateOption1 { get; set; }

        public string ServiceDateOption2 { get; set; }

        public DateTime SupplierDateTime { get; set; }

        public Boolean IsLiftRequired { get; set; }

        public LocationType SelectedLocType { get; set; }

        public LocationType SelectedLocationType { get; set; }

        public DestinationType SelectedDestinationType { get; set; }

        public String SelectedServiceType { get; set; }

        public string ConfirmedDate { get; set; }

        private DateTime serviceDateOpt1;
        public DateTime ServiceDateOpt1
        {
            get { return serviceDateOpt1; }
            set { SetProperty(ref serviceDateOpt1, value); }
        }


        private DateTime serviceDateOpt2;
        public DateTime ServiceDateOpt2
        {
            get { return serviceDateOpt2; }
            set { SetProperty(ref serviceDateOpt2, value); }
        }

      
    }
}
