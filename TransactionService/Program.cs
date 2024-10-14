using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TransactionService;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<TransactionWorker>();
builder.Services.AddHttpClient();

var host = builder.Build();
host.Run();