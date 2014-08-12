using Eqstra.BusinessLogic;
using Eqstra.BusinessLogic.DeliveryModel;
using Eqstra.BusinessLogic.DocumentDelivery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eqstra.DocumentDelivery.UILogic.Helpers
{
    public class PersistentData
    {
        private static PersistentData _instance = new PersistentData();
        public static PersistentData Instance { get { return _instance; } }
        public CDUserInfo UserInfo { get; set; }
        public CDCustomerDetails CustomerDetails { get; set; }
        public CollectDeliveryTask CollectDeliveryTask { get; set; }
        public static void RefreshInstance()
        {
            _instance = new PersistentData();
        }
    }
}
