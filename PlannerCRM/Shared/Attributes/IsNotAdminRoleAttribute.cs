namespace PlannerCRM.Shared.Attributes;

public class IsNotAdminRoleAttribute : ValidationAttribute
{
    private Roles AdminRole { get; set; }

    public IsNotAdminRoleAttribute(Roles adminRole) {
        AdminRole = adminRole;
    }

    public IsNotAdminRoleAttribute()
    { }

    public override bool IsValid(object value) {
        if (value is null) return false;

        if (value.GetType() == typeof(Roles)) {
            var isInAdminRole = value.GetType()
                .GetProperties()
                .Any(role => role.Name == nameof(AdminRole));
            
            return !isInAdminRole;
        } else {
            return false;
        }
    }
}