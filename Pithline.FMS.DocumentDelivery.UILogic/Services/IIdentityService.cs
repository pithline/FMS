﻿using Eqstra.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eqstra.DocumentDelivery.UILogic.Services
{
   public interface IIdentityService
    {
       Task<Tuple<CDLogonResult, string>> LogonAsync(string userId, string password);

       Task<bool> VerifyAcitveSessionAsync(string userId);

    }
}
