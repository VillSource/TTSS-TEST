namespace TTSS.Game.Analysis.Api.Endpoints.Players.Activities.Get;

public class ActivitiesResponse
{
    public string UserId { get; set; } = string.Empty;
    public int TotalLogin { get; set; }
    public TimeSpan TotalOnlineTime { get; set; }
    public int AchievementCount { get; set; }
    public int PurchasedCount { get; set; }
    public decimal MoneySpend { get; set; }
}
