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
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace Eqstra.ServiceScheduling.UILogic.ViewModels
{
    public class SupplierSelectionPageViewModel : BaseViewModel
    {
        INavigationService _navigationService;
        IEventAggregator _eventAggregator;
        bool isCached;
        Dictionary<string, object> navigationData = new Dictionary<string, object>();
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
                    await Util.WriteToDiskAsync(JsonConvert.SerializeObject(this.Model), "SupplierSelection");

                    _navigationService.Navigate("Confirmation", string.Empty);
                }
            });
            this.CountryChangedCommand = new DelegateCommand<Country>(async (param) =>
            {
                this.IsBusy = true;
                this.Model.Provinces = await SSProxyHelper.Instance.GetProvinceListFromSvcAsync(param.Id);
                this.IsBusy = false;
            });

            this.ProvinceChangedCommand = new DelegateCommand<province>(async (param) =>
            {
                this.IsBusy = true;
                this.Model.Cities = await SSProxyHelper.Instance.getCityListFromSvcAsync(this.Model.SelectedCountry.Id, param.Id);
                this.IsBusy = false;
            });

            this.CityChangedCommand = new DelegateCommand<City>(async (param) =>
            {
                this.IsBusy = true;
                this.Model.Suburbs = await SSProxyHelper.Instance.getSuburbListFromSvcAsync(this.Model.SelectedCountry.Id, param.Id);
                this.IsBusy = false;

            });
            //this.SuburbChangedCommand = new DelegateCommand<Suburb>(async (param) =>
            //{
            //    this.Model.Provinces = await SSProxyHelper.Instance.GetProvinceListFromSvcAsync(param.Id);


            //});
            this.SubmitQueryCommand = new DelegateCommand<string>(async (param) =>
            {

            });

            this.SupplierFilterCommand = new DelegateCommand(async () =>
            {
                if (this.Model.Suppliers != null && this.Model.Suppliers.Any() && !isCached)
                {
                    await Util.WriteToDiskAsync(JsonConvert.SerializeObject(this.Model.Suppliers), "SuppliersGridItemsSourceFile.json");
                    isCached = true;
                }
                var result = await Util.ReadFromDiskAsync<Supplier>("SuppliersGridItemsSourceFile.json");
                if (result != null)
                {
                    IEnumerable<Supplier> filteredResult = null;
                    if (!String.IsNullOrEmpty(this.Model.SelectedCountry.Id))
                    {
                        filteredResult = result.Where(w => w.Country == this.Model.SelectedCountry.Id);
                        if (!String.IsNullOrEmpty(this.Model.Selectedprovince.Id))
                        {
                            filteredResult = filteredResult.Where(w => w.Province == this.Model.Selectedprovince.Id);
                            if (!String.IsNullOrEmpty(this.Model.SelectedCity.Id))
                            {
                                filteredResult = filteredResult.Where(w => w.City == this.Model.SelectedCity.Id);
                                if (!String.IsNullOrEmpty(this.Model.SelectedSuburb.Id))
                                {
                                    filteredResult = filteredResult.Where(w => w.Suburb == this.Model.SelectedSuburb.Id);
                                }
                            }
                        }
                    }
                    this.Model.Suppliers = filteredResult.ToList<Supplier>();
                }
            });
        }

        public async System.Threading.Tasks.Task d()
        {
            //var result = await Util.ReadFromDiskAsync<Supplier>("SuppliersGridItemsSourceFile.json");
            //if (result != null)
            //{
            //    this.suppliersGrid.ItemsSource = result.Where(x =>
            //             x.SupplierContactName.Contains(args.QueryText) ||
            //            Convert.ToString(x.SupplierContactNumber).Contains(args.QueryText) ||
            //             x.SupplierName.Contains(args.QueryText));
            //}
        }
        public async override void OnNavigatedTo(object navigationParameter, Windows.UI.Xaml.Navigation.NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            try
            {
                this.IsBusy = true;
                base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
                this.DriverTask = PersistentData.Instance.DriverTask;
                this.CustomerDetails = PersistentData.Instance.CustomerDetails;
                var countries = await SSProxyHelper.Instance.GetCountryRegionListFromSvcAsync();
                if (countries != null)
                {
                    this.Model.Countries.AddRange(countries);
                }
                Model.Suppliers = await SSProxyHelper.Instance.GetVendSupplirerSvcAsync();
                this.IsBusy = false;
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

        public DelegateCommand<Country> CountryChangedCommand { get; set; }
        public DelegateCommand<province> ProvinceChangedCommand { get; set; }
        public DelegateCommand<City> CityChangedCommand { get; set; }
        public DelegateCommand<Suburb> SuburbChangedCommand { get; set; }

        public DelegateCommand<string> SubmitQueryCommand { get; set; }

        public DelegateCommand SupplierFilterCommand { get; set; }

        private SupplierSelection model;
        [RestorableState]
        public SupplierSelection Model
        {
            get { return model; }
            set { SetProperty(ref model, value); }
        }
    }
}
