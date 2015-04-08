using Eqstra.BusinessLogic.Portable.SSModels;
using Eqstra.ServiceScheduling.UILogic;
using Eqstra.ServiceScheduling.UILogic.Portable;
using Eqstra.ServiceScheduling.Views;
using Microsoft.Practices.Prism.StoreApps;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Appointments;
using Windows.Devices.Sensors;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.UI.Input;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Eqstra.ServiceScheduling.WindowsPhone.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : VisualStateAwarePage
    {
        Accelerometer accelerometer;
        MainPageViewModel vm;
        public MainPage()
        {
            this.InitializeComponent();
          
        }

      
      
        void accelerometer_ReadingChanged(Accelerometer sender, AccelerometerReadingChangedEventArgs args)
        {
            if (accelerometer != null)
            {
                var accel = accelerometer.GetCurrentReading();
                var ax = accel.AccelerationX;
                var ay = accel.AccelerationY;
                var az = accel.AccelerationZ;
            }
        }

        async void accelerometer_Shaken(Accelerometer sender, AccelerometerShakenEventArgs args)
        {
            await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
           {
               int s = 564611;
           });
        }

        protected override void OnNavigatedFrom(Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
        }
        protected override void OnNavigatedTo(Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            vm = ((MainPageViewModel)this.DataContext);
            accelerometer = Accelerometer.GetDefault();
            base.OnNavigatedTo(e);
            accelerometer.ReadingChanged += accelerometer_ReadingChanged;
            accelerometer.Shaken += accelerometer_Shaken;

        }



        async private void Message_Click(object sender, RoutedEventArgs e)
        {

            if (!string.IsNullOrEmpty(vm.InspectionTask.CustPhone))
            {
                Windows.ApplicationModel.Chat.ChatMessage msg = new Windows.ApplicationModel.Chat.ChatMessage();
                msg.Body = "";
                msg.Recipients.Add(vm.InspectionTask.CustPhone);
                await Windows.ApplicationModel.Chat.ChatMessageManager.ShowComposeSmsMessageAsync(msg); 
            }
            else
            {
              await  new MessageDialog("No phone number exist").ShowAsync() ;
               
            }
        }

        private async void Calendar_Click(object sender, RoutedEventArgs e)
        {
            await AppointmentManager.ShowTimeFrameAsync(DateTime.Today, TimeSpan.FromDays(7));
        }

        private void Filter_TextChanged(object sender, TextChangedEventArgs e)
        {
            var index = TasksPivot.SelectedIndex;
            var text = ((TextBox)sender).Text;
            if (!String.IsNullOrEmpty(text))
            {
                ObservableCollection<BusinessLogic.Portable.SSModels.Task> currentTasks;
                if (index == 0)
                {
                    currentTasks = PersistentData.Instance.PoolofTasks;
                }
                else
                {
                    currentTasks = PersistentData.Instance.Tasks;
                }

                ObservableCollection<BusinessLogic.Portable.SSModels.Task> filterResult = new ObservableCollection<BusinessLogic.Portable.SSModels.Task>();
                foreach (var task in currentTasks)
                {
                    if (task.ContactName.Contains(text) || task.CustomerName.Contains(text) || task.RegistrationNumber.Contains(text) || task.CaseNumber.Contains(text))
                    {
                        filterResult.Add(task);
                    }
                }

                if (index == 0)
                {
                    vm.PoolofTasks = filterResult;

                }
                else
                {
                    vm.Tasks = filterResult;

                }
            }
            else
            {
                vm.PoolofTasks = PersistentData.Instance.PoolofTasks;

                vm.Tasks = PersistentData.Instance.Tasks;

            }
        }

        private void Flyout_Closed(object sender, object e)
        {
            FlyoutBase.ShowAttachedFlyout(filter);
        }

        private void filter_Click(object sender, RoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout(filter);
        }

        private void TasksPivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            vm.InspectionTask = null;
        }

        private void contextmenu_Holding(object sender, HoldingRoutedEventArgs e)
        {
            FrameworkElement senderElement = sender as FrameworkElement;
            vm.InspectionTask = (Eqstra.BusinessLogic.Portable.SSModels.Task)senderElement.DataContext;
            FlyoutBase flyoutBase = FlyoutBase.GetAttachedFlyout(senderElement);
            flyoutBase.ShowAt(senderElement);
        }

        private void GridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            vm.NextPageCommand.Execute(e.ClickedItem);
        }

        private void phone_Click(object sender, RoutedEventArgs e)
        {
            vm.MakeCallCommand.Execute();

        }

        private void mail_Click(object sender, RoutedEventArgs e)
        {
            vm.MailToCommand.Execute();
        }


        private void map_Click(object sender, RoutedEventArgs e)
        {
            vm.LocateCommand.Execute();
        }

        private void Details_Click(object sender, RoutedEventArgs e)
        {
            MoreInfo m = new MoreInfo();
            m.Open(vm.InspectionTask);
        }

        private async void Profile_Click(object sender, RoutedEventArgs e)
        {

            UserProfile contentDialog = new UserProfile();
            await contentDialog.ShowAsync();

        }

        void appBarButton_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
