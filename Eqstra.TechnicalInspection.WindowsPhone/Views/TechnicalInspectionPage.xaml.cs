using Eqstra.BusinessLogic.Portable.TIModels;
using Eqstra.TechnicalInspection.WindowsPhone.Common;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.StoreApps;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        ComponentsDetailPage cdp;
        InspectionDetailDialog insp;
        DetailsDialog dd;
        public TechnicalInspectionPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            if (insp != null)
            {
                insp.Hide();
            }
            if (dd != null)
            {
                dd.Hide();
            }

        }
        async private void ViewAll_Click(object sender, RoutedEventArgs e)
        {
            insp = new InspectionDetailDialog();
            insp.DataContext = this.DataContext;
            await insp.ShowAsync();
        }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var item = (MaintenanceRepair)e.ClickedItem;
            Frame.Navigate(typeof(ComponentsDetailPage), JsonConvert.SerializeObject(item));
        }

        async private void More_Click(object sender, RoutedEventArgs e)
        {
            dd = new DetailsDialog();
            await dd.ShowAsync();

        }
    }

}
