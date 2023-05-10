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
        if (value.GetType() == typeof(Roles)) {
            if ((Roles) value != Roles.ACCOUNT_MANAGER) {
                return true;
            } else {
                return false;
            }
        } else {
            return false;
        }
    }
}