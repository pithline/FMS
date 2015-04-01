using Eqstra.BusinessLogic.Portable.SSModels;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Eqstra.ServiceScheduling.UILogic.Portable.Services
{
    public interface ITaskService
    {
         Task<ObservableCollection<Eqstra.BusinessLogic.Portable.SSModels.Task>> GetTasksAsync(UserInfo userInfo);

         Task<CaseStatus> UpdateStatusListAsync(Eqstra.BusinessLogic.Portable.SSModels.Task task, UserInfo userInfo);
    }
}
