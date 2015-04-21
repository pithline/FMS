using Eqstra.TechnicalInspection.UILogic;
using Eqstra.TechnicalInspection.UILogic.WindowsPhone.ViewModels;
using Microsoft.Practices.Prism.StoreApps;
using System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Eqstra.TechnicalInspection.WindowsPhone.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ComponentsDetailPage : VisualStateAwarePage
    {
        ComponentsDetailPageViewModel vm;
        private SnapshotsViewer _snapShotsPopup;
        public ComponentsDetailPage()
        {
            this.InitializeComponent();
            this.vm = (ComponentsDetailPageViewModel)this.DataContext;
            this.NavigationCacheMode = NavigationCacheMode.Required;

            Window.Current.SizeChanged += Current_SizeChanged;
        }

        void Current_SizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            var b = Window.Current.Bounds;
            ImagesPivot.Width = b.Width;
        }

        private void TasksPivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((Pivot)sender).SelectedIndex == 1)
            {
                this.vm.SelectedMaintenanceRepair.IsMajorPivot = false;
            }
            else
            {
                this.vm.SelectedMaintenanceRepair.IsMajorPivot = true;
            }

        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }

        private void Image_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            ((Image)sender).Height = 600;
            ((Image)sender).Width = 600;
        }

        private void Image_Tapped_1(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            ((Image)sender).Height = 600;
            ((Image)sender).Width = 600;
        }

        private void Image_Holding(object sender, Windows.UI.Xaml.Input.HoldingRoutedEventArgs e)
        {
            ((Image)sender).Height = 120;
            ((Image)sender).Width = 120;
        }

        private void Image_Holding_1(object sender, Windows.UI.Xaml.Input.HoldingRoutedEventArgs e)
        {
            ((Image)sender).Height = 120;
            ((Image)sender).Width = 120;
        }

        private void GridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (this.vm!=null)
            {
                _snapShotsPopup = new SnapshotsViewer();
                _snapShotsPopup.Open(this.vm.SelectedMaintenanceRepair); 
            }
        }

    }
}
