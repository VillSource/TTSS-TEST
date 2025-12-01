using Microsoft.EntityFrameworkCore;
using TTSS.Game.Analysis.Api.Entities.Churning;
using TTSS.Game.Analysis.Api.Entities.Event;

namespace TTSS.Game.Analysis.Api.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> opt) : DbContext(opt)
{
    public DbSet<ActivityEventBase> Activities { get; init; }
    public DbSet<Churning> Churnings { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var churning = modelBuilder.Entity<Churning>();
        churning.ToTable("churning_list");
        churning.HasKey(c => c.Id);
        churning.Property(c => c.UserId).IsRequired();
        churning.Property(c => c.DetecedTime).IsRequired();

        var activityEventBase = modelBuilder.Entity<ActivityEventBase>();
        activityEventBase.ToTable("activity_event_log");
        activityEventBase.HasKey(a => a.EventId);
        activityEventBase.Property(a => a.EventId).IsRequired();
        activityEventBase.Property(a => a.UserId).IsRequired();
        activityEventBase.Property(a => a.EventName).IsRequired();
        activityEventBase.Property(a => a.Timestamp).IsRequired();

        activityEventBase.HasDiscriminator(a => a.EventName)
            .HasValue<LoginEvent>(nameof(LoginEvent))
            .HasValue<LogoutEvent>(nameof(LogoutEvent))
            .HasValue<PurchasedEvent>(nameof(PurchasedEvent))
            .HasValue<AchievementCompletedEvent>(nameof(AchievementCompletedEvent));
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ApplyValueGenerationRules();
        return await base.SaveChangesAsync(cancellationToken);
    }
    public override int SaveChanges()
    {
        ApplyValueGenerationRules();
        return base.SaveChanges();
    }
    private void ApplyValueGenerationRules()
    {
        foreach (var entry in ChangeTracker.Entries<ActivityEventBase>()
            .Where(e => e.State == EntityState.Added))
        {
            if (entry.Entity.Timestamp == default)
                entry.Entity.Timestamp = DateTime.UtcNow;
            entry.Entity.EventName = entry.Entity.GetType().Name;
        }

        foreach (var entry in ChangeTracker.Entries<Churning>()
            .Where(e => e.State == EntityState.Added))
        {
            if (entry.Entity.DetecedTime == default)
                entry.Entity.DetecedTime = DateTime.UtcNow;
        }

    }

}
