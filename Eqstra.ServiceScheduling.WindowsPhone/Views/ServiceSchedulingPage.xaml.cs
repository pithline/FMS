﻿using Eqstra.ServiceScheduling.Common;
using Eqstra.ServiceScheduling.UILogic.Portable;
using Eqstra.ServiceScheduling.Views;
using Microsoft.Practices.Prism.StoreApps;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Appointments;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.Phone.UI.Input;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Eqstra.ServiceScheduling.WindowsPhone.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ServiceSchedulingPage : VisualStateAwarePage
    {
        DetailsDialog moreInfo;
        SearchSupplierDialog sp;
        public ServiceSchedulingPage()
        {
            this.InitializeComponent();
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
        }

        void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            ((ServiceSchedulingPageViewModel)this.DataContext)._busyIndicator.Close();
            if (sp != null)
            {
                sp.Hide();
            }
            if (moreInfo != null)
            {
                moreInfo.Hide();
            }

        }

        protected override void OnNavigatedFrom(Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            if (sp != null)
            {
                sp.Hide();
            }
            if (moreInfo != null)
            {
                moreInfo.Hide();
            }
        }
        protected override void OnNavigatedTo(Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }
        async private void More_Click(object sender, RoutedEventArgs e)
        {
            var vm = this.DataContext as ServiceSchedulingPageViewModel;
            moreInfo = new DetailsDialog();
            moreInfo.DataContext = vm.SelectedTask;
            await moreInfo.ShowAsync();
        }
        private async void Calendar_Click(object sender, RoutedEventArgs e)
        {
            await AppointmentManager.ShowTimeFrameAsync(DateTime.Today, TimeSpan.FromDays(7));
        }

        async private void filterSup_Click(object sender, RoutedEventArgs e)
        {
            var vm = this.DataContext as ServiceSchedulingPageViewModel;
            if (vm != null)
            {
                sp = new SearchSupplierDialog(vm._locationService, vm._eventAggregator, vm._supplierService);
                sp.ShowAsync();

            }
        }

        async private void ddLocationType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var vm = this.DataContext as ServiceSchedulingPageViewModel;
            try
            {

                vm._busyIndicator.Open("Please wait,loading destination types ...");
                if (vm != null)
                {
                    if (vm.Model.SelectedLocationType.LocType == "Other")
                    {
                        vm.IsEnabledDesType = false;
                        vm.AddVisibility = Visibility.Collapsed;
                    }
                    else
                    {
                        vm.IsEnabledDesType = false;
                        vm.AddVisibility = Visibility.Collapsed;

                        if (vm.SelectedTask != null && vm.Model.SelectedLocationType != null)
                        {
                            vm.DestinationTypes = await vm._serviceDetailService.GetDestinationTypeList(vm.Model.SelectedLocationType.LocType, vm.SelectedTask.CustomerId, vm.UserInfo);
                        }

                    }
                }
                vm._busyIndicator.Close();
            }
            catch (Exception)
            {
                vm._busyIndicator.Close();

            }
        }

        private void ddDestinationTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var vm = this.DataContext as ServiceSchedulingPageViewModel;
            if (vm != null)
            {
                vm.Model.Address = vm.Model.SelectedDestinationType.Address;
            }

        }

    }
}
