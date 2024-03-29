﻿using Eqstra.BusinessLogic.Portable.TIModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;


namespace Eqstra.TechnicalInspection.UILogic.WindowsPhone.Services
{
    public interface ITaskService
    {
         Task<ObservableCollection<TITask>> GetTasksAsync(string userId , string companyId);

         Task<bool> InsertInspectionDataAsync(List<TIData> tiData, Eqstra.BusinessLogic.Portable.TIModels.Task task, List<ImageCapture> imageCaptureList, string companyId);
    }
}
