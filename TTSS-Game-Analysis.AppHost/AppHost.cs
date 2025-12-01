var builder = DistributedApplication.CreateBuilder(args);

var sql = builder.AddSqlServer("sql-server")
                 .WithDataVolume()
                 .WithDbGate() 
                 .WithLifetime(ContainerLifetime.Persistent);

var sqlDb = sql.AddDatabase("Game-Analysis");

builder.AddProject<Projects.TTSS_Game_Analysis_Api>("ttss-game-analysis-api")
    .WithReference(sqlDb);

builder.Build().Run();
