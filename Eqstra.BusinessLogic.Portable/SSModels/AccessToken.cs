﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eqstra.BusinessLogic.Portable.SSModels
{
    public class AccessToken
    {
        public string Access_Token { get; set; }

        public int Expires_In { get; set; }

        public string Token_Type { get; set; }

        public string UserName { get; set; }
    }
}
