var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.TTSS_Game_Analysis_Api>("ttss-game-analysis-api");

builder.Build().Run();
