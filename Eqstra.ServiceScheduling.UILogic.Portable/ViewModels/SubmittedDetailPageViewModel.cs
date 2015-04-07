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
        public SubmittedDetailPageViewModel(INavigationService navigationService, IServiceDetailService serviceDetailService)
        {
            this._navigationService = navigationService;
            this._serviceDetailService = serviceDetailService;
            this.Model = new ServiceSchedulingDetail();
            this.NextPageCommand = DelegateCommand.FromAsyncHandler(
        async () =>
        {
            try
            {
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
                var task = ((Eqstra.BusinessLogic.Portable.SSModels.Task)navigationParameter);
                this.Model = await _serviceDetailService.GetServiceDetailAsync(task.CaseNumber, task.CaseServiceRecID, task.ServiceRecID, new UserInfo { UserId = "axbcsvc", CompanyId = "1095" });
            }

        }
        private ServiceSchedulingDetail model;
        public ServiceSchedulingDetail Model
        {
            get { return model; }
            set { SetProperty(ref model, value); }
        }
        public DelegateCommand HomePageCommand { get; private set; }
        public DelegateCommand NextPageCommand { get; private set; }

    }
}
