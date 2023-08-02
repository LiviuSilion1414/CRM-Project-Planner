namespace PlannerCRM.Shared.Attributes;

public class WorkOrderFinishDateRangeAttribute : ValidationAttribute
{
    private int Minimum { get; set; }
    private int Maximum { get; set; }

    public WorkOrderFinishDateRangeAttribute(int minimum, int maximum) {
        Minimum = minimum;
        Maximum = maximum;
    }

    public WorkOrderFinishDateRangeAttribute() 
    { }

    public override bool IsValid(object value) {
        if (value is null) return false;

        if (value.GetType() == typeof(DateTime)) {
            var date = Convert.ToDateTime(value);
            if (date > CURRENT_DATE) {
                if ((date.Month >= Minimum) && (date.Month <= Maximum)) {
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