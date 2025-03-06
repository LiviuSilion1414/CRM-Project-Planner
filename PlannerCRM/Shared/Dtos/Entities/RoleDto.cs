namespace PlannerCRM.Shared.Dtos.Entities;

public class RoleDto
{
    public Guid  Guid { get; set; }
    public Roles RoleName { get; set; }
}

public class RoleFilterDto : FilterDto
{
    public Guid RoleId { get; set; }
}