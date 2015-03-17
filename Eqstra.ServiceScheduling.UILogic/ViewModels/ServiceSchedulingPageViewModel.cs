﻿using Eqstra.BusinessLogic;
using Eqstra.BusinessLogic.Helpers;
using Eqstra.BusinessLogic.ServiceSchedule;
using Eqstra.ServiceScheduling.UILogic.AifServices;
using Eqstra.ServiceScheduling.UILogic.Helpers;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Windows.Media.Capture;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Eqstra.ServiceScheduling.UILogic.ViewModels
{
    public class ServiceSchedulingPageViewModel : BaseViewModel
    {
        private DriverTask _task;
        private INavigationService _navigationService;
        private IEventAggregator _eventAggregator;
        private SettingsFlyout _settingsFlyout;
        public ServiceSchedulingPageViewModel(INavigationService navigationService, IEventAggregator eventAggregator, SettingsFlyout settingsFlyout)
            : base(navigationService)
        {
            _navigationService = navigationService;
            _settingsFlyout = settingsFlyout;
            _eventAggregator = eventAggregator;
            this.Address = new Address();
            this.IsAddFlyoutOn = Visibility.Collapsed;
            this.Model = new ServiceSchedulingDetail();
            this.GoToSupplierSelectionCommand = new DelegateCommand(async () =>
            {
                try
                {
                    if (ODOImageValidate() && this.Model.ValidateProperties())
                    {
                        var messageDialog = new MessageDialog("Details once saved cannot be edited. Do you want to continue ?");
                        messageDialog.Commands.Add(new UICommand("Yes", OnYesButtonClicked));
                        messageDialog.Commands.Add(new UICommand("No"));
                        await messageDialog.ShowAsync();
                    }
                }
                catch (Exception ex)
                {

                    this.IsBusy = false;
                    AppSettings.Instance.ErrorMessage = ex.Message;
                }
            });

            this.AddAddressCommand = new DelegateCommand<string>((x) =>
            {
                settingsFlyout.Title = x + " Address";
                settingsFlyout.ShowIndependent();
            });

            this.ODOReadingPictureCommand = new DelegateCommand(async () =>
            {
                CameraCaptureUI cam = new CameraCaptureUI();
                var file = await cam.CaptureFileAsync(CameraCaptureUIMode.Photo);
                if (file != null)
                {
                    ODOReadingImagePath = file.Path;

                }
            });

            this._eventAggregator.GetEvent<AddressEvent>().Subscribe((address) =>
            {
                if (address != null)
                {
                    this.Address = address;
                    StringBuilder sb = new StringBuilder();


                    sb.Append(address.Street).Append(",").Append(Environment.NewLine);

                    if ((address.SelectedSuburb != null) && !String.IsNullOrEmpty(address.SelectedSuburb.Name))
                    {
                        sb.Append(address.SelectedSuburb.Name).Append(",").Append(Environment.NewLine);
                    }
                    if (address.SelectedRegion != null)
                    {
                        sb.Append(address.SelectedRegion.Name).Append(",").Append(Environment.NewLine);
                    }
                    if ((address.SelectedCity != null) && !String.IsNullOrEmpty(address.SelectedCity.Name))
                    {
                        sb.Append(address.SelectedCity.Name).Append(",").Append(Environment.NewLine);
                    }
                    if ((address.SelectedProvince != null) && !String.IsNullOrEmpty(address.SelectedProvince.Name))
                    {
                        sb.Append(address.SelectedProvince.Name).Append(",").Append(Environment.NewLine);
                    }

                    if ((address.SelectedCountry != null) && !String.IsNullOrEmpty(address.SelectedCountry.Name))
                    {
                        sb.Append(address.SelectedCountry.Name).Append(",").Append(Environment.NewLine);
                    }

                    sb.Append(address.SelectedZip);


                    this.Model.Address = sb.ToString();
                }
                settingsFlyout.Hide();
            });

            this.TakePictureCommand = DelegateCommand<ImageCapture>.FromAsyncHandler(async (param) =>
            {
                await TakePictureAsync(param);
            });

            this.LocTypeChangedCommand = new DelegateCommand<object>(async (param) =>
            {
                try
                {
                    var locType = ((LocationType)param).LocType;
                    this.IsBusy = true;
                    if (this.Model.DestinationTypes != null)
                    {
                        this.Model.DestinationTypes.Clear();
                        this.Model.SelectedDestinationType = null;
                    }
                    if (this.Model.DestinationTypes != null && locType == LocationTypeConstants.Driver && this._task != null)
                    {
                        this.Model.DestinationTypes.AddRange(await SSProxyHelper.Instance.GetDriversFromSvcAsync(this._task.CustomerId));
                    }
                    if (this.Model.DestinationTypes != null && locType == LocationTypeConstants.Customer && this._task != null)
                    {
                        this.Model.DestinationTypes.AddRange(await SSProxyHelper.Instance.GetCustomersFromSvcAsync(this._task.CustomerId));
                    }

                    if (this.Model.DestinationTypes != null && locType == LocationTypeConstants.Vendor)
                    {
                        this.Model.DestinationTypes.AddRange(await SSProxyHelper.Instance.GetVendorsFromSvcAsync());
                    }
                    this.IsAddFlyoutOn = Visibility.Collapsed;
                    this.IsAlternative = Visibility.Visible;
                    if (locType == LocationTypeConstants.Other)
                    {
                        this.Model.DestinationTypes = new ObservableCollection<DestinationType>();
                        this.Model.SelectedDestinationType = new DestinationType();
                        this.IsAddFlyoutOn = Visibility.Visible;
                        this.IsAlternative = Visibility.Collapsed;
                    }

                    this.IsBusy = false;
                }
                catch (Exception ex)
                {
                    this.IsBusy = false;
                    AppSettings.Instance.ErrorMessage = ex.Message;
                }

            });

            this.DestiTypeChangedCommand = new DelegateCommand<object>(async (param) =>
            {
                try
                {
                    this.IsBusy = true;
                    if (param != null)
                    {
                        if (param is DestinationType)
                        {
                            this.Address.EntityRecId = ((DestinationType)param).RecID;
                            DestinationType destinationType = param as DestinationType;
                            this.Model.Address = destinationType.Address;
                        }
                    }
                    this.IsBusy = false;
                }
                catch (Exception ex)
                {
                    this.IsBusy = false;
                    AppSettings.Instance.ErrorMessage = ex.Message;
                }
            });
        }

        private Boolean ODOImageValidate()
        {
            if (this.Model.ODOReadingSnapshot.ImagePath.Equals("ms-appx:///Assets/ODO_meter.png"))
            {
                var messageDialog = new MessageDialog("Please take missing images");
                messageDialog.Commands.Add(new UICommand("Ok"));
                messageDialog.ShowAsync();
                return false;
            }
            return true;
        }

        async private void OnYesButtonClicked(IUICommand command)
        {
            this.IsBusy = true;

            if (!this.Model.IsLiftRequired)
            {
                this.Model.SelectedLocationType = new LocationType();
                this.Model.Address = string.Empty;
                this.Model.SelectedDestinationType = new DestinationType();
                this.Address.EntityRecId = default(long);
            }

            bool isInserted = await SSProxyHelper.Instance.InsertServiceDetailsToSvcAsync(this.Model, this.Address, this._task.CaseNumber, this._task.CaseServiceRecID, this.Address.EntityRecId);
            //if (isInserted)
            //{

                PersistentData.Instance.CustomerDetails.Status = await SSProxyHelper.Instance.UpdateStatusListToSvcAsync(this._task);
                PersistentData.Instance.DriverTask.Status = PersistentData.Instance.CustomerDetails.Status;
           // }
            _navigationService.Navigate("SupplierSelection", string.Empty);


            this.IsBusy = false;
        }
        async public override void OnNavigatedTo(object navigationParameter, Windows.UI.Xaml.Navigation.NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            try
            {
                this.IsBusy = true;
                base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
                this._task = PersistentData.Instance.DriverTask;
                this.CustomerDetails = PersistentData.Instance.CustomerDetails;
                this.Model = await SSProxyHelper.Instance.GetServiceDetailsFromSvcAsync(this._task.CaseNumber, this._task.CaseServiceRecID, this._task.ServiceRecID);
                this.Model.IsValidationEnabled = true;
                this.ODOReadingImagePath = "ms-appx:///Assets/odo_meter.png";
                this.IsBusy = false;
            }
            catch (Exception ex)
            {
                this.IsBusy = false;
                AppSettings.Instance.ErrorMessage = ex.Message;
            }
        }
        private CustomerDetails customerDetails;
        public CustomerDetails CustomerDetails
        {
            get { return customerDetails; }
            set { SetProperty(ref customerDetails, value); }
        }
        private Customer customer;
        public Customer Customer
        {
            get { return customer; }
            set { SetProperty(ref customer, value); }
        }
        public DelegateCommand GoToSupplierSelectionCommand { get; set; }

        public DelegateCommand ODOReadingPictureCommand { get; set; }
        public DelegateCommand<string> AddAddressCommand { get; set; }
        public ICommand LocTypeChangedCommand { get; set; }
        public DelegateCommand<object> DestiTypeChangedCommand { get; set; }

        private string odoReadingImagePath;
        public string ODOReadingImagePath
        {
            get { return odoReadingImagePath; }
            set { SetProperty(ref odoReadingImagePath, value); }
        }
        public DelegateCommand<ImageCapture> TakePictureCommand { get; set; }
        async public virtual System.Threading.Tasks.Task TakePictureAsync(ImageCapture param)
        {
            try
            {
                CameraCaptureUI cam = new CameraCaptureUI();
                var file = await cam.CaptureFileAsync(CameraCaptureUIMode.Photo);
                using (var stream = await file.OpenReadAsync())
                {
                    byte[] bytes = new byte[stream.Size];
                    using (var reader = new DataReader(stream))
                    {
                        await reader.LoadAsync((uint)stream.Size);
                        reader.ReadBytes(bytes);
                    }


                    if (file != null)
                    {
                        param.ImagePath = file.Path;
                        param.ImageBinary = Convert.ToBase64String(bytes);
                    }
                }

            }
            catch (Exception ex)
            {
                AppSettings.Instance.ErrorMessage = ex.Message;
            }
        }
        private Visibility isAddFlyoutOn;
        public Visibility IsAddFlyoutOn
        {
            get { return isAddFlyoutOn; }
            set { SetProperty(ref isAddFlyoutOn, value); }
        }
        private ServiceSchedulingDetail model;
        public ServiceSchedulingDetail Model
        {
            get { return model; }
            set { SetProperty(ref model, value); }
        }

        private Visibility isAlternative;
        public Visibility IsAlternative
        {
            get { return isAlternative; }
            set { SetProperty(ref isAlternative, value); }
        }

        private Address address;
        public Address Address
        {
            get { return address; }
            set { SetProperty(ref address, value); }
        }
    }
}
