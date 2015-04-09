using Eqstra.BusinessLogic.Portable.SSModels;
using Eqstra.ServiceScheduling.UILogic.Portable.Services;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Eqstra.ServiceScheduling.UILogic.Portable
{
    public class SubmittedDetailPageViewModel : ViewModel
    {
        private INavigationService _navigationService;
        private IServiceDetailService _serviceDetailService;
        private ITaskService _taskService;
        public SubmittedDetailPageViewModel(INavigationService navigationService, IServiceDetailService serviceDetailService, ITaskService taskService)
        {
            this._navigationService = navigationService;
            this._taskService = taskService;
            this._serviceDetailService = serviceDetailService;
            this.Model = new ServiceSchedulingDetail();
            this.NextPageCommand = DelegateCommand.FromAsyncHandler(
        async () =>
        {
            try
            {
                var respone = await this._taskService.UpdateStatusListAsync(this.SelectedTask, this.UserInfo);

                navigationService.Navigate("Main", string.Empty);

            }
            catch (Exception ex)
            {
            }
            finally
            {
            }
        },

         () => { return this.Model != null; });

        }


        public async override void OnNavigatedTo(object navigationParameter, Windows.UI.Xaml.Navigation.NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            if (ApplicationData.Current.RoamingSettings.Values.ContainsKey(Constants.SelectedTask))
            {
                this.SelectedTask = JsonConvert.DeserializeObject<Eqstra.BusinessLogic.Portable.SSModels.Task>(ApplicationData.Current.RoamingSettings.Values[Constants.SelectedTask].ToString());
            }
            if (ApplicationData.Current.RoamingSettings.Values.ContainsKey(Constants.UserInfo))
            {
                this.UserInfo = JsonConvert.DeserializeObject<UserInfo>(ApplicationData.Current.RoamingSettings.Values[Constants.UserInfo].ToString());
            }
            this.Model = await _serviceDetailService.GetServiceDetailAsync(this.SelectedTask.CaseNumber, this.SelectedTask.CaseServiceRecID, this.SelectedTask.ServiceRecID, this.UserInfo);
        }
        private ServiceSchedulingDetail model;
        public ServiceSchedulingDetail Model
        {
            get { return model; }
            set { SetProperty(ref model, value); }
        }
        public Eqstra.BusinessLogic.Portable.SSModels.Task SelectedTask { get; set; }
        public UserInfo UserInfo { get; set; }
        public DelegateCommand HomePageCommand { get; private set; }
        public DelegateCommand NextPageCommand { get; private set; }

    }
}
