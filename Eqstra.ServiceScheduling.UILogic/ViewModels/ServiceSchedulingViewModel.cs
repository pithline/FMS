using Eqstra.BusinessLogic;
using Eqstra.BusinessLogic.Helpers;
using Eqstra.BusinessLogic.ServiceSchedule;
using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eqstra.ServiceScheduling.UILogic.ViewModels
{
   public class ServiceSchedulingPageViewModel : ViewModel
    {
       private DriverTask _task;
       INavigationService _navigationService;
       public ServiceSchedulingPageViewModel(INavigationService navigationService)
       {
           _navigationService = navigationService;
           this.CustomerDetails = new CustomerDetails();
           this.GoToSupplierSelectionCommand = new DelegateCommand(() =>
           {
               _navigationService.Navigate("SupplierSelection",this.CustomerDetails);
           });


           
       }

       async public override void OnNavigatedTo(object navigationParameter, Windows.UI.Xaml.Navigation.NavigationMode navigationMode, Dictionary<string, object> viewModelState)
       {
           base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
           _task = navigationParameter as DriverTask;
           await GetCustomerDetailsAsync();
           this.ODOReadingImagePath = "ms-appx:///Assets/odo_meter.png";
       }

       private CustomerDetails customerDetails;

       public CustomerDetails CustomerDetails
       {
           get { return customerDetails; }
           set { SetProperty(ref customerDetails, value); }
       }

       private Customer customer;

       public Customer Customer
       {
           get { return customer; }
           set { SetProperty(ref customer, value); }
       }

       async private System.Threading.Tasks.Task GetCustomerDetailsAsync()
       {
           try
           {
               if (this._task != null)
               {
                   this.Customer = await SqliteHelper.Storage.GetSingleRecordAsync<Customer>(c => c.Id == this._task.CustomerId);
                   this.CustomerDetails.ContactNumber = this.Customer.ContactNumber;
                   this.CustomerDetails.CaseNumber = this._task.CaseNumber;
                   this.CustomerDetails.Status = this._task.Status;
                   this.CustomerDetails.StatusDueDate = this._task.StatusDueDate;
                   this.CustomerDetails.Address = this.Customer.Address;
                   this.CustomerDetails.AllocatedTo = this._task.AllocatedTo;
                   this.CustomerDetails.Name = this.Customer.Name;
                   this.CustomerDetails.CellNumber = this._task.CellNumber;
                   this.CustomerDetails.CaseType = this._task.CaseType;
                   this.CustomerDetails.EmailId = this.Customer.EmailId;
               }
           }
           catch (Exception)
           {
               throw;
           }

       }

       public DelegateCommand GoToSupplierSelectionCommand { get; set; }

       public DelegateCommand ODOReadingPictureCommand { get; set; }

       private string odoReadingImagePath;

       public string ODOReadingImagePath
       {
           get { return odoReadingImagePath; }
           set { SetProperty(ref odoReadingImagePath, value); }
       }

    }
}
