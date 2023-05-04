using System.ComponentModel.DataAnnotations;
using static PlannerCRM.Shared.Constants.ConstantValues;

namespace PlannerCRM.Shared.Attributes;

public class WorkOrderStartDateRangeAttribute : ValidationAttribute
{
    public override bool IsValid(object value) {
        if (value.GetType() == typeof(DateTime)) {
            var startDate = Convert.ToDateTime(value);
            
            return ((startDate.Month >= MIN_WORKORDER_MONTH_CONTRACT) 
                || (startDate.Month <= MAX_WORKORDER_MONTH_CONTRACT)) 
                ? true 
                : false;
        } else {
            return false;
        }
    }
}