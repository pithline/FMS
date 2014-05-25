using Eqstra.BusinessLogic;
using Eqstra.BusinessLogic.Base;
using Eqstra.BusinessLogic.Passenger;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reflection;
using Eqstra.BusinessLogic.Commercial;

namespace Eqstra.VehicleInspection
{
    public static class VIExtension
    {
        public static void LoadSnapshotsFromDb(this VIBase viBase)
        {

            TypeInfo t = viBase.GetType().GetTypeInfo();
            IEnumerable<FieldInfo> fieldInfoList = t.DeclaredFields;
            IEnumerable<PropertyInfo> propertyInfoList = t.DeclaredProperties;
            foreach (var fieldInfo in fieldInfoList.Where(x => x.Name.Contains("Path")))
            {
                var pathValue = t.GetDeclaredField(fieldInfo.Name).GetValue(viBase);
                if (pathValue != null)
                {
                    var prop = propertyInfoList.First(x => x.Name.ToUpper().Equals(fieldInfo.Name.Replace("Path", "").ToUpper()));
                    if (prop.PropertyType.Equals(typeof(ObservableCollection<ImageCapture>)))
                    {
                        ObservableCollection<ImageCapture> imgListvalue = new ObservableCollection<ImageCapture>();
                        string[] pathlist = pathValue.ToString().Split(new char[]{'~'}, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string path in pathlist)
                        {
                            imgListvalue.Add(new ImageCapture() { ImagePath = path });
                        }
                        prop.SetValue(viBase, imgListvalue);
                    }
                    else if (prop.PropertyType.Equals(typeof(ImageCapture)))
                    {
                        prop.SetValue(viBase, new ImageCapture() { ImagePath = pathValue.ToString() });
                    }
                }
            }
        }
    }
}
