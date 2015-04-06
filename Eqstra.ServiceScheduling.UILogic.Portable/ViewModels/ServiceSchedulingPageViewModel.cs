using Eqstra.BusinessLogic;
using Eqstra.BusinessLogic.Portable.SSModels;
using Eqstra.ServiceScheduling.UILogic.Portable.Services;
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
        private IServiceDetailService _serviceDetailService;
        Windows.Media.Capture.MediaCapture captureManager;
        public ServiceSchedulingPageViewModel(INavigationService navigationService, IServiceDetailService serviceDetailService)
        {
            this._navigationService = navigationService;
            this._serviceDetailService = serviceDetailService;
            this.Model = new ServiceSchedulingDetail();
            this.Address = new BusinessLogic.Portable.SSModels.Address();
            captureManager = new MediaCapture();
            this.IsLiftRequired = false;

            this.NextPageCommand = DelegateCommand.FromAsyncHandler(
           async () =>
           {
               try
               {
                  // bool response =await _serviceDetailService.InsertServiceDetailsAsync(this.Model, this.Address, new UserInfo { UserId = "axbcsvc", CompanyId = "1095" });
                  // if (response)
                   //{
                       navigationService.Navigate("PreferredSupplier", string.Empty); 
                   //}
               }
               catch (Exception ex)
               {
               }
               finally
               {
               }
           },

            () => { return this.Model != null; });


            this.TakePictureCommand = DelegateCommand<ImageCapture>.FromAsyncHandler(async (param) =>
          {
              _navigationService.Navigate("CameraCapture",null);

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

                _imageViewer.DataContext = this.Model.ODOReadingSnapshot;
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
            var task = ((Eqstra.BusinessLogic.Portable.SSModels.Task)navigationParameter);
            this.Model = await _serviceDetailService.GetServiceDetailAsync(task.CaseNumber, task.CaseServiceRecID, task.ServiceRecID, new UserInfo { UserId = "axbcsvc", CompanyId = "1095" });
        }

        private ServiceSchedulingDetail model;
        public ServiceSchedulingDetail Model
        {
            get { return model; }
            set { SetProperty(ref model, value); }
        }

        private Address address;
        public Address Address
        {
            get { return address; }
            set { SetProperty(ref address, value); }
        }
        public DelegateCommand NextPageCommand { get; private set; }
        public DelegateCommand<ImageCapture> TakePictureCommand { get; set; }
        public DelegateCommand OpenImageViewerCommand { get; set; }

        private Boolean isLiftRequired;
        public Boolean IsLiftRequired
        {
            get { return isLiftRequired; }
            set
            {

                SetProperty(ref isLiftRequired, value);
                if (value)
                {
                    this.IsReqVisibility = Visibility.Visible;
                }
                else
                {
                    this.IsReqVisibility = Visibility.Collapsed;
                }

            }
        }

        private Visibility isReqVisibility;
        public Visibility IsReqVisibility
        {
            get { return isReqVisibility; }
            set { SetProperty(ref isReqVisibility, value); }
        }
    }
}