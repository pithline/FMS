using Eqstra.BusinessLogic;
using Eqstra.BusinessLogic.Portable.SSModels;
using Eqstra.ServiceScheduling.UILogic.Portable.Services;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using Microsoft.Practices.Prism.PubSubEvents;
using Newtonsoft.Json;
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
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;


namespace Eqstra.ServiceScheduling.UILogic.Portable
{
    public class ServiceSchedulingPageViewModel : ViewModel
    {
        public IEventAggregator _eventAggregator;
        public ILocationService _locationService;
        public ISupplierService _supplierService;

        private ImageViewerPopup _imageViewer;
        private INavigationService _navigationService;
        private IServiceDetailService _serviceDetailService;
        public BusyIndicator _busyIndicator;

        private ITaskService _taskService;
        public ServiceSchedulingPageViewModel(INavigationService navigationService, IEventAggregator eventAggregator, ILocationService locationService, IServiceDetailService serviceDetailService, ISupplierService supplierService, ITaskService taskService)
        {
            this._navigationService = navigationService;
            this._serviceDetailService = serviceDetailService;
            this._taskService = taskService;
            this._eventAggregator = eventAggregator;
            this._locationService = locationService;
            this._supplierService = supplierService;
            this.Model = new ServiceSchedulingDetail();
            _busyIndicator = new BusyIndicator();
            this.Address = new BusinessLogic.Portable.SSModels.Address();

            this.IsLiftRequired = false;

            this.NextPageCommand = DelegateCommand.FromAsyncHandler(
           async () =>
           {
               try
               {
                   if (this.Validate())
                   {
                       _busyIndicator.Open();
                       this.Model.ServiceDateOption1 = this.Model.ServiceDateOpt1.ToString("MM/dd/yyyy");
                       this.Model.ServiceDateOption2 = this.Model.ServiceDateOpt2.ToString("MM/dd/yyyy");
                       this.Model.ODOReadingDate = this.Model.ODOReadingDt.ToString("MM/dd/yyyy");
                       this.Model.ConfirmedDate = this.Model.ConfirmedDateDt.ToString("MM/dd/yyyy");
                       bool response = await _serviceDetailService.InsertServiceDetailsAsync(this.Model, this.Address, this.UserInfo);
                       if (response)
                       {
                           var caseStatus = await this._taskService.UpdateStatusListAsync(this.SelectedTask, this.UserInfo);
                           var supplier = new SupplierSelection() { CaseNumber = this.SelectedTask.CaseNumber, CaseServiceRecID = this.SelectedTask.CaseServiceRecID, SelectedSupplier = this.SelectedSupplier };
                           var res = await this._supplierService.InsertSelectedSupplierAsync(supplier, this.UserInfo);
                           if (res)
                           {
                               this.SelectedTask.Status = caseStatus.Status;
                               await this._taskService.UpdateStatusListAsync(this.SelectedTask, this.UserInfo);
                               navigationService.Navigate("Main", string.Empty);
                           }

                       }
                       _busyIndicator.Close();
                   }
               }
               catch (Exception ex)
               {
                   _busyIndicator.Close();
               }
               finally
               {
               }
           },

            () => { return this.Model != null; });


            this.TakePictureCommand = DelegateCommand<ImageCapture>.FromAsyncHandler(async (param) =>
          {
              var camCap =
              new CameraCaptureDialog();
              camCap.Tag = this.Model;
              await camCap.ShowAsync();


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

            _eventAggregator.GetEvent<SupplierFilterEvent>().Subscribe(poolofSupplier =>
            {
                this.PoolofSupplier = poolofSupplier;
            });

        }
        private bool Validate()
        {
            Boolean resp = true;
            if (String.IsNullOrEmpty(this.Model.SelectedServiceType))
            {
                this.StBorderBrush = new SolidColorBrush(Colors.Red);
                resp = false;
            }
            else
            {
                this.StBorderBrush = null;
            }

            if (this.IsLiftRequired)
            {
                if (this.Model.SelectedLocationType == null)
                {
                    this.LtBorderBrush = new SolidColorBrush(Colors.Red);
                    resp = false;
                }
                else
                {
                    this.LtBorderBrush = null;
                }
                if (this.Model.SelectedDestinationType == null)
                {
                    this.DtBorderBrush = new SolidColorBrush(Colors.Red);
                    resp = false;
                }
                else
                {
                    this.DtBorderBrush = null;
                }
                if (String.IsNullOrEmpty(this.Model.Address))
                {
                    this.AdBorderBrush = new SolidColorBrush(Colors.Red);
                    resp = false;
                }
                else
                {
                    this.AdBorderBrush = null;
                }
            }
            return resp;
        }
        public async override void OnNavigatedTo(object navigationParameter, Windows.UI.Xaml.Navigation.NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            try
            {
                _busyIndicator.Open();
                if (ApplicationData.Current.RoamingSettings.Values.ContainsKey(Constants.USERINFO))
                {
                    this.UserInfo = JsonConvert.DeserializeObject<UserInfo>(ApplicationData.Current.RoamingSettings.Values[Constants.USERINFO].ToString());
                }

                if (ApplicationData.Current.RoamingSettings.Values.ContainsKey(Constants.SELECTEDTASK))
                {
                    this.SelectedTask = JsonConvert.DeserializeObject<Eqstra.BusinessLogic.Portable.SSModels.Task>(ApplicationData.Current.RoamingSettings.Values[Constants.SELECTEDTASK].ToString());
                }

                this.Model = await _serviceDetailService.GetServiceDetailAsync(SelectedTask.CaseNumber, SelectedTask.CaseServiceRecID, SelectedTask.ServiceRecID, this.UserInfo);
                this.PoolofSupplier = await this._supplierService.GetSuppliersByClassAsync(SelectedTask.VehicleClassId, this.UserInfo);
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
                if (SelectedTask != null)
                {
                    this.DestinationTypes = await _serviceDetailService.GetDestinationTypeList("Vendor", SelectedTask.CustomerId, this.UserInfo);
                }


                if (this.Model == null)
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

                if (this.Model != null)
                {
                    this.IsLiftRequired = this.Model.IsLiftRequired;
                }
                _busyIndicator.Close();
            }
            catch (Exception)
            {
                _busyIndicator.Close();
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
        public UserInfo UserInfo { get; set; }
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

        public Eqstra.BusinessLogic.Portable.SSModels.Task SelectedTask { get; set; }
        public DelegateCommand NextPageCommand { get; private set; }
        public DelegateCommand<ImageCapture> TakePictureCommand { get; set; }
        public DelegateCommand OpenImageViewerCommand { get; set; }
        public DelegateCommand SupplierFilterCommand { get; set; }

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

        private ObservableCollection<Supplier> poolofSupplier;
        public ObservableCollection<Supplier> PoolofSupplier
        {
            get { return poolofSupplier; }
            set
            {
                SetProperty(ref poolofSupplier, value);
            }
        }

        private Visibility isReqVisibility;
        public Visibility IsReqVisibility
        {
            get { return isReqVisibility; }
            set { SetProperty(ref isReqVisibility, value); }
        }

        private Supplier selectedSupplier;
        public Supplier SelectedSupplier
        {
            get { return selectedSupplier; }
            set
            {
                SetProperty(ref selectedSupplier, value);
                this.NextPageCommand.RaiseCanExecuteChanged();
            }
        }

        private Brush stBorderBrush;
        public Brush StBorderBrush
        {
            get { return stBorderBrush; }
            set { SetProperty(ref stBorderBrush, value); }
        }

        private Brush ltBorderBrush;
        public Brush LtBorderBrush
        {
            get { return ltBorderBrush; }
            set { SetProperty(ref ltBorderBrush, value); }
        }


        private Brush dtBorderBrush;
        public Brush DtBorderBrush
        {
            get { return dtBorderBrush; }
            set { SetProperty(ref dtBorderBrush, value); }
        }
        private Brush adBorderBrush;
        public Brush AdBorderBrush
        {
            get { return adBorderBrush; }
            set { SetProperty(ref adBorderBrush, value); }
        }

    }
}