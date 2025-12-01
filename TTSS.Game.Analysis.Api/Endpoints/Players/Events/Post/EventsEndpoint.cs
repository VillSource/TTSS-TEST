using FastEndpoints;

namespace TTSS.Game.Analysis.Api.Endpoints.Players.Events.Post;

public class EventsEndpoint : Ep.NoReq.Res<string>
{
    public override void Configure()
    {
        Post("players/events");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        await Send.OkAsync("OK");
    }
}
