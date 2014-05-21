using Eqstra.BusinessLogic.Base;
using Eqstra.BusinessLogic.Helpers;
using Microsoft.Practices.Prism.StoreApps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eqstra.BusinessLogic.Passenger
{
    public class PMechanicalCond : VIBase
    {
        private string remarks;
        public string Remarks
        {
            get { return remarks; }

            set { SetProperty(ref  remarks, value); }
        }


        public async override Task<VIBase> GetDataAsync(string caseNumber)
        {
            return await SqliteHelper.Storage.GetSingleRecordAsync<PMechanicalCond>(x => x.CaseNumber == caseNumber);
        }
    }
}
