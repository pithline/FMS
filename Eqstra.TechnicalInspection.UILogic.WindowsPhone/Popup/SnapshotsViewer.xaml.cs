
using Eqstra.BusinessLogic.Portable.TIModels;
using System;
using System.Collections.ObjectModel;
using Windows.UI.Core;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace Eqstra.TechnicalInspection.UILogic
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
            GestureRecognizer gestureRecognizer = new Windows.UI.Input.GestureRecognizer();
            gestureRecognizer.CrossSlideExact = true;
            gestureRecognizer.CrossSliding += gestureRecognizer_CrossSliding;

        }
        public void Open(MaintenanceRepair selectedMaintenanceRepair)
        {
            CoreWindow currentWindow = Window.Current.CoreWindow;
            Popup popup = new Popup();
            popup.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Stretch;
            popup.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Stretch;
            this.DataContext = selectedMaintenanceRepair;
            if (selectedMaintenanceRepair.IsMajorPivot)
            {

                fvSnaps.ItemsSource = selectedMaintenanceRepair.MajorComponentImgList;
            }
            else
            {
                fvSnaps.ItemsSource = selectedMaintenanceRepair.SubComponentImgList;
            }

            this.Height = currentWindow.Bounds.Height;
            this.Width = currentWindow.Bounds.Width;

            popup.IsOpen = true;

        }
        void gestureRecognizer_CrossSliding(GestureRecognizer sender, CrossSlidingEventArgs args)
        {
            try
            {
                var snaps = fvSnaps.ItemsSource as ObservableCollection<ImageCapture>;
                var ic = fvSnaps.SelectedItem as ImageCapture;
                snaps.Remove(ic);
            }
            catch (Exception ex)
            {

            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            var popup = this.Tag as Popup;
            popup.IsOpen = false;
        }

    }
}
