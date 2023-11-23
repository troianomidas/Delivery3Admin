namespace Delivery3Admin.Models.Stores;

public class StoreModel
{
    public int Id { get; set; }
    public Guid ExternalId { get; set; }
    public string? Name { get; set; }
    public string? Partner { get; set; }
    public string? Description { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Document { get; set; }
    public string? Hostname { get; set; }
    public string? LogoUrl { get; set; }
    public string? BannerUrl { get; set; }
    public AdminModel? AdminModel { get; set; } 
}