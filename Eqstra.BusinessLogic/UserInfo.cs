using Microsoft.Practices.Prism.StoreApps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Eqstra.BusinessLogic
{
    public class UserInfo : ValidatableBindableBase
    {
        private string userId;
        [RestorableState]
        public string UserId
        {
            get { return userId; }
            set { SetProperty(ref userId, value); }
        }

        private string name;
        [RestorableState]
        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }

        private string emailId;
        [RestorableState]
        public string EmailId
        {
            get { return emailId; }
            set { SetProperty(ref emailId, value); }
        }

    }
}
