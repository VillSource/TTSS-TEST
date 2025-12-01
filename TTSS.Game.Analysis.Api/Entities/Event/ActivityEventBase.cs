namespace TTSS.Game.Analysis.Api.Entities.Event;

public class ActivityEventBase
{
    public Guid? EventId { get; set; }
    public string EventName { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
}
