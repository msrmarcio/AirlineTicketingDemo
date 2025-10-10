using MassTransit;
using PaymentService.Application.Interfaces;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// ----------------------------------------------
// Add services to the container.
// ----------------------------------------------
// configuracao massTransit e serilog
builder.Services.AddMassTransit(x =>
{
    x.UsingInMemory((context, cfg) =>
    {
        cfg.ConfigureEndpoints(context);
    });
});

builder.Host.UseSerilog((ctx, lc) => lc
    .WriteTo.Console()
    .WriteTo.File("logs/Paymentlog.txt", rollingInterval: RollingInterval.Day));

builder.Services.AddScoped<IPaymentService, PaymentService.Application.Services.PaymentService>(); 

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
