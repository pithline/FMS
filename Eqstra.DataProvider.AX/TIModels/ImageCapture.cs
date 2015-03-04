using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eqstra.DataProvider.AX.TIModels
{
  public  class ImageCapture
    {
        public long VehicleInspecId { get; set; }

        public string CaseNumber { get; set; }

        public long RepairId { get; set; }

        public string Component { get; set; }

        public string ImageData { get; set; }
    }
}
