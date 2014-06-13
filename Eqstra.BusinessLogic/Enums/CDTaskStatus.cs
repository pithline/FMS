using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eqstra.BusinessLogic.Enums
{
    public class CDTaskStatus
    {

        public static string AwaitingConfirmation = "AwaitingConfirmation";
        public static string AwaitingDriverCollection = "AwaitingDriverCollection";
        public static string AwaitingCustomerCollection = "AwaitingCustomerCollection";
        public static string AwaitingCourierCollection = "AwaitingCourierCollection";
        public static string AwaitingDelivery = "AwaitingDelivery";
        public static string Complete = "Complete";

    }
}
