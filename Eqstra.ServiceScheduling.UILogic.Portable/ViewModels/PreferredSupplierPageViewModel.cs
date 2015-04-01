using Eqstra.BusinessLogic.Portable.SSModels;
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
            this.PoolofSupplier = new ObservableCollection<Supplier>();

        }

        private ObservableCollection<Supplier> poolofSupplier;
        public ObservableCollection<Supplier> PoolofSupplier
        {
            get { return poolofSupplier; }
            set
            {
                SetProperty(ref poolofSupplier, value);
            }
        }
    }
}
