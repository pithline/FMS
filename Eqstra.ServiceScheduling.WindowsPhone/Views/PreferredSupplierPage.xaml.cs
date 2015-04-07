using Eqstra.ServiceScheduling.UILogic;
using Eqstra.ServiceScheduling.UILogic.Portable;
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

        private void country_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.vm = this.DataContext as PreferredSupplierPageViewModel;
            vm.CountryChanged();
        }

        private void Provinces_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            vm.ProvinceChanged();
        }

        private void City_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            vm.CityChanged();
        }

        private void suburb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void region_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

    }

}
