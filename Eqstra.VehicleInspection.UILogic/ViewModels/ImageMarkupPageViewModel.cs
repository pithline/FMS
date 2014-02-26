using Eqstra.BusinessLogic;
using Eqstra.BusinessLogic.Passenger;
using Microsoft.Practices.Prism.StoreApps;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Eqstra.VehicleInspection.UILogic.ViewModels
{
   public class ImageMarkupPageViewModel : ViewModel
    {
       public ImageMarkupPageViewModel()
       {
           this.Snapshots = new ObservableCollection<ImageCapture>();
       }
       public override void OnNavigatedTo(object navigationParameter, Windows.UI.Xaml.Navigation.NavigationMode navigationMode, Dictionary<string, object> viewModelState)
       {
           base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
           var model = (PVehicleDetails)navigationParameter;
           this.Snapshots.Add(model.FrontSnapshot);
           this.Snapshots.Add(model.BackSnapshot);
           this.Snapshots.Add(model.LeftSnapshot);
           this.Snapshots.Add(model.RightSnapshot);
           this.Snapshots.Add(model.TopSnapshot);
           PanelBackground = new ImageBrush()
           {
               ImageSource = new BitmapImage(new Uri(model.BackSnapshot.ImagePath)),
               Stretch = Stretch.Fill
           };

       }
       private ObservableCollection<ImageCapture> snapshots;

       public ObservableCollection<ImageCapture> Snapshots
       {
           get { return snapshots; }
           set { SetProperty(ref snapshots, value); }
       }

       
       

       private ImageBrush panelBackground;

       public ImageBrush PanelBackground
       {
           get { return panelBackground; }
           set { SetProperty(ref panelBackground, value); }
       }

    }
}
