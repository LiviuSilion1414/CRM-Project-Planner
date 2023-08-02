namespace PlannerCRM.Shared.Attributes;

public class MinimumHourlyRateAttribute : ValidationAttribute
{
    private int MinimumHourlyRate { get; set; }

    public MinimumHourlyRateAttribute(int minimumHourlyRate) {
        MinimumHourlyRate = minimumHourlyRate;
    }

    public MinimumHourlyRateAttribute() 
    { }

    public override bool IsValid(object value) {
        if (value is null) return false;

        if (value.GetType() == typeof(decimal)) {
            var hourlyRate = (decimal)value;

            return hourlyRate >= MinimumHourlyRate;
        } else {
            return false;
        }
    }
}