using SimulateCredit.Application.Ports.Incoming;
using SimulateCredit.Application.UseCases.SimulateCredit;
using SimulateCredit.Infrastructure.Converters;
using SimulateCredit.Infrastructure.IoC;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddControllers()
    .AddJsonOptions(opts =>
    {
        opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        opts.JsonSerializerOptions.Converters.Add(new CurrencyJsonConverter());
    });

builder.Services.AddScoped<ISimulateCreditUseCase, SimulateCreditUseCase>();
builder.Services.AddSimulateCreditServices(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();