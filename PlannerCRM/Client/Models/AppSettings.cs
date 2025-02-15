namespace PlannerCRM.Shared.Models;

public class AppSettings
{
    public string EmailHost { get; set; }
    public string EmailPort { get; set; }
    public string EnableSSL { get; set; }
    public string EmailCredentialUsername { get; set; }
    public string EmailCredentialPassword { get; set; }
    public string EmailFrom { get; set; }
    public string EmailDisplayNameFrom { get; set; }
    public string[] EmailTo { get; set; }
    public string[] EmailBCC { get; set; }

}
