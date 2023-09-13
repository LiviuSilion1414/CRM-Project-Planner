namespace PlannerCRM.Client.Services.Utilities.Filtering;

public class FilterService<IDtoComparer>
{
    private readonly HttpClient _http;
    private readonly ILogger<FilterService<IDtoComparer>> _logger;

    public FilterService(HttpClient http, ILogger<FilterService<IDtoComparer>> logger) {
        _http = http;
        _logger = logger;
    }

    public IEnumerable<IDtoComparer> FilterBy(
        IEnumerable<IDtoComparer> collection, 
        Func<IDtoComparer, bool> filter) 
    {
        try {
            return collection
                .Where(item => filter(item))
                .ToList();
        } catch (Exception exc) {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            throw;
        }
    }
}