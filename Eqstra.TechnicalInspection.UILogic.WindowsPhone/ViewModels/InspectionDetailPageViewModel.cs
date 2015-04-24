using Eqstra.BusinessLogic.Helpers;
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage;

namespace Eqstra.TechnicalInspection.UILogic.WindowsPhone.ViewModels
{
    public class InspectionDetailPageViewModel : ViewModel
    {

        private TITask _task;
        private INavigationService _navigationService;
        private ITaskService _taskService;
        public InspectionDetailPageViewModel(INavigationService navigationService, ITaskService taskService)
        {
            this._navigationService = navigationService;
            this._taskService = taskService;
            this.Model = new TIData();
            CompleteCommand = new DelegateCommand(async () =>
            {
                try
                {
                    var imageCaptureList = await Util.ReadFromDiskAsync<List<ImageCapture>>("ImageCaptureList");
                    var resp = await this._taskService.InsertInspectionDataAsync(new List<TIData> { this.Model }, this.SelectedTask, imageCaptureList, UserInfo.CompanyId);
                }
                catch (Exception ex)
                {
                }
                finally
                {
                    _navigationService.Navigate("Main", string.Empty);
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

            }
            catch (Exception)
            {

            }
        }

        public Eqstra.BusinessLogic.Portable.TIModels.TITask SelectedTask { get; set; }
        private TIData model;

        public TIData Model
        {
            get { return model; }
            set { SetProperty(ref model, value); }
        }
        public Eqstra.BusinessLogic.Portable.TIModels.UserInfo UserInfo { get; set; }
        public DelegateCommand CompleteCommand { get; set; }
    }
}