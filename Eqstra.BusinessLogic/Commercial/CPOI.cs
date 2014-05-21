using Eqstra.BusinessLogic.Base;
using Eqstra.BusinessLogic.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eqstra.BusinessLogic.Commercial
{
    public class CPOI : VIBase
    {
     
            public async override Task<VIBase> GetDataAsync(string caseNumber)
            {
                return await SqliteHelper.Storage.GetSingleRecordAsync<CPOI>(x => x.CaseNumber == caseNumber);
            }
            public CPOI()
            {
                this.CRTime = DateTime.Now;
                this.CRDate = DateTime.Today;
                this.EQRDate = DateTime.Today;
                this.EQRTime = DateTime.Now;
                this.CRSignFileName = "cr_" + new Random().Next(1000) + TimeSpan.TicksPerMillisecond;
                this.EQRSignFileName = "eqr_" + new Random().Next(1000) + TimeSpan.TicksPerMillisecond;
            }

            private string crSignFileName;

            public string CRSignFileName
            {
                get { return crSignFileName; }
                set { SetProperty(ref crSignFileName, value); }
            }


            private string eqrSignFileName;

            public string EQRSignFileName
            {
                get { return eqrSignFileName; }
                set { SetProperty(ref eqrSignFileName, value); }
            }

            private DateTime cRDate;
            public DateTime CRDate
            {
                get { return cRDate; }
                set { SetProperty(ref cRDate, value); }
            }

            private DateTime cRTime;
            public DateTime CRTime
            {
                get { return cRTime; }
                set { SetProperty(ref cRTime, value); }
            }

            private DateTime eQRDate;
            public DateTime EQRDate
            {
                get { return eQRDate; }
                set { SetProperty(ref eQRDate, value); }
            }

            private DateTime eQRTime;
            public DateTime EQRTime
            {
                get { return eQRTime; }
                set { SetProperty(ref eQRTime, value); }
            }
        }
    }

