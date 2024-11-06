using FluentValidation;
using Pitang.OrderBook.Application.DTOs;

namespace Pitang.OrderBook.Application.Validations;

public class SimulationRequestDtoValidator : AbstractValidator<SimulationRequestDto>
{
    public SimulationRequestDtoValidator()
    {
        RuleFor(x => x.Operation)
            .NotEmpty()
            .WithMessage("O campo operação é obrigatório.");

        RuleFor(x => x.Instrument)
            .NotEmpty()
            .WithMessage("O campo instrumento é obrigatório.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .NotEmpty()
            .WithMessage("O campo quantidade deve ser maior que 0.");
    }
}