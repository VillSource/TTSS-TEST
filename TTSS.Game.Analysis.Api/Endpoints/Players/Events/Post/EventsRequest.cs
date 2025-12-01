using System.Text.Json.Serialization;
using TTSS.Game.Analysis.Api.Entities.Event;

namespace TTSS.Game.Analysis.Api.Endpoints.Players.Events.Post;

public class EventsRequest
{
    [JsonPropertyName("timestamp")]
    public DateTime ClientTimestamp { get; set; }
    public List<ActivityEventBase> Events { get; set; } = [];
}
