using System.Security.Claims;
using CurrieTechnologies.Razor.SweetAlert2;
using Delivery3Admin.Models;
using Delivery3Admin.Services;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSweetAlert2();
builder.Services.AddScoped<SweetAlert>();

builder.Services.AddScoped<UserSession>();
builder.Services.AddScoped<DataRequest>();
builder.Services.AddScoped<IStorageBlob>(_ =>
    new StorageBlob(builder.Configuration.GetConnectionString("Storage") ?? string.Empty));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/";
        options.LogoutPath = "/web/auth/logout";
    });

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCookiePolicy();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

var supportedCultures = new[] { "pt-BR" };

RequestLocalizationOptions localizationOptions = new RequestLocalizationOptions()
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures("pt-BR")
    .SetDefaultCulture(supportedCultures[0]);

app.UseRequestLocalization(localizationOptions);

app.Run();