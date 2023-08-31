namespace PlannerCRM.Shared.Attributes;

public class IsNotAdminRoleAttribute : ValidationAttribute
{
    private readonly Roles _adminRole;

    public IsNotAdminRoleAttribute(Roles adminRole) {
        _adminRole = adminRole;
    }

    public IsNotAdminRoleAttribute()
    { }

    public override bool IsValid(object value) {
        if (value is null) return false;

        if (value.GetType() == typeof(Roles)) {
            var isInAdminRole = value.GetType()
                .GetProperties()
                .Any(role => role.Name == nameof(_adminRole));
            
            return !isInAdminRole;
        }
    
        return false;
    }
}