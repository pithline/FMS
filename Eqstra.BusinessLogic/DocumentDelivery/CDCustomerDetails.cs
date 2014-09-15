using Eqstra.BusinessLogic.Enums;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.StoreApps;
using SQLite;
using Syncfusion.UI.Xaml.Schedule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eqstra.BusinessLogic.DocumentDelivery
{
    public class CDCustomerDetails : ValidatableBindableBase
    {
        private long vehicleInsRecId;
        [PrimaryKey]
        public long VehicleInsRecId
        {
            get { return vehicleInsRecId; }
            set { SetProperty(ref vehicleInsRecId, value); }

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
        
        private string customerName;
        public string CustomerName
        {
            get { return customerName; }
            set { SetProperty(ref customerName, value); }
        }
        private string customerNumber;
        public string CustomerNumber
        {
            get { return customerNumber; }
            set { SetProperty(ref customerNumber, value); }
        }

        private string address;
        public string Address
        {
            get { return address; }
            set { SetProperty(ref address, value); }
        }

        private string registrationNumber;
        public string RegistrationNumber
        {
            get { return registrationNumber; }
            set { SetProperty(ref registrationNumber, value); }
        }

        private string makeModel;
        public string MakeModel
        {
            get { return makeModel; }
            set { SetProperty(ref makeModel, value); }
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

    }

     public class CustomerDetailsEvent : PubSubEvent<CDCustomerDetails>
     {

     }
}
