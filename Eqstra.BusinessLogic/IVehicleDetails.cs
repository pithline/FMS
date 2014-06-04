using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Eqstra.BusinessLogic
{
   public interface IVehicleDetails : IValidatableBindableBase
   {
       [RestorableState]
         string Color{get;set;}
        [RestorableState]
         bool IsLicenseDiscCurrent{get;set;}
        [RestorableState]
         string ODOReading{get;set;}
        [RestorableState]
         string RegistrationNumber{get;set;}
        [RestorableState]
         string EngineNumber{get;set;}
        [RestorableState]
         DateTime LicenseDiscExpireDate{get;set;}
        [RestorableState]
         string ChassisNumber{get;set;}
        [RestorableState]
         string Make{get;set;}
        [RestorableState]
         bool IsSpareKeysTested{get;set;}
        [RestorableState]
         string Year{get;set;}
        [RestorableState]
         ImageCapture LicenseDiscSnapshot{get;set;}
        [RestorableState]
         ImageCapture ODOReadingSnapshot{get;set;}
        [RestorableState]
         ImageCapture LeftSnapshot{get;set;}
        [RestorableState]
         ImageCapture BackSnapshot{get;set;}
        [RestorableState]
         ImageCapture RightSnapshot{get;set;}
        [RestorableState]
         ImageCapture FrontSnapshot{get;set;}
        [RestorableState]
         ImageCapture TopSnapshot{get;set;}
    }
}
