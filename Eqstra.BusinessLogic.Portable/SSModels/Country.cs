﻿using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eqstra.BusinessLogic.Portable.SSModels
{
    public class Country : BindableBase
    {
        private string name;
        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }
        private string id;
        public string Id
        {
            get { return id; }
            set { SetProperty(ref id, value); }
        }

    }
}
