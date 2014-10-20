
using Eqstra.BusinessLogic.DocumentDelivery;
using Eqstra.BusinessLogic.Helpers;
using Eqstra.DocumentDelivery.UILogic.Helpers;
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
            this.Model = new AlternateContactPerson();
            this._eventAggregator = eventAggregator;
            this.AddCustomerCommand = DelegateCommand.FromAsyncHandler(async () =>
            {
                this.Model.UserId = PersistentData.Instance.UserInfo.UserId;
                var alternateData = await SqliteHelper.Storage.LoadTableAsync<AlternateContactPerson>();
                if (alternateData != null && alternateData.Any(a => a.FirstName == this.Model.FirstName && a.Surname == this.Model.Surname))
                {
                    await SqliteHelper.Storage.UpdateSingleRecordAsync<AlternateContactPerson>(this.Model);
                }
                else
                {
                    await SqliteHelper.Storage.InsertSingleRecordAsync<AlternateContactPerson>(this.Model);
                }

                this._eventAggregator.GetEvent<AlternateContactPersonEvent>().Publish(this.Model);
            });

            this.ClearCustomerCommand = new DelegateCommand(() =>
            {
                this.Model = new AlternateContactPerson();
                this._eventAggregator.GetEvent<AlternateContactPersonEvent>().Publish(this.Model);
            });

        }
        public DelegateCommand AddCustomerCommand { get; set; }
        public DelegateCommand ClearCustomerCommand { get; set; }

        private AlternateContactPerson model;
        public AlternateContactPerson Model
        {
            get { return model; }
            set { SetProperty(ref model, value); }
        }
    }

}
