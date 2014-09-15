﻿
using System.Collections.Generic;

namespace Eqstra.Framework.Web.Security
{
    public interface ISecurityProvider
    {
        List<string> GetMenuPermissions(string user);
        List<object> GetViewPermissions(string user, string form);
    }
}
