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
