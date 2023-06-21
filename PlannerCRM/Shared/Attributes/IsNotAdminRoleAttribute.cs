using System.ComponentModel.DataAnnotations;
using PlannerCRM.Shared.Models;

namespace PlannerCRM.Shared.Attributes;

public class IsNotAdminRoleAttribute : ValidationAttribute
{
    private Roles _adminRole { get; set; }

    public IsNotAdminRoleAttribute(Roles adminRole) {
        _adminRole = adminRole;
    }

    public override bool IsValid(object value) {
        if (value == null) {
            return false;
        }
        if (value.GetType() == typeof(Roles)) {
            var isInAdminRole = value.GetType()
                .GetProperties()
                .Any(role => role.Name == nameof(Roles.ACCOUNT_MANAGER));
            
            if (!isInAdminRole) {
                return true;
            } else {
                return false;
            }
        } else {
            return false;
        }
    }
}