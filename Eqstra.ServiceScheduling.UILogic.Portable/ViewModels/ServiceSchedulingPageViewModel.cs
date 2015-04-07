using Eqstra.BusinessLogic;
using Eqstra.BusinessLogic.Portable.SSModels;
using Eqstra.ServiceScheduling.UILogic.Portable.Services;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
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
        private BusyIndicator _busyIndicator;
        public ServiceSchedulingPageViewModel(INavigationService navigationService, IServiceDetailService serviceDetailService)
        {
            this._navigationService = navigationService;
            this._serviceDetailService = serviceDetailService;
            this.Model = new ServiceSchedulingDetail();
            _busyIndicator = new BusyIndicator();
            this.Address = new BusinessLogic.Portable.SSModels.Address();

            this.IsLiftRequired = false;

            this.NextPageCommand = DelegateCommand.FromAsyncHandler(
           async () =>
           {
               try
               {

                   this.Model.ServiceDateOption1 = this.Model.ServiceDateOpt1.ToString("MM/dd/yyyy");
                   this.Model.ServiceDateOption2 = this.Model.ServiceDateOpt2.ToString("MM/dd/yyyy");
                   this.Model.ODOReadingDate = this.Model.ODOReadingDt.ToString("MM/dd/yyyy");
                   bool response = await _serviceDetailService.InsertServiceDetailsAsync(this.Model, this.Address, new UserInfo { UserId = "axbcsvc", CompanyId = "1095" });
                   if (response)
                   {
                       navigationService.Navigate("PreferredSupplier", string.Empty);
                   }
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
              _navigationService.Navigate("CameraCapture", this.Model);

          });
            this.OpenImageViewerCommand = new DelegateCommand(
                () =>
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

                    _imageViewer.DataContext = this.Model.OdoReadingImageCapture;
                    popup.Child = _imageViewer;
                    this._imageViewer.Tag = popup;


                    this._imageViewer.Height = currentWindow.Bounds.Height;
                    this._imageViewer.Width = currentWindow.Bounds.Width;

                    popup.IsOpen = true;


                });

        }


        public async override void OnNavigatedTo(object navigationParameter, Windows.UI.Xaml.Navigation.NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            try
            {
                if (navigationParameter is Eqstra.BusinessLogic.Portable.SSModels.Task)
                {
                    _busyIndicator.Open();

                    var task = ((Eqstra.BusinessLogic.Portable.SSModels.Task)navigationParameter);
                    this.Model = await _serviceDetailService.GetServiceDetailAsync(task.CaseNumber, task.CaseServiceRecID, task.ServiceRecID, new UserInfo { UserId = "axbcsvc", CompanyId = "1095" });
                    if (string.IsNullOrEmpty(this.Model.ODOReadingSnapshot))
                    {
                        this.Model.OdoReadingImageCapture = new ImageCapture() { ImageBitmap = new BitmapImage(new Uri("ms-appx:///Assets/odo_meter.png")) };
                    }
                    else
                    {
                        var bitmap = new BitmapImage();
                        await bitmap.SetSourceAsync(await this.ConvertToRandomAccessStreamAsync(Convert.FromBase64String(this.Model.ODOReadingSnapshot)));
                        this.Model.OdoReadingImageCapture = new ImageCapture() { ImageBitmap = bitmap };
                    }
                    if (task != null)
                    {
                        this.DestinationTypes = await _serviceDetailService.GetDestinationTypeList("Vendor", task.CustomerId, new UserInfo { UserId = "axbcsvc", CompanyId = "1095" });
                    }

                }
                else
                {
                    this.Model = navigationParameter as ServiceSchedulingDetail;
                }
                if (!String.IsNullOrEmpty(this.Model.ServiceDateOption1))
                {
                    this.Model.ServiceDateOpt1 = DateTime.Parse(this.Model.ServiceDateOption1);
                }
                if (!String.IsNullOrEmpty(this.Model.ServiceDateOption2))
                {
                    this.Model.ServiceDateOpt2 = DateTime.Parse(this.Model.ServiceDateOption2);
                }
                if (!String.IsNullOrEmpty(this.Model.ODOReadingDate))
                {
                    this.Model.ODOReadingDt = DateTime.Parse(this.Model.ODOReadingDate);
                }
                _busyIndicator.Close();
            }
            catch (Exception)
            {

            }
        }

        private async Task<IRandomAccessStream> ConvertToRandomAccessStreamAsync(byte[] bytes)
        {
            var randomAccessStream = new InMemoryRandomAccessStream();
            using (var writer = new DataWriter(randomAccessStream))
            {
                writer.WriteBytes(bytes);
                await writer.StoreAsync();
                await writer.FlushAsync();
                writer.DetachStream();
            }
            randomAccessStream.Seek(0);

            return randomAccessStream;
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

        private List<Supplier> suppliers;
        public List<Supplier> Suppliers
        {
            get { return suppliers; }
            set { SetProperty(ref suppliers, value); }
        }

        private ObservableCollection<DestinationType> destinationTypes;
        public ObservableCollection<DestinationType> DestinationTypes
        {
            get { return destinationTypes; }
            set { SetProperty(ref destinationTypes, value); }
        }


        private Supplier selectedSupplier;

        public Supplier SelectedSupplier
        {
            get { return selectedSupplier; }
            set { SetProperty(ref selectedSupplier, value); }
        }

        private Visibility isReqVisibility;
        public Visibility IsReqVisibility
        {
            get { return isReqVisibility; }
            set { SetProperty(ref isReqVisibility, value); }
        }
    }
}