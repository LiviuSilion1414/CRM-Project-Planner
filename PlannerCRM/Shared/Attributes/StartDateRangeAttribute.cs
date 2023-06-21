using System.ComponentModel.DataAnnotations;
using static PlannerCRM.Shared.Constants.ConstantValues;

namespace PlannerCRM.Shared.Attributes;

public class StartDateRangeAttribute : ValidationAttribute
{
    public override bool IsValid(object value) {
        if (value is null) return false;
        
        if (value.GetType() == typeof(DateTime) ) {
            var startDate = Convert.ToDateTime(value);
            
            return startDate <= CURRENT_DATE;
        } else {
            return false;
        }
    }
}