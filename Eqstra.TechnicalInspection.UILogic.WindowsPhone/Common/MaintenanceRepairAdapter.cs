//using Microsoft.Practices.Prism.Commands;
//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Windows.UI.Xaml;
//using System.Reflection;
//using Windows.UI.Core;
//using Windows.UI.Xaml.Controls.Primitives;
//using System.Windows.Input;
//using Microsoft.Practices.Prism.Mvvm;
//using Eqstra.TechnicalInspection;
//using Eqstra.TechnicalInspection.UILogic;
//namespace Eqstra.BusinessLogic.Portable.TIModels
//{
//    public class MaintenanceRepairAdapter : BindableBase
//    {
//        ImageViewerPopup _snapShotsPopup;

//        public MaintenanceRepairAdapter()
//        {
//            this.MajorComponentImgList = new ObservableCollection<ImageCapture>();
//            this.SubComponentImgList = new ObservableCollection<ImageCapture>();

//            TakeSnapshotCommand = DelegateCommand<Tuple<object, object>>.FromAsyncHandler(async (param) =>
//            {
//                var itemsource = param.Item1 as ObservableCollection<ImageCapture>;
//                var suffix = param.Item2.ToString();

//                var camCap = new CameraCaptureDialog();
//                camCap.Tag = itemsource;
//                camCap.Name = suffix;
//                await camCap.ShowAsync();
//            });

//            this.OpenSnapshotViewerCommand = new DelegateCommand<dynamic>((param) =>
//            {
//                OpenPopup(param);
//            });
//        }

//        public void OpenPopup(dynamic dc)
//        {
//            CoreWindow currentWindow = Window.Current.CoreWindow;
//            Popup popup = new Popup();
//            popup.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Stretch;
//            popup.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Stretch;

//            if (_snapShotsPopup == null)
//            {
//                _snapShotsPopup = new ImageViewerPopup();

//            }
//            else
//            {
//                _snapShotsPopup = null;
//                this._snapShotsPopup = new ImageViewerPopup();
//            }
//            _snapShotsPopup.DataContext = dc;

//            popup.Child = _snapShotsPopup;
//            this._snapShotsPopup.Tag = popup;

//            this._snapShotsPopup.Height = currentWindow.Bounds.Height;
//            this._snapShotsPopup.Width = currentWindow.Bounds.Width;

//            popup.IsOpen = true;

//        }
//        public ICommand OpenSnapshotViewerCommand { get; set; }
//        public ICommand TakeSnapshotCommand { get; set; }
//        public int Id { get; set; }

//        private long repairid;
//        public long Repairid
//        {
//            get { return repairid; }
//            set { SetProperty(ref repairid, value); }
//        }

//        private long caseServiceRecId;
//        public long CaseServiceRecId
//        {
//            get { return caseServiceRecId; }
//            set { SetProperty(ref caseServiceRecId, value); }
//        }

//        private string majorComponent;

//        public string MajorComponent
//        {
//            get { return majorComponent; }
//            set { SetProperty(ref majorComponent, value); }
//        }

//        private string subComponent;

//        public string SubComponent
//        {
//            get { return subComponent; }
//            set { SetProperty(ref subComponent, value); }
//        }

//        private string cause;
//        public string Cause
//        {
//            get { return cause; }
//            set { SetProperty(ref cause, value); }
//        }

//        private string action;

//        public string Action
//        {
//            get { return action; }
//            set { SetProperty(ref action, value); }
//        }

//        private ObservableCollection<ImageCapture> majorComponentImgList;
//        public ObservableCollection<ImageCapture> MajorComponentImgList
//        {
//            get { return majorComponentImgList; }
//            set { SetProperty(ref majorComponentImgList, value); }
//        }

//        private string majorComponentImgPathList;

//        public string MajorComponentImgPathList
//        {
//            get { return string.Join("~", MajorComponentImgList.Select(x => x.ImageData)); }
//            set { SetProperty(ref majorComponentImgPathList, value); }
//        }


//        private ObservableCollection<ImageCapture> subComponentImgList;
//        public ObservableCollection<ImageCapture> SubComponentImgList
//        {
//            get { return subComponentImgList; }
//            set { SetProperty(ref subComponentImgList, value); }
//        }

//        private string subComponentImgPathList;

//        public string SubComponentImgPathList
//        {
//            get { return string.Join("~", SubComponentImgList.Select(x => x.ImageData)); }
//            set { SetProperty(ref subComponentImgPathList, value); }
//        }

//    }
//}
