using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Windows.ApplicationModel;
using Windows.Storage;
using Windows.UI.Xaml.Markup;

namespace Eqstra.BusinessLogic.Helpers
{

    public class AddressHelper
    {
        public async static Task<Catalog> GetAddressCatalog()
        {
            var catalog = new Catalog();

            StorageFile packFile = await Package.Current.InstalledLocation.GetFileAsync("GeoNameConfig\\GeoNamesData.xml");
            var ms = await packFile.OpenStreamForReadAsync().ConfigureAwait(false);
            var xml = new XmlSerializer(typeof(Catalog));
            catalog = xml.Deserialize(ms.AsInputStream().AsStreamForRead()) as Catalog;
            return catalog;
        }
    }
    public class Catalog
    {
        public Country Country { get; set; }
    }
    public class Country
    {
        public string title { get; set; }
        public List<Province> Provinces { get; set; }
    }
    public class Province
    {
        public string title { get; set; }
        public List<City> Cities { get; set; }
    }

    public class City
    {
        public string title { get; set; }
        public List<Suburb> Suburbs { get; set; }
    }

    public class Suburb
    {
        public string title { get; set; }
    }




}
