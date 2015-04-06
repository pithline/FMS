using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;

namespace Eqstra.ServiceScheduling.UILogic.Portable
{

    public class CameraCapturePageViewModel : ViewModel
    {
        private INavigationService _navigationService;


        public CameraCapturePageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            this.MediaCapture = new MediaCapture();
            TakePictureCommand = new DelegateCommand(async () =>
            {
                var imageEncodingProps = ImageEncodingProperties.CreatePng();
                using (var stream = new InMemoryRandomAccessStream())
                {

                    await this.MediaCapture.CapturePhotoToStreamAsync(imageEncodingProps, stream);
                    if (ImageSource == null)
                    {
                        ImageSource = new BitmapImage();
                    }
                    await this.ImageSource.SetSourceAsync(stream);
                    this.ImageVisibility = Visibility.Visible;
                }
            });

            RetakePictureCommand = new DelegateCommand( () =>
            {
                ImageVisibility = Visibility.Collapsed;
            });

            AcceptCommand = new DelegateCommand(() =>
            {
                _navigationService.Navigate("ServiceSchedulingPage", null);
                _navigationService.ClearHistory();
            });
           
        }



        async public override void OnNavigatedTo(object navigationParameter, Windows.UI.Xaml.Navigation.NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            this.ImageVisibility = Visibility.Collapsed;
            base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);

            this.MediaCapture.VideoDeviceController.PrimaryUse = Windows.Media.Devices.CaptureUse.Photo;
            await this.MediaCapture.InitializeAsync();
            
            await this.MediaCapture.StartPreviewAsync();

        }

        public ICommand TakePictureCommand { get; set; }

        public ICommand RetakePictureCommand { get; set; }

        public ICommand AcceptCommand { get; set; }

        private MediaCapture mediaCapture;
        public MediaCapture MediaCapture
        {
            get { return mediaCapture; }
            set { SetProperty(ref mediaCapture, value); }
        }

        private BitmapImage imageSource;
        public BitmapImage ImageSource
        {
            get { return imageSource; }
            set { SetProperty(ref imageSource, value); }
        }

        private Visibility imageVisibility;
        public Visibility ImageVisibility
        {
            get { return imageVisibility; }
            set { SetProperty(ref imageVisibility, value); }
        }
    }
}
