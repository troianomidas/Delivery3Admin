namespace Delivery3Admin.Models;

public class AdminModel
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? ExternalId { get; set; }
    public string? BearerToken { get; set; }
    public DateTime? LastAccessAt { get; set; }
}