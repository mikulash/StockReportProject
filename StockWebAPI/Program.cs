using StockBusinessLayer.DependencyInjection;
using DataAccessLayer.Data;
using DataAccessLayer.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using StockInfrastructure.DependencyInjection;
using StockWebAPI.Middleware;

var builder = WebApplication.CreateBuilder(args);
var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

var configuration = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true)
    .Build();

builder.Services.RegisterDALDependencies(configuration);

builder.Services.RegisterInfrastructureDependencies();

builder.Services.RegisterBLDependencies();

builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
    opt.MapType<DateOnly>(() => new OpenApiSchema
    {
        Type = "string",
        Format = "date",
        Example = new OpenApiString(DateTime.Today.ToString("yyyy-MM-dd"))
    })
);

var app = builder.Build();

if (Convert.ToBoolean(configuration.GetSection("ApplyMigrations").Value))
{
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<StockDbContext>();
        db.Database.Migrate();
    }
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();

app.Run();
