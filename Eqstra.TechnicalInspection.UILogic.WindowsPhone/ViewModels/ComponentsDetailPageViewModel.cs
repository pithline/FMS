using Eqstra.BusinessLogic.Portable.TIModels;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using Microsoft.Practices.Prism.PubSubEvents;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Windows.ApplicationModel.Activation;
using Windows.Storage.Pickers;
using System.Linq;
using Windows.Storage;
using Eqstra.BusinessLogic.Portable;

namespace Eqstra.TechnicalInspection.UILogic.WindowsPhone.ViewModels
{
    public class ComponentsDetailPageViewModel : ViewModel
    {
        private SnapshotsViewer _snapShotsPopup;
        public INavigationService _navigationService;
        public IEventAggregator _eventAggregator;
        public ComponentsDetailPageViewModel(INavigationService navigationService, IEventAggregator eventAggregator)
        {
            this._navigationService = navigationService;
            this._eventAggregator = eventAggregator;
            PersistentData.Instance.MaintenanceRepairKVPair = new Dictionary<long, MaintenanceRepair>();
            TakeSnapshotCommand = DelegateCommand.FromAsyncHandler(async () =>
            {
                FileOpenPicker openPicker = new FileOpenPicker();
                openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
                openPicker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
                openPicker.FileTypeFilter.Add(".bmp");
                openPicker.FileTypeFilter.Add(".png");
                openPicker.FileTypeFilter.Add(".jpeg");
                openPicker.FileTypeFilter.Add(".jpg");
             
                PersistentData.Instance.SelectedMaintenanceRepair = this.SelectedMaintenanceRepair;

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

        public override void OnNavigatedFrom(Dictionary<string, object> viewModelState, bool suspending)
        {
            if (this._snapShotsPopup != null)
            {
                this._snapShotsPopup.Hide();
            }
            PersistentData.Instance.SelectedMaintenanceRepair = this.SelectedMaintenanceRepair;
            if (!PersistentData.Instance.MaintenanceRepairKVPair.ContainsKey(this.SelectedMaintenanceRepair.Repairid))
            {
                PersistentData.Instance.MaintenanceRepairKVPair.Add(this.SelectedMaintenanceRepair.Repairid, this.SelectedMaintenanceRepair);
            }
            else
            {
                PersistentData.Instance.MaintenanceRepairKVPair[this.SelectedMaintenanceRepair.Repairid] = this.SelectedMaintenanceRepair;
            }
            base.OnNavigatedFrom(viewModelState, suspending);
        }
        public async override void OnNavigatedTo(object navigationParameter, Windows.UI.Xaml.Navigation.NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            try
            {
                base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
                this.SelectedMaintenanceRepair = JsonConvert.DeserializeObject<MaintenanceRepair>(navigationParameter.ToString());
                if (PersistentData.Instance.MaintenanceRepairKVPair != null && PersistentData.Instance.MaintenanceRepairKVPair.Any())
                {
                    this.SelectedMaintenanceRepair = PersistentData.Instance.MaintenanceRepairKVPair.Values.FirstOrDefault(f => f.Repairid == this.SelectedMaintenanceRepair.Repairid);
                }
                if (ApplicationData.Current.RoamingSettings.Values.ContainsKey(Constants.SELECTEDTASK))
                {
                    this.SelectedTask = JsonConvert.DeserializeObject<Eqstra.BusinessLogic.Portable.TIModels.TITask>(ApplicationData.Current.RoamingSettings.Values[Constants.SELECTEDTASK].ToString());
                }
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
        public Eqstra.BusinessLogic.Portable.TIModels.TITask SelectedTask { get; set; }
        public ICommand TakeSnapshotCommand { get; set; }

        public ICommand PreviousCommand { get; set; }

        public ICommand OpenSnapshotViewerCommand { get; set; }
    }
}
