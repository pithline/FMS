using Eqstra.BusinessLogic;
using Eqstra.VehicleInspection.UILogic.Popups;
using Microsoft.Practices.Prism.StoreApps;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Capture;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;

namespace Eqstra.VehicleInspection.UILogic
{
    public class BaseViewModel : ViewModel
    {
        SnapshotsViewer _snapShotsPopup;
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

            this.OpenSnapshotViewerCommand = DelegateCommand<dynamic>.FromAsyncHandler(async (param) =>
            {
                OpenPopup(param);
            });
        }

        async public System.Threading.Tasks.Task TakePictureAsync(ImageCapture param)
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
        private Object model;

        public Object Model
        {
            get { return model; }
            set { SetProperty(ref model, value); }
        }

        public DelegateCommand<ObservableCollection<ImageCapture>> TakeSnapshotCommand { get; set; }
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

        public DelegateCommand<ImageCapture> TakePictureCommand { get; set; }

        public DelegateCommand<object> OpenSnapshotViewerCommand { get; set; }

      public  void OpenPopup(dynamic dc)
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
    }
}
