﻿using Eqstra.BusinessLogic.Portable.SSModels;
using Eqstra.ServiceScheduling.UILogic;
using Eqstra.ServiceScheduling.UILogic.Portable;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.StoreApps;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Eqstra.ServiceScheduling.WindowsPhone.Views
{
    public sealed partial class PreferredSupplierPage : VisualStateAwarePage
    {
     
        public PreferredSupplierPage()
        {
            this.InitializeComponent();
            this.Loaded += PreferredSupplierPage_Loaded;
        }

        void PreferredSupplierPage_Loaded(object sender, RoutedEventArgs e)
        {
            this.vm = this.DataContext as PreferredSupplierPageViewModel;
        }
        public PreferredSupplierPageViewModel vm { get; set; }
        private void More_Click(object sender, RoutedEventArgs e)
        {
            MoreInfo m = new MoreInfo();
            this.vm = this.DataContext as PreferredSupplierPageViewModel;
            m.Open(this);
        }

        private void Find_Click(object sender, RoutedEventArgs e)
        {

        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {


        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
        }


        private void filter_TextChanged(object sender, TextChangedEventArgs e)
        {
            var text = ((TextBox)sender).Text;
            if (!String.IsNullOrEmpty(text))
            {
                ObservableCollection<BusinessLogic.Portable.SSModels.Supplier> filterResult = new ObservableCollection<BusinessLogic.Portable.SSModels.Supplier>();
                foreach (var task in PersistentData.Instance.PoolofSupplier)
                {
                    if (task.SupplierName.Contains(text))
                    {
                        filterResult.Add(task);
                    }
                }
                ((PreferredSupplierPageViewModel)this.DataContext).PoolofSupplier = filterResult;
            }
            else
            {
                ((PreferredSupplierPageViewModel)this.DataContext).PoolofSupplier = PersistentData.Instance.PoolofSupplier;

            }
        }

        private void filter_Click(object sender, RoutedEventArgs e)
        {

            if (vm != null)
            {
                SearchSupplierPopup sp = new SearchSupplierPopup(vm._locationService,vm._eventAggregator, vm._supplierService);
                sp.Open();
            }
        }
    }

}
