using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Delivery3Admin.Controllers;

[AllowAnonymous]
[Route("web/[controller]/[action]")]
[ApiController]
public class AuthController : Controller
{
    private readonly string _apiBaseUrl;
    private readonly IHttpClientFactory _clientFactory;

    public AuthController(IHttpClientFactory clientFactory, IConfiguration configuration)
    {
        _clientFactory = clientFactory;
        _apiBaseUrl = configuration["ApiBaseUrl"] ?? throw new InvalidOperationException();
    }

    [HttpGet]
    public async Task<IActionResult> SignIn()
    {
        HttpClient client = _clientFactory.CreateClient();
        HttpResponseMessage response = await client.PostAsJsonAsync($"{_apiBaseUrl}/v1/auth/Admin", new {});
        if (!response.IsSuccessStatusCode)
            return await Logout();

        var bearerToken = await response.Content.ReadAsStringAsync();
        if (string.IsNullOrEmpty(bearerToken))
            return await Logout();
        
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        ClaimsIdentity claimsIdentity = new(new[]
        {
            new Claim(ClaimTypes.Sid, bearerToken),
        }, CookieAuthenticationDefaults.AuthenticationScheme);
        
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity),
            new AuthenticationProperties { ExpiresUtc = DateTime.Now.AddHours(6), IsPersistent = false });

        return LocalRedirectPermanent("/");
    }

    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectPermanent("/signin");
    }
    
    [HttpGet]
    public async Task<IActionResult> Rewrite()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectPermanent("http://localhost:5020/");
    }
}