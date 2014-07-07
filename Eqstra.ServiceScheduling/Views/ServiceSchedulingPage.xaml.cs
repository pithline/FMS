using Eqstra.BusinessLogic.ServiceSchedule;
using Eqstra.ServiceScheduling.Common;
using Eqstra.ServiceScheduling.UILogic.AifServices;
using Eqstra.ServiceScheduling.UILogic.ViewModels;
using Microsoft.Practices.Prism.StoreApps;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace Eqstra.ServiceScheduling.Views
{
    public sealed partial class ServiceSchedulingPage : VisualStateAwarePage
    {

        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        ServiceSchedulingDetail serviceSchedulingDetail = null;
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }
        public ServiceSchedulingPage()
        {
            this.InitializeComponent();
            serviceSchedulingDetail = ((ServiceSchedulingDetail)((ServiceSchedulingPageViewModel)this.DataContext).Model);
        }
      
        private void ddLocationType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
          LocationType selectedLocationType= this.serviceSchedulingDetail.SelectedLocationType;
           //if(selectedLocationType.LocationTyp="")
           //{
           //    SSProxyHelper.Instance.GetCustomersFromSvcAsync();
           //}
        }

    }
}
