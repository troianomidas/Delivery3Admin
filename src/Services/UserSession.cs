using System.Security.Claims;

namespace Delivery3Admin.Services;

public class UserSession
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserSession(IHttpContextAccessor httpContextAccessor) => _httpContextAccessor = httpContextAccessor;

    public string? BearerToken => _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Sid);
    public string? User =>_httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Name);
    public string? Email =>_httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Email);
}