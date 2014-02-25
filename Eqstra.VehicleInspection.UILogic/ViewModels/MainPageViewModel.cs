using Eqstra.BusinessLogic;
using Eqstra.BusinessLogic.Helpers;
using Microsoft.Practices.Prism.StoreApps;
using Syncfusion.UI.Xaml.Schedule;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace Eqstra.VehicleInspection.UILogic.ViewModels
{
    public class MainPageViewModel : ViewModel
    {

        public MainPageViewModel()
        {
            this.poolofTasks = new ObservableCollection<BusinessLogic.Task>(); this.appointments = new ScheduleAppointmentCollection
            {
                new ScheduleAppointment(){
                    Subject = "Inspection at Peter Johnson",
                    Notes = "some noise from engine",
                    Location = "Cape Town",
                    StartTime = DateTime.Now,
                    EndTime = DateTime.Now.AddHours(2),
                    ReadOnly = true,
                   AppointmentBackground = new SolidColorBrush(Colors.Crimson),                   
                    Status = new ScheduleAppointmentStatus{Status = "Tentative",Brush = new SolidColorBrush(Colors.Chocolate)}

                },
                new ScheduleAppointment(){
                    Subject = "Inspection at Peter Johnson",
                    Notes = "some noise from differential",
                    Location = "Cape Town",
                     ReadOnly = true,
                    StartTime =new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,8,00,00),
                    EndTime = new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,9,00,00),
                    Status = new ScheduleAppointmentStatus{Brush = new SolidColorBrush(Colors.Green), Status  = "Free"},
                },                    
            };

            this.BingWeatherCommand = new DelegateCommand(() =>
            {
                    
            });
        }

        async public override void OnNavigatedTo(object navigationParameter, Windows.UI.Xaml.Navigation.NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
            var list = await SqliteHelper.Storage.LoadTableAsync<Eqstra.BusinessLogic.Task>();
            foreach (Eqstra.BusinessLogic.Task item in list)
            {
                var cust = await SqliteHelper.Storage.GetSingleRecordAsync<Customer>(x => x.Id == item.CustomerId);
                item.CustomerName = cust.Name;
                item.Address = cust.Address;
                this.poolofTasks.Add(item);
            }

        }

        private ObservableCollection<Eqstra.BusinessLogic.Task> poolofTasks;
        public ObservableCollection<Eqstra.BusinessLogic.Task> PoolofTasks
        {
            get { return poolofTasks; }
            set { SetProperty(ref poolofTasks, value); }
        }


        private ScheduleAppointmentCollection appointments;
        public ScheduleAppointmentCollection Appointments
        {
            get { return appointments; }
            set { SetProperty(ref appointments, value); }
        }

        public DelegateCommand BingWeatherCommand { get; set; }

    }
}
