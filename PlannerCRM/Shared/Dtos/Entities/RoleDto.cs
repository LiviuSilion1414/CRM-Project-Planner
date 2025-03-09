namespace PlannerCRM.Shared.Dtos.Entities;

public class RoleDto
{
    public Guid id { get; set; }
    public string roleName { get; set; }
}

public class RoleFilterDto : FilterDto
{
    public Guid roleId { get; set; }
}