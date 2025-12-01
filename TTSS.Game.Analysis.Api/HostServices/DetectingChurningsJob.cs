
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

                var timespanForNoLongerPlay = TimeSpan.FromDays(30);
                var lastLoginOfChurningCriteria = now.Date - timespanForNoLongerPlay;
                var noLoginPlayers = _db.Activities
                    .OfType<LoginEvent>()
                    .GroupBy(player => player.UserId)
                    .Where(players => !players.Any(player=>player.Timestamp>lastLoginOfChurningCriteria))
                    .Select(player => new Churning
                    {
                        DetecedTime = now,
                        UserId = player.Key,
                        ChurningReason = $"Have No Login in for {timespanForNoLongerPlay.TotalDays} days!",
                        IsValid = true
                    });

                var timeSpanForCriteria = TimeSpan.FromDays(7);
                var activityInTimeCriteria = _db.Activities
                    .Where(player => player.Timestamp >= now - timeSpanForCriteria)
                    .Where(player => player.Timestamp <= now);

                var playersWhoBuyItems = activityInTimeCriteria
                    .OfType<PurchasedEvent>()
                    .GroupBy(player => player.UserId)
                    .Where(players => players.Any())
                    .Select(players => players.Key);

                var playersWhoCompleteAchievment = activityInTimeCriteria
                    .OfType<AchievementCompletedEvent>()
                    .GroupBy(player => player.UserId)
                    .Where(players => players.Any())
                    .Select(players => players.Key);

                var lowPlayTimeCritreria = TimeSpan.FromHours(1);
                var whoHaveNoPayAndNoAchievmentInCriteriaTime = activityInTimeCriteria
                    .OfType<LogoutEvent>()
                    .GroupBy(player => player.UserId)
                    .Where(players => players.Sum(p=>p.DurationInSeconds) < lowPlayTimeCritreria.TotalSeconds)
                    .Where(player => !playersWhoBuyItems.Contains(player.Key))
                    .Where(player => !playersWhoCompleteAchievment.Contains(player.Key))
                    .Select(players => new Churning
                    {
                        DetecedTime = now,
                        UserId = players.Key,
                        ChurningReason = $"Player Have {TimeSpan.FromSeconds(players.Sum(p=>p.DurationInSeconds)).TotalMinutes} Minutes Screen Time and No Purchased and No Achievment complete In {timeSpanForCriteria.TotalDays} Days!",
                        IsValid = true
                    });

                
                await _db.AddRangeAsync(noLoginPlayers, stoppingToken);
                await _db.AddRangeAsync(whoHaveNoPayAndNoAchievmentInCriteriaTime, stoppingToken);
                await _db.SaveChangesAsync(stoppingToken);
            }

            await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
        }
    }
}
