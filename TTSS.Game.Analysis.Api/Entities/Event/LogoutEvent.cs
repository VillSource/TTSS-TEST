namespace TTSS.Game.Analysis.Api.Entities.Event;

public class LogoutEvent: ActivityEventBase
{
    public string Device { get; set; } = string.Empty;
    public int DurationInSeconds { get; set; }
}
