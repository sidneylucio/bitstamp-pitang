using MediatR;

namespace Pitang.OrderBook.Application.Commands;

public record MetricsCommand(string Instrument) : IRequest<Unit>;