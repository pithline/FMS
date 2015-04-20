using Eqstra.BusinessLogic.Portable;
using Eqstra.BusinessLogic.Portable.TIModels;
using Eqstra.TechnicalInspection.UILogic.WindowsPhone.Services;
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
using Windows.Storage;

namespace Eqstra.TechnicalInspection.UILogic.WindowsPhone.ViewModels
{
    public class TechnicalInspectionPageViewModel : ViewModel
    {
        private INavigationService _navigationService;
        private ITaskService _taskService;
        public TechnicalInspectionPageViewModel(INavigationService navigationService, ITaskService taskService)
        {
            _navigationService = navigationService;
            this._taskService = taskService;
            this.MaintenanceRepairList = new ObservableCollection<MaintenanceRepair>();

            this.NextCommand = new DelegateCommand(async () =>
            {
                try
                {

                    List<ImageCapture> imageCaptureList = new List<ImageCapture>();
                    foreach (var item in this.MaintenanceRepairList)
                    {
                        if (item.MajorComponentImgList.Any())
                        {
                            imageCaptureList.AddRange(item.MajorComponentImgList);
                        }
                        if (item.SubComponentImgList.Any())
                        {
                            imageCaptureList.AddRange(item.SubComponentImgList);
                        }

                    }
                    PersistentData.Instance.ImageCaptureList = imageCaptureList;

                    _navigationService.Navigate("InspectionDetail", string.Empty);

                }
                catch (Exception ex)
                {
                }
            });

        }

        public async override void OnNavigatedTo(object navigationParameter, Windows.UI.Xaml.Navigation.NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            try
            {
                base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
                if (ApplicationData.Current.RoamingSettings.Values.ContainsKey(Constants.USERINFO))
                {
                    this.UserInfo = JsonConvert.DeserializeObject<UserInfo>(ApplicationData.Current.RoamingSettings.Values[Constants.USERINFO].ToString());
                }

                if (ApplicationData.Current.RoamingSettings.Values.ContainsKey(Constants.SELECTEDTASK))
                {
                    this.SelectedTask = JsonConvert.DeserializeObject<Eqstra.BusinessLogic.Portable.TIModels.TITask>(ApplicationData.Current.RoamingSettings.Values[Constants.SELECTEDTASK].ToString());
                }

                ObservableCollection<MaintenanceRepair> mRepairList = new ObservableCollection<MaintenanceRepair>();
                foreach (var repair in PersistentData.Instance.MaintenanceRepairKVPair.Values)
                {
                    mRepairList.Add(repair);

                }
                this.MaintenanceRepairList = mRepairList;
            }
            catch (Exception)
            {

            }
        }

        private ObservableCollection<MaintenanceRepair> maintenanceRepairList;
        public ObservableCollection<MaintenanceRepair> MaintenanceRepairList
        {
            get { return maintenanceRepairList; }
            set { SetProperty(ref maintenanceRepairList, value); }
        }

        public Eqstra.BusinessLogic.Portable.TIModels.TITask SelectedTask { get; set; }
        public Eqstra.BusinessLogic.Portable.TIModels.UserInfo UserInfo { get; set; }
        public DelegateCommand NextCommand { get; set; }
    }
}
