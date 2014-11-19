using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.PubSubEvents;
using System.Collections.ObjectModel;
using Eqstra.BusinessLogic.Base;

namespace Eqstra.TechnicalInspection.UILogic
{
    public class ErrorsRaisedEvent : PubSubEvent<ObservableCollection<ValidationError>>
    {
    }
}
