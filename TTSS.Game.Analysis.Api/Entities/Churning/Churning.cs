namespace TTSS.Game.Analysis.Api.Entities.Churning;

public class Churning
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string ChurningReason { get; set; } = string.Empty;
    public DateTime DetecedTime { get; set; }
    public bool IsValid { get; set; }
}
