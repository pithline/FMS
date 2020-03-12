using Eqstra.BusinessLogic.ServiceSchedule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eqstra.BusinessLogic;
namespace Eqstra.BusinessLogic
{
    public class StampPersistData
    {

        private static StampPersistData _instance = new StampPersistData();
        public static StampPersistData Instance { get { return _instance; } }
        public Eqstra.BusinessLogic.Task Task { get; set; }
        public Eqstra.BusinessLogic.CustomerDetails CustomerDetails { get; set; }
        public Eqstra.BusinessLogic.DataStamp DataStamp { get; set; }
        public static void RefreshInstance()
        {
            _instance = new StampPersistData();
        }

    }
}
