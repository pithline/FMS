
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
    public sealed partial class SnapshotsViewer : ContentDialog
    {
        /// <summary>
        /// This can be changed to a strongly typed view model.
        /// </summary>

        private double ImageSize;
        public SnapshotsViewer()
        {
            this.InitializeComponent();
            GestureRecognizer gestureRecognizer = new Windows.UI.Input.GestureRecognizer();
            gestureRecognizer.CrossSliding += gestureRecognizer_CrossSliding;

        }
        async public void Open(MaintenanceRepair selectedMaintenanceRepair)
        {
            this.DataContext = selectedMaintenanceRepair;
            if (selectedMaintenanceRepair.IsMajorPivot)
            {

                fvSnaps.ItemsSource = selectedMaintenanceRepair.MajorComponentImgList;

            }
            else
            {
                fvSnaps.ItemsSource = selectedMaintenanceRepair.SubComponentImgList;

            }
            await this.ShowAsync();
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
                    this.Hide();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void DeletePic_Click(ContentDialog sender, ContentDialogButtonClickEventArgs args)
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
