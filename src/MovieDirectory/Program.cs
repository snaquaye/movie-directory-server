using System.Text.Json;

using FastEndpoints.Swagger;

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;

using MovieDirectory.Application.Services;
using MovieDirectory.Core.Services;
using MovieDirectory.Extensions;
using MovieDirectory.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.AddServerHeader = false;
    options.AllowSynchronousIO = false;
});

var configuration = builder.Configuration;

builder.Host.UseConsoleLifetime(options => options.SuppressStatusMessages = true);

builder.Services.AddMovieDirectoryService(configuration);

builder.Services.AddFastEndpoints(options =>
{
    options.SourceGeneratorDiscoveredTypes = DiscoveredTypes.All;
});

builder.Services.AddSwaggerDoc(addJWTBearerAuth: false);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDefaultExceptionHandler();
}

app.UseStaticFiles();
app.UseCors();

app.UseFastEndpoints(options =>
{
    options.Errors.ResponseBuilder = (errors, _, _) => errors.ToResponse();
    options.Errors.StatusCode = StatusCodes.Status422UnprocessableEntity;
    options.Serializer.Options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
});

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi3(x => x.ConfigureDefaults());
}

await app.RunAsync();