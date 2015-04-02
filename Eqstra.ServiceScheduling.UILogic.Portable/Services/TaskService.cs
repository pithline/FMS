﻿using Eqstra.BusinessLogic.Portable.SSModels;
using Eqstra.ServiceScheduling.UILogic.Portable.Factories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace Eqstra.ServiceScheduling.UILogic.Portable.Services
{

    public class TaskService : ITaskService
    {
        IHttpFactory _httpFactory;
        public TaskService(IHttpFactory httpFactory)
        {
            _httpFactory = httpFactory;
        }

        async public Task<System.Collections.ObjectModel.ObservableCollection<BusinessLogic.Portable.SSModels.Task>> GetTasksAsync(UserInfo userInfo)
        {
            var postData = new { target = "ServiceScheduling", parameters = new[] { "GetTasks", Newtonsoft.Json.JsonConvert.SerializeObject(userInfo) } };
            var response = await _httpFactory.PostAsync(new HttpStringContent(JsonConvert.SerializeObject(postData), Windows.Storage.Streams.UnicodeEncoding.Utf8, "application/json"));
            response.EnsureSuccessStatusCode();
            if (response.IsSuccessStatusCode)
            {
                var tasks = await response.Content.ReadAsStringAsync();
            }

            return JsonConvert.DeserializeObject<ObservableCollection<Eqstra.BusinessLogic.Portable.SSModels.Task>>(await response.Content.ReadAsStringAsync());

        }



        async public Task<BusinessLogic.Portable.SSModels.CaseStatus> UpdateStatusListAsync(BusinessLogic.Portable.SSModels.Task task, UserInfo userInfo)
        {
            var postData = new { target = "ServiceScheduling", parameters = new[] { "UpdateStatusList", JsonConvert.SerializeObject(task), Newtonsoft.Json.JsonConvert.SerializeObject(userInfo) } };
            var response = await _httpFactory.PostAsync(new HttpStringContent(JsonConvert.SerializeObject(postData), Windows.Storage.Streams.UnicodeEncoding.Utf8, "application/json"));
            response.EnsureSuccessStatusCode();
            if (response.IsSuccessStatusCode)
            {
                var tasks = await response.Content.ReadAsStringAsync();
            }
            return JsonConvert.DeserializeObject<CaseStatus>(await response.Content.ReadAsStringAsync());
        }
    }

}
