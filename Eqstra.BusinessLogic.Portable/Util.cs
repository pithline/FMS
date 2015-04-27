using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Notifications;
using System.Linq;
namespace Eqstra.BusinessLogic.Portable
{
    public static class Util
    {

        async public static System.Threading.Tasks.Task WriteToDiskAsync<T>(T obj, string fileName)
        {
            string content = JsonConvert.SerializeObject(obj);
            StorageFile itemsSourceFile = await ApplicationData.Current.TemporaryFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(itemsSourceFile, content);

        }

        async public static System.Threading.Tasks.Task<T> ReadFromDiskAsync<T>(string fileName)
        {
            try
            {
                string content = string.Empty;
                var itemsSourceFile = await ApplicationData.Current.TemporaryFolder.GetItemAsync(fileName) as StorageFile;
                if (itemsSourceFile != null)
                {
                    content = await FileIO.ReadTextAsync(itemsSourceFile);
                }
                return JsonConvert.DeserializeObject<T>(content);

            }
            catch (Exception)
            {
                return default(T);
            }
        }


    }
}
