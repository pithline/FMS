using Eqstra.BusinessLogic.Portable.SSModels;
using Eqstra.ServiceScheduling.UILogic.Portable.Services;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using Microsoft.Practices.Prism;
using Microsoft.Practices.Prism.StoreApps;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Microsoft.Practices.Prism.PubSubEvents;

namespace Eqstra.ServiceScheduling.UILogic.Portable
{
    public class PreferredSupplierPageViewModel : ViewModel
    {
        public IEventAggregator _eventAggregator;
        private INavigationService _navigationService;
        public ISupplierService _supplierService;
        public ILocationService _locationService;
        public PreferredSupplierPageViewModel(INavigationService navigationService, IEventAggregator eventAggregator, ISupplierService supplierService, ILocationService locationService)
        {
            this._navigationService = navigationService;
            this._eventAggregator = eventAggregator;
            this.PoolofSupplier = new ObservableCollection<Supplier>();
            this._supplierService = supplierService;
            this._locationService = locationService;

            this.NextPageCommand = DelegateCommand.FromAsyncHandler(
         async () =>
         {
             try
             {
                 if (this.SelectedSupplier != null)
                 {
                     var supplier = new SupplierSelection() { CaseNumber = this.SelectedTask.CaseNumber, CaseServiceRecID = this.SelectedTask.CaseServiceRecID, SelectedSupplier = this.SelectedSupplier };
                     var response = await this._supplierService.InsertSelectedSupplierAsync(supplier, new UserInfo { UserId = "axbcsvc", CompanyId = "1095" });
                     if (response)
                     {
                         navigationService.Navigate("SubmittedDetail", this.SelectedTask);
                     }
                 }

             }
             catch (Exception ex)
             {
             }
             finally
             {
             }
         },

          () => { return this.SelectedSupplier != null; });


            _eventAggregator.GetEvent<SupplierFilterEvent>().Subscribe(poolofSupplier =>
            {
                this.PoolofSupplier = poolofSupplier;
            });

        }

        public async override void OnNavigatedTo(object navigationParameter, Windows.UI.Xaml.Navigation.NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            try
            {
                this.TaskProgressBar = Visibility.Visible;
                this.SelectedTask = ((Eqstra.BusinessLogic.Portable.SSModels.Task)navigationParameter);
                if (PersistentData.Instance.PoolofSupplier != null)
                {
                    this.PoolofSupplier = PersistentData.Instance.PoolofSupplier;
                }
                this.PoolofSupplier = await this._supplierService.GetSuppliersByClassAsync(this.SelectedTask.VehicleClassId, new UserInfo { UserId = "axbcsvc", CompanyId = "1095" });
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

        public DelegateCommand NextPageCommand { get; private set; }

        public Eqstra.BusinessLogic.Portable.SSModels.Task SelectedTask { get; set; }
    }
}
