using Eqstra.BusinessLogic.Helpers;
using Microsoft.Practices.Prism.StoreApps;
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
using Eqstra.BusinessLogic.ServiceSchedule;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Eqstra.ServiceScheduling.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : VisualStateAwarePage
    {
        bool isCached;
        public MainPage()
        {
            this.InitializeComponent();
        }
        private void WeatherInfo_Tapped(object sender, TappedRoutedEventArgs e)
        {
        }
        async private void filterBox_QuerySubmitted(SearchBox sender, SearchBoxQuerySubmittedEventArgs args)
        {
            var result = await Util.ReadDriverTasksFromDiskAsync("MainItemsSourceFile.txt");
            if (result != null)
            {
                this.mainGrid.ItemsSource = result.Where(x =>
                         x.CaseNumber.Contains(args.QueryText) ||
                        Convert.ToString(x.CaseType).Contains(args.QueryText) ||
                         x.CustomerName.Contains(args.QueryText) ||
                         x.RegistrationNumber.Contains(args.QueryText) ||
                         x.Status.Contains(args.QueryText));
            }
        }
        async private void filterBox_SuggestionsRequested(SearchBox sender, SearchBoxSuggestionsRequestedEventArgs args)
        {
            if (this.mainGrid.ItemsSource != null & ((IEnumerable<DriverTask>)this.mainGrid.ItemsSource).Any())
            {
                var deferral = args.Request.GetDeferral();
                if (!string.IsNullOrEmpty(args.QueryText))
                {
                    if (!isCached)
                    {
                        await Util.WriteDriverTasksToDiskAsync(JsonConvert.SerializeObject(this.mainGrid.ItemsSource), "MainItemsSourceFile.txt");
                        isCached = true;
                    }

                    var searchSuggestionList = new List<string>();
                    foreach (var task in await Util.ReadDriverTasksFromDiskAsync("MainItemsSourceFile.txt"))
                    {
                        foreach (var propInfo in task.GetType().GetRuntimeProperties())
                        {
                            if (propInfo.Name.Equals("ModelYear") || propInfo.Name.Equals("ConfirmedTime") || propInfo.Name.Equals("ConfirmedDate") || propInfo.Name.Equals("VehicleInsRecId") || propInfo.Name.Equals("CustomerId")
                                || propInfo.Name.Equals("CaseServiceRecID") ||propInfo.PropertyType.Name.Equals(typeof(System.Boolean).Name) || propInfo.PropertyType.Name.Equals(typeof(BindableValidator).Name) || propInfo.Name.Equals("Address")|| propInfo.Name.Equals("CollectionRecID") || propInfo.Name.Equals("DriverPhone") || propInfo.Name.Equals("DriverLastName") || propInfo.Name.Equals("DriverFirstName"))
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
