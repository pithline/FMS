using Eqstra.ServiceScheduling.UILogic;
using System;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Eqstra.ServiceScheduling
{
    public sealed partial class MoreInfo : Page
    {
        private Popup _popup;
        public MoreInfo()
        {
            this.InitializeComponent();
            this.Loaded += MoreInfo_Loaded;

        }

        void MoreInfo_Loaded(object sender, RoutedEventArgs e)
        {
            //profileGrid.DataContext = PersistentData.Instance.UserInfo;
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

            this.DataContext = dataContext;
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

        private void home_Click(object sender, RoutedEventArgs e)
        {

            ((Popup)this.Tag).IsOpen = false;
        }

    }
}