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
        private TITask _task;
        private INavigationService _navigationService;
        private IEventAggregator _eventAggregator;
        private ITaskService _taskService;
        public TechnicalInspectionPageViewModel(INavigationService navigationService, IEventAggregator eventAggregator, ITaskService taskService)
        {
            _navigationService = navigationService;
            this._eventAggregator = eventAggregator;
            this._taskService = taskService;
            this.MaintenanceRepairList = new ObservableCollection<MaintenanceRepair>();
          
            this.Model = new TIData();

            this.CompleteCommand = new DelegateCommand(async () =>
            {
                try
                {
                    this.SelectedTask.Status = Eqstra.BusinessLogic.Portable.SSModels.DriverTaskStatus.Completed;

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
                    var resp = await this._taskService.InsertInspectionDataAsync(new List<TIData> { this.Model }, this.SelectedTask, imageCaptureList, UserInfo.CompanyId);
                    if (resp)
                    {
                        _navigationService.Navigate("Main", string.Empty);
                    }
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

                //foreach (var item in this.SelectedTask.ComponentList)
                //{
                //    this.MaintenanceRepairAdpList.Add(new MaintenanceRepairAdapter()
                //    {
                //        Action = item.Action,
                //        MajorComponent = item.MajorComponent,
                //        SubComponent = item.SubComponent,
                //        Repairid = item.Repairid,
                //        CaseServiceRecId = item.CaseServiceRecId,
                //        Cause = item.Cause
                //    });
                //}

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

        //private ObservableCollection<MaintenanceRepairAdapter> maintenanceRepairAdpList;
        //public ObservableCollection<MaintenanceRepairAdapter> MaintenanceRepairAdpList
        //{
        //    get { return maintenanceRepairAdpList; }
        //    set { SetProperty(ref maintenanceRepairAdpList, value); }
        //}

        private TIData model;

        public TIData Model
        {
            get { return model; }
            set { SetProperty(ref model, value); }
        }
        public Eqstra.BusinessLogic.Portable.TIModels.TITask SelectedTask { get; set; }
        public Eqstra.BusinessLogic.Portable.TIModels.UserInfo UserInfo { get; set; }
        public DelegateCommand CompleteCommand { get; set; }
    }
}
