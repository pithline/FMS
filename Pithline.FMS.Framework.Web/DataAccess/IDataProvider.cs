﻿using System.Collections;

namespace Eqstra.Framework.Web.DataAccess
{
    public interface IDataProvider
    {
        IList GetDataList(object[] criterias);
        object GetSingleData(object[] criterias);
        bool DeleteData(object[] criterias);
        object SaveData(object[] criterias);
        object GetService();
    }
}
