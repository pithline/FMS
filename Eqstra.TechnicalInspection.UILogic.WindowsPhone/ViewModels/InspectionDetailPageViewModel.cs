using Eqstra.BusinessLogic.Portable;
using Eqstra.BusinessLogic.Portable.TIModels;
using Eqstra.TechnicalInspection.UILogic.WindowsPhone.Services;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using Windows.Media.SpeechRecognition;
using Windows.Storage;
using Windows.UI.Popups;

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
                    var imageCaptureList = await Util.ReadFromDiskAsync<List<Eqstra.BusinessLogic.Portable.TIModels.ImageCapture>>("ImageCaptureList");
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
            this.VoiceCommand = new DelegateCommand<string>(async(param) =>
            {
                SpeechRecognizer recognizer = new SpeechRecognizer();

                SpeechRecognitionTopicConstraint topicConstraint
                        = new SpeechRecognitionTopicConstraint(SpeechRecognitionScenario.Dictation, "Development");

                recognizer.Constraints.Add(topicConstraint);
                await recognizer.CompileConstraintsAsync();

                var results = await recognizer.RecognizeWithUIAsync();
                if (results != null & (results.Confidence != SpeechRecognitionConfidence.Rejected))
                {
                    if (param == "Remedy")
                    {
                        this.Model.Remedy = results.Text; 
                    }
                    if (param == "Recommendation")
                    {
                        this.Model.Recommendation = results.Text;
                    }
                    if (param == "CauseOfDamage")
                    {
                        this.Model.CauseOfDamage = results.Text;
                    }
                }
                else
                {
                    await new MessageDialog("Sorry, I did not get that.").ShowAsync();
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

        public DelegateCommand<string> VoiceCommand { get; set; }
    }
}