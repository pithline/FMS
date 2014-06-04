using Eqstra.BusinessLogic;
using Eqstra.VehicleInspection.Common;
using Eqstra.VehicleInspection.UILogic.ViewModels;
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
using Windows.Storage;
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

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace Eqstra.VehicleInspection.Views
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class MainPage : VisualStateAwarePage
    {

        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        private Windows.Storage.StorageFolder temporaryFolder = ApplicationData.Current.TemporaryFolder;
        private bool isCached;

        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }
        public MainPage()
        {
            this.InitializeComponent();
        }
        async System.Threading.Tasks.Task WriteTasksToDiskAsync(string content)
        {
            StorageFile itemsSourceFile = await temporaryFolder.CreateFileAsync("MainItemsSourceFile.txt", CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(itemsSourceFile, content);
        }

        async System.Threading.Tasks.Task<ObservableCollection<Eqstra.BusinessLogic.Task>> ReadTasksFromDiskAsync()
        {
            try
            {
                StorageFile itemsSourceFile = await temporaryFolder.GetFileAsync("MainItemsSourceFile.txt");
                String jsonItemsSource = await FileIO.ReadTextAsync(itemsSourceFile);
                return JsonConvert.DeserializeObject<ObservableCollection<Eqstra.BusinessLogic.Task>>(jsonItemsSource);
            }
            catch (Exception)
            {
                return null;
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
        async private void filterBox_QuerySubmitted(SearchBox sender, SearchBoxQuerySubmittedEventArgs args)
        {
            this.mainGrid.ItemsSource = (await ReadTasksFromDiskAsync()).Where(x => x.CaseCategory.Contains(args.QueryText) ||
                 x.CaseNumber.Contains(args.QueryText) ||
                 x.CaseType.ToString().Contains(args.QueryText) ||
                 x.CustomerName.Contains(args.QueryText) ||
                 x.RegistrationNumber.Contains(args.QueryText) ||
                  x.DisplayStatus.Contains(args.QueryText) ||
                 x.Status.ToString().Contains(args.QueryText));
        }


        async private void filterBox_SuggestionsRequested(SearchBox sender, SearchBoxSuggestionsRequestedEventArgs args)
        {
            var deferral = args.Request.GetDeferral();
            if (!string.IsNullOrEmpty(args.QueryText))
            {
                if (!isCached)
                {
                    await WriteTasksToDiskAsync(JsonConvert.SerializeObject(this.mainGrid.ItemsSource));
                    isCached = true;
                }

                var searchSuggestionList = new List<string>();
                foreach (var task in await ReadTasksFromDiskAsync())
                {
                    foreach (var propInfo in task.GetType().GetRuntimeProperties())
                    {
                        if (propInfo.PropertyType.Name.Equals(typeof(System.Boolean).Name) || propInfo.PropertyType.Name.Equals(typeof(BindableValidator).Name) || propInfo.Name.Equals("Address"))
                            continue;
                        var propVal = propInfo.GetValue(task).ToString();
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
