﻿using Microsoft.Practices.Prism.StoreApps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eqstra.BusinessLogic.Passenger;

namespace Eqstra.VehicleInspection.UILogic.ViewModels
{
    public class VehicleDetailsUserControlViewModel : BaseViewModel
    {
        public VehicleDetailsUserControlViewModel()
        {
            this.Model = new PVehicleDetails();
        }
        public override void OnNavigatedTo(object navigationParameter, Windows.UI.Xaml.Navigation.NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
        }
    }
}
