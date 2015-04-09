﻿using Eqstra.BusinessLogic.Portable.SSModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Capture;
using Windows.Media.Devices;
using Windows.Media.MediaProperties;
using Windows.Phone.UI.Input;
using Windows.Storage.Pickers.Provider;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Content Dialog item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Eqstra.ServiceScheduling.UILogic
{
    public sealed partial class CameraCaptureDialog : ContentDialog
    {
        MediaCapture _mediaCapture;
        byte[] _bytes;
        public CameraCaptureDialog()
        {
            this.InitializeComponent();
            this.Loaded += CameraCaptureDialog_Loaded;
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            this.Closing += CameraCaptureDialog_Closing;
        }

        async void CameraCaptureDialog_Closing(ContentDialog sender, ContentDialogClosingEventArgs args)
        {

            await _mediaCapture.StopPreviewAsync();
        }

        async void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
        }

        async void CameraCaptureDialog_Loaded(object sender, RoutedEventArgs e)
        {

        }



        private static async Task<DeviceInformation> GetCameraDeviceInfoAsync(Windows.Devices.Enumeration.Panel desiredPanel)
        {

            DeviceInformation device = (await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture))
                .FirstOrDefault(d => d.EnclosureLocation != null && d.EnclosureLocation.Panel == desiredPanel);

            if (device == null)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "No suitable devices found for the camera of type {0}.", desiredPanel));
            }
            return device;
        }

        async private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            args.Cancel = true;
            var imageEncodingProps = ImageEncodingProperties.CreatePng();
            using (var stream = new InMemoryRandomAccessStream())
            {

                await _mediaCapture.CapturePhotoToStreamAsync(imageEncodingProps, stream);
                _bytes = new byte[stream.Size];
                var buffer = await stream.ReadAsync(_bytes.AsBuffer(), (uint)stream.Size, InputStreamOptions.None);
                _bytes = buffer.ToArray(0, (int)stream.Size);
                var bitmap = new BitmapImage();
                stream.Seek(0);
                await bitmap.SetSourceAsync(stream);
                var model = this.Tag as ServiceSchedulingDetail;
                if (model.OdoReadingImageCapture==null)
                {
                    model.OdoReadingImageCapture = new BusinessLogic.ImageCapture();
                }
                model.OdoReadingImageCapture.ImageBitmap = bitmap;
                model.ODOReadingSnapshot = Convert.ToBase64String(_bytes);
                Img.Source = bitmap;

            }
            args.Cancel = false;
         
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {


        }

        async private void PreviewElement_Loaded(object sender, RoutedEventArgs e)
        {
            _mediaCapture = new MediaCapture();


            var _deviceInformation = await GetCameraDeviceInfoAsync(Windows.Devices.Enumeration.Panel.Back);

            var settings = new MediaCaptureInitializationSettings();
            //settings.StreamingCaptureMode = StreamingCaptureMode.Video;
            settings.PhotoCaptureSource = PhotoCaptureSource.Photo;
            settings.AudioDeviceId = "";
            if (_deviceInformation != null)
                settings.VideoDeviceId = _deviceInformation.Id;

            await _mediaCapture.InitializeAsync(settings);

            var focusSettings = new FocusSettings();
            focusSettings.AutoFocusRange = AutoFocusRange.FullRange;
            focusSettings.Mode = FocusMode.Auto;
            focusSettings.WaitForFocus = true;
            focusSettings.DisableDriverFallback = false;

            _mediaCapture.VideoDeviceController.FocusControl.Configure(focusSettings);
            await _mediaCapture.VideoDeviceController.ExposureControl.SetAutoAsync(true);

            _mediaCapture.SetPreviewRotation(VideoRotation.Clockwise90Degrees);
            _mediaCapture.SetRecordRotation(VideoRotation.Clockwise90Degrees);


            PreviewElement.Source = _mediaCapture;
            await _mediaCapture.StartPreviewAsync();
        }


    }
}
