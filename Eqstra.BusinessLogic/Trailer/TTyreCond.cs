﻿using Eqstra.BusinessLogic.Base;
using Eqstra.BusinessLogic.Helpers;
using Eqstra.BusinessLogic.Popups;
using Microsoft.Practices.Prism.StoreApps;
using SQLite;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
namespace Eqstra.BusinessLogic
{
    public class TTyreCond : BaseModel
    {
        SnapshotsViewer _snapShotsPopup;

        public TTyreCond()
        {
            this.SnapshotImgList = new ObservableCollection<ImageCapture>();


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
                var storagefile = await ccui.CaptureFileAsync(CameraCaptureUIMode.Photo);
                if (storagefile != null)
                {

                    var ms = await RenderDataStampOnSnap.RenderStaticTextToBitmap(storagefile);
                    var msrandom = new MemoryRandomAccessStream(ms);
                    Byte[] bytes = new Byte[ms.Length];
                    await ms.ReadAsync(bytes, 0, (int)ms.Length);

                    StorageFile file = await ApplicationData.Current.TemporaryFolder.CreateFileAsync(DateTime.Now.Ticks.ToString() + storagefile.Name, CreationCollisionOption.ReplaceExisting);
                    using (var strm = await file.OpenStreamForWriteAsync())
                    {
                        await strm.WriteAsync(bytes, 0, bytes.Length);
                        strm.Flush();
                    }
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

        private int tyreCondID;
        [PrimaryKey, AutoIncrement]
        public int TyreCondID
        {
            get { return tyreCondID; }
            set { SetProperty(ref tyreCondID, value); }
        }

        private long vehicleInsRecID;

        public new long VehicleInsRecID
        {
            get { return vehicleInsRecID; }
            set { SetProperty(ref vehicleInsRecID, value); }
        }


        private string position;

        public string Position
        {
            get { return position; }
            set { SetProperty(ref position, value); }
        }

        private string comment;

        public string Comment
        {
            get { return comment; }
            set { SetProperty(ref comment, value); }
        }
        private string threadDepth;

        public string ThreadDepth
        {
            get { return threadDepth; }
            set { SetProperty(ref threadDepth, value); }
        }

        private string make;

        public string Make
        {
            get { return make; }
            set { SetProperty(ref make, value); }
        }

        private bool isFair;

        public bool IsFair
        {
            get { return isFair; }
            set { SetProperty(ref isFair, value); }
        }

        private bool isGood;

        public bool IsGood
        {
            get { return isGood; }
            set { SetProperty(ref isGood, value); }
        }
        private bool isPoor;
        public bool IsPoor
        {
            get { return isPoor; }
            set
            {
                if (SetProperty(ref  isPoor, value) && !this.IsPoor)
                {
                    this.SnapshotImgList.Clear();
                }
            }
        }


        [Ignore]
        public ICommand OpenSnapshotViewerCommand { get; set; }
        [Ignore]
        public ICommand TakeSnapshotCommand { get; set; }

        private ObservableCollection<ImageCapture> snapshotImgList;
        [Ignore]
        public ObservableCollection<ImageCapture> SnapshotImgList
        {
            get { return snapshotImgList; }
            set { SetProperty(ref snapshotImgList, value); }
        }

        public async override Task<BaseModel> GetDataAsync(long vehicleInsRecID)
        {
            return await SqliteHelper.Storage.GetSingleRecordAsync<TTyreCond>(x => x.VehicleInsRecID == vehicleInsRecID);
        }
    }
}
