using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pitang.OrderBook.Application.Commands;
using Pitang.OrderBook.Application.DTOs;
using System;
using System.Threading.Tasks;

namespace Pitang.OrderBook.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderBookController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrderBookController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("simulate")]
        public async Task<IActionResult> SimulateBestPrice([FromBody] SimulationRequestDto request)
        {
            var response = await _mediator.Send(new SimulateBestPriceCommand(request.Operation, request.Instrument, request.Quantity));
            return Ok(response);
        }
    }
}
