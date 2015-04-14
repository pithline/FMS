using Eqstra.BusinessLogic.Portable.TIModels;
using Eqstra.TechnicalInspection.WindowsPhone.Common;
using Microsoft.Practices.Prism.StoreApps;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Eqstra.TechnicalInspection.WindowsPhone.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TechnicalInspectionPage : VisualStateAwarePage
    {
        public TechnicalInspectionPage()
        {
            this.InitializeComponent();
        }
        async private void ViewAll_Click(object sender, RoutedEventArgs e)
        {
            //InspectionDetailDialog insp = new InspectionDetailDialog();
            //insp.DataContext = this.DataContext;
            //await insp.ShowAsync();
            ComponentsDetailPage CP = new ComponentsDetailPage();
            CP.Open(this.DataContext);
        }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var item = (MaintenanceRepair)e.ClickedItem;
            Frame.Navigate(typeof(ComponentsDetailPage), JsonConvert.SerializeObject(item));
        }

        private void More_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
