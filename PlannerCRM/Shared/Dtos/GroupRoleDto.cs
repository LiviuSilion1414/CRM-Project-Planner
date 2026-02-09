using System.ComponentModel;

namespace PlannerCRM.Shared.Dtos;
public class GroupRoleDto
{
	public Guid? id { get; set; }

	[Required]
    [MaxLength(50)]
    [Description("Title")]
    public string? title { get; set; }
}
// Filtro Ricerca
public class GroupRolesFilterDto : FilterDto
{
	public Guid? id { get; set; }
	public string? stringaRicerca { get; set; }
}
