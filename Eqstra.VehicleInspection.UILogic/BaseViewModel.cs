﻿using Eqstra.BusinessLogic;
using Eqstra.BusinessLogic.Base;
using Eqstra.BusinessLogic.Helpers;
using Eqstra.VehicleInspection.UILogic.AifServices;
using Eqstra.VehicleInspection.UILogic.Popups;
using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Capture;
using Windows.Networking.Connectivity;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Notifications;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Navigation;

namespace Eqstra.VehicleInspection.UILogic
{
    [DataContract]
    public class BaseViewModel : ViewModel
    {
        SnapshotsViewer _snapShotsPopup;

        INavigationService _navigationService;

        public BaseViewModel()
        {
            TakeSnapshotCommand = DelegateCommand<ObservableCollection<ImageCapture>>.FromAsyncHandler(async (param) =>
            {
                await TakeSnapshotAsync(param);
            });

            TakePictureCommand = DelegateCommand<ImageCapture>.FromAsyncHandler(async (param) =>
                {
                    await TakePictureAsync(param);
                });

            this.OpenSnapshotViewerCommand = new DelegateCommand<dynamic>((param) =>
            {
                OpenPopup(param);
            });

            this.SaveModelCommand = new DelegateCommand<object>(async (param) =>
                {
                    BaseModel baseModel = param as BaseModel;
                    long vehicleInsRecId = long.Parse(ApplicationData.Current.LocalSettings.Values["VehicleInsRecId"].ToString());
                    int successFlag = 0;
                    try
                    {
                        var viobj = await baseModel.GetDataAsync(vehicleInsRecId);
                        if (viobj != null)
                        {
                            successFlag = await SqliteHelper.Storage.UpdateSingleRecordAsync(baseModel);
                        }
                        else
                        {
                            baseModel.VehicleInsRecID = vehicleInsRecId;
                            successFlag = await SqliteHelper.Storage.InsertSingleRecordAsync(baseModel);
                        }

                        if (successFlag != 0)
                        {
                            baseModel.ShouldSave = false;
                            await VIServiceHelper.Instance.SyncFromSvcAsync(baseModel);
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }

                });
        }
        public BaseViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            this.GoHomeCommand = new DelegateCommand(() =>
            {
                _navigationService.ClearHistory();
                _navigationService.Navigate("Main", string.Empty);
            });
        }

        private Object model;
        [RestorableState]
        public Object Model
        {
            get { return model; }
            set { SetProperty(ref model, value); }
        }



        public DelegateCommand<object> SaveModelCommand { get; set; }

        public DelegateCommand GoHomeCommand { get; set; }

        public DelegateCommand<ObservableCollection<ImageCapture>> TakeSnapshotCommand { get; set; }

        public DelegateCommand<ImageCapture> TakePictureCommand { get; set; }

        public DelegateCommand<object> OpenSnapshotViewerCommand { get; set; }

        protected async System.Threading.Tasks.Task TakeSnapshotAsync<T>(T list) where T : ObservableCollection<ImageCapture>
        {
            try
            {
                CameraCaptureUI ccui = new CameraCaptureUI();
                var file = await ccui.CaptureFileAsync(CameraCaptureUIMode.Photo);
                if (file != null)
                {
                    list.Add(new ImageCapture { ImagePath = file.Path });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        async public virtual System.Threading.Tasks.Task TakePictureAsync(ImageCapture param)
        {
            try
            {
                CameraCaptureUI cam = new CameraCaptureUI();

                var file = await cam.CaptureFileAsync(CameraCaptureUIMode.Photo);
                if (file != null)
                {
                    param.ImagePath = file.Path;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void OpenPopup(dynamic dc)
        {
            CoreWindow currentWindow = Window.Current.CoreWindow;
            Popup popup = new Popup();
            popup.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Stretch;
            popup.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Stretch;

            if (_snapShotsPopup == null)
            {
                _snapShotsPopup = new SnapshotsViewer();

            }
            else
            {
                _snapShotsPopup = null;
                this._snapShotsPopup = new SnapshotsViewer();
            }
            _snapShotsPopup.DataContext = dc;


            popup.Child = _snapShotsPopup;
            this._snapShotsPopup.Tag = popup;

            this._snapShotsPopup.Height = currentWindow.Bounds.Height;
            this._snapShotsPopup.Width = currentWindow.Bounds.Width;

            popup.IsOpen = true;

        }
        public virtual System.Threading.Tasks.Task LoadModelFromDbAsync(long vehicleInsRecID)
        {
            return null;
        }



        /// <summary>
        /// /  This metod is only for testing suspension ,later we can remove it
        /// </summary>
        /// <param name="viewModelState"></param>
        /// <param name="suspending"></param>
        public override void OnNavigatedFrom(Dictionary<string, object> viewModelState, bool suspending)
        {
            base.OnNavigatedFrom(viewModelState, suspending);

        }
        /// <summary>
        /// This metod is only for testing suspension, later we can remove it
        /// </summary>
        /// <param name="navigationParameter"></param>
        /// <param name="navigationMode"></param>
        /// <param name="viewModelState"></param>
        public override void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
        }
    }
}
