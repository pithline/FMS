using Eqstra.BusinessLogic.Helpers;
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

        private List<string> countries;
        public List<string> Countries
        {
            get { return countries; }
            set { SetProperty(ref countries, value); }
        }

        private List<string> provinces;

        public List<string> Provinces
        {
            get { return provinces; }
            set { SetProperty(ref provinces, value); }
        }
        private List<string> cities;

        public List<string> Cities
        {
            get { return cities; }
            set { SetProperty(ref cities, value); }
        }

        private List<string> suburbs;

        public List<string> Suburbs
        {
            get { return suburbs; }
            set { SetProperty(ref suburbs, value); }
        }

        private List<string> region;

        public List<string> Region
        {
            get { return region; }
            set { SetProperty(ref region, value); }
        }

        private List<Supplier> suppliers;

        public List<Supplier> Suppliers
        {
            get { return suppliers; }
            set { SetProperty(ref suppliers, value); }
        }

        private Supplier selectedSupplier;

        public Supplier SelectedSupplier
        {
            get { return selectedSupplier; }
            set { SetProperty(ref selectedSupplier, value); }
        }

    }
}