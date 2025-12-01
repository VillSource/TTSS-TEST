namespace TTSS.Game.Analysis.Api.Endpoints.Players.Churnings;

public class ChurningsResponse
{
    public string UserId { get; set; } = string.Empty;
    public List<string> Reason { get; set; } = [];
    public DateTime DetectedAt { get; set; }
}
