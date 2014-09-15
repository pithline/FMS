using Eqstra.BusinessLogic.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eqstra.BusinessLogic
{
   public class TIData : BaseModel
    {
        public override Task<BaseModel> GetDataAsync(long vehicleInsRecID)
        {
            return Task<TIData>.FromResult<BaseModel>( this);
        }
    }
}
