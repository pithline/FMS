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
        private ILocationService _locationService;
        public PreferredSupplierPageViewModel(INavigationService navigationService, ISupplierService supplierService, ILocationService locationService)
        {
            this._navigationService = navigationService;
            this.PoolofSupplier = new ObservableCollection<Supplier>();
            this._supplierService = supplierService;
            this._locationService = locationService;
            this.SupplierFilter = new SupplierFilter();

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
                         navigationService.Navigate("SubmittedDetail", string.Empty);
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

            this.GoToConfirmationCommand = new DelegateCommand(async () =>
            {
                try
                {
                    this.LoadingCriteriaProgressVisibility = Visibility.Visible;

                    _navigationService.Navigate("Confirmation", string.Empty);

                    this.LoadingCriteriaProgressVisibility = Visibility.Collapsed;
                }
                catch (Exception)
                {
                    this.LoadingCriteriaProgressVisibility = Visibility.Collapsed;
                }
            }, () =>
            {
                return (this.SelectedSupplier != null);
            });
            this.ConcelCommand = new DelegateCommand(async () =>
          {
              this.PoolofSupplier = await this._supplierService.GetSuppliersByClassAsync(this.SelectedTask.VehicleClassId, new UserInfo { UserId = "axbcsvc", CompanyId = "1095" });

          });
            this.SupplierFilterCommand = new DelegateCommand(async () =>
            {
                try
                {
                    this.LoadingCriteriaProgressVisibility = Visibility.Visible;
                    string countryId = this.SupplierFilter.SelectedCountry != null ? this.SupplierFilter.SelectedCountry.Id : string.Empty;
                    string provinceId = this.SupplierFilter.Selectedprovince != null ? this.SupplierFilter.Selectedprovince.Id : string.Empty;
                    string cityId = this.SupplierFilter.SelectedCity != null ? this.SupplierFilter.SelectedCity.Id : string.Empty;
                    string suburbId = this.SupplierFilter.SelectedSuburb != null ? this.SupplierFilter.SelectedSuburb.Id : string.Empty;
                    string regionId = this.SupplierFilter.SelectedRegion != null ? this.SupplierFilter.SelectedRegion.Id : string.Empty;

                    this.PoolofSupplier = await this._supplierService.SearchSupplierByLocationAsync(countryId, provinceId, cityId, suburbId, regionId, new UserInfo { UserId = "axbcsvc", CompanyId = "1095" });

                    this.LoadingCriteriaProgressVisibility = Visibility.Collapsed;
                }
                catch (Exception ex)
                {
                    this.LoadingCriteriaProgressVisibility = Visibility.Collapsed;
                }
            }
          );

        }

        public async override void OnNavigatedTo(object navigationParameter, Windows.UI.Xaml.Navigation.NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            try
            {
                this.SelectedTask = ((Eqstra.BusinessLogic.Portable.SSModels.Task)navigationParameter);
                this.PoolofSupplier = await this._supplierService.GetSuppliersByClassAsync(this.SelectedTask.VehicleClassId, new UserInfo { UserId = "axbcsvc", CompanyId = "1095" });

                this.SupplierFilter.Countries = await _locationService.GetCountryList(new UserInfo { UserId = "axbcsvc", CompanyId = "1095" });

                PersistentData.Instance.PoolofSupplier = this.PoolofSupplier;
                this.TaskProgressBar = Visibility.Collapsed;
            }
            catch (Exception)
            {
                this.TaskProgressBar = Visibility.Collapsed;
            }
        }

        public async void CountryChanged()
        {
            if (this.SupplierFilter.SelectedCountry != null)
            {
                this.LoadingCriteriaProgressVisibility = Visibility.Visible;
                this.SupplierFilter.Provinces = await _locationService.GetProvinceList(this.SupplierFilter.SelectedCountry.Id, new UserInfo { UserId = "axbcsvc", CompanyId = "1095" });
                this.LoadingCriteriaProgressVisibility = Visibility.Collapsed;
            }

        }
        public async void ProvinceChanged()
        {
            if (this.SupplierFilter.Selectedprovince != null)
            {
                this.LoadingCriteriaProgressVisibility = Visibility.Visible;
                this.SupplierFilter.Cities = await _locationService.GetCityList(this.SupplierFilter.SelectedCountry.Id, this.SupplierFilter.Selectedprovince.Id, new UserInfo { UserId = "axbcsvc", CompanyId = "1095" });
                this.LoadingCriteriaProgressVisibility = Visibility.Collapsed;
            }

        }
        public async void CityChanged()
        {
            if (this.SupplierFilter.Selectedprovince != null)
            {
                this.LoadingCriteriaProgressVisibility = Visibility.Visible;
                this.SupplierFilter.Suburbs = await _locationService.GetSuburbList(this.SupplierFilter.SelectedCountry.Id, this.SupplierFilter.Selectedprovince.Id, new UserInfo { UserId = "axbcsvc", CompanyId = "1095" });
                this.LoadingCriteriaProgressVisibility = Visibility.Collapsed;
            }
        }
        public async void SuburbChanged()
        {
            if (this.SupplierFilter.Selectedprovince != null)
            {
                this.LoadingCriteriaProgressVisibility = Visibility.Visible;
                this.SupplierFilter.Region = await _locationService.GetRegionList(this.SupplierFilter.SelectedCountry.Id, this.SupplierFilter.Selectedprovince.Id, new UserInfo { UserId = "axbcsvc", CompanyId = "1095" });
                this.LoadingCriteriaProgressVisibility = Visibility.Collapsed;
            }
        }

        public DelegateCommand GoToConfirmationCommand { get; set; }
        public DelegateCommand ConcelCommand { get; set; }

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

        private SupplierFilter supplierFilter;
        public SupplierFilter SupplierFilter
        {
            get { return supplierFilter; }
            set { SetProperty(ref supplierFilter, value); }
        }

        private Visibility loadingCriteriaProgressVisibility;

        public Visibility LoadingCriteriaProgressVisibility
        {
            get { return loadingCriteriaProgressVisibility; }
            set { SetProperty(ref loadingCriteriaProgressVisibility, value); }
        }
        public DelegateCommand SupplierFilterCommand { get; set; }
        public DelegateCommand NextPageCommand { get; private set; }

        public Eqstra.BusinessLogic.Portable.SSModels.Task SelectedTask { get; set; }
    }
}
