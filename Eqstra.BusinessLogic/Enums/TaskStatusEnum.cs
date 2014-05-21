using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eqstra.BusinessLogic.Enums
{
    public enum TaskStatusEnum
    {
        [Display(Name = "Awaiting Confirmation")]
        AwaitingConfirmation,
        [Display(Name = "Awaiting Inspection")]
        AwaitingInspection,
        InProgress,
        Completed
    }
}
