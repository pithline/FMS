using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eqstra.DataProvider.AX.SSModels
{
    public class Task
    {
        public DateTime ModelYear { get; set; }
        public string CaseNumber { get; set; }
        public string CusEmailId { get; set; }
        public string CaseCategory { get; set; }
        public string UserId { get; set; }
        public string Address { get; set; }
        public DateTime ConfirmedTime { get; set; }
        public string CaseType { get; set; }
        public DateTime ConfirmedDate { get; set; }
        public long VehicleInsRecId { get; set; }
        public DateTime StatusDueDate { get; set; }
        public string Status { get; set; }
        public string AllocatedTo { get; set; }
        public DateTime ScheduledDate { get; set; }
        public string CustomerId { get; set; }
        public string registrationNumber { get; set; }
       
    }
}
