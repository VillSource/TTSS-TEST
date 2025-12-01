using FastEndpoints;
using System.Security.Claims;
using TTSS.Game.Analysis.Api.Constants;

namespace TTSS.Game.Analysis.Api.Endpoints.Players.Events.Post;

public class EventsEndpoint : Ep.NoReq.Res<string>
{
    public override void Configure()
    {
        Post("players/events");
        Policies(AuthPolicyConstant.Player);
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        await Send.OkAsync(userId);
    }
}
