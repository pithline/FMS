using Eqstra.BusinessLogic.Portable.SSModels;
using Eqstra.ServiceScheduling.UILogic.Portable.Services;
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
        private ISupplierService _supplierService;
        public PreferredSupplierPageViewModel(INavigationService navigationService, ISupplierService supplierService)
        {
            this._navigationService = navigationService;
            this.PoolofSupplier = new ObservableCollection<Supplier>();
            this._supplierService = supplierService;
        }

        public async override void OnNavigatedTo(object navigationParameter, Windows.UI.Xaml.Navigation.NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            var task = ((Eqstra.BusinessLogic.Portable.SSModels.Task)navigationParameter);
            this.PoolofSupplier = await this._supplierService.GetSuppliersByClassAsync(task.VehicleClassId, new UserInfo { UserId = "axbcsvc", CompanyId = "1095" });
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
