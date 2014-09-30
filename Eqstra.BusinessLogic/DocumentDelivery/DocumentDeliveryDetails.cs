using Eqstra.BusinessLogic.Base;
using Eqstra.BusinessLogic.Helpers;
using Microsoft.Practices.Prism.StoreApps;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eqstra.BusinessLogic.DocumentDelivery
{
    public class DocumentDeliveryDetails : ValidatableBindableBase
    {
        public DocumentDeliveryDetails()
        {
            this.ContactPersons = (SqliteHelper.Storage.LoadTableAsync<ContactPerson>()).Result;
            this.CollectedFrom = (SqliteHelper.Storage.LoadTableAsync<CollectedFromData>()).Result;
        }

        private string caseNumber;
        [PrimaryKey]
        public string CaseNumber
        {
            get { return caseNumber; }
            set { SetProperty(ref caseNumber, value); }
        }

        private long caseCategoryRecID;
        public long CaseCategoryRecID
        {
            get { return caseCategoryRecID; }
            set { SetProperty(ref caseCategoryRecID, value); }
        }

        private List<ContactPerson> contactPersons;
        [Ignore]
        public List<ContactPerson> ContactPersons
        {
            get { return contactPersons; }
            set { SetProperty(ref contactPersons, value); }
        }

        private List<CollectedFromData> collectedFrom;
        [Ignore]
        public List<CollectedFromData> CollectedFrom
        {
            get { return collectedFrom; }
            set { SetProperty(ref collectedFrom, value); }
        }

        private string selectedCollectedFrom;
        public string SelectedCollectedFrom
        {
            get { return selectedCollectedFrom; }
            set { SetProperty(ref selectedCollectedFrom, value); }
        }


        private string deliveryPersonName;
        public string DeliveryPersonName
        {
            get { return deliveryPersonName; }
            set { SetProperty(ref deliveryPersonName, value); }
        }

        private string cRSignature;

        public string CRSignature
        {
            get { return cRSignature; }
            set { SetProperty(ref cRSignature, value); }
        }



        private string deliveredAt;

        public string DeliveredAt
        {
            get { return deliveredAt; }
            set { SetProperty(ref deliveredAt, value); }
        }

        private DateTime collectedAt;

        public DateTime CollectedAt
        {
            get { return collectedAt; }
            set { SetProperty(ref collectedAt, value); }
        }

        private string comment;

        public string Comment
        {
            get { return comment; }
            set { SetProperty(ref comment, value); }
        }

        private string email;

        public string Email
        {
            get { return email; }
            set { SetProperty(ref email, value); }
        }

        private string position;

        public string Position
        {
            get { return position; }
            set { SetProperty(ref position, value); }
        }

        private string receivedBy;

        public string ReceivedBy
        {
            get { return receivedBy; }
            set { SetProperty(ref receivedBy, value); }
        }

        private string referenceNo;

        public string ReferenceNo
        {
            get { return referenceNo; }
            set { SetProperty(ref referenceNo, value); }
        }

        private DateTime receivedDate;

        public DateTime ReceivedDate
        {
            get { return receivedDate; }
            set { SetProperty(ref receivedDate, value); }
        }

        private string phone;

        public string Phone
        {
            get { return phone; }
            set { SetProperty(ref phone, value); }
        }

        private bool isCollected;

        public bool IsCollected
        {
            get { return isCollected; }
            set { SetProperty(ref isCollected, value); }
        }
        private bool isDelivered;

        public bool IsDelivered
        {
            get { return isDelivered; }
            set { SetProperty(ref isDelivered, value); }
        }
        private DateTime deliveryDate;

        public DateTime DeliveryDate
        {
            get { return deliveryDate; }
            set { SetProperty(ref deliveryDate, value); }
        }


    }
}
