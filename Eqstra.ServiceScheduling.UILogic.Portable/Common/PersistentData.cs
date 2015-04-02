
using Eqstra.BusinessLogic.Portable.SSModels;
using System.Collections.ObjectModel;
namespace Eqstra.ServiceScheduling.UILogic
{
    public class PersistentData
    {
        private static PersistentData _instance = new PersistentData();
        public static PersistentData Instance { get { return _instance; } }
        public UserInfo UserInfo { get; set; }
        public BusinessLogic.Portable.SSModels.Task Task { get; set; }
        public ObservableCollection<BusinessLogic.Portable.SSModels.Task> Tasks { get; set; }
        public static void RefreshInstance()
        {
            _instance = new PersistentData();
        }

        public ObservableCollection<BusinessLogic.Portable.SSModels.Task> PoolofTasks { get; set; }

        public ObservableCollection<Supplier> PoolofSupplier { get; set; }
    }
}
