using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Eqstra.TechnicalInspection
{
    public sealed partial class DetailsDialog : ContentDialog
    {
        public DetailsDialog()
        {
            this.InitializeComponent();
            Window.Current.SizeChanged += Current_SizeChanged;
        }
        void Current_SizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            var b = Window.Current.Bounds;
            this.scrlViewTask.Height = b.Height - 50;
            this.scrlViewCust.Height = b.Height - 50;
        }
    }
}