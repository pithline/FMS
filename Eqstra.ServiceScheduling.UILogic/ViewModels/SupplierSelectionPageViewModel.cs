using Eqstra.BusinessLogic;
using Eqstra.BusinessLogic.Helpers;
using Eqstra.BusinessLogic.ServiceSchedule;
using Eqstra.ServiceScheduling.UILogic.AifServices;
using Eqstra.ServiceScheduling.UILogic.Helpers;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using Newtonsoft.Json;
using Syncfusion.UI.Xaml.Schedule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace Eqstra.ServiceScheduling.UILogic.ViewModels
{
    public class SupplierSelectionPageViewModel : BaseViewModel
    {
        INavigationService _navigationService;
        IEventAggregator _eventAggregator;
        bool isCached;
        public SupplierSelectionPageViewModel(INavigationService navigationSerive, IEventAggregator eventAggregator)
            : base(navigationSerive)
        {
            _navigationService = navigationSerive;
            _eventAggregator = eventAggregator;
            this.Model = new SupplierSelection();
            this.GoToConfirmationCommand = new DelegateCommand(async () =>
            {
                if (this.Model.ValidateProperties())
                {
                    bool isinserted = await SSProxyHelper.Instance.InsertSelectedSupplierToSvcAsync(this.Model, this.DriverTask.CaseNumber, this.DriverTask.CaseServiceRecID);
                    if (isinserted)
                    {
                        _navigationService.Navigate("Confirmation", string.Empty);
                    }
                }
            }, () =>
            {
                return (this.SelectedSupplier != null);
            });
            this.CountryChangedCommand = new DelegateCommand<object>(async (param) =>
            {
                if ((param != null) && (param is Country))
                {
                    Country country = param as Country;
                    if (!String.IsNullOrEmpty(country.Id) && this.Model.Provinces != null && !this.Model.Provinces.Any())
                    {
                        this.ProgressbarMessage = "Loading Provinces ....  ";
                        this.ProgressbarVisiblity = Visibility.Visible;
                        this.Model.Provinces = await SSProxyHelper.Instance.GetProvinceListFromSvcAsync(country.Id);
                        this.ProgressbarVisiblity = Visibility.Collapsed;
                        this.Model.SelectedCountry = country;
                        this.Model.Selectedprovince = null;
                    }
                }
                else
                {
                    this.Model.SelectedCountry = null;
                    this.Model.Selectedprovince = null;
                }
            });

            this.ProvinceChangedCommand = new DelegateCommand<object>(async (param) =>
            {
                if ((param != null) && (param is Province))
                {
                    Province province = param as Province;
                    if (!String.IsNullOrEmpty(province.Id) && this.Model.Cities != null && !this.Model.Cities.Any())
                    {
                        this.ProgressbarMessage = "Loading Cities ....  ";
                        this.ProgressbarVisiblity = Visibility.Visible;
                        this.Model.Cities = await SSProxyHelper.Instance.GetCityListFromSvcAsync(this.Model.SelectedCountry.Id, province.Id);
                        this.ProgressbarVisiblity = Visibility.Collapsed;
                        this.Model.Selectedprovince = province;
                        this.Model.SelectedCity = null;
                    }
                }
                else
                {
                    this.Model.Selectedprovince = null;
                    this.Model.SelectedCity = null;
                }
            });

            this.CityChangedCommand = new DelegateCommand<object>(async (param) =>
            {
                if ((param != null) && (param is City))
                {
                    City city = param as City;
                    if (!String.IsNullOrEmpty(city.Id) && this.Model.Suburbs != null && !this.Model.Suburbs.Any())
                    {
                        this.ProgressbarMessage = "Loading Suburbs ....  ";
                        this.ProgressbarVisiblity = Visibility.Visible;
                        this.Model.Suburbs = await SSProxyHelper.Instance.GetSuburbListFromSvcAsync(this.Model.SelectedCountry.Id, city.Id);
                        this.ProgressbarVisiblity = Visibility.Collapsed;
                        this.Model.SelectedCity = city;
                        this.Model.SelectedSuburb = null;
                    }
                }
                else
                {
                    this.Model.SelectedCity = null;
                    this.Model.SelectedSuburb = null;
                }

            });

            this.SuburbChangedCommand = new DelegateCommand<object>(async (param) =>
            {
                if ((param != null) && (param is Suburb))
                {
                    if (this.Model.Selectedprovince != null && this.Model.Regions != null && !this.Model.Regions.Any())
                    {
                        this.ProgressbarMessage = "Loading Regions ....  ";
                        this.ProgressbarVisiblity = Visibility.Visible;
                        this.Model.Regions = await SSProxyHelper.Instance.GetRegionListFromSvcAsync(this.Model.SelectedCountry.Id, this.Model.Selectedprovince.Id);
                        this.ProgressbarVisiblity = Visibility.Collapsed;
                        this.Model.SelectedSuburb = (Suburb)param;
                    }
                }
                else
                {
                    this.Model.SelectedSuburb = null;
                    this.Model.SelectedRegion = null;
                }

            });

            this.RegionChangedCommand = new DelegateCommand<object>(async (param) =>
            {
                if ((param != null) && (param is Region))
                {
                    this.Model.SelectedRegion = (Region)param;
                }
                else
                {
                    this.Model.SelectedRegion = null;
                }
            });

            this.SubmitQueryCommand = new DelegateCommand<string>(async (param) =>
            {

            });

            this.SupplierFilterCommand = new DelegateCommand(async () =>
            {
                try
                {
                    this.IsBusy = true;
                    if (!isCached)
                    {
                        await Util.WriteToDiskAsync(JsonConvert.SerializeObject(await SSProxyHelper.Instance.GetVendSupplirerSvcAsync()), "SuppliersGridItemsSourceFile.json");
                        isCached = true;
                    }
                    var result = await Util.ReadFromDiskAsync<Supplier>("SuppliersGridItemsSourceFile.json");
                    if (result != null)
                    {
                        IEnumerable<Supplier> filteredResult = new List<Supplier>();
                        if ((this.Model != null) && this.Model.SelectedCountry != null && !String.IsNullOrEmpty(this.Model.SelectedCountry.Id))
                        {
                            filteredResult = result.Where(w => w.Country == this.Model.SelectedCountry.Id);
                            if (this.Model.Selectedprovince != null && !String.IsNullOrEmpty(this.Model.Selectedprovince.Id))
                            {
                                filteredResult = filteredResult.Where(w => w.Province == this.Model.Selectedprovince.Id);
                                if (this.Model.SelectedCity != null && !String.IsNullOrEmpty(this.Model.SelectedCity.Id))
                                {
                                    filteredResult = filteredResult.Where(w => w.City == this.Model.SelectedCity.Id);
                                    if (this.Model.SelectedSuburb != null && !String.IsNullOrEmpty(this.Model.SelectedSuburb.Id))
                                    {
                                        filteredResult = filteredResult.Where(w => w.Suburb == this.Model.SelectedSuburb.Id);

                                        if (this.Model.SelectedRegion != null && !String.IsNullOrEmpty(this.Model.SelectedRegion.Id))
                                        {
                                            filteredResult = filteredResult.Where(w => w.Suburb == this.Model.SelectedRegion.Id);
                                        }
                                    }
                                }
                            }
                        }
                        this.Model.Suppliers = filteredResult.ToList<Supplier>();
                    }
                    this.IsBusy = false;
                }
                catch (Exception ex)
                {
                    AppSettings.Instance.ErrorMessage = ex.Message;
                }
            }
            );
        }

        public async override void OnNavigatedTo(object navigationParameter, Windows.UI.Xaml.Navigation.NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            try
            {
                this.ProgressbarMessage = "Loading Countries ....  ";
                this.ProgressbarVisiblity = Visibility.Visible;
                base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
                this.DriverTask = PersistentData.Instance.DriverTask;
                this.CustomerDetails = PersistentData.Instance.CustomerDetails;
                var countries = await SSProxyHelper.Instance.GetCountryRegionListFromSvcAsync();
                if (countries != null)
                {
                    this.Model.Countries.AddRange(countries);
                }
                this.ProgressbarVisiblity = Visibility.Collapsed;

            }
            catch (Exception ex)
            {
                AppSettings.Instance.ErrorMessage = ex.Message;
            }
        }
        private CustomerDetails customerDetails;
        public CustomerDetails CustomerDetails
        {
            get { return customerDetails; }
            set { SetProperty(ref customerDetails, value); }
        }
        private DriverTask driverTask;
        public DriverTask DriverTask
        {
            get { return driverTask; }
            set { SetProperty(ref driverTask, value); }
        }
        public DelegateCommand GoToConfirmationCommand { get; set; }
        public DelegateCommand<object> CountryChangedCommand { get; set; }
        public DelegateCommand<object> ProvinceChangedCommand { get; set; }
        public DelegateCommand<object> CityChangedCommand { get; set; }
        public DelegateCommand<object> SuburbChangedCommand { get; set; }

        public DelegateCommand<object> RegionChangedCommand { get; set; }
        public DelegateCommand<string> SubmitQueryCommand { get; set; }
        public DelegateCommand SupplierFilterCommand { get; set; }

        private SupplierSelection model;
        [RestorableState]
        public SupplierSelection Model
        {
            get { return model; }
            set { SetProperty(ref model, value); }
        }

        private Visibility progressbarVisiblity;
        public Visibility ProgressbarVisiblity
        {
            get { return progressbarVisiblity; }
            set { SetProperty(ref progressbarVisiblity, value); }
        }
        private string progressbarMessage;
        public string ProgressbarMessage
        {
            get { return progressbarMessage; }
            set { SetProperty(ref progressbarMessage, value); }
        }
        private Supplier selectedSupplier;
        public Supplier SelectedSupplier
        {
            get { return selectedSupplier; }
            set
            {
                if (SetProperty(ref selectedSupplier, value))
                {
                    this.GoToConfirmationCommand.RaiseCanExecuteChanged();
                    this.Model.SelectedSupplier = this.SelectedSupplier;
                }
            }
        }
    }
}
