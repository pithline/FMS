﻿using Microsoft.Practices.Prism.StoreApps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eqstra.BusinessLogic.Passenger;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using Eqstra.BusinessLogic.Helpers;
using Eqstra.BusinessLogic.Base;
using Windows.Storage;
using Eqstra.BusinessLogic;
using Eqstra.BusinessLogic.Common;

namespace Eqstra.VehicleInspection.UILogic.ViewModels
{
    public class VehicleDetailsUserControlViewModel : BaseViewModel
    {
        INavigationService _navigationService;
        private Eqstra.BusinessLogic.Task _inspection;
        public VehicleDetailsUserControlViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            this.Model = new PVehicleDetails();
            string caseNumber = (string)ApplicationData.Current.LocalSettings.Values["CaseNumber"];
            LoadModelFromDbAsync(caseNumber);
            this.GoToImageMarkupPageCommand = new DelegateCommand(() =>
            {
                _navigationService.Navigate("ImageMarkup", this.Model);
            });
        }
        public override void OnNavigatedTo(object navigationParameter, Windows.UI.Xaml.Navigation.NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
        }

        public DelegateCommand GoToImageMarkupPageCommand { get; set; }


        public async override System.Threading.Tasks.Task LoadModelFromDbAsync(string caseNumber)
        {
            this.Model = await SqliteHelper.Storage.GetSingleRecordAsync<PVehicleDetails>(x => x.CaseNumber == caseNumber);
            if (this.Model == null)
            {
                this.Model = new PVehicleDetails();
            }
            BaseModel viBaseObject = (PVehicleDetails)this.Model;
            viBaseObject.LoadSnapshotsFromDb();
            viBaseObject.ShouldSave = false;
            PropertyHistory.Instance.SetPropertyHistory(viBaseObject);
        }

        async public override System.Threading.Tasks.Task TakePictureAsync(ImageCapture param)
        {
            await base.TakePictureAsync(param);
            string caseNumber = (string)ApplicationData.Current.LocalSettings.Values["CaseNumber"];
            if (caseNumber != null)
            {
                var viobj = await (this.Model as BaseModel).GetDataAsync(caseNumber);
                if (viobj != null)
                {
                    var successFlag = await SqliteHelper.Storage.UpdateSingleRecordAsync(this.Model);
                }
                else
                {
                    ((BaseModel)this.Model).CaseNumber = caseNumber;
                    var successFlag = await SqliteHelper.Storage.InsertSingleRecordAsync(this.Model);
                }
            }
        }
    }
}
