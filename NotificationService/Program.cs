using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NotificationService.Application.Interfaces;
using NotificationService.Consumers;
using NotificationService.Infrastructure.Configurations;
using NotificationService.Infrastructure.Persistence;
using Serilog;
using NotificationService.Application.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// ====================================================================
// CONFIGURAÇÃO DE LOG COM SERILOG
// ====================================================================
builder.Host.UseSerilog((ctx, lc) => lc
    .WriteTo.Console()
    .WriteTo.File("logs/NotificationLog.txt", rollingInterval: RollingInterval.Day));

// ====================================================================
// MAPEAMENTO DA SEÇÃO "RabbitMq" DO appsettings.json PARA RabbitMqOptions
// ====================================================================
builder.Services.Configure<RabbitMqOptions>(
    builder.Configuration.GetSection(RabbitMqOptions.RabbitMq));

// ====================================================================
// CONFIGURAÇÃO DO MASS TRANSIT COM RABBITMQ
// ====================================================================
builder.Services.AddMassTransit(x =>
{
    // Registra explicitamente os consumers que o serviço escuta
    x.AddConsumer<PaymentApprovedConsumer>();
    x.AddConsumer<PaymentRejectedConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        var rabbitMqOptions = context.GetRequiredService<IOptions<RabbitMqOptions>>().Value;

        cfg.Host(rabbitMqOptions.Host, rabbitMqOptions.VirtualHost, h =>
        {
            h.Username(rabbitMqOptions.Username);
            h.Password(rabbitMqOptions.Password);
        });

        // Cria endpoints nomeados para cada tipo de evento
        cfg.ReceiveEndpoint("payment-approved-notification-queue", e =>
        {
            e.ConfigureConsumer<PaymentApprovedConsumer>(context);
        });

        cfg.ReceiveEndpoint("payment-rejected-notification-queue", e =>
        {
            e.ConfigureConsumer<PaymentRejectedConsumer>(context);
        });
    });
});

// ====================================================================
// CONFIGURAÇÃO DO BANCO DE DADOS
// ====================================================================
builder.Services.AddDbContext<NotificationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("NotificationConnectionString")));

// ====================================================================
// INJEÇÃO DE DEPENDÊNCIAS
// ====================================================================
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<INotificationService, NotificationServices>();

builder.Services.AddControllers();
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
