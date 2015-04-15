using Eqstra.BusinessLogic.Portable.TIModels;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using Microsoft.Practices.Prism.PubSubEvents;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace Eqstra.TechnicalInspection.UILogic.WindowsPhone.ViewModels
{
    public class ComponentsDetailPageViewModel : ViewModel
    {
        public INavigationService _navigationService;
        public IEventAggregator _eventAggregator;
        public ComponentsDetailPageViewModel(INavigationService navigationService, IEventAggregator eventAggregator)
        {
            this._navigationService = navigationService;
            this._eventAggregator = eventAggregator;
            TakeSnapshotCommand = DelegateCommand.FromAsyncHandler(async () =>
            {
                FileOpenPicker openPicker = new FileOpenPicker();
                openPicker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
                openPicker.FileTypeFilter.Add(".bmp");
                openPicker.FileTypeFilter.Add(".png");
                openPicker.FileTypeFilter.Add(".jpeg");
                openPicker.FileTypeFilter.Add(".jpg");
                PersistentData.Instance.SelectedMaintenanceRepair = this.SelectedMaintenanceRepair;
                //openPicker.ContinuationData["SelectedMaintenanceRepair"] = this.SelectedMaintenanceRepair;

                openPicker.PickSingleFileAndContinue();

                // var camCap = new CameraCaptureDialog();
                //camCap.Tag = this.SelectedMaintenanceRepair;
                //await camCap.ShowAsync();

                this._eventAggregator.GetEvent<MaintenanceRepairEvent>().Subscribe(repair =>
                {
                    this.SelectedMaintenanceRepair = repair;
                });


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
