using Eqstra.BusinessLogic.Base;
using Eqstra.BusinessLogic.Helpers;
using Eqstra.BusinessLogic.Popups;
using Microsoft.Practices.Prism.StoreApps;
using SQLite;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Windows.Media.Capture;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace Eqstra.BusinessLogic.TI
{
    public class MaintenanceRepair : ValidatableBindableBase
    {
        SnapshotsViewer _snapShotsPopup;

        public MaintenanceRepair()
        {
            this.MajorComponentImgList = new ObservableCollection<ImageCapture>();
            this.SubComponentImgList = new ObservableCollection<ImageCapture>();

            TakeSnapshotCommand = DelegateCommand<ObservableCollection<ImageCapture>>.FromAsyncHandler(async (param) =>
            {
                await TakeSnapshotAsync(param);
            });

            this.OpenSnapshotViewerCommand = new DelegateCommand<dynamic>((param) =>
            {
                OpenPopup(param);
            });
        }

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


        public void OpenPopup(dynamic dc)
        {
            CoreWindow currentWindow = Window.Current.CoreWindow;
            Popup popup = new Popup();
            popup.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Stretch;
            popup.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Stretch;

            if (_snapShotsPopup == null)
            {
                _snapShotsPopup = new SnapshotsViewer();

            }
            else
            {
                _snapShotsPopup = null;
                this._snapShotsPopup = new SnapshotsViewer();
            }
            _snapShotsPopup.DataContext = dc;
            
            popup.Child = _snapShotsPopup;
            this._snapShotsPopup.Tag = popup;

            this._snapShotsPopup.Height = currentWindow.Bounds.Height;
            this._snapShotsPopup.Width = currentWindow.Bounds.Width;

            popup.IsOpen = true;

        }
        [Ignore]
        public ICommand OpenSnapshotViewerCommand { get; set; }
        [Ignore]
        public ICommand TakeSnapshotCommand { get; set; }

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        private long repairid;

        public long Repairid
        {
            get { return repairid; }
            set { SetProperty(ref repairid, value); }
        }


        private long caseServiceRecId;

        public long CaseServiceRecId
        {
            get { return caseServiceRecId; }
            set { SetProperty(ref caseServiceRecId, value); }
        }


        private string majorComponent;

        public string MajorComponent
        {
            get { return majorComponent; }
            set { SetProperty(ref majorComponent, value); }
        }


        private string subComponent;

        public string SubComponent
        {
            get { return subComponent; }
            set { SetProperty(ref subComponent, value); }
        }

        private string cause;
        public string Cause
        {
            get { return cause; }
            set { SetProperty(ref cause, value); }
        }

        private string action;

        public string Action
        {
            get { return action; }
            set { SetProperty(ref action, value); }
        }

        private ObservableCollection<ImageCapture> majorComponentImgList;
        [Ignore]
        public ObservableCollection<ImageCapture> MajorComponentImgList
        {
            get { return majorComponentImgList; }
            set { SetProperty(ref majorComponentImgList, value); }
        }

        private ObservableCollection<ImageCapture> subComponentImgList;
        [Ignore]
        public ObservableCollection<ImageCapture> SubComponentImgList
        {
            get { return subComponentImgList; }
            set { SetProperty(ref subComponentImgList, value); }
        }



        public async System.Threading.Tasks.Task<ValidatableBindableBase> GetDataAsync(long vehicleInsRecID)
        {
            return await SqliteHelper.Storage.GetSingleRecordAsync<MaintenanceRepair>(x => x.Repairid == vehicleInsRecID);
        }
    }
}
