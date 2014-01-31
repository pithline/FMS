using Eqstra.BusinessLogic.Enums;
using Microsoft.Practices.Prism.StoreApps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eqstra.BusinessLogic
{
   public class Task : ValidatableBindableBase
    {
        private string caseNumber;

        public string CaseNumber
        {
            get { return caseNumber; }
            set { caseNumber = value; }
        }

        private long cellNumber;

        public long CellNumber
        {
            get { return cellNumber; }
            set { SetProperty(ref cellNumber, value); }
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

        public string AllocatedTo
        {
            get { return allocatedTo; }
            set { SetProperty(ref allocatedTo, value); }
        }

        private DateTime confirmedDateTime;

        public DateTime ConfirmedDateTime
        {
            get { return confirmedDateTime; }
            set { SetProperty(ref confirmedDateTime, value); }
        }

        private string customerId;

        public string CustomerId
        {
            get { return customerId; }
            set { SetProperty(ref customerId, value); }
        }

        private string registrationNumber;

        public string RegistrationNumber
        {
            get { return registrationNumber; }
            set { SetProperty(ref registrationNumber, value); }
        }


      
      

    }
}
