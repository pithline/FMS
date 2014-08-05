using Eqstra.BusinessLogic.Base;
using Eqstra.BusinessLogic.Helpers;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eqstra.BusinessLogic.DocumentDelivery
{
    public class CDProof : BaseModel
    {
        public CDProof()
        {
            this.Customers = (SqliteHelper.Storage.LoadTableAsync<DestinationContacts>()).Result;
        }

        private List<DestinationContacts> customers;
        [Ignore]
        public List<DestinationContacts> Customers
        {
            get { return customers; }
            set { SetProperty(ref customers, value); }
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

        private string collectedFrom;

        public string CollectedFrom
        {
            get { return collectedFrom; }
            set { SetProperty(ref collectedFrom, value); }
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
        public override Task<BaseModel> GetDataAsync(long vehicleInsRecID)
        {
            throw new NotImplementedException();
        }
    }
}
