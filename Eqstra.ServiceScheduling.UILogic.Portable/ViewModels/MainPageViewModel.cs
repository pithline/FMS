using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Xaml;

namespace Eqstra.ServiceScheduling.UILogic.Portable
{
    public class MainPageViewModel : ViewModel
    {
        private INavigationService _navigationService;
        public MainPageViewModel(INavigationService navigationService)
        {
            this._navigationService = navigationService;

            this.AppBarVisibility = Visibility.Collapsed;

            this.PoolofTasks = new ObservableCollection<DataProvider.AX.SSModels.Task>();
            this.PoolofTasks.Add(new DataProvider.AX.SSModels.Task { CaseNumber = "556131", Status = "aaaaaaaaaaaa", RegistrationNumber = "5646133" });
            this.PoolofTasks.Add(new DataProvider.AX.SSModels.Task { CaseNumber = "556131", Status = "aaaaaaaaaaaa", RegistrationNumber = "5646133" });
            this.PoolofTasks.Add(new DataProvider.AX.SSModels.Task { CaseNumber = "556131", Status = "aaaaaaaaaaaa", RegistrationNumber = "5646133" });
            this.PoolofTasks.Add(new DataProvider.AX.SSModels.Task { CaseNumber = "556131", Status = "aaaaaaaaaaaa", RegistrationNumber = "5646133" });
            this.PoolofTasks.Add(new DataProvider.AX.SSModels.Task { CaseNumber = "556131", Status = "aaaaaaaaaaaa", RegistrationNumber = "5646133" });
            this.PoolofTasks.Add(new DataProvider.AX.SSModels.Task { CaseNumber = "556131", Status = "aaaaaaaaaaaa", RegistrationNumber = "5646133" });


            this.PoolofTasks.Add(new DataProvider.AX.SSModels.Task { CaseNumber = "556131", Status = "aaaaaaaaaaaa", RegistrationNumber = "5646133" });
            this.PoolofTasks.Add(new DataProvider.AX.SSModels.Task { CaseNumber = "556131", Status = "aaaaaaaaaaaa", RegistrationNumber = "5646133" });
            this.PoolofTasks.Add(new DataProvider.AX.SSModels.Task { CaseNumber = "556131", Status = "aaaaaaaaaaaa", RegistrationNumber = "5646133" });
            this.PoolofTasks.Add(new DataProvider.AX.SSModels.Task { CaseNumber = "556131", Status = "aaaaaaaaaaaa", RegistrationNumber = "5646133" });
            this.PoolofTasks.Add(new DataProvider.AX.SSModels.Task { CaseNumber = "556131", Status = "aaaaaaaaaaaa", RegistrationNumber = "5646133" });
            this.PoolofTasks.Add(new DataProvider.AX.SSModels.Task { CaseNumber = "556131", Status = "aaaaaaaaaaaa", RegistrationNumber = "5646133" });


            this.PoolofTasks.Add(new DataProvider.AX.SSModels.Task { CaseNumber = "556131", Status = "aaaaaaaaaaaa", RegistrationNumber = "5646133" });
            this.PoolofTasks.Add(new DataProvider.AX.SSModels.Task { CaseNumber = "556131", Status = "aaaaaaaaaaaa", RegistrationNumber = "5646133" });
            this.PoolofTasks.Add(new DataProvider.AX.SSModels.Task { CaseNumber = "556131", Status = "aaaaaaaaaaaa", RegistrationNumber = "5646133" });
            this.PoolofTasks.Add(new DataProvider.AX.SSModels.Task { CaseNumber = "556131", Status = "aaaaaaaaaaaa", RegistrationNumber = "5646133" });
            this.PoolofTasks.Add(new DataProvider.AX.SSModels.Task { CaseNumber = "556131", Status = "aaaaaaaaaaaa", RegistrationNumber = "5646133" });
            this.PoolofTasks.Add(new DataProvider.AX.SSModels.Task { CaseNumber = "556131", Status = "aaaaaaaaaaaa", RegistrationNumber = "5646133" });



            this.NextPageCommand = DelegateCommand.FromAsyncHandler(
             async () =>
             {
                 try
                 {
                     //navigationService.Navigate("ServiceScheduling", string.Empty);
                     navigationService.Navigate("PreferredSupplier", string.Empty);
                 }
                 catch (Exception ex)
                 {
                 }
                 finally
                 {
                     
                 }
             },

              () => { return this.InspectionTask != null; });




           // this.Location = new Bing.Maps.Location();
            this.MakeIMCommand = DelegateCommand<string>.FromAsyncHandler(async (emailId) =>
            {
                await Launcher.LaunchUriAsync(new Uri("skype:shoaibrafi?chat"));
            }, (emailId) => { return !string.IsNullOrEmpty(emailId); });

            this.MakeCallCommand = DelegateCommand<string>.FromAsyncHandler(async (number) =>
            {
                await Launcher.LaunchUriAsync(new Uri("audiocall-skype-com:" + number));
            }, (number) => { return !string.IsNullOrEmpty(number); });

            this.MailToCommand = DelegateCommand<string>.FromAsyncHandler(async (email) =>
            {
                await Launcher.LaunchUriAsync(new Uri("mailto:" + email));
            }, (email) => { return !string.IsNullOrEmpty(email); });

            //this.LocateCommand = DelegateCommand<string>.FromAsyncHandler(async (address) =>
            //{
            //    //await this.GeocodeAddressAsync(Regex.Replace(address, "\n", ","));
            //    //var stringBuilder = new StringBuilder("bingmaps:?rtp=pos.");
            //    //stringBuilder.Append(Location.Latitude);
            //    //stringBuilder.Append("_");
            //    //stringBuilder.Append(Location.Longitude);
            //    var stringBuilder = new StringBuilder("bingmaps:?where=" + Regex.Replace(address, "\n", ","));
            //    await Launcher.LaunchUriAsync(new Uri(stringBuilder.ToString()));
            //}, (address) =>
            //{
            //    return !string.IsNullOrEmpty(address);
            //});
        }



        private Visibility appBarVisibility;
        public Visibility AppBarVisibility
        {
            get { return appBarVisibility; }
            set
            {
                SetProperty(ref appBarVisibility, value);
            }
        }

        private Eqstra.DataProvider.AX.SSModels.Task task;
        public Eqstra.DataProvider.AX.SSModels.Task InspectionTask
        {
            get { return task; }
            set
            {
                SetProperty(ref task, value);
                if (value != null)
                {
                    AppBarVisibility = Visibility.Visible;
                }
                else
                {
                    AppBarVisibility = Visibility.Collapsed;

                }
            }
        }

        private ObservableCollection<Eqstra.DataProvider.AX.SSModels.Task> poolofTasks;
        public ObservableCollection<Eqstra.DataProvider.AX.SSModels.Task> PoolofTasks
        {
            get { return poolofTasks; }
            set
            {
                SetProperty(ref poolofTasks, value);
            }
        }
        public DelegateCommand NextPageCommand { get; private set; }
        public override void OnNavigatedTo(object navigationParameter, Windows.UI.Xaml.Navigation.NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
        }


        public DelegateCommand<string> MailToCommand { get; set; }

        public DelegateCommand<string> MakeIMCommand { get; set; }

        public DelegateCommand<string> LocateCommand { get; set; }

        public DelegateCommand<string> MakeCallCommand { get; set; }

        //private Bing.Maps.Location location;
        //public Bing.Maps.Location Location
        //{
        //    get { return location; }
        //    set { SetProperty(ref location, value); }
        //}

    }
}
