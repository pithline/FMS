using Eqstra.BusinessLogic.Portable.SSModels;
using Eqstra.ServiceScheduling.UILogic.Portable.Services;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                await this._taskService.UpdateStatusListAsync(this.SelectedTask, new UserInfo { UserId = "axbcsvc", CompanyId = "1095" });
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
            if (navigationParameter is Eqstra.BusinessLogic.Portable.SSModels.Task)
            {
                SelectedTask = ((Eqstra.BusinessLogic.Portable.SSModels.Task)navigationParameter);
                this.Model = await _serviceDetailService.GetServiceDetailAsync(this.SelectedTask.CaseNumber, this.SelectedTask.CaseServiceRecID, this.SelectedTask.ServiceRecID, new UserInfo { UserId = "axbcsvc", CompanyId = "1095" });
            }

        }
        private ServiceSchedulingDetail model;
        public ServiceSchedulingDetail Model
        {
            get { return model; }
            set { SetProperty(ref model, value); }
        }
        public Eqstra.BusinessLogic.Portable.SSModels.Task SelectedTask { get; set; }
        public DelegateCommand HomePageCommand { get; private set; }
        public DelegateCommand NextPageCommand { get; private set; }



    }
}
