using Eqstra.BusinessLogic.Helpers;
using Eqstra.ServiceScheduling.Common;
using Newtonsoft.Json;
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
using System.Reflection;
using Microsoft.Practices.Prism.StoreApps;
using Eqstra.BusinessLogic.ServiceSchedule;
using Eqstra.ServiceScheduling.UILogic.ViewModels;
using Eqstra.ServiceScheduling.UILogic.AifServices;
// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace Eqstra.ServiceScheduling.Views
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class SupplierSelectionPage : Microsoft.Practices.Prism.StoreApps.VisualStateAwarePage
    {

        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        private SupplierSelection supplierSelection = null;
        Country selectedCountry = null;
        province selectedprovince = null;
        City selectedCity = null;
        bool isCached;
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }
        public SupplierSelectionPage()
        {
            this.InitializeComponent();
            this.supplierSelection = ((SupplierSelection)((SupplierSelectionPageViewModel)this.DataContext).Model);
            this.selectedCountry = new Country();
            this.selectedprovince = new province();
            this.selectedCity = new City();
        }

        async private void filterBox_QuerySubmitted(SearchBox sender, SearchBoxQuerySubmittedEventArgs args)
        {
            var result = await Util.ReadFromDiskAsync<Supplier>("SuppliersGridItemsSourceFile.json");
            if (result != null)
            {
                this.suppliersGrid.ItemsSource = result.Where(x =>
                         x.SupplierContactName.Contains(args.QueryText) ||
                        Convert.ToString(x.SupplierContactNumber).Contains(args.QueryText) ||
                         x.SupplierName.Contains(args.QueryText));
            }
        }
        async private void filterBox_SuggestionsRequested(SearchBox sender, SearchBoxSuggestionsRequestedEventArgs args)
        {
            if (this.suppliersGrid.ItemsSource != null)
            {
                var deferral = args.Request.GetDeferral();
                if (!string.IsNullOrEmpty(args.QueryText))
                {
                    if (!isCached)
                    {
                        await Util.WriteToDiskAsync(JsonConvert.SerializeObject(this.suppliersGrid.ItemsSource), "SuppliersGridItemsSourceFile.json");
                        isCached = true;
                    }

                    var searchSuggestionList = new List<string>();
                    foreach (var task in await Util.ReadFromDiskAsync<Supplier>("SuppliersGridItemsSourceFile.json"))
                    {
                        foreach (var propInfo in task.GetType().GetRuntimeProperties())
                        {
                            if (!(propInfo.PropertyType.Name.Equals("SupplierName")||propInfo.PropertyType.Name.Equals("supplierContactName")||propInfo.PropertyType.Name.Equals("SupplierContactNumber")))
                                continue;
                            var propVal = Convert.ToString(propInfo.GetValue(task));
                            if (propVal.ToLowerInvariant().Contains(args.QueryText))
                            {
                                searchSuggestionList.Add(propVal);
                            }
                        }
                    }
                    args.Request.SearchSuggestionCollection.AppendQuerySuggestions(searchSuggestionList);
                }
                deferral.Complete();
            }
        }
    }
}