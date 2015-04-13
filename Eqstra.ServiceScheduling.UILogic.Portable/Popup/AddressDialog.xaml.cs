using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Pickers.Provider;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Content Dialog item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Eqstra.ServiceScheduling
{
    public sealed partial class AddressDialog : ContentDialog
    {
        public AddressDialog()
        {
            this.InitializeComponent();
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void country_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Provinces_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void City_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void suburb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void region_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
