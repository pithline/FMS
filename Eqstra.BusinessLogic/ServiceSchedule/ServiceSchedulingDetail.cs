using Microsoft.Practices.Prism.StoreApps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eqstra.BusinessLogic.ServiceSchedule
{
    public class ServiceSchedulingDetail : ValidatableBindableBase
    {
        public ServiceSchedulingDetail()
        {
            this.SelectedItems = new Dictionary<string, object>();
            this.ODOReadingSnapshot = new ImageCapture { ImagePath = "ms-appx:///Assets/ODO_meter.png" };
        }

        private ImageCapture odoReadingSnapshot;
        [RestorableState]
        public ImageCapture ODOReadingSnapshot
        {
            get { return odoReadingSnapshot; }
            set { SetProperty(ref odoReadingSnapshot, value); }
        }

        private decimal odoReading;

        public decimal ODOReading
        {
            get { return odoReading; }
            set { SetProperty(ref odoReading, value); }
        }

        private DateTime odoReadingDate;

        public DateTime ODOReadingDate
        {
            get { return odoReadingDate; }
            set { SetProperty(ref odoReadingDate, value); }
        }

        private List<string> serviceType;

        public List<string> ServiceType
        {
            get { return serviceType; }
            set { SetProperty(ref serviceType, value); }
        }

        private string contactPersonName;

        public string ContactPersonName
        {
            get { return contactPersonName; }
            set { SetProperty(ref contactPersonName, value); }
        }

        private string contactPersonPhone;

        public string ContactPersonPhone
        {
            get { return contactPersonPhone; }
            set { SetProperty(ref contactPersonPhone, value); }
        }

        private string supplierName;

        public string SupplierName
        {
            get { return supplierName; }
            set { SetProperty(ref supplierName, value); }
        }

        private string eventDesc;

        public string EventDesc
        {
            get { return eventDesc; }
            set { SetProperty(ref eventDesc, value); }
        }

        private string deliveryOption;

        public string DeliveryOption
        {
            get { return deliveryOption; }
            set { SetProperty(ref deliveryOption, value); }
        }

        private List<string> locationType;

        public List<string> LocationType
        {
            get { return locationType; }
            set { SetProperty(ref locationType, value); }
        }

        private string address;

        public string Address
        {
            get { return address; }
            set { SetProperty(ref address, value); }
        }

        private string additionalWork;

        public string AdditionalWork
        {
            get { return additionalWork; }
            set { SetProperty(ref additionalWork, value); }
        }

        private DateTime serviceDateOption1;

        public DateTime ServiceDateOption1
        {
            get { return serviceDateOption1; }
            set { SetProperty(ref serviceDateOption1, value); }
        }

        private DateTime serviceDateOption2;

        public DateTime ServiceDateOption2
        {
            get { return serviceDateOption2; }
            set { SetProperty(ref serviceDateOption2, value); }
        }

        private bool isLiftRequired;

        public bool IsLiftRequired
        {
            get { return isLiftRequired; }
            set { SetProperty(ref isLiftRequired, value); }
        }

        private Dictionary<string, object> selectedItems;

        public Dictionary<string, object> SelectedItems
        {
            get { return selectedItems; }
            set { SetProperty(ref selectedItems, value); }
        }

    }
}
