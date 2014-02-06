using Eqstra.BusinessLogic.Enums;
using Microsoft.Practices.Prism.StoreApps;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eqstra.BusinessLogic
{
    [Table("Vehicle")]
    public class Vehicle : ValidatableBindableBase
    {
        
        private string registrationNumber;
        [SQLite.Column("RegistrationNumber"), PrimaryKey, SQLite.Unique, ]
        public string RegistrationNumber
        {
            get { return registrationNumber; }
            set { SetProperty(ref registrationNumber, value); }
        }

        private string color;
        [Column("Color")]
        public string Color
        {
            get { return color; }
            set { SetProperty(ref color, value); }
        }

        private string chassisNumber;
        [Column("ChassisNumber"),]
        public string ChassisNumber
        {
            get { return chassisNumber; }
            set { SetProperty(ref chassisNumber, value); }
        }


        private DateTime modelYear;
        [Column("ModelYear")]
        public DateTime ModelYear
        {
            get { return modelYear; }
            set { SetProperty(ref modelYear, value); }
        }

        private VehicleTypeEnum vehicleType;
        [Column("VehicleType")]
        public VehicleTypeEnum VehicleType
        {
            get { return vehicleType; }
            set { SetProperty(ref vehicleType, value); }
        }

    }
}
