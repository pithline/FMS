using Eqstra.BusinessLogic;
using Eqstra.VehicleInspection.Views;
using Microsoft.Practices.Prism.StoreApps;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Eqstra.VehicleInspection.ViewModels
{
    public class PsgrVhcInspecPageViewModel : ViewModel
    {
        public PsgrVhcInspecPageViewModel()
        {
            this.InspectionUserControls = new ObservableCollection<UserControl>();
        }
        public override void OnNavigatedTo(object navigationParameter, Windows.UI.Xaml.Navigation.NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
            this.inspectionHistList = new ObservableCollection<InspectionHistory>{
                new InspectionHistory{InspectionResult=new List<string>{"Engine and brake oil replacement","Wheel alignment"},CustomerId="1",InspectedBy="Jon Tabor",InspectedOn = DateTime.Now},
                new InspectionHistory{InspectionResult=new List<string>{"Vehicle coolant replacement","Few dent repairs"},CustomerId="1",InspectedBy="Jon Tabor",InspectedOn = DateTime.Now},
                new InspectionHistory{InspectionResult=new List<string>{"Vehicle is in perfect condition"},CustomerId="1",InspectedBy="Jon Tabor",InspectedOn = DateTime.Now},
            };

            this.InspectionUserControls.Add(new VehicleDetailsUserControl());
            this.InspectionUserControls.Add(new TrimIntUserControl());
            this.InspectionUserControls.Add(new BodyworkUserControl());
            this.InspectionUserControls.Add(new GlassUserControl());
            this.InspectionUserControls.Add(new AccessoriesUserControl());
        }

        private ObservableCollection<UserControl> inpectionUserControls;

        public ObservableCollection<UserControl> InspectionUserControls
        {
            get { return inpectionUserControls; }
            set { SetProperty(ref inpectionUserControls, value); }
        }

        private ObservableCollection<InspectionHistory> inspectionHistList;

        public ObservableCollection<InspectionHistory> InspectionHistList
        {
            get { return inspectionHistList; }
            set { SetProperty(ref inspectionHistList, value); }
        }

    }
}
