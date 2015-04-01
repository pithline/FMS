using Eqstra.BusinessLogic.Portable.SSModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eqstra.ServiceScheduling.UILogic.Portable.Services
{
    public interface ISupplierService
    {
        async public Task<ObservableCollection<Supplier>> GetSuppliersByClassAsync(string classId, UserInfo userInfo);
    }
}
