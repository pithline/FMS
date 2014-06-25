using Microsoft.Practices.Prism.StoreApps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eqstra.BusinessLogic.ServiceSchedule
{
    public class SupplierSelection : ValidatableBindableBase
    {

        private List<string> country;
        public List<string> Country
        {
            get { return country; }
            set { SetProperty(ref country, value); }
        }

        private List<string> suburb;

        public List<string> Suburb
        {
            get { return suburb; }
            set { SetProperty(ref suburb, value); }
        }

        private List<string> city;

        public List<string> City
        {
            get { return city; }
            set { SetProperty(ref city, value); }
        }

        private List<string> province;

        public List<string> Province
        {
            get { return province; }
            set { SetProperty(ref province, value); }
        }

        private List<string> region;

        public List<string> Region
        {
            get { return region; }
            set { SetProperty(ref region, value); }
        }

        private Suppliers suppliers;

        public Suppliers Suppliers
        {
            get { return suppliers; }
            set { SetProperty(ref suppliers, value); }
        }

    }

    public class Suppliers : ValidatableBindableBase
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
       
    }
}