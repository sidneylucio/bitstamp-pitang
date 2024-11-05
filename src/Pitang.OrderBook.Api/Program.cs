using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.OpenApi.Models;
using Pitang.OrderBook.Api.Middlewares;
using Pitang.OrderBook.Infra.IoC.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerUI;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
            options.ApiVersionReader = new UrlSegmentApiVersionReader();
        });

        builder.Services.AddVersionedApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });

        builder.Services.AddSwaggerGen(options =>
        {
            var provider = builder.Services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, new OpenApiInfo
                {
                    Title = "To Do List API",
                    Version = description.ApiVersion.ToString(),
                    Description = "API endpoints for To Do List API - Bitstamp",
                    TermsOfService = new Uri("https://www.bitstamp.net/"),
                    Contact = new OpenApiContact
                    {
                        Name = "Sidney Borges",
                        Email = "sbl.dev.br3@gmail.com"
                    }
                });
            }
        });

        builder.Services.AddControllers();
        builder.Services.AddApplicationDependencies();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    c.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", $"API {description.ApiVersion}");
                }
                c.RoutePrefix = "swagger";
                c.ConfigObject.DisplayOperationId = false;
                c.ConfigObject.DocExpansion = DocExpansion.List;
                c.ConfigObject.DefaultModelsExpandDepth = 5;
            });
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
        app.UseMiddleware<ExceptionMiddleware>();
        app.Run();
    }
}
