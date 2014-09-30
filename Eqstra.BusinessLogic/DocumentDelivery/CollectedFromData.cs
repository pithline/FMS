using Microsoft.Practices.Prism.StoreApps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eqstra.BusinessLogic.DocumentDelivery
{
    public class CollectedFromData : ValidatableBindableBase
    {
        private string userID;
        public string UserID
        {
            get { return userID; }
            set { SetProperty(ref userID, value); }
        }

        private string userName;
        public string UserName
        {
            get { return userName; }
            set { SetProperty(ref userName, value); }
        }

    }
}
