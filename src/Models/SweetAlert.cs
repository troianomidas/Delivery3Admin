using CurrieTechnologies.Razor.SweetAlert2;

namespace Delivery3Admin.Models;

public class SweetAlert
{
    private readonly SweetAlertService _sweetAlertService;

    public SweetAlert(SweetAlertService sweetAlertService) => _sweetAlertService = sweetAlertService;

    public async Task SuccessAsync(string title, string message)
    {
        await _sweetAlertService.FireAsync(new SweetAlertOptions
        {
            Title = title,
            Text = message,
            Icon = SweetAlertIcon.Success,
            ShowCloseButton = false,
            ShowConfirmButton = false,
            AllowEscapeKey = false,
            AllowEnterKey = false,
            AllowOutsideClick = false,
            TimerProgressBar = true,
            Timer = 1500
        });
    }

    public async Task ErrorAsync(string message)
    {
        await _sweetAlertService.FireAsync(new SweetAlertOptions
        {
            Text = message,
            Icon = SweetAlertIcon.Error,
            ShowCloseButton = false,
            ShowConfirmButton = true
        });
    }

    public async Task WarningAsync(string message)
    {
        await _sweetAlertService.FireAsync(new SweetAlertOptions
        {
            Title = "Atenção!",
            Text = message,
            Icon = SweetAlertIcon.Warning,
            ShowCloseButton = false,
            ShowConfirmButton = true
        });
    }

    public async Task CloseAsync() => await _sweetAlertService.CloseAsync();
}