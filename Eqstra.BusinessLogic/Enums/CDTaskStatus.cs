using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eqstra.BusinessLogic.Enums
{
  public  enum CDTaskStatusEnum
    {
      AwaitingConfirmation,
      AwaitingDriverCollection,
      AwaitingCustomerCollection,
      AwaitingCourierCollection,
      AwaitingDelivery,
      Complete,
      
    }
}
