using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Eqstra.BusinessLogic.Portable.SSModels;

namespace Eqstra.ServiceScheduling.UILogic.Portable
{
    public class MainPageViewModel : ViewModel
    {
        private INavigationService _navigationService;
        public MainPageViewModel(INavigationService navigationService)
        {
            this._navigationService = navigationService;

            this.AppBarVisibility = Visibility.Collapsed;

          



            this.NextPageCommand = DelegateCommand.FromAsyncHandler(
             async () =>
             {
                 try
                 {
                     navigationService.Navigate("ServiceScheduling", string.Empty);

                 }
                 catch (Exception ex)
                 {
                 }
                 finally
                 {
                     
                 }
             },

              () => { return this.InspectionTask != null; });
        }



        private Visibility appBarVisibility;
        public Visibility AppBarVisibility
        {
            get { return appBarVisibility; }
            set
            {
                SetProperty(ref appBarVisibility, value);
            }
        }

        private Eqstra.BusinessLogic.Portable.SSModels.Task task;
        public Eqstra.BusinessLogic.Portable.SSModels.Task InspectionTask
        {
            get { return task; }
            set
            {
                SetProperty(ref task, value);
                if (value != null)
                {
                    AppBarVisibility = Visibility.Visible;
                }
                else
                {
                    AppBarVisibility = Visibility.Collapsed;

                }
            }
        }

        private ObservableCollection<Eqstra.BusinessLogic.Portable.SSModels.Task> poolofTasks;
        public ObservableCollection<Eqstra.BusinessLogic.Portable.SSModels.Task> PoolofTasks
        {
            get { return poolofTasks; }
            set
            {
                SetProperty(ref poolofTasks, value);
            }
        }
        public DelegateCommand NextPageCommand { get; private set; }
        public override void OnNavigatedTo(object navigationParameter, Windows.UI.Xaml.Navigation.NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
        }

    }
}
