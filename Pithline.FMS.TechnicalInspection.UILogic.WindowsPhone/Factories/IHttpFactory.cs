using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace Eqstra.TechnicalInspection.UILogic.WindowsPhone.Factories
{
    public interface IHttpFactory
    {
        Task<HttpResponseMessage> PostAsync(HttpStringContent data);
    }
}
