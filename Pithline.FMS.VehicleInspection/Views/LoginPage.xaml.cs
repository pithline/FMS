﻿using Eqstra.VehicleInspection.Common;
using Eqstra.VehicleInspection.UILogic.ViewModels;
using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Xaml.Interactions.Core;
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

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace Eqstra.VehicleInspection.Views
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class LoginPage : VisualStateAwarePage
    {
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        public LoginPage()
        {
            this.InitializeComponent();
        }
        private void PasswordBox_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            var vm = (LoginPageViewModel)this.DataContext;
            vm.ErrorMessage = string.Empty;
            if(e.Key == Windows.System.VirtualKey.Enter)
            {
                this.btnLogin.Command.Execute(null);
            }
        }
    }
}
