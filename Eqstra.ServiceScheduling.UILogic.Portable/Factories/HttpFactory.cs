using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eqstra.ServiceScheduling.UILogic.Portable.Factories
{
   public class HttpFactory : IHttpFactory
    {
        public Task<Windows.Web.Http.HttpResponseMessage> PostAsync(Windows.Web.Http.HttpStringContent data)
        {
          
        }
    }
}
