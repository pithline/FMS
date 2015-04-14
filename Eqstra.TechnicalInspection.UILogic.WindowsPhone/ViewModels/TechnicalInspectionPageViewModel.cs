using Eqstra.BusinessLogic.Portable.TIModels;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using Microsoft.Practices.Prism.PubSubEvents;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eqstra.TechnicalInspection.UILogic.WindowsPhone.ViewModels
{
    public class TechnicalInspectionPageViewModel : ViewModel
    {
        private TITask _task;
        private INavigationService _navigationService;
        private IEventAggregator _eventAggregator;

        public TechnicalInspectionPageViewModel(INavigationService navigationService, IEventAggregator eventAggregator)
        {
            _navigationService = navigationService;
            this._eventAggregator = eventAggregator;
            this.MaintenanceRepairList = new ObservableCollection<MaintenanceRepair>();
            this.MaintenanceRepairAdpList = new ObservableCollection<MaintenanceRepairAdapter>();
            
            this.Model = new TIData();
        }

        public async override void OnNavigatedTo(object navigationParameter, Windows.UI.Xaml.Navigation.NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            try
            {
                base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
                this.MaintenanceRepairAdpList.Add(new MaintenanceRepairAdapter() { });
                this.MaintenanceRepairAdpList.Add(new MaintenanceRepairAdapter() { });
                this.MaintenanceRepairAdpList.Add(new MaintenanceRepairAdapter() { });
            }
            catch(Exception )
            {

            }
        }




        private ObservableCollection<MaintenanceRepair> maintenanceRepairList;
        public ObservableCollection<MaintenanceRepair> MaintenanceRepairList
        {
            get { return maintenanceRepairList; }
            set { SetProperty(ref maintenanceRepairList, value); }
        }

        private ObservableCollection<MaintenanceRepairAdapter> maintenanceRepairAdpList;
        public ObservableCollection<MaintenanceRepairAdapter> MaintenanceRepairAdpList
        {
            get { return maintenanceRepairAdpList; }
            set { SetProperty(ref maintenanceRepairAdpList, value); }
        }

        private TIData model;

        public TIData Model
        {
            get { return model; }
            set { SetProperty(ref model, value); }
        }

        public DelegateCommand NextCommand { get; set; }
    }
}
