namespace TTSS.Game.Analysis.Api.Entities.Event;

public class LoginEvent: ActivityEventBase
{
    public string Device { get; set; } = string.Empty;
}
