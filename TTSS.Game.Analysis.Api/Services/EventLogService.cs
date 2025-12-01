using TTSS.Game.Analysis.Api.Data;
using TTSS.Game.Analysis.Api.Entities.Event;

namespace TTSS.Game.Analysis.Api.Services;

public class EventLogService
{
    private readonly ApplicationDbContext _db;

    public EventLogService(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task SaveEvents(IEnumerable<ActivityEventBase> @events, CancellationToken ct)
    {
        await _db.Activities.AddRangeAsync( @events, ct );
        await _db.SaveChangesAsync(ct);
    }
}
