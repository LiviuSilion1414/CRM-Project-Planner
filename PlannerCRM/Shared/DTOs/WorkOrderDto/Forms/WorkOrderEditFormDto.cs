using System.ComponentModel.DataAnnotations;
using PlannerCRM.Shared.Attributes;
using static PlannerCRM.Shared.Constants.ConstantValues;

namespace PlannerCRM.Shared.DTOs.Workorder.Forms;

public partial class WorkOrderEditFormDto
{
    public int Id { get; set; }

    [Editable(allowEdit: true, AllowInitialValue = true)]
    public string Name { get; set; }
    
    [Editable(allowEdit: true, AllowInitialValue = true)]
    public DateTime StartDate { get; set; }
    
    [Editable(allowEdit: true, AllowInitialValue = true)]
    public DateTime FinishDate { get; set; }
}