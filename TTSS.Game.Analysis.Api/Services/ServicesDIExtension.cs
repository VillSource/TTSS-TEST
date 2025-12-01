using TTSS.Game.Analysis.Api.HostServices;

namespace TTSS.Game.Analysis.Api.Services;

public static class ServicesDIExtension
{
    public static void AddAppServices(this IServiceCollection services)
    {
        services.AddScoped<EventLogService>();
        services.AddScoped<ServerTimeProvider>();

        services.AddHostedService<DetectingChurningsJob>();
    }
}
