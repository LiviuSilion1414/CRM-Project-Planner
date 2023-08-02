namespace PlannerCRM.Shared.Attributes;

public class BirthDayRangeAttribute : ValidationAttribute
{
    private int Minimum { get; set; }
    private int Maximum { get; set; }

    public BirthDayRangeAttribute(int minimum, int maximum) {
        Minimum = minimum;
        Maximum = maximum;
    }

    public BirthDayRangeAttribute()
    { }

    public override bool IsValid(object value) {
        if (value is null) return false;
        
        if (value.GetType() == typeof(DateTime)) { 
            var date = Convert.ToDateTime(value);
            var totalYears = CURRENT_DATE.Year - date.Year;
        
            return (((totalYears >= Minimum) && (totalYears <= Maximum)) && (date <= CURRENT_DATE));
        } else {
            return false;
        }
    }
}