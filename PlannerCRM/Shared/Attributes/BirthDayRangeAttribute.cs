using System.ComponentModel.DataAnnotations;
using static PlannerCRM.Shared.Constants.ConstantValues;

namespace PlannerCRM.Shared.Attributes;

public class BirthDayRangeAttribute : ValidationAttribute
{
    public override bool IsValid(object value) {
        if (value.GetType() == typeof(DateTime)) { 
            var birthDay = Convert.ToDateTime(value).Year;
        
            return ((birthDay >= MAJOR_AGE) || (birthDay <= MAX_AGE)) 
                ? true 
                : false;
        } else {
            return false;
        }
    }
}