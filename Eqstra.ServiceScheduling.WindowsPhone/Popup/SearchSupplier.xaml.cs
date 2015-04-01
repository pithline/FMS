﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Eqstra.ServiceScheduling
{

    public sealed partial class SearchSupplier : Page
    {
        private Popup _popup;
        public SearchSupplier()
        {
            this.InitializeComponent();
        }

        public void Open(object dataContext)
        {
            CoreWindow currentWindow = Window.Current.CoreWindow;
            if (_popup == null)
            {
                _popup = new Popup();
            }
            _popup.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Stretch;
            _popup.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Stretch;

            //this.DataContext = dataContext;
            this.Tag = _popup;
            this.Height = currentWindow.Bounds.Height;
            this.Width = currentWindow.Bounds.Width;

            _popup.Child = this;
            _popup.IsOpen = true;
        }
        public void Close()
        {
            ((Popup)this.Tag).IsOpen = false;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();

        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
