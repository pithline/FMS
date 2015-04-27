
using Eqstra.BusinessLogic.Portable.SSModels;
using Eqstra.BusinessLogic.Portable.TIModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Eqstra.TechnicalInspection.UILogic
{
    public class PersistentData
    {
        private PersistentData()
        {

        }
        private static PersistentData _instance = new PersistentData();
        public static PersistentData Instance { get { return _instance; } }
        public static void RefreshInstance()
        {
            _instance = new PersistentData();
        }
        public ObservableCollection<TITask> PoolofTasks { get; set; }
        public MaintenanceRepair SelectedMaintenanceRepair { get; set; }
        public Dictionary<long, MaintenanceRepair> MaintenanceRepairKVPair { get; set; }
    }
}
