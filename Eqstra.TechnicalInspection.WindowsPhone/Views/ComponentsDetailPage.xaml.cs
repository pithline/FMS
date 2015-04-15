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
        public ComponentsDetailPage()
        {
            this.InitializeComponent();
            this.vm = (ComponentsDetailPageViewModel)this.DataContext;
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

    }
}
