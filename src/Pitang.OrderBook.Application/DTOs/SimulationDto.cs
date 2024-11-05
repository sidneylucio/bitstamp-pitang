namespace Pitang.OrderBook.Application.DTOs;

public record SimulationRequestDto(string Operation, string Instrument, double Quantity);

public record SimulationResponseDto(
    Guid Id,
    string Operation,
    string Instrument,
    double QuantityRequested,
    double TotalPrice,
    IList<OrderDetail> UsedOrders
);

public record OrderDetail(
    string Price,
    string Quantity
);