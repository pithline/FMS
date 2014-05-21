using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eqstra.BusinessLogic.ServiceSchedule
{
  public  class DriverTask : Task
    {
        private string registrationNumber;        
        public  string RegistrationNumber
        {
            get { return registrationNumber; }
            set { SetProperty(ref registrationNumber, value); }
        }

        private string description;

        public string Description
        {
            get { return description; }
            set { SetProperty(ref description, value); }
        }

        private string make;

        public string Make
        {
            get { return make; }
            set { SetProperty(ref make, value); }
        }

        private DateTime modelYear;

        public DateTime ModelYear
        {
            get { return modelYear; }
            set { SetProperty(ref modelYear, value); }
        }

    }
}
