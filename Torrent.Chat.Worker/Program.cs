using Npgsql;
using Torrent.Chat.Storage;
using Torrent.Chat.Worker;
using Torrent.Chat.Worker.Configuration;
using Torrent.Chat.Worker.Repositories;

var builder = Host.CreateApplicationBuilder(args);

AddServices(builder);

var host = builder.Build();
host.Run();

static void AddServices(HostApplicationBuilder builder)
{
    builder.Services.AddHostedService<RabbitMqWorker>();

    var dbConfig = builder.Configuration.GetSection(nameof(DatabaseConfig));
    var dbConnString = dbConfig.Get<DatabaseConfig>()!.ToConnectionString();
    builder.Services.Configure<DatabaseConfig>(dbConfig);

    var dcBuilder = new NpgsqlDataSourceBuilder(dbConnString);
    var dc = dcBuilder.Build();
    builder.Services.AddSingleton<ChatContext>(c => new(dc));
    builder.Services.AddSingleton<DbRepository>();
}