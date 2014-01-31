using Eqstra.BusinessLogic.Helpers;
using Microsoft.Practices.Prism.StoreApps;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eqstra.VehicleInspection.UILogic.ViewModels
{
    public class MainPageViewModel : ViewModel
    {

        public MainPageViewModel()
        {
            this.poolofTasks = new ObservableCollection<BusinessLogic.Task>();
        }

        async public override void OnNavigatedTo(object navigationParameter, Windows.UI.Xaml.Navigation.NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
            var list = await SqliteHelper.Instance.LoadTableAsync<Eqstra.BusinessLogic.Task>();
            foreach (Eqstra.BusinessLogic.Task item in list)
            {
                this.poolofTasks.Add(item);
            }

        }

        private ObservableCollection<Eqstra.BusinessLogic.Task> poolofTasks;

        public ObservableCollection<Eqstra.BusinessLogic.Task> PoolofTasks
        {
            get { return poolofTasks; }
            set { SetProperty(ref poolofTasks, value); }
        }


    }
}
