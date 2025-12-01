using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using TTSS.Game.Analysis.Api.Constants;
using TTSS.Game.Analysis.Api.Data;

namespace TTSS.Game.Analysis.Api.Endpoints.Players.Churnings;

public class ChurningsEndpoint : Ep.NoReq.Res<List<ChurningsResponse>>
{
    private readonly ApplicationDbContext _db;

    public ChurningsEndpoint(ApplicationDbContext db)
    {
        _db = db;
    }

    public override void Configure()
    {
        Get("players/churnings");
        Policies(AuthPolicyConstant.Admin);
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var x = _db.Churnings.AsNoTracking()
            .Where(churning => churning.IsValid)
            .GroupBy(churning => churning.UserId)
            .Select(churning => new ChurningsResponse
            {
                DetectedAt = churning.First().DetecedTime,
                Reason = churning.Select(c => c.ChurningReason).ToList(),
                UserId = churning.Key
            });
        Response = await x.ToListAsync(ct);
    }
}
