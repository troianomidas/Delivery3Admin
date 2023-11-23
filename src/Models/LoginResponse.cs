namespace Delivery3Admin.Models;

public class LoginResponse
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Store { get; set; }
    public string? Token { get; set; }
    public string? Wallet { get; set; }
    public int Status { get; set; }
}