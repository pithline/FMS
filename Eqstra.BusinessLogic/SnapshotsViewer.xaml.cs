﻿
using Eqstra.BusinessLogic;
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

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace Eqstra.BusinessLogic.Popups
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class SnapshotsViewer : Page
    {
        /// <summary>
        /// This can be changed to a strongly typed view model.
        /// </summary>

        public SnapshotsViewer()
        {
            this.InitializeComponent();

        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            var popup = this.Tag as Popup;
            popup.IsOpen = false;
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var snaps = fvSnaps.ItemsSource as ObservableCollection<ImageCapture>;
                snaps.Remove(fvSnaps.SelectedItem as ImageCapture);
                if (!snaps.Any())
                {
                    this.DeleteButton.Visibility = Visibility.Collapsed;
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
