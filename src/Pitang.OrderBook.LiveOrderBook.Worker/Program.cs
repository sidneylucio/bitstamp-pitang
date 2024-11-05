using Pitang.OrderBook.LiveOrderBook.Worker;
using Pitang.OrderBook.Infra.IoC.DependencyInjection;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.Services.AddApplicationDependencies();

var host = builder.Build();
host.Run();
