using System.ComponentModel.DataAnnotations;
using static PlannerCRM.Shared.Constants.ConstantValues;

namespace PlannerCRM.Shared.Attributes;

public class WorkOrderFinishDateRangeAttribute : ValidationAttribute
{
    private int _Minimum { get; set; }
    private int _Maximum { get; set; }

    public WorkOrderFinishDateRangeAttribute(int minimum, int maximum) {
        _Minimum = minimum;
        _Maximum = maximum;
    }

    public override bool IsValid(object value) {
        if (value is null) return false;

        if (value.GetType() == typeof(DateTime)) {
            var date = Convert.ToDateTime(value);
            if (date > CURRENT_DATE) {
                if ((date.Month >= _Minimum) && (date.Month <= _Maximum)) {
                    return true;
                } else {
                    return false;
                }
            } else {
                return false;
            }
        } else {
            return false;
        }
    }
}