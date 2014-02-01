using Bing.Maps;
using Microsoft.Practices.Prism.StoreApps;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Eqstra.VehicleInspection.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DrivingDirectionPage : VisualStateAwarePage
    {
        readonly Geolocator geolocator = new Geolocator();
        Pushpin pushpin;
        public DrivingDirectionPage()
        {
            this.InitializeComponent();
            pushpin = new Pushpin{Visibility = Windows.UI.Xaml.Visibility.Collapsed};
            this.MyMap.Children.Add(pushpin);
            geolocator.PositionChanged += geolocator_PositionChanged;
        }

        async void geolocator_PositionChanged(Geolocator sender, PositionChangedEventArgs args)
        {
            await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,new DispatchedHandler(()=>
            {
                var location = new Location(args.Position.Coordinate.Point.Position.Latitude,args.Position.Coordinate.Point.Position.Longitude);
                
                MapLayer.SetPosition(pushpin,location);
                pushpin.Visibility = Windows.UI.Xaml.Visibility.Visible;
                MyMap.SetView(location,16);
            }));
        }
    }
}
