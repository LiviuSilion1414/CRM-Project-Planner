using System.ComponentModel.DataAnnotations;
using PlannerCRM.Shared.DTOs.ActivityDto.Forms;
using PlannerCRM.Shared.DTOs.EmployeeDto.Forms;

namespace PlannerCRM.Shared.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public sealed class CannotBeEmptyAttribute : RequiredAttribute
{
    public override bool IsValid(object value) {
        if (value.GetType() == typeof(List<EmployeeSalaryDTO>)) {
            var list = value as List<EmployeeSalaryDTO>;
            list = new();
            
            return list != null || list.GetEnumerator().MoveNext()
                ? true
                : false;
        } else {
            return false;
        }
    }
}