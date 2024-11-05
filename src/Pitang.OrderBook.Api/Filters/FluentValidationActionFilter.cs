using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Pitang.OrderBook.Api.Filters;

public class FluentValidationActionFilter<TModel, TValidator> : Attribute, IAsyncActionFilter
    where TValidator : IValidator<TModel>
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var validator = (TValidator)Activator.CreateInstance(typeof(TValidator))!;
        var model = context.ActionArguments.FirstOrDefault(arg => arg.Value is TModel).Value;

        if (model != null)
        {
            var validationResult = await validator.ValidateAsync(new ValidationContext<TModel>((TModel)model));
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(error => new { error.PropertyName, error.ErrorMessage }).ToList();

                var baseResponse = new { Message = "Erro na validação dos dados.", Errors = errors };

                context.Result = new BadRequestObjectResult(baseResponse);
                return;
            }
        }

        await next();
    }
}