namespace PlannerCRM.Server.Models;

public class EmployeeProfilePicture
{
    public int Id { get; set; }
    public string ImageType { get; set; } = "image/*";

    public string Thumbnail { get; set; } = string.Empty;

    public Employee EmployeeInfo { get; set; }

    public int EmployeeId { get; set; }
}