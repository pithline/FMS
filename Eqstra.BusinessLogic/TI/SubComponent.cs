using Microsoft.Practices.Prism.StoreApps;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eqstra.BusinessLogic.TI
{
    public class MaintenanceRepair : ValidatableBindableBase
    {

        private long recId;
        [PrimaryKey]
        public long RecId
        {
            get { return recId; }
            set { SetProperty(ref recId, value); }
        }

        private long caseServiceRecId;

        public long CaseServiceRecId
        {
            get { return caseServiceRecId; }
            set { SetProperty(ref caseServiceRecId, value); }
        }


        private string majorComponent;

        public string MajorComponent
        {
            get { return majorComponent; }
            set { SetProperty(ref majorComponent, value); }
        }


        private string subComponent;

        public string SubComponent
        {
            get { return subComponent; }
            set { SetProperty(ref subComponent, value); }
        }


        private string cause;

        public string Cause
        {
            get { return cause; }
            set { SetProperty(ref cause, value); }
        }

        private string action;

        public string Action
        {
            get { return action; }
            set { SetProperty(ref action, value); }
        }



    }
}
