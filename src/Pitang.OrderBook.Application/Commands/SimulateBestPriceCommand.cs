using MediatR;
using Pitang.OrderBook.Application.DTOs;

namespace Pitang.OrderBook.Application.Commands;

public record SimulateBestPriceCommand(string Operation, string Instrument, double Quantity) : IRequest<SimulationResponseDto>;
