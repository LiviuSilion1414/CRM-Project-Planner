using System.ComponentModel.DataAnnotations;

namespace PlannerCRM.Shared.Attributes;

public class MinimumHourlyRateAttribute : ValidationAttribute
{
    private int _MinimumHourlyRate { get; set; }

    public MinimumHourlyRateAttribute(int minimumHourlyRate) {
        _MinimumHourlyRate = minimumHourlyRate;
    }

    public override bool IsValid(object value) {
        if (value is null) return false;

        if (value.GetType() == typeof(decimal)) {
            var hourlyRate = (decimal)value;

            return hourlyRate >= _MinimumHourlyRate;
        } else {
            return false;
        }
    }
}