namespace PlannerCRM.Client.Services.Utilities.Validation;

public class AuthenticationStateService : AuthenticationStateProvider
{
    private readonly CurrentUserInfoService _authInfoService;
    private readonly ILogger<AuthenticationStateService> _logger;
    private CurrentUser _currentUser;

    public AuthenticationStateService(
        CurrentUserInfoService authInfoService, 
        ILogger<AuthenticationStateService> logger) 
    {
        _authInfoService = authInfoService;
        _logger = logger;
        _currentUser = new();
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync() {
        try {
            var identity = new ClaimsIdentity();
           
            _currentUser = await GetCurrentUserAsync();
            if (_currentUser.IsAuthenticated) {
                var claims = new List<Claim> { new(ClaimTypes.Name, _currentUser.UserName) }
                    .Concat(_currentUser.Claims
                        .Select(c => new Claim(c.Key, c.Value))
                    );
               
                identity = new ClaimsIdentity(claims, "Server authentication");

                return new AuthenticationState(new ClaimsPrincipal(identity));
            } else {
                identity = new ClaimsIdentity(new List<Claim> {
                    new(ClaimTypes.Name, "Anonymous")
                });

                return new AuthenticationState(new ClaimsPrincipal(identity));
            }
        } catch (Exception exc) {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);
            
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }
    }

    public async Task<CurrentUser> GetCurrentUserAsync() {
        try {
            _currentUser = await _authInfoService.GetCurrentUserInfoAsync();
            
            return _currentUser is not null ? _currentUser : new();
        } catch (Exception exc) {
            _logger.LogError("\nError: {0} \n\nMessage: {1}", exc.StackTrace, exc.Message);

            return new();
        }
    }
}