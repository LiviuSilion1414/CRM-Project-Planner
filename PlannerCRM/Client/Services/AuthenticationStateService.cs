namespace PlannerCRM.Client.Services;

public class AuthenticationStateService : AuthenticationStateProvider
{
    private readonly CurrentUserInfoService _authInfoService;
    private CurrentUser _currentUser = new();

    public AuthenticationStateService(CurrentUserInfoService authInfoService) {
        _authInfoService = authInfoService;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync() {
        try {
            var identity = new ClaimsIdentity();
           
            _currentUser = await GetCurrentUserAsync();
            if (_currentUser.IsAuthenticated) {
                var claims = new List<Claim> { 
                    new Claim(ClaimTypes.Name, _currentUser.UserName) 
                }
                .Concat(_currentUser.Claims
                .Select(c => new Claim(c.Key, c.Value)));
               
                identity = new ClaimsIdentity(claims, "Server authentication");

                return new AuthenticationState(new ClaimsPrincipal(identity));
            } else {
                identity = new ClaimsIdentity(new List<Claim> {
                    new Claim(ClaimTypes.Name, "Anonymous")
                });

                return new AuthenticationState(new ClaimsPrincipal(identity));
            }
        } catch (HttpRequestException ex) {
            Console.WriteLine("Request failed:" + ex.ToString());
        } catch (Exception exc) {
            Console.WriteLine("Exception:" + exc.ToString());
        }
        
        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
    }

    public async Task<CurrentUser> GetCurrentUserAsync() {
        try {
            _currentUser = await _authInfoService.GetCurrentUserInfoAsync();
            
            return _currentUser is not null ? _currentUser : new();
        } catch {
            return new();
        }
    }
}