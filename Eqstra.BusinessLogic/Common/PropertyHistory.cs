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
                foreach (var propInfo in propertyInfoList)
                {
                    object value = propInfo.GetValue(baseModel);
                    StorageHistory.Add(propInfo.Name, value);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public bool IsProperiesValuesChanged(object context)
        {

            TypeInfo typeInfo = context.GetType().GetTypeInfo();
            bool result = false;
            IEnumerable<PropertyInfo> propertyInfoList = typeInfo.DeclaredProperties;
            foreach (var propInfo in propertyInfoList)
            {
                string value = Convert.ToString(propInfo.GetValue(context));

                object origanlvalue;
                StorageHistory.TryGetValue(propInfo.Name, out origanlvalue);
                string origanlvaluestr = Convert.ToString(origanlvalue);

                if (!String.IsNullOrEmpty(value))
                {
                    if (!value.Equals(origanlvaluestr))
                    {
                        return true;
                    }
                }
            }
            return result;
        }
    }
}
