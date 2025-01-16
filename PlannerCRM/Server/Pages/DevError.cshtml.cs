namespace PlannerCRM.Server.Pages;

[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
[IgnoreAntiforgeryToken]
public class DevErrorModel : PageModel
{
    public string RequestId { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

    private readonly ILogger<ErrorModel> _logger;

    public DevErrorModel(ILogger<ErrorModel> logger)
    {
        _logger = logger;
    }

    public void OnGet() {
        RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier;
        _logger.LogError("Somethign Went Wrong While Processing Your Request. Try Again Later.");
    }
}
