using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eqstra.Framework.Web.Exceptions
{
    public interface IExceptionHandler
    {
        void Handle(Exception ex);
    }
}
