using Eqstra.BusinessLogic.ServiceSchedule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eqstra.BusinessLogic;

namespace Eqstra.TechnicalInspection
{
    public class PersistentData
    {

        private static PersistentData _instance = new PersistentData();
        public static PersistentData Instance { get { return _instance; } }
        public Eqstra.BusinessLogic.TITask Task { get; set; }
        public Eqstra.BusinessLogic.CustomerDetails CustomerDetails { get; set; }
        public static void RefreshInstance()
        {
            _instance = new PersistentData();
        }

    }
}
