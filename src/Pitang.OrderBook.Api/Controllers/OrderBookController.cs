using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pitang.OrderBook.Api.Filters;
using Pitang.OrderBook.Application.Commands;
using Pitang.OrderBook.Application.DTOs;
using Pitang.OrderBook.Application.Validations;

namespace Pitang.OrderBook.API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/orders")]
public class OrderBookController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrderBookController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("simulate")]
    [ProducesResponseType(typeof(SimulationResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [FluentValidationActionFilter<SimulationRequestDto, SimulationRequestDtoValidator>]
    public async Task<IActionResult> SimulateBestPrice([FromBody] SimulationRequestDto request)
    {
        var response = await _mediator.Send(new SimulateBestPriceCommand(request.Operation, request.Instrument, request.Quantity));
        return Ok(response);
    }
}
