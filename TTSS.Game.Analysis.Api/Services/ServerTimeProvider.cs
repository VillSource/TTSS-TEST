namespace TTSS.Game.Analysis.Api.Services;

public class ServerTimeProvider
{
    public DateTime UtcNow() => DateTime.UtcNow;
    public DateTime Now() => DateTime.Now;
}
