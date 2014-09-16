using Eqstra.BusinessLogic;
using Eqstra.BusinessLogic.Helpers;
using Eqstra.BusinessLogic.ServiceSchedule;
using Eqstra.ServiceScheduling.UILogic.AifServices;
using Eqstra.ServiceScheduling.UILogic.Helpers;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

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
                try
                {
                    this.IsBusy = true;
                    if (this.Model.ValidateProperties())
                    {
                        bool isinserted = await SSProxyHelper.Instance.InsertSelectedSupplierToSvcAsync(this.Model, this.DriverTask.CaseNumber, this.DriverTask.CaseServiceRecID);
                         _navigationService.Navigate("Confirmation", string.Empty);
                    }
                    this.IsBusy = false;
                }
                catch (Exception)
                {
                    this.IsBusy = false;
                    throw;
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

                        this.Model.Provinces = await SSProxyHelper.Instance.GetProvinceListFromSvcAsync(country.Id);

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

                        this.Model.Cities = await SSProxyHelper.Instance.GetCityListFromSvcAsync(this.Model.SelectedCountry.Id, province.Id);

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

                        this.Model.Suburbs = await SSProxyHelper.Instance.GetSuburbListFromSvcAsync(this.Model.SelectedCountry.Id, city.Id);

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

                        this.Model.Regions = await SSProxyHelper.Instance.GetRegionListFromSvcAsync(this.Model.SelectedCountry.Id, this.Model.Selectedprovince.Id);

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
                    this.IsBusy = false;
                }
            }
            );
        }

        public async override void OnNavigatedTo(object navigationParameter, Windows.UI.Xaml.Navigation.NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            try
            {

                base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
                this.DriverTask = PersistentData.Instance.DriverTask;
                this.CustomerDetails = PersistentData.Instance.CustomerDetails;
                var countries = await SSProxyHelper.Instance.GetCountryRegionListFromSvcAsync();
                if (countries != null)
                {
                    this.Model.Countries.AddRange(countries);
                }

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
