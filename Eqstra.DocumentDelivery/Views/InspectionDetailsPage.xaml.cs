﻿using Eqstra.BusinessLogic;
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
using Eqstra.DocumentDelivery.UILogic;
using Eqstra.BusinessLogic.DocumentDelivery;
// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace Eqstra.DocumentDelivery.Views
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class InspectionDetailsPage : VisualStateAwarePage
    {
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        private List<string> suggestLookup = new List<string>();
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        public InspectionDetailsPage()
        {
            this.InitializeComponent();
            suggestLookup.Add("TaskType");
            suggestLookup.Add("CustomerName");
            suggestLookup.Add("ContactName");
            suggestLookup.Add("CustomerNumber");

        }
        async private void filterBox_QuerySubmitted(SearchBox sender, SearchBoxQuerySubmittedEventArgs args)
        {
            try
            {
                var result = ((InspectionDetailsPageViewModel)this.DataContext).CDTaskList;
                if (result != null)
                {
                    var filterResult = result.Where(x => Convert.ToString(x.TaskType).Contains(args.QueryText) ||
                                                               Convert.ToString(x.CustomerName).Contains(args.QueryText) ||
                                                               Convert.ToString(x.ContactName).Contains(args.QueryText) ||
                                                               Convert.ToString(x.CustomerNumber).Contains(args.QueryText));

                    if (filterResult != null)
                    {
                        this.sfDataGrid.ItemsSource = filterResult;
                    }
                    else
                    {
                        this.sfDataGrid.ItemsSource = new ObservableCollection<CollectDeliveryTask>();
                    }
                }
            }
            catch (Exception ex)
            {
                AppSettings.Instance.ErrorMessage = ex.Message;
            }

        }

        async private void filterBox_SuggestionsRequested(SearchBox sender, SearchBoxSuggestionsRequestedEventArgs args)
        {
            try
            {
                if (((InspectionDetailsPageViewModel)this.DataContext).CDTaskList != null)
                {
                    var deferral = args.Request.GetDeferral();
                    if (!string.IsNullOrEmpty(args.QueryText))
                    {
                        var searchSuggestionList = new List<string>();
                        foreach (var task in ((InspectionDetailsPageViewModel)this.DataContext).CDTaskList)
                        {
                            foreach (var propInfo in task.GetType().GetRuntimeProperties())
                            {
                                if (this.suggestLookup.Contains(propInfo.Name))
                                {
                                    var propVal = Convert.ToString(propInfo.GetValue(task));
                                    if (propVal.ToLowerInvariant().Contains(args.QueryText))
                                    {
                                        if (!searchSuggestionList.Contains(propVal))
                                            searchSuggestionList.Add(propVal);
                                    }
                                }
                            }
                        }
                        args.Request.SearchSuggestionCollection.AppendQuerySuggestions(searchSuggestionList);
                    }
                    else
                    {
                        this.sfDataGrid.ItemsSource = ((InspectionDetailsPageViewModel)this.DataContext).CDTaskList;
                    }
                    deferral.Complete();

                }
            }
            catch (Exception ex)
            {
                AppSettings.Instance.ErrorMessage = ex.Message;
            }
        }

        private async void sfDataGrid_SelectionChanged(object sender, Syncfusion.UI.Xaml.Grid.GridSelectionChangedEventArgs e)
        {
            try
            {
                var dc = (InspectionDetailsPageViewModel)this.DataContext;
                await dc.GetAllDocumentFromDbByCustomer();
            }
            catch (Exception ex)
            {
                new MessageDialog(ex.Message);
            }
        }

    }
}
