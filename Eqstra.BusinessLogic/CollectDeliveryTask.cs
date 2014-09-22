﻿using Eqstra.BusinessLogic.Enums;
using Microsoft.Practices.Prism.PubSubEvents;
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
        public CollectDeliveryTask()
        {
            this.Make = "45755";
            this.Model = "ASD457";
        }

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
        private string customerNumber;
        public string CustomerNumber
        {
            get { return customerNumber; }
            set { SetProperty(ref customerNumber, value); }
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

        private CDTaskType taskType;
        public CDTaskType TaskType
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

        private string emailId;
        public string EmailId
        {
            get { return emailId; }
            set { SetProperty(ref emailId, value); }
        }

        private DateTime deliveryTime;
        public DateTime DeliveryTime
        {
            get { return deliveryTime; }
            set { SetProperty(ref deliveryTime, value); }
        }


        private long custPartyId;

        public long CustPartyId
        {
            get { return custPartyId; }
            set { SetProperty(ref custPartyId, value); }
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

        private string registrationNumber;
        public string RegistrationNumber
        {
            get { return registrationNumber; }
            set { SetProperty(ref registrationNumber, value); }
        }
        private string make;
        public string Make
        {
            get { return make; }
            set { SetProperty(ref make, value); }
        }

        private string model;
        public string Model
        {
            get { return model; }
            set { SetProperty(ref model, value); }
        }

        private long caseRecID;

        public long CaseRecID
        {
            get { return caseRecID; }
            set { SetProperty(ref caseRecID, value); }
        }


        private long caseServiceRecID;

        public long CaseServiceRecID
        {
            get { return caseServiceRecID; }
            set { SetProperty(ref caseServiceRecID, value); }
        }


        private string noOfRecords;

        public string NoOfRecords
        {
            get { return noOfRecords; }
            set { SetProperty(ref noOfRecords, value); }
        }

        private string serviceId;

        public string ServiceId
        {
            get { return serviceId; }
            set { SetProperty(ref serviceId, value); }
        }
        private long serviceRecID;

        public long ServiceRecID
        {
            get { return serviceRecID; }
            set { SetProperty(ref serviceRecID, value); }
        }

        private string userID;

        public string UserID
        {
            get { return userID; }
            set { SetProperty(ref userID, value); }
        }

        private string custAccount;

        public string CustAccount
        {
            get { return custAccount; }
            set { SetProperty(ref custAccount, value); }
        }

    }

    public class TasksFetchedEvent : PubSubEvent<CollectDeliveryTask>
    {

    }
}


