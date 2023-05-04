using System.ComponentModel.DataAnnotations;
using PlannerCRM.Shared.DTOs.ActivityDto.Forms;
using PlannerCRM.Shared.DTOs.EmployeeDto.Forms;

namespace PlannerCRM.Shared.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public sealed class CannotBeEmptyAttribute : RequiredAttribute
{
    public override bool IsValid(object value) {
        if (value.GetType() == typeof(IEnumerable<EmployeeActivityDTO>) || 
            value.GetType() == typeof(IEnumerable<EmployeeSalaryDTO>)) {
            
            var listEA = value as IEnumerable<EmployeeActivityDTO>; 
            var listES = value as IEnumerable<EmployeeSalaryDTO>;
            
            return listEA != null && listEA.GetEnumerator().MoveNext() ||
                listES != null && listES.GetEnumerator().MoveNext()
                ? true
                : false;
        } else {
            return false;
        }
    }
}