﻿using Eqstra.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eqstra.VehicleInspection.UILogic.Services
{
  public  interface IAccountService
    {
         UserInfo SignedInUser { get; }
         void SignInAsync();

    }
}