using Eqstra.BusinessLogic.Helpers;
using Microsoft.Practices.Prism.StoreApps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eqstra.BusinessLogic.ServiceSchedule
{
    public class Address : ValidatableBindableBase
    {
        public Address()
        {
            this.Catalog = (AddressHelper.GetAddressCatalog()).Result;
        }
        private Catalog catalog;
        public Catalog Catalog
        {
            get { return catalog; }
            set { SetProperty(ref catalog, value); }
        }

        private string street;
        public string Street
        {
            get { return street; }
            set { SetProperty(ref street, value); }
        }
        private List<string> postcode;
        public List<string> Postcode
        {
            get { return postcode; }
            set { SetProperty(ref postcode, value); }
        }
    }
}
