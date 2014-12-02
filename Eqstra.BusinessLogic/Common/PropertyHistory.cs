using Eqstra.BusinessLogic.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Eqstra.BusinessLogic.Common
{
    public class PropertyHistory
    {

        private static readonly PropertyHistory instance = new PropertyHistory();
        public Dictionary<string, object> StorageHistory = new Dictionary<string, object>();
        public static PropertyHistory Instance
        {
            get
            {
                return instance;
            }
        }
        public void SetPropertyHistory(BaseModel baseModel)
        {

            try
            {
                StorageHistory.Clear();
                TypeInfo typeInfo = baseModel.GetType().GetTypeInfo();
                IEnumerable<PropertyInfo> propertyInfoList = typeInfo.DeclaredProperties;
                propertyInfoList.AsParallel().ForAll(propInfo =>
                {
                    object value = propInfo.GetValue(baseModel);
                    StorageHistory.Add(propInfo.Name, value);
                });
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool IsPropertyOriginalValueChanged(object context)
        {
            try
            {
                TypeInfo typeInfo = context.GetType().GetTypeInfo();
                IEnumerable<PropertyInfo> propertyInfoList = typeInfo.DeclaredProperties;

                foreach (var propInfo in propertyInfoList)
                {
                    string currentValue = Convert.ToString(propInfo.GetValue(context));
                    object originalvalue;
                    StorageHistory.TryGetValue(propInfo.Name, out originalvalue);
                    if (!currentValue.Equals(Convert.ToString(originalvalue)))
                    {
                        return true;

                    }
                }
                return false;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
