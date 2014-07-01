using Eqstra.BusinessLogic.ServiceSchedule;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.StoreApps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                this._eventAggregator.GetEvent<AddressEvent>().Publish(this.Model as Address);
            });
        }
        private object model;
        public object Model
        {
            get { return model; }
            set { SetProperty(ref model, value); }
        }
        public DelegateCommand SaveAddressCommand { get; set; }
    }
}
