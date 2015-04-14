using Eqstra.BusinessLogic.Portable.TIModels;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Eqstra.TechnicalInspection.UILogic.WindowsPhone.ViewModels
{
    public class ComponentsDetailPageViewModel : ViewModel
    {
        public INavigationService _navigationService;
        public ComponentsDetailPageViewModel(INavigationService navigationService)
        {
            this._navigationService = navigationService;
            TakeSnapshotCommand = DelegateCommand.FromAsyncHandler(async() =>
            {
                var camCap = new CameraCaptureDialog();
                camCap.Tag = this.SelectedMaintenanceRepair;
                await camCap.ShowAsync();
            });

            PreviousCommand = new DelegateCommand(() =>
            {

                navigationService.Navigate("TechnicalInspection", string.Empty);
            });

        }


        public async override void OnNavigatedTo(object navigationParameter, Windows.UI.Xaml.Navigation.NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            try
            {
                base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
                this.SelectedMaintenanceRepair = JsonConvert.DeserializeObject<MaintenanceRepair>(navigationParameter.ToString());
            }
            catch (Exception ex)
            {

            }
        }

        private MaintenanceRepair selectedMaintenanceRepair;
        public MaintenanceRepair SelectedMaintenanceRepair
        {
            get { return selectedMaintenanceRepair; }
            set { SetProperty(ref selectedMaintenanceRepair, value); }
        }
        public ICommand TakeSnapshotCommand { get; set; }

        public ICommand PreviousCommand { get; set; }
    }
}
