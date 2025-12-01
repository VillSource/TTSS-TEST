using FastEndpoints;
using System.Security.Claims;
using TTSS.Game.Analysis.Api.Constants;
using TTSS.Game.Analysis.Api.Services;

namespace TTSS.Game.Analysis.Api.Endpoints.Players.Events.Post;

public class EventsEndpoint : Ep.Req<EventsRequest>.Res<string>
{
    private readonly EventLogService _service;
    private readonly ServerTimeProvider _timeProvider;

    public EventsEndpoint(EventLogService service, ServerTimeProvider timeProvider)
    {
        _service = service;
        _timeProvider = timeProvider;
    }

    public override void Configure()
    {
        Post("players/events");
        Policies(AuthPolicyConstant.Player);
    }

    public override async Task HandleAsync(EventsRequest req, CancellationToken ct)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null)
        {
            await Send.ResultAsync(TypedResults.InternalServerError("Can not get user Id."));
            return;
        }

        var now = _timeProvider.UtcNow();
        if (Math.Abs((now - req.ClientTimestamp).Minutes) > 1)
        {
            await Send.ResultAsync(TypedResults.BadRequest("Time travel is not allowed."));
            return;
        }

        var @events = req.Events.Select(e =>
        {
            e.UserId = userId;
            return e;
        });

        await _service.SaveEvents(@events, ct);

        await Send.OkAsync(userId);
    }
}
