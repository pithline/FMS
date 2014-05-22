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
        [Display(Name = "Await Inspection Data Capture")]
        AwaitInspectionDataCapture,
        [Display(Name = "Await Inspection Confirmation ")]
        AwaitingConfirmation,
        [Display(Name = "Await Inspection Acceptance")]
        AwaitInspectionAcceptance,
        [Display(Name = "Awaiting Inspection")]
        AwaitingInspection,
        [Display(Name = "Await Damage Confirmation")]
        AwaitDamageConfirmation,
        InProgress,
        Completed
    }
}
