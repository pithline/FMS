using Eqstra.BusinessLogic.Helpers;
using Microsoft.Practices.Prism.PubSubEvents;
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

        private string street;
        public string Street
        {
            get { return street; }
            set { SetProperty(ref street, value); }
        }
        private string postcode;
        public string Postcode
        {
            get { return postcode; }
            set { SetProperty(ref postcode, value); }
        }
        private string country;
        public string Country
        {
            get { return country; }
            set { SetProperty(ref country, value); }
        }

        private string province;
        public string Province
        {
            get { return province; }
            set { SetProperty(ref province, value); }
        }
        private string city;
        public string City
        {
            get { return city; }
            set { SetProperty(ref city, value); }
        }

        private string suburb;
        public string Suburb
        {
            get { return suburb; }
            set { SetProperty(ref suburb, value); }
        }
    }

    public class AddressEvent : PubSubEvent<Address>
    {
    }
}
