using Microsoft.Practices.Prism.PubSubEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eqstra.VehicleInspection.UILogic.ViewModels
{
    class TechnicalInspectionUserControlViewModel : BaseViewModel
    {
        public TechnicalInspectionUserControlViewModel(IEventAggregator eventAggregator) : base(eventAggregator)
        {

        }

        public override Task LoadModelFromDbAsync(long vehicleInsRecID)
        {
            return Task.FromResult<object>(null);
        }
    }
}
