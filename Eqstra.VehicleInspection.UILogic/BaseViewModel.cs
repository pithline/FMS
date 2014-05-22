using Eqstra.BusinessLogic;
using Eqstra.BusinessLogic.Base;
using Eqstra.BusinessLogic.Helpers;
using Eqstra.VehicleInspection.UILogic.Popups;
using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Capture;
using Windows.Networking.Connectivity;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;

namespace Eqstra.VehicleInspection.UILogic
{
     public class BaseViewModel : ViewModel
    {
        SnapshotsViewer _snapShotsPopup;
        ConnectionProfile _connectionProfile;
        Action _syncExecute;
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

        }
        public BaseViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            this.GoHomeCommand = new DelegateCommand(() =>
            {
                _navigationService.ClearHistory();
                _navigationService.Navigate("Main", null);
            });
                }

        private Object model;
        public Object Model
        {
            get { return model; }
            set { SetProperty(ref model, value); }
        }

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
        public virtual System.Threading.Tasks.Task UpdateModelAsync(string caseNumber)
        {
            return null;
        }
      
         public void Synchronize(Action syncExecute)
        {
            _syncExecute = syncExecute;
            _connectionProfile = NetworkInformation.GetInternetConnectionProfile();
            NetworkInformation.NetworkStatusChanged += NetworkInformation_NetworkStatusChanged;
            if (_connectionProfile!= null && _connectionProfile.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess)
            {                
                _syncExecute.Invoke();               
            }
        }

         void NetworkInformation_NetworkStatusChanged(object sender)
         {
             if (_connectionProfile != null && _connectionProfile.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess)
             {
                 _syncExecute.Invoke();
             }
         }
    }
}
