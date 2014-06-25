using Eqstra.BusinessLogic;
using Eqstra.BusinessLogic.Helpers;
using Eqstra.DocumentDelivery.Common;
using Eqstra.DocumentDelivery.UILogic.ViewModels;
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
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Reflection;
// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace Eqstra.DocumentDelivery.Views
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class InspectionDetailsPage : VisualStateAwarePage
    {
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        private bool isCached;
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
                if (e.AddedItems != null && e.AddedItems.Count > 0)
                {
                    var dc = (InspectionDetailsPageViewModel)this.DataContext;
                    dc.SelectedTaskList.Clear();
                    foreach (CollectDeliveryTask item in e.AddedItems)
                    {
                        dc.SelectedTaskList.Add(item);
                    }

                    dc.SaveTaskCommand.RaiseCanExecuteChanged();
                    dc.NextStepCommand.RaiseCanExecuteChanged();
                    await dc.GetCustomerDetailsAsync();
                }

            }
            catch (Exception ex)
            {
                new MessageDialog(ex.Message);
            }

        }
        async private void filterBox_QuerySubmitted(SearchBox sender, SearchBoxQuerySubmittedEventArgs args)
        {
            try
            {
                var result = await Util.ReadDeliveryTaskFromDiskAsync("DetailsItemsSourceFile.txt");
                if (result != null)
                {
                    this.sfDataGrid.ItemsSource = result.Where(x => x.CustomerName.Contains(args.QueryText) ||
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
            var deferral = args.Request.GetDeferral();
            if (!string.IsNullOrEmpty(args.QueryText))
            {
                if (!isCached)
                {
                    await Util.WriteDeliveryTaskToDiskAsync(JsonConvert.SerializeObject(this.sfDataGrid.ItemsSource), "DetailsItemsSourceFile.txt");
                    isCached = true;
                }

                var searchSuggestionList = new List<string>();
                foreach (var task in await Util.ReadDeliveryTaskFromDiskAsync("DetailsItemsSourceFile.txt"))
                {
                    foreach (var propInfo in task.GetType().GetRuntimeProperties())
                    {
                        if (propInfo.PropertyType.Name.Equals(typeof(System.Boolean).Name) || propInfo.Name.Equals("VehicleInsRecId") ||
                            propInfo.PropertyType.Name.Equals(typeof(BindableValidator).Name) || propInfo.Name.Equals("Address"))
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
}
