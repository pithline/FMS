using Microsoft.Practices.Prism.StoreApps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace Eqstra.VehicleInspection.UILogic
{
    public class AppSettings : BindableBase
    {
        private static readonly AppSettings _instance = new AppSettings();

        public AppSettings()
        {
            
        }

        public static AppSettings Instance { get { return _instance; } }

        private int isSynchronizing;

        public int IsSynchronizing
        {
            get { return isSynchronizing; }
            set { SetProperty(ref isSynchronizing, value); }
        }


        private string errorMessage;

        public string ErrorMessage
        {
            get { return errorMessage; }
            set { SetProperty(ref errorMessage, value); }
        }


    }
}
