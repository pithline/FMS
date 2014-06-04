using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Eqstra.VehicleInspection.UILogic.AifServices
{
   public class VIServiceHelper
    {
       private static readonly VIServiceHelper instance = new VIServiceHelper();

       private  VIServiceHelper()
       {

       }

       public static VIServiceHelper Instance
       {
           get { return instance; }
       }

       public VIService.MzkVehicleInspectionServiceClient ConnectAsync(string userName="rchivukula",string password = "Password3",string domain = "lfmd")
       {
           VIService.MzkVehicleInspectionServiceClient client = new VIService.MzkVehicleInspectionServiceClient();
           client.ClientCredentials.Windows.ClientCredential = new NetworkCredential("rchivukula", "Password3", "lfmd");
           return client;
       }
    }
}
