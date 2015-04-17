
using Eqstra.BusinessLogic.Portable.TIModels;
using System;
using System.Collections.ObjectModel;
using Windows.UI.Core;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using System.Linq;
using Windows.UI.Xaml.Media;
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
        private Popup _popup;
        private double ImageSize;
        public SnapshotsViewer()
        {
            this.InitializeComponent();
            GestureRecognizer gestureRecognizer = new Windows.UI.Input.GestureRecognizer();
            gestureRecognizer.CrossSliding += gestureRecognizer_CrossSliding;

        }
        public void Open(MaintenanceRepair selectedMaintenanceRepair)
        {
            CoreWindow currentWindow = Window.Current.CoreWindow;
            if (_popup == null)
            {
                _popup = new Popup();
            }
            _popup.VerticalOffset = 0;
            _popup.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Stretch;
            _popup.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Stretch;

            this.Tag = _popup;
            this.Height = currentWindow.Bounds.Height;
            this.Width = currentWindow.Bounds.Width;

            _popup.Child = this;
            _popup.IsOpen = true;
            this.DataContext = selectedMaintenanceRepair;
            if (selectedMaintenanceRepair.IsMajorPivot)
            {

                fvSnaps.ItemsSource = selectedMaintenanceRepair.MajorComponentImgList;

            }
            else
            {
                fvSnaps.ItemsSource = selectedMaintenanceRepair.SubComponentImgList;

            }
        }
        void gestureRecognizer_CrossSliding(GestureRecognizer sender, CrossSlidingEventArgs args)
        {
            try
            {
                var snaps = fvSnaps.ItemsSource as ObservableCollection<ImageCapture>;
                var ic = fvSnaps.SelectedItem as ImageCapture;
                snaps.Remove(ic);

                if (((ObservableCollection<ImageCapture>)fvSnaps.ItemsSource).Any())
                {
                    this.Close();
                }
            }
            catch (Exception ex)
            {

            }
        }

        public void Close()
        {
            var popup = this.Tag as Popup;
            popup.IsOpen = false;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            var popup = this.Tag as Popup;
            popup.IsOpen = false;
        }


        private void DeletePic_Click(object sender, RoutedEventArgs e)
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

        private void Image_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            try
            {
                if (sv.ViewportWidth < ImageSize / 2)
                {
                    var snaps = fvSnaps.ItemsSource as ObservableCollection<ImageCapture>;
                    var ic = fvSnaps.SelectedItem as ImageCapture;
                    snaps.Remove(ic);
                }
            }
            catch (Exception ex)
            {

            }

        }
        ScrollViewer sv;
        private void ScrollViewer_Loaded(object sender, RoutedEventArgs e)
        {
            ImageSize = ((ScrollViewer)sender).ViewportWidth;
            sv = ((ScrollViewer)sender);
        }

        private void OnManipulationDelta(object sender, Windows.UI.Xaml.Input.ManipulationDeltaRoutedEventArgs e)
        {
            var ct = (CompositeTransform)((Image)sender).RenderTransform;
            // Scale horizontal.
            ct.ScaleX *= e.Delta.Scale;
            // Scale vertical.
            ct.ScaleY *= e.Delta.Scale;
        }

    }
}
