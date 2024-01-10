namespace PlannerCRM.Server.Models;

public class EmployeeProfilePicture
{
    public int Id { get; set; }
    public string ImageType { get; set; }

    public string? Thumbnail { get; set; }

    public string? ImageUrl { get; set; }

    public virtual Employee? EmployeeInfo { get; set; }

    public int EmployeeId { get; set; }
}