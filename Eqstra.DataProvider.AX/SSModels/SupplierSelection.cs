using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eqstra.DataProvider.AX.SSModels
{
    public class SupplierSelection
    {
        public List<Country> Countries { get; set; }

        public List<Province> Provinces { get; set; }

        public List<City> Cities { get; set; }

        public List<Suburb> Suburbs { get; set; }

        public List<Region> Regions { get; set; }

        public List<Supplier> Suppliers { get; set; }

        public Supplier SelectedSupplier { get; set; }

        public Country SelectedCountry { get; set; }

        public Province Selectedprovince { get; set; }

        public City SelectedCity { get; set; }

        public Suburb SelectedSuburb { get; set; }

        public Region SelectedRegion { get; set; }

    }
}
