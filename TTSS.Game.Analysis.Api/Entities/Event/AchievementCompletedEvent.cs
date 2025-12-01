namespace TTSS.Game.Analysis.Api.Entities.Event;

public class AchievementCompletedEvent: ActivityEventBase

{
    public string AchievementId { get; set; } = string.Empty;
    public string AchievementName { get; set; } = string.Empty;
}
