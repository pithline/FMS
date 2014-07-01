using Microsoft.Practices.Prism.StoreApps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eqstra.BusinessLogic.ServiceSchedule
{
    public class Supplier : ValidatableBindableBase
    {
        private string supplierName;

        public string SupplierName
        {
            get { return supplierName; }
            set { SetProperty(ref supplierName, value); }
        }

        private string supplierContactName;

        public string SupplierContactName
        {
            get { return supplierContactName; }
            set { SetProperty(ref supplierContactName, value); }
        }

        private string supplierContactNumber;

        public string SupplierContactNumber
        {
            get { return supplierContactNumber; }
            set { SetProperty(ref  supplierContactNumber, value); }
        }

        private DateTime supplierDate;

        public DateTime SupplierDate
        {
            get { return supplierDate; }
            set { SetProperty(ref supplierDate, value); }
        }

        private DateTime supplierTime;

        public DateTime SupplierTime
        {
            get { return supplierTime; }
            set { SetProperty(ref supplierTime, value); }
        }

    }
}
