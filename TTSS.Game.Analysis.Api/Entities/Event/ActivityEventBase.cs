using System.Text.Json.Serialization;

namespace TTSS.Game.Analysis.Api.Entities.Event;


[JsonPolymorphic(TypeDiscriminatorPropertyName = nameof(EventName))]
[JsonDerivedType(typeof(LoginEvent), typeDiscriminator: nameof(LoginEvent))]
[JsonDerivedType(typeof(LogoutEvent), typeDiscriminator: nameof(LogoutEvent))]
[JsonDerivedType(typeof(AchievementCompletedEvent), typeDiscriminator: nameof(AchievementCompletedEvent))]
[JsonDerivedType(typeof(PurchasedEvent), typeDiscriminator: nameof(PurchasedEvent))]
public class ActivityEventBase
{
    public Guid? EventId { get; set; }
    public string EventName { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
}
