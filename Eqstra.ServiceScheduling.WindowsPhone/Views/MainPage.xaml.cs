using Eqstra.ServiceScheduling.UILogic.Portable;
using Microsoft.Practices.Prism.StoreApps;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Appointments;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
        public MainPage()
        {
            this.InitializeComponent();
            // this.DataContext = new MainPageViewModel();
        }

        protected override void OnNavigatedFrom(Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
        }
        protected override void OnNavigatedTo(Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }

        private void More_Click(object sender, RoutedEventArgs e)
        {
            MoreInfo m = new MoreInfo();
            m.Open(((MainPageViewModel)this.DataContext).InspectionTask);
        }


        async private void Message_Click(object sender, RoutedEventArgs e)
        {
            Windows.ApplicationModel.Chat.ChatMessage msg
         = new Windows.ApplicationModel.Chat.ChatMessage();
            msg.Body = "";
            msg.Recipients.Add(((MainPageViewModel)this.DataContext).InspectionTask.CustPhone);
            await Windows.ApplicationModel.Chat.ChatMessageManager
                     .ShowComposeSmsMessageAsync(msg);

        }

        private async void Calendar_Click(object sender, RoutedEventArgs e)
        {
            await AppointmentManager.ShowTimeFrameAsync(DateTime.Today,TimeSpan.FromDays(7));
        }

    }
}
