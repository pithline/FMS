using Eqstra.VehicleInspection.Common;
using Eqstra.VehicleInspection.UILogic.ViewModels;
using Microsoft.Practices.Prism.StoreApps;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace Eqstra.VehicleInspection.Views
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class InspectionDetailsPage : VisualStateAwarePage
    {

        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }
        public InspectionDetailsPage()
        {
            this.InitializeComponent();
        }
        async private void sfDataGrid_SelectionChanged(object sender, Syncfusion.UI.Xaml.Grid.GridSelectionChangedEventArgs e)
        {
            try
            {
                var dc = (InspectionDetailsPageViewModel)this.DataContext;
                await dc.GetCustomerDetailsAsync();

            }
            catch (Exception ex)
            {
                new MessageDialog(ex.Message);
            }

        }
    }
}
