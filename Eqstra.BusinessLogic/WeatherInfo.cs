using Microsoft.Practices.Prism.StoreApps;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eqstra.BusinessLogic
{
    public class WeatherInfo : BindableBase
    {
        private int id;
        [PrimaryKey, Unique]
        public int Id
        {
            get { return id; }
            set { SetProperty(ref id, value); }
        }

        private string cloudCover;

        public string CloudCover
        {
            get { return cloudCover; }
            set { SetProperty(ref cloudCover, value); }
        }

        private string humidity;

        public string Humidity
        {
            get { return humidity; }
            set { SetProperty(ref humidity, value); }
        }

        private string temp_C;

        public string Temp_C
        {
            get { return temp_C; }
            set { SetProperty(ref temp_C, value); }
        }

        private string temp_F;

        public string Temp_F
        {
            get { return temp_F; }
            set { SetProperty(ref temp_F, value); }
        }

        private string weatherIconUrl;

        public string WeatherIconUrl
        {
            get { return weatherIconUrl; }
            set { SetProperty(ref weatherIconUrl, value); }
        }

        private string weatherDesc;

        public string WeatherDesc
        {
            get { return weatherDesc; }
            set { SetProperty(ref weatherDesc, value); }
        }

        private string precipMM;

        public string PrecipMM
        {
            get { return precipMM; }
            set { SetProperty(ref precipMM, value); }
        }

    }
}
