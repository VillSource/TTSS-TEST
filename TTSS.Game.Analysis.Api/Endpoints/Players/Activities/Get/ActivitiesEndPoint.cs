using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using TTSS.Game.Analysis.Api.Constants;
using TTSS.Game.Analysis.Api.Data;
using TTSS.Game.Analysis.Api.Entities.Event;
using TTSS.Game.Analysis.Api.Services;

namespace TTSS.Game.Analysis.Api.Endpoints.Players.Activities.Get;

public class ActivitiesEndPoint : Ep.Req<ActivitiesRequest>.Res<List<ActivitiesResponse>>
{
    private readonly ApplicationDbContext _db;
    private readonly ServerTimeProvider _timeProvider;

    public ActivitiesEndPoint(ApplicationDbContext db, ServerTimeProvider timeProvider)
    {
        _db = db;
        _timeProvider = timeProvider;
    }

    public override void Configure()
    {
        Get("players/activities/{FromNDayAgo}");
        Policies(AuthPolicyConstant.Admin);
    }

    public override async Task HandleAsync(ActivitiesRequest request, CancellationToken ct)
    {
        var now = _timeProvider.UtcNow();
        var startPoint = now - TimeSpan.FromDays(request.FromNDayAgo.GetValueOrDefault());

        var x = _db.Activities.AsNoTracking()
            .Where(@event => @event.Timestamp >= startPoint)
            .Where(@event => @event.Timestamp <= now)
            .OrderBy(@event => @event.Timestamp)
            .GroupBy(@event => @event.UserId)
            .Select(@event => new ActivitiesResponse
            {
                UserId = @event.Key,
                TotalLogin = @event.OfType<LoginEvent>().Count(),
                TotalOnlineTime = TimeSpan.FromSeconds(@event.OfType<LogoutEvent>().Sum(logout => logout.DurationInSeconds)),
                AchievementCount = @event.OfType<AchievementCompletedEvent>().Count(),
                PurchasedCount = @event.OfType<PurchasedEvent>().Count(),
                MoneySpend = Convert.ToDecimal( @event.OfType<PurchasedEvent>().Select(p => p.Quantity * p.PricePerUnit).Sum())
            });
        Response = await x.ToListAsync(ct);
    }
}
