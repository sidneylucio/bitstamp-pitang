using MediatR;

namespace Pitang.OrderBook.Application.Commands;

public class MetricsCommand : IRequest<Unit>
{
    public string Instrument { get; }

    public MetricsCommand(string instrument)
    {
        Instrument = instrument;
    }
}