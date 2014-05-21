using Microsoft.Practices.Prism.StoreApps;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eqstra.BusinessLogic.Base
{
    abstract public class VIBase : ValidatableBindableBase
    {
        private string caseNumber;
        [SQLite.Column("CaseNumber"), PrimaryKey]
        public string CaseNumber
        {
            get { return caseNumber; }
            set { caseNumber = value; }
        }
        public abstract System.Threading.Tasks.Task<VIBase> GetDataAsync(string caseNumber);
    }
}
