namespace TTSS.Game.Analysis.Api.Entities.Event;

public class PurchasedEvent: ActivityEventBase
{
    public string ItemId { get; set; } = string.Empty;
    public string ItemName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal? PricePerUnit { get; set; }
}
