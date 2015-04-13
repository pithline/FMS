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
    public sealed partial class DetailsDialog : ContentDialog
    {
        private Popup _popup;
        public DetailsDialog()
        {
            this.InitializeComponent();
            this.Loaded += MoreInfo_Loaded;

        }

        void MoreInfo_Loaded(object sender, RoutedEventArgs e)
        {
            //profileGrid.DataContext = PersistentData.Instance.UserInfo;
        }

        private void home_Click(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            this.Hide();
        }

    }
}