using Eqstra.BusinessLogic.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eqstra.BusinessLogic
{
   public class CollectDeliveryTask : Task
    {
        private CDTaskStatusEnum cdTaskStatus;

        public CDTaskStatusEnum CDTaskStatus
        {
            get { return cdTaskStatus; }
            set { SetProperty(ref cdTaskStatus, value); }
        }

        private CDTaskTypeEnum taskType;

        public CDTaskTypeEnum TaskType
        {
            get { return taskType; }
            set { SetProperty(ref taskType, value); }
        }

        private DateTime deliveryDate;

        public DateTime DeliveryDate
        {
            get { return deliveryDate; }
            set { SetProperty(ref deliveryDate, value); }
        }

        private DateTime deliveryTime;

        public DateTime DeliveryTime
        {
            get { return deliveryTime; }
            set { SetProperty(ref deliveryTime, value); }
        }

        private int documentCount;

        public int DocumentCount
        {
            get { return documentCount; }
            set { SetProperty(ref documentCount, value); }
        }

    }
}
