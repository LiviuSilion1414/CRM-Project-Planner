using System.ComponentModel.DataAnnotations;
using static PlannerCRM.Shared.Constants.ConstantValues;

namespace PlannerCRM.Shared.Attributes;

public class FinishDateRangeHourlyRateAttribute : ValidationAttribute
{

    public override bool IsValid(object value) {
        if (value is null) return false;

        if (value.GetType() == typeof(DateTime)) {
            var finishDate = Convert.ToDateTime(value);
            return finishDate >= CURRENT_DATE;
        } else {
            return false;
        }
    }
}