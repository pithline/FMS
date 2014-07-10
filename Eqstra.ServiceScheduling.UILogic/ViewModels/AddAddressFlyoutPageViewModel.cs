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
                    this.ProgressbarMessage = "Loading Countries ....  ";
                    this.ProgressbarVisiblity = Visibility.Visible;
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
            });
            this.CountryChangedCommand = new DelegateCommand<object>(async (param) =>
            {
                if ((param is Country) && (param != null))
                {
                    Country country = param as Country;
                    if (!String.IsNullOrEmpty(country.Id))
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
                if ((param is Province) && (param != null))
                {
                    Province province = param as Province;
                    if (!String.IsNullOrEmpty(province.Id))
                    {
                        this.ProgressbarMessage = "Loading Cities ....  ";
                        this.ProgressbarVisiblity = Visibility.Visible;
                        this.Model.Cities = await SSProxyHelper.Instance.getCityListFromSvcAsync(this.Model.SelectedCountry.Id, province.Id);
                        this.ProgressbarVisiblity = Visibility.Collapsed;
                        this.Model.Selectedprovince = province;
                    }
                }
            });

            this.CityChangedCommand = new DelegateCommand<object>(async (param) =>
            {
                if ((param is City) && (param != null))
                {
                    City city = param as City;
                    if (!String.IsNullOrEmpty(city.Id))
                    {
                        this.ProgressbarMessage = "Loading Suburbs ....  ";
                        this.ProgressbarVisiblity = Visibility.Visible;
                        this.Model.Suburbs = await SSProxyHelper.Instance.getSuburbListFromSvcAsync(this.Model.SelectedCountry.Id, city.Id);
                        this.ProgressbarVisiblity = Visibility.Collapsed;
                        this.Model.SelectedCity = city;
                    }
                }

            });

            //this.SuburbChangedCommand = new DelegateCommand<Suburb>(async (param) =>
            //{
            //    this.Model.Provinces = await SSProxyHelper.Instance.GetProvinceListFromSvcAsync(param.Id);


            //});
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
