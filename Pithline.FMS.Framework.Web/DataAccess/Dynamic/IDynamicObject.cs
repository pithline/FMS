﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eqstra.Framework.Web.DataAccess.Dynamic
{
    public interface IDynamicObject
    {
        object CallMethod(string methodname, object[] paramList);
    }
}
