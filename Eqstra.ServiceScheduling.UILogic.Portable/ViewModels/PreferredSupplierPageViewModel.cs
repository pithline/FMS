using Eqstra.BusinessLogic.Portable.SSModels;
using Eqstra.ServiceScheduling.UILogic.Portable.Services;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

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
            this.Address = new Address();

            this.NextPageCommand = DelegateCommand.FromAsyncHandler(
         async () =>
         {
             try
             {
                 navigationService.Navigate("SubmittedDetail", string.Empty);
             }
             catch (Exception ex)
             {
             }
             finally
             {
             }
         },

          () => { return this.SelectedSupplier != null; });

        }

        public async override void OnNavigatedTo(object navigationParameter, Windows.UI.Xaml.Navigation.NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            try
            {
                var task = ((Eqstra.BusinessLogic.Portable.SSModels.Task)navigationParameter);
                this.PoolofSupplier = await this._supplierService.GetSuppliersByClassAsync(task.VehicleClassId, new UserInfo { UserId = "axbcsvc", CompanyId = "1095" });
                PersistentData.Instance.PoolofSupplier = this.PoolofSupplier;
                this.TaskProgressBar = Visibility.Collapsed;
            }
            catch (Exception)
            {
                this.TaskProgressBar = Visibility.Collapsed;
            }
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

        private Visibility taskProgressBar;
        public Visibility TaskProgressBar
        {
            get { return taskProgressBar; }
            set
            {
                SetProperty(ref taskProgressBar, value);
            }
        }

        private Supplier selectedSupplier;
        public Supplier SelectedSupplier
        {
            get { return selectedSupplier; }
            set
            {
                SetProperty(ref selectedSupplier, value);
                this.NextPageCommand.RaiseCanExecuteChanged();
            }
        }

        private Address address;
        public Address Address
        {
            get { return address; }
            set { SetProperty(ref address, value); }
        }
        public DelegateCommand NextPageCommand { get; private set; }
    }
}
