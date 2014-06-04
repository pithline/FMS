using Eqstra.BusinessLogic.Enums;
using Microsoft.Practices.Prism.StoreApps;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
namespace Eqstra.BusinessLogic
{
    public class Task : ValidatableBindableBase
    {

        private string caseNumber;

        [PrimaryKey]
        [RestorableState]
        public string CaseNumber
        {
            get { return caseNumber; }
            set { caseNumber = value; }
        }
        private CaseTypeEnum caseType;
        public CaseTypeEnum CaseType
        {
            get { return caseType; }
            set { SetProperty(ref caseType, value); }
        }

        private string caseCategory;
        [RestorableState]
        public string CaseCategory
        {
            get { return caseCategory; }
            set { SetProperty(ref caseCategory, value); }
        }
        private DateTime statusDueDate;

        public DateTime StatusDueDate
        {
            get { return statusDueDate; }
            set { SetProperty(ref statusDueDate, value); }
        }

        private TaskStatusEnum status;

        public TaskStatusEnum Status
        {
            get { return status; }
            set { SetProperty(ref status, value); }
        }

        private string allocatedTo;
        [RestorableState]
        public string AllocatedTo
        {
            get { return allocatedTo; }
            set { SetProperty(ref allocatedTo, value); }
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

        private string registrationNumber;
        [RestorableState]
        public string RegistrationNumber
        {
            get { return registrationNumber; }
            set { SetProperty(ref registrationNumber, value); }
        }

        private string customerName;
        [Ignore]
        [RestorableState]
        public string CustomerName
        {
            get { return customerName; }
            set { SetProperty(ref customerName, value); }
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
        
        [Ignore]
        [RestorableState]
        public string DisplayStatus
        {
            get
            {
                var member = this.Status.GetType().GetRuntimeField(this.Status.ToString());
                return member.GetCustomAttribute<DisplayAttribute>().Name;                
            }
        }
    }
}
