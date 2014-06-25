using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eqstra.BusinessLogic.Helpers
{
    public class TaskStatus
    {
        public const string AwaitInspectionDetail = "Await Inspection Detail";
        public const string AwaitInspectionDataCapture = "Await Inspection Data Capture";
        public const string AwaitingConfirmation = "Await Inspection Confirmation";
        public const string AwaitInspectionAcceptance = "Await Inspection Acceptance";
        public const string AwaitDamageConfirmation = "Await Damage Confirmation";
        public const string AwaitCollectionDetail = "Await Collection Detail";
        public const string AwaitVehicleCollection = "Await Vehicle Collection";
        public const string Completed = "Completed";
    }
}
