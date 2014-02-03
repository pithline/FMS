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
   public class BaseViewModel:ViewModel
    {
       public BaseViewModel()
       {
           TakeSnapshotCommand = DelegateCommand<ObservableCollection<ImageCapture>>.FromAsyncHandler(async (param) =>
           {
               await TakeSnapshotAsync(param);
           });
       }
        public Object Model { get; set; }
        public DelegateCommand<ObservableCollection<ImageCapture>> TakeSnapshotCommand { get; set; }
        protected async System.Threading.Tasks.Task TakeSnapshotAsync<T>(T list) where T : ObservableCollection<ImageCapture>
        {
            CameraCaptureUI ccui = new CameraCaptureUI();
            var file = await ccui.CaptureFileAsync(CameraCaptureUIMode.Photo);
            if (file != null)
            {
                list.Add(new ImageCapture { ImagePath = file.Path });
            }
        } 
    }
}
