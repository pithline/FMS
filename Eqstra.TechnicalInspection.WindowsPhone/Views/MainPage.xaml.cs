using Eqstra.BusinessLogic.Portable.TIModels;
using Eqstra.TechnicalInspection.UILogic;
using Eqstra.TechnicalInspection.UILogic.WindowsPhone.ViewModels;
using Microsoft.Practices.Prism.StoreApps;
using ShakeGestures;
using System;
using System.Collections.ObjectModel;
using Windows.ApplicationModel.Appointments;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Eqstra.TechnicalInspection.WindowsPhone.Views
{
    public sealed partial class MainPage : VisualStateAwarePage
    {
        MainPageViewModel vm;
        public MainPage()
        {
            this.InitializeComponent();
            ShakeGesturesHelper.Instance.ShakeGesture += new EventHandler<ShakeGestureEventArgs>(Instance_ShakeGesture);
            ShakeGesturesHelper.Instance.MinimumRequiredMovesForShake = 2;
            ShakeGesturesHelper.Instance.Active = true;

        }
        private async void Instance_ShakeGesture(object sender, ShakeGestures.ShakeGestureEventArgs e)
        {
            await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {
                await vm.FetchTasks();
            });
        }

        protected override void OnNavigatedFrom(Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
        }
        protected override void OnNavigatedTo(Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            vm = ((MainPageViewModel)this.DataContext);
            base.OnNavigatedTo(e);
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
                await new MessageDialog("No phone number exist").ShowAsync();
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
                ObservableCollection<TITask> currentTasks;
                if (index == 0)
                {
                   currentTasks = PersistentData.Instance.PoolofTasks;
                }
                else
                {
                    currentTasks = PersistentData.Instance.Tasks;
                }

                ObservableCollection<TITask> filterResult = new ObservableCollection<TITask>();
                foreach (var task in currentTasks)
                {
                    if (task.ContactName.ToLower().Contains(text.ToLower()) ||
                        task.CustomerName.ToLower().Contains(text.ToLower()) ||
                        task.RegistrationNumber.ToLower().Contains(text.ToLower()) ||
                        task.CaseNumber.ToLower().Contains(text.ToLower()))
                    {
                        filterResult.Add(task);
                    }
                }

                if (index == 0)
                {
                  //  vm.PoolofTasks = filterResult;

                }
                else
                {
                  //  vm.Tasks = filterResult;

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
            vm.InspectionTask = (TITask)senderElement.DataContext;
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

        async private void Details_Click(object sender, RoutedEventArgs e)
        {
            //DetailsDialog m = new DetailsDialog();
            //m.DataContext = this.vm.InspectionTask;
            //await m.ShowAsync();
        }

        private async void Profile_Click(object sender, RoutedEventArgs e)
        {
            //UserProfile contentDialog = new UserProfile(vm._navigationService);
            //await contentDialog.ShowAsync();

        }
    }
}
