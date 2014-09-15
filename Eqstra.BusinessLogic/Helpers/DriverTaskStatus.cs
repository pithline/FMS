using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eqstra.BusinessLogic.Helpers
{
    public class DriverTaskStatus
    {
        public const string AwaitServiceDetail = "Await Service Booking Detail";
        public const string AwaitSupplierSelection = "Await Supplier Selection";
        public const string AwaitServiceConfirmation = "Await Service Booking Confirmation";
        public const string AwaitJobCardCapture = "Await Job Card Capture";
        public const string Completed = "Completed";
    }
}
