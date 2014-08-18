using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eqstra.DataProvider.AX.SSModels
{
    public class Address
    {
        public Int64 EntityRecId { get; set; }

        public String Street { get; set; }

        public List<string> Postcodes { get; set; }

        public List<Country> Countries { get; set; }

        public List<Province> Provinces { get; set; }

        public List<City> Cities { get; set; }

        public List<Suburb> Suburbs { get; set; }

        public List<Region> Region { get; set; }

        public Country SelectedCountry { get; set; }

        public Province Selectedprovince { get; set; }

        public City SelectedCity { get; set; }

        public Suburb SelectedSuburb { get; set; }

        public String SelectedZip { get; set; }

    }
}
