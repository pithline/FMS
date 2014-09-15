using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eqstra.BusinessLogic.Enums
{
    public class CDTaskStatus
    {

        public static string AwaitingConfirmation = "Awaiting Confirmation";
        public static string AwaitingDriverCollection = "Await Driver Collection";
        public static string AwaitingCustomerCollection = "Awaiting Customer Collection";
        public static string AwaitingCourierCollection = "Awaiting Courier Collection";
        public static string AwaitingDelivery = "Awaiting Delivery";
        public static string Complete = "Complete";

    }
}
