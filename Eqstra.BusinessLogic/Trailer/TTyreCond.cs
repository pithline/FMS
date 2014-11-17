using Eqstra.BusinessLogic.Base;
using Eqstra.BusinessLogic.Helpers;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eqstra.BusinessLogic
{
    public class TTyreCond : BaseModel
    {

        private int tyreCondID;
        [PrimaryKey,AutoIncrement]
        public int TyreCondID
        {
            get { return tyreCondID; }
            set { SetProperty(ref tyreCondID, value); }
        }

        private long vehicleInsRecID;

        public new long VehicleInsRecID
        {
            get { return vehicleInsRecID; }
            set { SetProperty(ref vehicleInsRecID, value); }
        }


        private string position;

        public string Position
        {
            get { return position; }
            set { SetProperty(ref position, value); }
        }

        private string comment;

        public string Comment
        {
            get { return comment; }
            set { SetProperty(ref comment, value); }
        }
        private string threadDepth;

        public string ThreadDepth
        {
            get { return threadDepth; }
            set { SetProperty(ref threadDepth, value); }
        }

        private string make;

        public string Make
        {
            get { return make; }
            set { SetProperty(ref make, value); }
        }

        private bool isFair;

        public bool IsFair
        {
            get { return isFair; }
            set { SetProperty(ref isFair, value); }
        }

        private bool isGood;

        public bool IsGood
        {
            get { return isGood; }
            set { SetProperty(ref isGood, value); }
        }
        private bool isPoor;
        public bool IsPoor
        {
            get { return isPoor; }
            set { SetProperty(ref isPoor, value); }
        }

        public async override Task<BaseModel> GetDataAsync(long vehicleInsRecID)
        {
            return await SqliteHelper.Storage.GetSingleRecordAsync<TTyreCond>(x => x.VehicleInsRecID == vehicleInsRecID);
        }
    }
}
