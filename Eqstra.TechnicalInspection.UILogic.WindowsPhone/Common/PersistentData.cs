
using Eqstra.BusinessLogic.Portable.SSModels;
using Eqstra.BusinessLogic.Portable.TIModels;
using System.Collections.ObjectModel;

namespace Eqstra.TechnicalInspection.UILogic
{
    public class PersistentData
    {
        private static PersistentData _instance = new PersistentData();
        public static PersistentData Instance { get { return _instance; } }
        public ObservableCollection<TITask> Tasks { get; set; }
        public static void RefreshInstance()
        {
            _instance = new PersistentData();
        }
        public ObservableCollection<TITask> PoolofTasks { get; set; }
        public ObservableCollection<Supplier> PoolofSupplier { get; set; }
    }
}
