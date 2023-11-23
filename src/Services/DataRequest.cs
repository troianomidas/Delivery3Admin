using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;

namespace Delivery3Admin.Services;

public class DataRequest
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    private readonly AuthenticationStateProvider _authenticationStateProvider;
    private readonly NavigationManager _navigationManager;
    private readonly ILogger<DataRequest> _logger;

    public DataRequest(
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration,
        AuthenticationStateProvider authenticationStateProvider,
        NavigationManager navigationManager,
        ILogger<DataRequest> logger)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
        _navigationManager = navigationManager;
        _authenticationStateProvider = authenticationStateProvider;
        _logger = logger;
    }

    public async Task<ApiResponse> SendAsync(HttpMethod method, string action, object? data = null)
    {
        HttpClient client = _httpClientFactory.CreateClient();
        ApiResponse? apiResponse;

        try
        {
            var request = new HttpRequestMessage
            {
                Method = method,
                RequestUri = new Uri($"{_configuration["ApiBaseUrl"]}/v1{action}"),
                Content = data != null
                    ? new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json")
                    : null
            };

            AuthenticationState userSession = await _authenticationStateProvider.GetAuthenticationStateAsync();
            string bearerToken = userSession.User.FindFirstValue(ClaimTypes.Sid) ?? string.Empty;

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", bearerToken);

            HttpResponseMessage response =
                await client.SendAsync(request, new CancellationTokenSource(TimeSpan.FromSeconds(30)).Token);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new UnauthorizedAccessException();

            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                string content = await response.Content.ReadAsStringAsync();

                if (content.StartsWith("System.UnauthorizedAccessException:"))
                    throw new UnauthorizedAccessException(content);
            }

            return new ApiResponse
            {
                Error = response.IsSuccessStatusCode ? null : await response.Content.ReadAsStringAsync(),
                Json = response.IsSuccessStatusCode ? await response.Content.ReadAsStringAsync() : null,
                StatusCode = (int)response.StatusCode
            };
        }
        catch (UnauthorizedAccessException ex)
        {
            apiResponse = new ApiResponse
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                Error = ex.Message
            };

            _navigationManager.NavigateTo("/web/auth/logout", true);
        }
        catch (Exception ex)
        {
            apiResponse = new ApiResponse
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                Error = ex.Message
            };

            _logger.LogError($"{ex.Message}{ex.InnerException?.Message}");
        }

        return apiResponse;
    }
}

public class ApiResponse
{
    public string? Json { get; set; }
    public string? Error { get; set; }
    public int StatusCode { get; set; }
    public bool IsSuccessRequest => StatusCode is 200 or 204;
}