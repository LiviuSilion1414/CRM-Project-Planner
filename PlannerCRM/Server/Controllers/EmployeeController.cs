namespace PlannerCRM.Server.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class EmployeeController(IRepository<Employee, EmployeeDto> repo) : CrudController<Employee, EmployeeDto>(repo)
{ }