using Eqstra.BusinessLogic.ServiceSchedule;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;
using Windows.Storage;
using Windows.UI.Notifications;

namespace Eqstra.BusinessLogic.Helpers
{
    public static class Util
    {
        public static void ShowToast(string message)
        {
            var toastXmlString = string.Format("<toast><visual version='1'><binding template='ToastText01'><text id='1'>{0}</text></binding></visual></toast>", message);
            var xmlDoc = new Windows.Data.Xml.Dom.XmlDocument();
            xmlDoc.LoadXml(toastXmlString);
            var toast = new ToastNotification(xmlDoc);
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }

        async public static System.Threading.Tasks.Task WriteTasksToDiskAsync(string content, string fileName)
        {
            StorageFile itemsSourceFile = await ApplicationData.Current.TemporaryFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(itemsSourceFile, content);
        }

        async public static System.Threading.Tasks.Task<ObservableCollection<Eqstra.BusinessLogic.Task>> ReadTasksFromDiskAsync(string fileName)
        {
            try
            {
                ObservableCollection<Task> tasks = null;
                var itemsSourceFile = await ApplicationData.Current.TemporaryFolder.TryGetItemAsync(fileName) as StorageFile;
                if (itemsSourceFile != null)
                {
                    var content = await FileIO.ReadTextAsync(itemsSourceFile);
                    tasks = JsonConvert.DeserializeObject<ObservableCollection<Eqstra.BusinessLogic.Task>>(content);
                }
                return tasks;
            }
            catch (Exception)
            {
                return null;
            }
        }



        async public static System.Threading.Tasks.Task WriteDeliveryTaskToDiskAsync(string content, string fileName)
        {
            StorageFile itemsSourceFile = await ApplicationData.Current.TemporaryFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(itemsSourceFile, content);
        }

        async public static System.Threading.Tasks.Task<ObservableCollection<Eqstra.BusinessLogic.CollectDeliveryTask>> ReadDeliveryTaskFromDiskAsync(string fileName)
        {
            try
            {
                ObservableCollection<BusinessLogic.CollectDeliveryTask> tasks = null;
                var itemsSourceFile = await ApplicationData.Current.TemporaryFolder.TryGetItemAsync(fileName) as StorageFile;
                if (itemsSourceFile != null)
                {
                    var content = await FileIO.ReadTextAsync(itemsSourceFile);
                    tasks = JsonConvert.DeserializeObject<ObservableCollection<Eqstra.BusinessLogic.CollectDeliveryTask>>(content);
                }
                return tasks;
            }
            catch (Exception)
            {
                return null;
            }
        }



        async public static System.Threading.Tasks.Task WriteDriverTasksToDiskAsync(string content, string fileName)
        {
            StorageFile itemsSourceFile = await ApplicationData.Current.TemporaryFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(itemsSourceFile, content);
        }

        async public static System.Threading.Tasks.Task<ObservableCollection<Eqstra.BusinessLogic.ServiceSchedule.DriverTask>> ReadDriverTasksFromDiskAsync(string fileName)
        {
            try
            {
                ObservableCollection<DriverTask> tasks = null;
                var itemsSourceFile = await ApplicationData.Current.TemporaryFolder.TryGetItemAsync(fileName) as StorageFile;
                if (itemsSourceFile != null)
                {
                    var content = await FileIO.ReadTextAsync(itemsSourceFile);
                    tasks = JsonConvert.DeserializeObject<ObservableCollection<Eqstra.BusinessLogic.ServiceSchedule.DriverTask>>(content);
                }
                return tasks;
            }
            catch (Exception)
            {
                return null;
            }
        }



        async public static System.Threading.Tasks.Task WriteSuppliersToDiskAsync(string content, string fileName)
        {
            StorageFile itemsSourceFile = await ApplicationData.Current.TemporaryFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(itemsSourceFile, content);
        }

        async public static System.Threading.Tasks.Task<ObservableCollection<Eqstra.BusinessLogic.ServiceSchedule.Suppliers>> ReadSuppliersFromDiskAsync(string fileName)
        {
            try
            {
                ObservableCollection<Suppliers> tasks = null;
                var itemsSourceFile = await ApplicationData.Current.TemporaryFolder.TryGetItemAsync(fileName) as StorageFile;
                if (itemsSourceFile != null)
                {
                    var content = await FileIO.ReadTextAsync(itemsSourceFile);
                    tasks = JsonConvert.DeserializeObject<ObservableCollection<Eqstra.BusinessLogic.ServiceSchedule.Suppliers>>(content);
                }
                return tasks;
            }
            catch (Exception)
            {
                return null;
            }
        }

    }
}
