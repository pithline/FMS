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
        HashSet<string> _searchSuggestionList = new HashSet<string>();
        public MainPage()
        {
            this.InitializeComponent();
        }
        private void WeatherInfo_Tapped(object sender, TappedRoutedEventArgs e)
        {
        }
        async private void filterBox_QuerySubmitted(SearchBox sender, SearchBoxQuerySubmittedEventArgs args)
        {
            var result = await Util.ReadFromDiskAsync<DriverTask>("MainItemsSourceFile.json");
            if (result != null)
            {
                this.mainGrid.ItemsSource = result.Where(x =>
                         x.CaseNumber.Contains(args.QueryText) ||

                         x.CustomerName.Contains(args.QueryText) ||
                         x.RegistrationNumber.Contains(args.QueryText) ||
                         x.Model.Contains(args.QueryText) ||
                         x.Make.Contains(args.QueryText) ||
                         x.Description.Contains(args.QueryText) ||
                         x.Address.Contains(args.QueryText) ||
                         x.Status.Contains(args.QueryText));
            }
        }
        async private void filterBox_SuggestionsRequested(SearchBox sender, SearchBoxSuggestionsRequestedEventArgs args)
        {
            try
            {
                if (this.mainGrid.ItemsSource != null & ((IEnumerable<DriverTask>)this.mainGrid.ItemsSource).Any())
                {
                    var deferral = args.Request.GetDeferral();
                    var query = args.QueryText != null ? args.QueryText.Trim() : null;
                    if (string.IsNullOrEmpty(query))
                    {
                        return;
                    }
                    if (!string.IsNullOrEmpty(args.QueryText))
                    {
                        if (!isCached)
                        {
                            await Util.WriteToDiskAsync(JsonConvert.SerializeObject(this.mainGrid.ItemsSource), "MainItemsSourceFile.json");
                            isCached = true;
                        }
                        _searchSuggestionList.Clear();
                        foreach (var task in await Util.ReadFromDiskAsync<DriverTask>("MainItemsSourceFile.json"))
                        {
                            foreach (var propInfo in task.GetType().GetRuntimeProperties())
                            {
                                if (propInfo.Name.Equals("CaseCategory") || propInfo.Name.Equals("CaseType") || propInfo.Name.Equals("ModelYear") || propInfo.Name.Equals("ConfirmedTime") || propInfo.Name.Equals("ConfirmedDate") || propInfo.Name.Equals("VehicleInsRecId") || propInfo.Name.Equals("CustomerId")
                                    || propInfo.Name.Equals("CaseServiceRecID") || propInfo.PropertyType.Name.Equals(typeof(System.Boolean).Name) || propInfo.PropertyType.Name.Equals(typeof(BindableValidator).Name) || propInfo.Name.Equals("Address") || propInfo.Name.Equals("CollectionRecID") || propInfo.Name.Equals("DriverPhone") || propInfo.Name.Equals("DriverLastName") || propInfo.Name.Equals("DriverFirstName"))
                                    continue;

                                var propVal = Convert.ToString(propInfo.GetValue(task));
                                if (propVal.ToLowerInvariant().Contains(query.ToLowerInvariant()))
                                {

                                    args.Request.SearchSuggestionCollection.AppendQuerySuggestion(propVal);

                                }
                            }
                        }

                    }
                    deferral.Complete();
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
