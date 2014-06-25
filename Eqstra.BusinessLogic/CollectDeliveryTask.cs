using Eqstra.BusinessLogic.Enums;
using Microsoft.Practices.Prism.StoreApps;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Eqstra.BusinessLogic
{
    public class CollectDeliveryTask : ValidatableBindableBase
    {

        private long vehicleInsRecId;
        [PrimaryKey]
        public long VehicleInsRecId
        {
            get { return vehicleInsRecId; }
            set { SetProperty(ref vehicleInsRecId, value); }

        }
        private string caseNumber;

        [RestorableState]
        public string CaseNumber
        {
            get { return caseNumber; }
            set { caseNumber = value; }

        }

        private string customerName;
        [RestorableState]
        public string CustomerName
        {
            get { return customerName; }
            set { SetProperty(ref customerName, value); }
        }
        private string cdTaskStatus;
        public string CDTaskStatus
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
        [RestorableState]
        public int DocumentCount
        {
            get { return documentCount; }
            set { SetProperty(ref documentCount, value); }
        }

        private string status;

        public string Status
        {
            get { return status; }
            set { SetProperty(ref status, value); }
        }

        private string address;
        [RestorableState]
        public string Address
        {
            get { return address; }
            set { SetProperty(ref address, value); }
        }

        private DateTime confirmedTime;

        public DateTime ConfirmedTime
        {
            get { return confirmedTime; }
            set { SetProperty(ref confirmedTime, value); }
        }

        private string allocatedTo;
        [RestorableState]
        public string AllocatedTo
        {
            get { return allocatedTo; }
            set { SetProperty(ref allocatedTo, value); }
        }
        private CaseTypeEnum caseType;
        public CaseTypeEnum CaseType
        {
            get { return caseType; }
            set { SetProperty(ref caseType, value); }
        }
        private DateTime confirmedDate;
        public DateTime ConfirmedDate
        {
            get { return confirmedDate; }
            set { SetProperty(ref confirmedDate, value); }
        }

        private string customerId;
        [RestorableState]
        public string CustomerId
        {
            get { return customerId; }
            set { SetProperty(ref customerId, value); }
        }

        private DateTime statusDueDate;
        public DateTime StatusDueDate
        {
            get { return statusDueDate; }
            set { SetProperty(ref statusDueDate, value); }
        }


    }
}


