using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eqstra.ServiceScheduling.UILogic.Portable
{
    public class PreferredSupplierPageViewModel : ViewModel
    {
        private INavigationService _navigationService;
        public PreferredSupplierPageViewModel(INavigationService navigationService)
        {
            this._navigationService = navigationService;
            this.PoolofSupplier = new ObservableCollection<DataProvider.AX.SSModels.Supplier>();
            this.PoolofSupplier.Add(new DataProvider.AX.SSModels.Supplier { SupplierName = "Adolph Blaine Charles David Earl Frederick Gerald" });
            this.PoolofSupplier.Add(new DataProvider.AX.SSModels.Supplier { SupplierName = "Adolph Blaine Charles David Earl Frederick Gerald" });
            this.PoolofSupplier.Add(new DataProvider.AX.SSModels.Supplier { SupplierName = "Adolph Blaine Charles David Earl Frederick Gerald" });
            this.PoolofSupplier.Add(new DataProvider.AX.SSModels.Supplier { SupplierName = "Adolph Blaine Charles David Earl Frederick Gerald" });
            this.PoolofSupplier.Add(new DataProvider.AX.SSModels.Supplier { SupplierName = "Adolph Blaine Charles David Earl Frederick Gerald" });
            this.PoolofSupplier.Add(new DataProvider.AX.SSModels.Supplier { SupplierName = "Adolph Blaine Charles David Earl Frederick Gerald" });
            this.PoolofSupplier.Add(new DataProvider.AX.SSModels.Supplier { SupplierName = "Adolph Blaine Charles David Earl Frederick Gerald" });
            this.PoolofSupplier.Add(new DataProvider.AX.SSModels.Supplier { SupplierName = "Adolph Blaine Charles David Earl Frederick Gerald" });
            this.PoolofSupplier.Add(new DataProvider.AX.SSModels.Supplier { SupplierName = "Adolph Blaine Charles David Earl Frederick Gerald" });

            this.PoolofSupplier.Add(new DataProvider.AX.SSModels.Supplier { SupplierName = "Adolph Blaine Charles David Earl Frederick Gerald" });
            this.PoolofSupplier.Add(new DataProvider.AX.SSModels.Supplier { SupplierName = "Adolph Blaine Charles David Earl Frederick Gerald" });
            this.PoolofSupplier.Add(new DataProvider.AX.SSModels.Supplier { SupplierName = "Adolph Blaine Charles David Earl Frederick Gerald" });
            this.PoolofSupplier.Add(new DataProvider.AX.SSModels.Supplier { SupplierName = "Adolph Blaine Charles David Earl Frederick Gerald" });
            this.PoolofSupplier.Add(new DataProvider.AX.SSModels.Supplier { SupplierName = "Adolph Blaine Charles David Earl Frederick Gerald" });
            this.PoolofSupplier.Add(new DataProvider.AX.SSModels.Supplier { SupplierName = "Adolph Blaine Charles David Earl Frederick Gerald" });
            this.PoolofSupplier.Add(new DataProvider.AX.SSModels.Supplier { SupplierName = "Adolph Blaine Charles David Earl Frederick Gerald" });
            this.PoolofSupplier.Add(new DataProvider.AX.SSModels.Supplier { SupplierName = "Adolph Blaine Charles David Earl Frederick Gerald" });
            this.PoolofSupplier.Add(new DataProvider.AX.SSModels.Supplier { SupplierName = "Adolph Blaine Charles David Earl Frederick Gerald" });

            this.PoolofSupplier.Add(new DataProvider.AX.SSModels.Supplier { SupplierName = "Adolph Blaine Charles David Earl Frederick Gerald" });
            this.PoolofSupplier.Add(new DataProvider.AX.SSModels.Supplier { SupplierName = "Adolph Blaine Charles David Earl Frederick Gerald" });
            this.PoolofSupplier.Add(new DataProvider.AX.SSModels.Supplier { SupplierName = "Adolph Blaine Charles David Earl Frederick Gerald" });
            this.PoolofSupplier.Add(new DataProvider.AX.SSModels.Supplier { SupplierName = "Adolph Blaine Charles David Earl Frederick Gerald" });
            this.PoolofSupplier.Add(new DataProvider.AX.SSModels.Supplier { SupplierName = "Adolph Blaine Charles David Earl Frederick Gerald" });
            this.PoolofSupplier.Add(new DataProvider.AX.SSModels.Supplier { SupplierName = "Adolph Blaine Charles David Earl Frederick Gerald" });
            this.PoolofSupplier.Add(new DataProvider.AX.SSModels.Supplier { SupplierName = "Adolph Blaine Charles David Earl Frederick Gerald" });
            this.PoolofSupplier.Add(new DataProvider.AX.SSModels.Supplier { SupplierName = "Adolph Blaine Charles David Earl Frederick Gerald" });
            this.PoolofSupplier.Add(new DataProvider.AX.SSModels.Supplier { SupplierName = "Adolph Blaine Charles David Earl Frederick Gerald" });
        }

        private ObservableCollection<Eqstra.DataProvider.AX.SSModels.Supplier> poolofSupplier;
        public ObservableCollection<Eqstra.DataProvider.AX.SSModels.Supplier> PoolofSupplier
        {
            get { return poolofSupplier; }
            set
            {
                SetProperty(ref poolofSupplier, value);
            }
        }
    }
}
