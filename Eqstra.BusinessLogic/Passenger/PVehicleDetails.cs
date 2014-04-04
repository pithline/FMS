using Microsoft.Practices.Prism.StoreApps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eqstra.BusinessLogic.Passenger
{
 public   class PVehicleDetails :  ValidatableBindableBase, IVehicleDetails
    {

     public PVehicleDetails()
     {
         this.LeftSnapshot = new ImageCapture { ImagePath = "ms-appx:///Assets/car_left.png" };
         this.BackSnapshot = new ImageCapture{ImagePath ="ms-appx:///Assets/car_back.png" };
         this.RightSnapshot = new ImageCapture { ImagePath = "ms-appx:///Assets/car_right.png" };
         this.FrontSnapshot = new ImageCapture { ImagePath = "ms-appx:///Assets/car_front.png" };
         this.TopSnapshot = new ImageCapture { ImagePath = "ms-appx:///Assets/car_top.png" };
         this.LicenseDiscSnapshot = new ImageCapture { ImagePath = "ms-appx:///Assets/license_disc.png" };
         this.ODOReadingSnapshot = new ImageCapture { ImagePath = "ms-appx:///Assets/ODO_meter.png" };
         this.ODOReading = "490292";
         this.registrationNumber = "HU 29209";
         this.EngineNumber = "0DG445G9332";
         this.LicenseDiscExpireDate = DateTime.Now;
         this.ChassisNumber = "99382302-8494";
         this.Make = "Audi A1";
         this.Year = "2014";
         this.Color = "Black";
     }

     private string color;

     public string Color
     {
         get { return color; }
         set { SetProperty(ref color, value); }
     }


     private bool isLicenseDiscCurrent;

     public bool IsLicenseDiscCurrent
     {
         get { return isLicenseDiscCurrent; }
         set { SetProperty(ref isLicenseDiscCurrent, value); }
     }

     private string odoReading;

     public string ODOReading
     {
         get { return odoReading; }
         set { SetProperty(ref odoReading, value); }
     }

     private string registrationNumber;

     public string RegistrationNumber
     {
         get { return registrationNumber; }
         set { SetProperty(ref registrationNumber, value); }
     }

     private string engineNumber;

     public string EngineNumber
     {
         get { return engineNumber; }
         set { SetProperty(ref engineNumber, value); }
     }

     private DateTime licenseDiscExpiryDate;

     public DateTime LicenseDiscExpireDate
     {
         get { return licenseDiscExpiryDate; }
         set { SetProperty(ref licenseDiscExpiryDate, value); }
     }

     private string chassisNumber;

     public string ChassisNumber
     {
         get { return chassisNumber; }
         set { SetProperty(ref chassisNumber, value); }
     }

     private string make;

     public string Make
     {
         get { return make; }
         set { SetProperty(ref make, value); }
     }

     private bool isSpareKeysShown;

     public bool IsSpareKeysShown
     {
         get { return isSpareKeysShown; }
         set { SetProperty(ref isSpareKeysShown, value); }
     }

     private bool isSpareKeysTested;

     public bool IsSpareKeysTested
     {
         get { return isSpareKeysTested; }
         set { SetProperty(ref isSpareKeysTested, value); }
     }

     private string year;

     public string Year
     {
         get { return year; }
         set { SetProperty(ref year, value); }
     }


     private ImageCapture licenseDiscSnapshot;

     public ImageCapture LicenseDiscSnapshot
     {
         get { return licenseDiscSnapshot; }
         set { SetProperty(ref licenseDiscSnapshot, value); }
     }

     private ImageCapture odoReadingSnapshot;

     public ImageCapture ODOReadingSnapshot
     {
         get { return odoReadingSnapshot; }
         set { SetProperty(ref odoReadingSnapshot, value); }
     }


        private ImageCapture leftSnapshot;

        public ImageCapture LeftSnapshot
        {
            get { return leftSnapshot; }
            set { SetProperty(ref leftSnapshot, value); }
        }

        private ImageCapture backSnapshot;

        public ImageCapture BackSnapshot
        {
            get { return backSnapshot; }
            set { SetProperty(ref backSnapshot, value); }
        }

        private ImageCapture rightSnapshot;

        public ImageCapture RightSnapshot
        {
            get { return rightSnapshot; }
            set { SetProperty(ref rightSnapshot, value); }
        }

        private ImageCapture frontSnapshot;

        public ImageCapture FrontSnapshot
        {
            get { return frontSnapshot; }
            set { SetProperty(ref frontSnapshot, value); }
        }

        private ImageCapture topSnapshot;

        public ImageCapture TopSnapshot
        {
            get { return topSnapshot; }
            set { SetProperty(ref topSnapshot, value); }
        }

    }
}
