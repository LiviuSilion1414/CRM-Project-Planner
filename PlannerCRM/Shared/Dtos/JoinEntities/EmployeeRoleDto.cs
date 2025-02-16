namespace PlannerCRM.Shared.Dtos.JoinEntities;

public class EmployeeRoleDto
{
    public Guid  Guid { get; set; }

    public string RoleName { get; set; }
    public Guid EmployeeId { get; set; }
    public Guid RoleId { get; set; }

    //rimosso per la ciclazione continua
    // Navigation properties
    //public EmployeeDto Employee { get; set; }
    //public RoleDto Role { get; set; }
}
