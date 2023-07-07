namespace PlannerCRM.Shared.Attributes;

public class IsNotAdminRoleAttribute : ValidationAttribute
{
    private Roles _AdminRole { get; set; }

    public IsNotAdminRoleAttribute(Roles adminRole) {
        _AdminRole = adminRole;
    }

    public IsNotAdminRoleAttribute()
    { }

    public override bool IsValid(object value) {
        if (value is null) return false;

        if (value.GetType() == typeof(Roles)) {
            var isInAdminRole = value.GetType()
                .GetProperties()
                .Any(role => role.Name == nameof(_AdminRole));
            
            return !isInAdminRole;
        } else {
            return false;
        }
    }
}