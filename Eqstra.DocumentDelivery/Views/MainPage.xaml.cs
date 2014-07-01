using Eqstra.BusinessLogic;
using Eqstra.BusinessLogic.Helpers;
using Eqstra.DocumentDelivery.Common;
using Microsoft.Practices.Prism.StoreApps;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.ApplicationSettings;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Reflection;
using System.Collections.ObjectModel;
// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace Eqstra.DocumentDelivery.Views
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class MainPage : VisualStateAwarePage
    {
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        private bool isCached;
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        public MainPage()
        {
            this.InitializeComponent();


        }
        async private void filterBox_QuerySubmitted(SearchBox sender, SearchBoxQuerySubmittedEventArgs args)
        {
            try
            {
                var result = await Util.ReadFromDiskAsync<CollectDeliveryTask>("MainItemsSourceFile.txt");
                if (result != null)
                {
                    this.mainGrid.ItemsSource = result.Where(x => x.CustomerName.Contains(args.QueryText) ||
                                      Convert.ToString(x.DocumentCount).Contains(args.QueryText) || x.AllocatedTo.Contains(args.QueryText) ||
                                     Convert.ToString(x.TaskType).Contains(args.QueryText) || Convert.ToString(x.ConfirmedDate).Contains(args.QueryText) ||
                                     Convert.ToString(x.StatusDueDate).Contains(args.QueryText) || x.Status.Contains(args.QueryText) ||
                                     Convert.ToString(x.DeliveryTime).Contains(args.QueryText) || Convert.ToString(x.DeliveryDate).Contains(args.QueryText));

                    
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        async private void filterBox_SuggestionsRequested(SearchBox sender, SearchBoxSuggestionsRequestedEventArgs args)
        {
            try
            {
                if (this.mainGrid.ItemsSource != null & ((IEnumerable<CollectDeliveryTask>)this.mainGrid.ItemsSource).Any())
                {
                    var deferral = args.Request.GetDeferral();
                    if (!string.IsNullOrEmpty(args.QueryText))
                    {
                        if (!isCached)
                        {
                            await Util.WriteToDiskAsync(JsonConvert.SerializeObject(this.mainGrid.ItemsSource), "MainItemsSourceFile.txt");
                            isCached = true;
                        }

                        var searchSuggestionList = new List<string>();
                        foreach (var task in await Util.ReadFromDiskAsync<CollectDeliveryTask>("MainItemsSourceFile.txt"))
                        {
                            foreach (var propInfo in task.GetType().GetRuntimeProperties())
                            {
                                if (propInfo.PropertyType.Name.Equals(typeof(System.Boolean).Name) || propInfo.Name.Equals("VehicleInsRecId") ||
                                    propInfo.PropertyType.Name.Equals(typeof(BindableValidator).Name) ||
                                    propInfo.Name.Equals("Address"))
                                    continue;
                                var propVal = Convert.ToString(propInfo.GetValue(task));
                                if (propVal.ToLowerInvariant().Contains(args.QueryText))
                                {
                                    if (!searchSuggestionList.Contains(propVal))
                                        searchSuggestionList.Add(propVal);
                                }
                            }
                        }
                        args.Request.SearchSuggestionCollection.AppendQuerySuggestions(searchSuggestionList);
                    }
                    deferral.Complete();
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
        private void ProfileUserControl_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var element = sender as FrameworkElement;
            if (element != null)
            {
                FlyoutBase.ShowAttachedFlyout(element);
            }
        }
        async private void WeatherInfo_Tapped(object sender, TappedRoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("bingweather:"));
        }
    }
}
