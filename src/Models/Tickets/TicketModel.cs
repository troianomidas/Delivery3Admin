using Delivery3Admin.Models.Stores;

namespace Delivery3Admin.Models.Tickets;

public class TicketModel
{
    public int Id { get; set; }
    public int StoreId { get; set; }
    public string? Title { get; set; }
    public string? Body { get; set; }
    public int Status { get; set; }
    public string? ImageUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public List<TicketAnswerModel>? TicketAnswer { get; set; }
    public virtual StoreModel? Store { get; set; }

    public TicketModel() => TicketAnswer = new List<TicketAnswerModel>();
}