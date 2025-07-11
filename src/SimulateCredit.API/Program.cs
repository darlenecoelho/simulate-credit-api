using SimulateCredit.Infrastructure.Converters;
using SimulateCredit.Infrastructure.IoC;
using System.Text.Json.Serialization;
using SimulateCredit.Application.IoC;

var builder = WebApplication.CreateBuilder(args);

// Infrastructure registrations
builder.Services.AddInfrastructureServices(builder.Configuration);

// Application registrations
builder.Services.AddApplicationServices();

// Presentation / API
builder.Services
    .AddControllers()
    .AddJsonOptions(opts =>
    {
        opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        opts.JsonSerializerOptions.Converters.Add(new CurrencyJsonConverter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
