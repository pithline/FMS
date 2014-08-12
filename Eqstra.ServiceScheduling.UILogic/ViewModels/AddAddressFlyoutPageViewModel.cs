using Eqstra.BusinessLogic.ServiceSchedule;
using Eqstra.ServiceScheduling.UILogic.AifServices;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.StoreApps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace Eqstra.ServiceScheduling.UILogic.ViewModels
{
    public class AddAddressFlyoutPageViewModel : ViewModel
    {
        private IEventAggregator _eventAggregator;
        public AddAddressFlyoutPageViewModel(IEventAggregator eventAggregator)
        {
            this._eventAggregator = eventAggregator;
            this.Model = new Address();

            this.SaveAddressCommand = new DelegateCommand(() =>
            {
                this._eventAggregator.GetEvent<AddressEvent>().Publish(this.Model);
            });

            this.CancelAddressCommand = new DelegateCommand(() =>
            {
                this.Model = new Address();
                this._eventAggregator.GetEvent<AddressEvent>().Publish(this.Model);
            });

            this.PageLoadedCommand = new DelegateCommand(async () =>
            {
                try
                {
                    if (!this.Model.Countries.Any())
                    {
                        this.ProgressbarMessage = "Loading Countries ....  ";
                        this.ProgressbarVisiblity = Visibility.Visible;
                        this.Model.Countries = await SSProxyHelper.Instance.GetCountryRegionListFromSvcAsync();
                    }
                    this.ProgressbarVisiblity = Visibility.Collapsed;
                }
                catch (Exception ex)
                {
                    AppSettings.Instance.ErrorMessage = ex.Message;
                }
            });
            this.CountryChangedCommand = new DelegateCommand<object>(async (param) =>
            {
                if (param != null && (param is Country))
                {
                    Country country = param as Country;
                    if (!String.IsNullOrEmpty(country.Id) && !this.Model.Provinces.Any())
                    {
                        this.ProgressbarMessage = "Loading Provinces ....  ";
                        this.ProgressbarVisiblity = Visibility.Visible;
                        this.Model.Provinces = await SSProxyHelper.Instance.GetProvinceListFromSvcAsync(country.Id);
                        this.ProgressbarVisiblity = Visibility.Collapsed;
                        this.Model.SelectedCountry = country;
                    }
                }
            });

            this.ProvinceChangedCommand = new DelegateCommand<object>(async (param) =>
            {
                if (param != null && (param is Province))
                {
                    Province province = param as Province;
                    if (!String.IsNullOrEmpty(province.Id) && !this.Model.Cities.Any())
                    {
                        this.ProgressbarMessage = "Loading Cities ....  ";
                        this.ProgressbarVisiblity = Visibility.Visible;
                        this.Model.Cities = await SSProxyHelper.Instance.GetCityListFromSvcAsync(this.Model.SelectedCountry.Id, province.Id);
                        this.Model.Postcodes = await SSProxyHelper.Instance.GetZipcodeListFromSvcAsync(this.Model.SelectedCountry.Id, province.Id);
                        this.ProgressbarVisiblity = Visibility.Collapsed;
                        this.Model.Selectedprovince = province;
                    }
                }
            });

            this.CityChangedCommand = new DelegateCommand<object>(async (param) =>
            {
                if (param != null && (param is City))
                {
                    City city = param as City;
                    if (!String.IsNullOrEmpty(city.Id) && !this.Model.Suburbs.Any())
                    {
                        this.ProgressbarMessage = "Loading Suburbs ....  ";
                        this.ProgressbarVisiblity = Visibility.Visible;
                        this.Model.Suburbs = await SSProxyHelper.Instance.GetSuburbListFromSvcAsync(this.Model.SelectedCountry.Id, city.Id);
                        this.ProgressbarVisiblity = Visibility.Collapsed;
                        this.Model.SelectedCity = city;
                    }
                }

            });

            this.SuburbChangedCommand = new DelegateCommand<object>((param) =>
            {
                if (param != null && (param is Suburb))
                {
                    Suburb suburb = param as Suburb;
                    if (!String.IsNullOrEmpty(suburb.Id))
                    {
                        this.Model.SelectedSuburb = suburb;
                    }
                }
            });

            this.ZipChangedCommand = new DelegateCommand<object>((param) =>
            {
                if (param != null && (param is string))
                {
                    if (!String.IsNullOrEmpty(param.ToString()))
                    {
                        this.Model.SelectedZip = param.ToString();
                    }
                }

            });
        }

        private Address model;
        public Address Model
        {
            get { return model; }
            set { SetProperty(ref model, value); }
        }
        public DelegateCommand SaveAddressCommand { get; set; }
        public DelegateCommand CancelAddressCommand { get; set; }
        public DelegateCommand<object> CountryChangedCommand { get; set; }
        public DelegateCommand<object> ProvinceChangedCommand { get; set; }
        public DelegateCommand<object> CityChangedCommand { get; set; }
        public DelegateCommand<object> SuburbChangedCommand { get; set; }
        public DelegateCommand<object> ZipChangedCommand { get; set; }

        public DelegateCommand PageLoadedCommand { get; set; }

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
    }
}
