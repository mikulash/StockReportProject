using MailBusinessLayer.DependencyInjection;
using MailDataAccessLayer.Data;
using MailDataAccessLayer.DependencyInjection;
using MailInfrastructure.DependencyInjection;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);
var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

var configuration = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true)
    .Build();

builder.Services.RegisterDALDependencies(configuration);

builder.Services.RegisterInfrastructureDependencies();

builder.Services.RegisterBLDependecies();

builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (Convert.ToBoolean(configuration.GetSection("ApplyMigrations").Value))
{
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<MailDbContext>();
        db.Database.Migrate();
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); }

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();

app.Run();