using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eqstra.BusinessLogic
{
   public interface IVehicleDetails : IValidatableBindableBase
    {
         string Color{get;set;}

         bool IsLicenseDiscCurrent{get;set;}

         string ODOReading{get;set;}

         string RegistrationNumber{get;set;}

         string EngineNumber{get;set;}

         DateTime LicenseDiscExpireDate{get;set;}

         string ChassisNumber{get;set;}

         string Make{get;set;}

         bool IsSpareKeysTested{get;set;}        

         string Year{get;set;}       

         ImageCapture LicenseDiscSnapshot{get;set;}       

         ImageCapture ODOReadingSnapshot{get;set;}

         ImageCapture LeftSnapshot{get;set;}        

         ImageCapture BackSnapshot{get;set;}       

         ImageCapture RightSnapshot{get;set;}

         ImageCapture FrontSnapshot{get;set;}

         ImageCapture TopSnapshot{get;set;}
    }
}
