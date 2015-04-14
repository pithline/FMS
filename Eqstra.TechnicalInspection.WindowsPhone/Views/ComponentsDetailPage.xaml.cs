using Eqstra.TechnicalInspection.UILogic;
using Eqstra.TechnicalInspection.UILogic.WindowsPhone.ViewModels;
using Eqstra.TechnicalInspection.WindowsPhone.Common;
using Microsoft.Practices.Prism.StoreApps;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Eqstra.TechnicalInspection.WindowsPhone.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ComponentsDetailPage : VisualStateAwarePage
    {
        private Popup _popup;
        ComponentsDetailPageViewModel vm;
        public ComponentsDetailPage()
        {
            this.InitializeComponent();
            this.vm = (ComponentsDetailPageViewModel)this.DataContext;
        }
        public void Open(object CurrentVewModel)
        {
            CoreWindow currentWindow = Window.Current.CoreWindow;
            if (_popup == null)
            {
                _popup = new Popup();
            }
            _popup.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Stretch;
            _popup.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Stretch;

            this.DataContext = CurrentVewModel;
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
        private void TasksPivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((Pivot)sender).SelectedIndex == 1)
            {
               // this.vm.SelectedMaintenanceRepair.IsMajorPivot = false;
            }
            else
            {
               // this.vm.SelectedMaintenanceRepair.IsMajorPivot = true;
            }

        }

       async private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            var camCap = new CameraCaptureDialog();
            camCap.Tag = this.vm.SelectedMaintenanceRepair;
            await camCap.ShowAsync();
        }

    }
}
