
using Eqstra.BusinessLogic.DocumentDelivery;
using Eqstra.BusinessLogic.Helpers;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.StoreApps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Eqstra.DocumentDelivery.UILogic.ViewModels
{
    public class AddCustomerPageViewModel : ViewModel
    {
        private IEventAggregator _eventAggregator;
        public AddCustomerPageViewModel(IEventAggregator eventAggregator)
        {
            this.Model = new ContactPerson();
            this._eventAggregator = eventAggregator;
            this.AddCustomerCommand = DelegateCommand.FromAsyncHandler(async () =>
            {
                this.Model.VehicleInsRecID = long.Parse(ApplicationData.Current.LocalSettings.Values["VehicleInsRecID"].ToString());
                await SqliteHelper.Storage.InsertSingleRecordAsync<ContactPerson>(this.Model);
                this._eventAggregator.GetEvent<ContactPersonEvent>().Publish(this.Model);
            });

            this.ClearCustomerCommand = new DelegateCommand(() =>
            {
                this.Model = new ContactPerson();
                this._eventAggregator.GetEvent<ContactPersonEvent>().Publish(this.Model);
            });

        }
        public DelegateCommand AddCustomerCommand { get; set; }
        public DelegateCommand ClearCustomerCommand { get; set; }

        private ContactPerson model;
        public ContactPerson Model
        {
            get { return model; }
            set { SetProperty(ref model, value); }
        }
    }

}
