using Microsoft.Practices.Prism.StoreApps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Eqstra.BusinessLogic
{

    public class ImageCapture
    {
        public int Id { get; set; }

        public string ImagePath { get; set; }

        private string imageBinary;

        public string ImageBinary { get; set; }

        public string FileName { get; set; }

        public long CaseServiceRecId { get; set; }

        public long PrimeId { get; set; }

        public bool IsSynced { get; set; }

    }
}
