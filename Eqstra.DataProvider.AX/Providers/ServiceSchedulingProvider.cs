using Eqstra.Framework.Web.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eqstra.DataProvider.AX.Providers
{
    [DataProvider(Name="ServiceScheduling")]
    
    class ServiceSchedulingProvider : IDataProvider
    {
        public System.Collections.IList GetDataList(object[] criterias)
        {
            throw new NotImplementedException();            
        }

        public object GetSingleData(object[] criterias)
        {
            throw new NotImplementedException();
        }

        public bool DeleteData(object[] criterias)
        {
            throw new NotImplementedException();
        }

        public object SaveData(object[] criterias)
        {
            throw new NotImplementedException();
        }

        public object GetService()
        {
            throw new NotImplementedException();
        }
    }
}
