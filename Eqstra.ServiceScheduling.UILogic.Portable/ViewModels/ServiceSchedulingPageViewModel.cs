using Eqstra.BusinessLogic;
using Eqstra.BusinessLogic.Portable.SSModels;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media.Imaging;

namespace Eqstra.ServiceScheduling.UILogic.Portable
{
    public class ServiceSchedulingPageViewModel : ViewModel
    {
        private ImageViewerPopup _imageViewer;
        private INavigationService _navigationService;
        
        Windows.Media.Capture.MediaCapture captureManager;
        public ServiceSchedulingPageViewModel(INavigationService navigationService)
        {
            this._navigationService = navigationService;
            this.Model = new ServiceSchedulingDetail();
            captureManager = new MediaCapture();

            this.TakePictureCommand = DelegateCommand<ImageCapture>.FromAsyncHandler(async (param) =>
          {
              TakePictureAsync();
          });
            this.OpenImageViewerCommand = new DelegateCommand(
                ()=>
            {

                CoreWindow currentWindow = Window.Current.CoreWindow;
                Popup popup = new Popup();
                popup.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Stretch;
                popup.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Stretch;

                if (_imageViewer == null)
                {
                    _imageViewer = new ImageViewerPopup();

                }
                else
                {
                    _imageViewer = null;
                    this._imageViewer = new ImageViewerPopup();
                }

                popup.Child = _imageViewer;
                this._imageViewer.Tag = popup;

                this._imageViewer.Height = currentWindow.Bounds.Height;
                this._imageViewer.Width = currentWindow.Bounds.Width;

                popup.IsOpen = true;


            });

        }

        async private void TakePictureAsync()
        {
            ImageEncodingProperties imgFormat = ImageEncodingProperties.CreateJpeg();
            StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync(
                "TestPhoto.jpg",
                CreationCollisionOption.GenerateUniqueName);
            await captureManager.CapturePhotoToStorageFileAsync(imgFormat, file);
            this.Model.ODOReadingSnapshot.ImagePath = file.Path;
            BitmapImage bmpImage = new BitmapImage(new Uri(file.Path));

        }
        public async override void OnNavigatedTo(object navigationParameter, Windows.UI.Xaml.Navigation.NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            await captureManager.InitializeAsync();
        }

        private ServiceSchedulingDetail model;
        public ServiceSchedulingDetail Model
        {
            get { return model; }
            set { SetProperty(ref model, value); }
        }

        public DelegateCommand<ImageCapture> TakePictureCommand { get; set; }
        public DelegateCommand OpenImageViewerCommand { get; set; }


    }
}