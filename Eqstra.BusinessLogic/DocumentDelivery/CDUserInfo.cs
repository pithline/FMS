﻿using Eqstra.BusinessLogic.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eqstra.BusinessLogic.DeliveryModel
{
    public class CDUserInfo : UserInfo
    {
        private CDUserType cdUserType;

        public CDUserType CDUserType
        {
            get { return cdUserType; }
            set { SetProperty(ref cdUserType, value); }
        }
    }
}
