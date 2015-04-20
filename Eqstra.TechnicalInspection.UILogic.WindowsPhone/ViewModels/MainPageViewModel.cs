﻿using Eqstra.BusinessLogic.Portable;
using Eqstra.BusinessLogic.Portable.SSModels;
using Eqstra.BusinessLogic.Portable.TIModels;
using Eqstra.TechnicalInspection.UILogic.WindowsPhone.Services;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Windows.Storage;
using Windows.System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
namespace Eqstra.TechnicalInspection.UILogic.WindowsPhone.ViewModels
{
    public class MainPageViewModel : ViewModel
    {
        public INavigationService _navigationService;
        private ITaskService _taskService;
        public MainPageViewModel(INavigationService navigationService, ITaskService taskService)
        {
            this._navigationService = navigationService;
            this._taskService = taskService;

            this.PoolofTasks = new ObservableCollection<TITask>();

            this.NextPageCommand = new DelegateCommand<Eqstra.BusinessLogic.Portable.TIModels.Task>((task) =>
            {
                try
                {
                    string serializedTask = JsonConvert.SerializeObject(task);
                    ApplicationData.Current.RoamingSettings.Values[Constants.SELECTEDTASK] = serializedTask;
                    if (task != null)
                    {
                        _navigationService.Navigate("TechnicalInspection", serializedTask);
                    }
                }
                catch (Exception ex)
                {
                }
                finally
                {

                }
            }, (task) =>
            {
                return (this.InspectionTask != null);
            }
             );

            this.RefreshTaskCommand = DelegateCommand.FromAsyncHandler(async () =>
            {

                await this.FetchTasks();
            });


            this.MakeCallCommand = DelegateCommand.FromAsyncHandler(async () =>
            {
                if (!String.IsNullOrEmpty(this.InspectionTask.CustPhone))
                {
                    await Launcher.LaunchUriAsync(new Uri("callto:" + this.InspectionTask.CustPhone));
                }
                else
                {
                    await new MessageDialog("No phone number exist").ShowAsync();
                }
            }, () =>
            {
                return (this.InspectionTask != null && !string.IsNullOrEmpty(this.InspectionTask.CustPhone));
            });

            this.MailToCommand = DelegateCommand.FromAsyncHandler(async () =>
            {
                if (!String.IsNullOrEmpty(this.InspectionTask.CustEmailId))
                {
                    await Launcher.LaunchUriAsync(new Uri("mailto:" + this.InspectionTask.CustEmailId));
                }
                else
                {
                    await new MessageDialog("No mail id exist").ShowAsync();
                }
            }, () => { return (this.InspectionTask != null && !string.IsNullOrEmpty(this.InspectionTask.CustEmailId)); });


            this.LocateCommand = DelegateCommand.FromAsyncHandler(async () =>
            {
                if (!String.IsNullOrEmpty(this.InspectionTask.Address))
                {
                    await Windows.System.Launcher.LaunchUriAsync(new Uri("bingmaps:?where=" + Regex.Replace(this.InspectionTask.Address, "\n", ",")));
                }
                else
                {
                    await new MessageDialog("No address exist").ShowAsync();
                }
            }, () =>
            {
                return (this.InspectionTask != null && !string.IsNullOrEmpty(this.InspectionTask.Address));
            });
        }

        private Visibility taskProgressBar;
        public Visibility TaskProgressBar
        {
            get { return taskProgressBar; }
            set
            {
                SetProperty(ref taskProgressBar, value);
            }
        }

        private Eqstra.BusinessLogic.Portable.TIModels.TITask task;
        public Eqstra.BusinessLogic.Portable.TIModels.TITask InspectionTask
        {
            get { return task; }
            set
            {
                SetProperty(ref task, value);



            }
        }

        private ObservableCollection<TITask> poolofTasks;
        public ObservableCollection<TITask> PoolofTasks
        {
            get { return poolofTasks; }
            set
            {
                SetProperty(ref poolofTasks, value);
            }
        }

        public async override void OnNavigatedTo(object navigationParameter, Windows.UI.Xaml.Navigation.NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            try
            {
                base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);

                if (ApplicationData.Current.RoamingSettings.Values.ContainsKey(Constants.USERINFO))
                {
                    this.UserInfo = JsonConvert.DeserializeObject<Eqstra.BusinessLogic.Portable.TIModels.UserInfo>(ApplicationData.Current.RoamingSettings.Values[Constants.USERINFO].ToString());
                }

                if ((PersistentData.Instance.PoolofTasks != null && PersistentData.Instance.PoolofTasks.Any()))
                {
                    this.PoolofTasks = PersistentData.Instance.PoolofTasks;
                }
                await FetchTasks();
            }
            catch (Exception)
            {
                this.TaskProgressBar = Visibility.Collapsed;

            }

        }
        public async System.Threading.Tasks.Task FetchTasks()
        {
            this.TaskProgressBar = Visibility.Visible;
            ObservableCollection<TITask> poolofTask = new ObservableCollection<TITask>();
            var tasksResult = await this._taskService.GetTasksAsync(this.UserInfo.UserId, this.UserInfo.CompanyId);
            if (tasksResult != null)
            {
                foreach (var task in tasksResult)
                {
                    task.Address = Regex.Replace(task.Address, ",", "\n");
                    poolofTask.Add(task);
                }
            }

            this.PoolofTasks = poolofTask;
            this.TaskProgressBar = Visibility.Collapsed;

            PersistentData.Instance.PoolofTasks = this.PoolofTasks;
        }
        public Eqstra.BusinessLogic.Portable.TIModels.UserInfo UserInfo { get; set; }
        public DelegateCommand RefreshTaskCommand { get; set; }

        public DelegateCommand MailToCommand { get; set; }

        public DelegateCommand MakeIMCommand { get; set; }

        public DelegateCommand LocateCommand { get; set; }
        public ICommand NextPageCommand { get; set; }
        public DelegateCommand MakeCallCommand { get; set; }

    }
}
