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
// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace Eqstra.ServiceScheduling.Views
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class SupplierSelectionPage : Microsoft.Practices.Prism.StoreApps.VisualStateAwarePage
    {

        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        bool isCached;
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        public SupplierSelectionPage()
        {
            this.InitializeComponent();
        }

        async private void filterBox_QuerySubmitted(SearchBox sender, SearchBoxQuerySubmittedEventArgs args)
        {
            var result = await Util.ReadFromDiskAsync<Supplier>("SuppliersGridItemsSourceFile.txt");
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
                        await Util.WriteToDiskAsync(JsonConvert.SerializeObject(this.suppliersGrid.ItemsSource), "SuppliersGridItemsSourceFile.txt");
                        isCached = true;
                    }

                    var searchSuggestionList = new List<string>();
                    foreach (var task in await Util.ReadFromDiskAsync<Supplier>("SuppliersGridItemsSourceFile.txt"))
                    {
                        foreach (var propInfo in task.GetType().GetRuntimeProperties())
                        {
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