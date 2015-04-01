using Eqstra.BusinessLogic.Portable.SSModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eqstra.ServiceScheduling.UILogic.Portable.Services
{
    public interface IServiceDetailService
    {
         Task<bool> InsertServiceDetailsAsync(ServiceSchedulingDetail serviceSchedulingDetail, Address address, UserInfo userInfo);

         Task<ServiceSchedulingDetail> GetServiceDetailAsync(string caseNumber, long caseServiceRecId, long serviceRecId, UserInfo userInfo);
    }
}
