using Eqstra.BusinessLogic;
using Eqstra.BusinessLogic.Base;
using Eqstra.BusinessLogic.Common;
using Eqstra.BusinessLogic.Helpers;
using Eqstra.BusinessLogic.Passenger;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.StoreApps;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Navigation;

namespace Eqstra.VehicleInspection.UILogic.ViewModels
{
    public class TTyreConditionUserControlViewModel : BaseViewModel
    {
        public TTyreConditionUserControlViewModel(IEventAggregator eventAggregator)
            : base(eventAggregator)
        {
            this.PoolOfTyreCondions = new ObservableCollection<TTyreCond>();

            PoolOfTyreCondions.Add(new TTyreCond { Position = "Postion1" });
            PoolOfTyreCondions.Add(new TTyreCond { Position = "Postion2" });
            PoolOfTyreCondions.Add(new TTyreCond { Position = "Postion3" });
            PoolOfTyreCondions.Add(new TTyreCond { Position = "Postion4" });
            PoolOfTyreCondions.Add(new TTyreCond { Position = "Postion5" });
            PoolOfTyreCondions.Add(new TTyreCond { Position = "Postion6" });

            PoolOfTyreCondions.Add(new TTyreCond { Position = "Postion7" });
            PoolOfTyreCondions.Add(new TTyreCond { Position = "Postion8" });
            PoolOfTyreCondions.Add(new TTyreCond { Position = "Postion9" });
            PoolOfTyreCondions.Add(new TTyreCond { Position = "Postion10" });
            PoolOfTyreCondions.Add(new TTyreCond { Position = "Postion11" });
            PoolOfTyreCondions.Add(new TTyreCond { Position = "Postion12" });

            PoolOfTyreCondions.Add(new TTyreCond { Position = "Postion13" });
            PoolOfTyreCondions.Add(new TTyreCond { Position = "Postion14" });
            PoolOfTyreCondions.Add(new TTyreCond { Position = "Postion15" });
            PoolOfTyreCondions.Add(new TTyreCond { Position = "Postion16" });


        }

        public async override System.Threading.Tasks.Task LoadModelFromDbAsync(long vehicleInsRecID)
        {
            this.Model = new TTyreCond();
            //BaseModel viBaseObject = (TTyreCond)this.Model;
            //viBaseObject.LoadSnapshotsFromDb();
            //PropertyHistory.Instance.SetPropertyHistory(viBaseObject);
            //viBaseObject.ShouldSave = false;
        }

        private ObservableCollection<TTyreCond> poolOfTyreCondions;

        public ObservableCollection<TTyreCond> PoolOfTyreCondions
        {
            get { return poolOfTyreCondions; }
            set { SetProperty(ref poolOfTyreCondions, value); }
        }

    }
}
