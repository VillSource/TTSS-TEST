
using Microsoft.EntityFrameworkCore;
using TTSS.Game.Analysis.Api.Data;
using TTSS.Game.Analysis.Api.Entities.Churning;
using TTSS.Game.Analysis.Api.Entities.Event;

namespace TTSS.Game.Analysis.Api.HostServices;

public class DetectingChurningsJob : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public DetectingChurningsJob(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var now = DateTime.UtcNow;
            using (var scope = _scopeFactory.CreateScope())
            {
                var _db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                await _db.Churnings
                    .Where(churning => churning.IsValid)
                    .ExecuteUpdateAsync(churnings => churnings.SetProperty(churning => churning.IsValid, false), stoppingToken);
                await _db.SaveChangesAsync(stoppingToken);

                var lastLoginOfChurning = now.Date.AddDays(-30);
                var noLoginPlayers = _db.Activities.AsNoTracking()
                    .OfType<LoginEvent>()
                    .GroupBy(player => player.UserId)
                    .Where(players => !players.Any(player=>player.Timestamp>lastLoginOfChurning))
                    .Select(player => new Churning
                    {
                        DetecedTime = now,
                        UserId = player.Key,
                        ChurningReason = "Have No Login in 30 days!",
                        IsValid = true
                    });
                await _db.AddRangeAsync(noLoginPlayers, stoppingToken);
                await _db.SaveChangesAsync(stoppingToken);
            }

            await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
        }
    }
}
