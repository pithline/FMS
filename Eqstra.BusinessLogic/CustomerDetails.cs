using Eqstra.BusinessLogic.Enums;
using Microsoft.Practices.Prism.StoreApps;
using SQLite;
using Syncfusion.UI.Xaml.Schedule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eqstra.BusinessLogic
{
    public class CustomerDetails : ValidatableBindableBase
    {
        private string name;

        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }

        private string contactNumber;

        public string ContactNumber
        {
            get { return contactNumber; }
            set { SetProperty(ref contactNumber, value); }
        }

        private string address;

        public string Address
        {
            get { return address; }
            set { SetProperty(ref address, value); }
        }

        private string emailId;

        public string EmailId
        {
            get { return emailId; }
            set { SetProperty(ref emailId, value); }
        }

        private ScheduleAppointmentCollection appointments;
        [Ignore]
        public ScheduleAppointmentCollection Appointments
        {
            get { return appointments; }
            set { SetProperty(ref appointments, value); }
        }

        private string caseNumber;

        public string CaseNumber
        {
            get { return caseNumber; }
            set { SetProperty(ref caseNumber, value); }
        }

        private CaseTypeEnum caseType;

        public CaseTypeEnum CaseType
        {
            get { return caseType; }
            set { SetProperty(ref caseType, value); }
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
    }
}
