using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TransactionService;
using TransactionService.Settings;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddOptions<TransactionWorkerSettings>()
    .Bind(builder.Configuration.GetSection(TransactionWorkerSettings.SectionName));
builder.Services.AddHostedService<TransactionWorker>();
builder.Services.AddHttpClient();

var host = builder.Build();
host.Run();