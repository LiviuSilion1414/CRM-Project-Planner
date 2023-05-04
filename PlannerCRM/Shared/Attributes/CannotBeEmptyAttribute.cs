using System.ComponentModel.DataAnnotations;
using PlannerCRM.Shared.DTOs.ActivityDto.Forms;

namespace PlannerCRM.Shared.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public sealed class CannotBeEmptyAttribute : RequiredAttribute
{
    public override bool IsValid(object value) {
        var list = value as IEnumerable<EmployeeActivityDto>; 
        return list != null && list.GetEnumerator().MoveNext()
            ? true
            : false;
    }
}