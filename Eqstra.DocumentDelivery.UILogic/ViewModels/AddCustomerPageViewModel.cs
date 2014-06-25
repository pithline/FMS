using Eqstra.BusinessLogic.DeliveryModel;
using Eqstra.BusinessLogic.Helpers;
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
        public AddCustomerPageViewModel()
        {
            this.Model = new AddCustomer();

            this.AddCustomerCommand = DelegateCommand<object>.FromAsyncHandler(async (param) =>
            {
                AddCustomer addCustomer = param as AddCustomer;
                var addCustomerData = await SqliteHelper.Storage.LoadTableAsync<AddCustomer>();
                var vehicleInsRecID = long.Parse(ApplicationData.Current.LocalSettings.Values["VehicleInsRecID"].ToString());
                addCustomer.VehicleInsRecID = vehicleInsRecID;
                await SqliteHelper.Storage.InsertSingleRecordAsync<AddCustomer>(addCustomer);
            });
        }
        public DelegateCommand<object> AddCustomerCommand { get; set; }

        private object model;
        public object Model
        {
            get { return model; }
            set { SetProperty(ref model, value); }
        }

    }


}
