using Pitang.OrderBook.Infra.IoC.DI;
using Pitang.OrderBook.LiveOrderBook.Worker;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.Services.AddOrderBookDependencies(builder.Configuration);

var host = builder.Build();
host.Run();
