using Eqstra.BusinessLogic;
using Eqstra.BusinessLogic.Commercial;
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
       private IVehicleDetails _model;
       public ImageMarkupPageViewModel()
       {
           this.Snapshots = new ObservableCollection<ImageCapture>();
       }
       public override void OnNavigatedTo(object navigationParameter, Windows.UI.Xaml.Navigation.NavigationMode navigationMode, Dictionary<string, object> viewModelState)
       {
           base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);

           if (navigationParameter is PVehicleDetails)
           {
               _model = (PVehicleDetails)navigationParameter;
               this.Snapshots.Add(_model.FrontSnapshot);
           this.Snapshots.Add(_model.BackSnapshot);
           this.Snapshots.Add(_model.LeftSnapshot);
           this.Snapshots.Add(_model.RightSnapshot);
           this.Snapshots.Add(_model.TopSnapshot);
           }
           else
           {
               _model = (CVehicleDetails)navigationParameter;
               this.Snapshots.Add(_model.FrontSnapshot);
               this.Snapshots.Add(_model.RightSnapshot);
               this.Snapshots.Add(_model.LeftSnapshot);
           }
           
           PanelBackground = new ImageBrush()
           {
               ImageSource = new BitmapImage(new Uri(_model.BackSnapshot.ImagePath)),
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
