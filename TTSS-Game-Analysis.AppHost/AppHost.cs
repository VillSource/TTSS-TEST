var builder = DistributedApplication.CreateBuilder(args);

var sql = builder.AddSqlServer("sql-server")
                 .WithDataVolume()
                 .WithDbGate() 
                 .WithLifetime(ContainerLifetime.Persistent);

var sqlDb = sql.AddDatabase("Game-Analysis");

var api = builder.AddProject<Projects.TTSS_Game_Analysis_Api>("ttss-game-analysis-api")
    .WithReference(sqlDb);

builder.AddNpmApp(name: "fe", workingDirectory: "../TTSS-Game-Analysis.Web")
    .WithReference(api)
    .WithUrl("http://localhost:4200");
//.WithNpmPackageInstallation();


builder.Build().Run();
