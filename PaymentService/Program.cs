using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PaymentService.Application.Interfaces;
using PaymentService.Consumers;
using PaymentService.Infrastructure.Configurations;
using PaymentService.Infrastructure.Persistence;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// ====================================================================
// Add services to the container.
// ====================================================================
// ====================================================================
// CONFIGURA��ES DE APPSETTINGS
// ====================================================================
builder.Services.Configure<RabbitMqOptions>(
    builder.Configuration.GetSection(RabbitMqOptions.RabbitMq));

// ====================================================================
// CONFIGURA��ES DE MASS TRANSIT
// ====================================================================
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<PaymentTimeoutConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        // 1. OBT�M as op��es de configura��o do provedor de servi�os (DI)
        var rabbitMqOptions = context.GetRequiredService<IOptions<RabbitMqOptions>>().Value;

        // 2. CONFIGURA o Host usando os valores obtidos do appsettings.json
        cfg.Host(rabbitMqOptions.Host, rabbitMqOptions.VirtualHost, h =>
        {
            h.Username(rabbitMqOptions.Username);
            h.Password(rabbitMqOptions.Password);
        });

        cfg.ReceiveEndpoint("payment-timeout-payment-queue", e =>
        {
            e.ConfigureConsumer<PaymentTimeoutConsumer>(context);
            e.SetQueueArgument("durable", true);
        });
    });
});

// ====================================================================
// CONFIGURA��ES DE LOG
// ====================================================================
builder.Host.UseSerilog((ctx, lc) => lc
    .WriteTo.Console()
    .WriteTo.File("logs/Paymentlog.txt", rollingInterval: RollingInterval.Day));

// ====================================================================
// CONFIGURA��ES DE BANCO DE DADOS
// ====================================================================
builder.Services.AddDbContext<PaymentDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("PaymentConnectionString")));

// ====================================================================
// INJE��O DE DEPEND�NCIAS
// ====================================================================
builder.Services.AddScoped<IPaymentService, PaymentService.Application.Services.PaymentService>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();

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
