using Eqstra.BusinessLogic.Base;
using Eqstra.BusinessLogic.Helpers;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eqstra.BusinessLogic.DeliveryModel
{
    public class ProofOfCollection : BaseModel
    {
         public ProofOfCollection()
        {
            this.AddCustomer = (SqliteHelper.Storage.LoadTableAsync<AddCustomer>()).Result;
        }

        private List<AddCustomer> addCustomer;
        [Ignore]
        public List<AddCustomer> AddCustomer
        {
            get { return addCustomer; }
            set { SetProperty(ref addCustomer, value); }
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
