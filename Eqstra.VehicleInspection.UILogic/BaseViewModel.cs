using Eqstra.BusinessLogic;
using Microsoft.Practices.Prism.StoreApps;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Capture;

namespace Eqstra.VehicleInspection.UILogic
{
    public class BaseViewModel : ViewModel
    {
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
    }
}
