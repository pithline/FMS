﻿using Eqstra.BusinessLogic;
using Microsoft.Practices.Prism.StoreApps;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eqstra.VehicleInspection.UILogic.ViewModels
{
   public class SchedulerPageViewModel :ViewModel
    {
       public SchedulerPageViewModel()
       {
         
       }

       private CustomerDetails customerDetails;

       public CustomerDetails CustomerDetails
       {
           get { return customerDetails; }
           set { SetProperty(ref customerDetails, value); }
       }
       
       public override void OnNavigatedTo(object navigationParameter, Windows.UI.Xaml.Navigation.NavigationMode navigationMode, Dictionary<string, object> viewModelState)
       {
           base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
           this.CustomerDetails = JsonConvert.DeserializeObject<CustomerDetails>(navigationParameter.ToString());
       }

    }
}
