using MassTransit;
using MassTransit.SqlTransport;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SchedulerService.Application.Interfaces;
using SchedulerService.Consumers;
using SchedulerService.Infrastructure.Configurations;
using SchedulerService.Infrastructure.Persistence;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// ====================================================================
// Add services to the container.
// ====================================================================
// ====================================================================
// CONFIGURAÇÃO DE LOG COM SERILOG
// ====================================================================
builder.Host.UseSerilog((ctx, lc) => lc
    .WriteTo.Console()
    .WriteTo.File("logs/Schedulerlog.txt", rollingInterval: RollingInterval.Day));

// ---------------------------------------------------------------------------------
// Mapear a seção "RabbitMq" do appsettings para a classe RabbitMqOptions
// ---------------------------------------------------------------------------------
builder.Services.Configure<RabbitMqOptions>(
    builder.Configuration.GetSection(RabbitMqOptions.RabbitMq));

// ====================================================================
// CONFIGURAÇÃO DO MASS TRANSIT COM RABBITMQ
// ====================================================================
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<ReservationCreatedConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        var rabbitMqOptions = context.GetRequiredService<IOptions<RabbitMqOptions>>().Value;

        cfg.Host(rabbitMqOptions.Host, rabbitMqOptions.VirtualHost, h =>
        {
            h.Username(rabbitMqOptions.Username);
            h.Password(rabbitMqOptions.Password);
        });

        cfg.ReceiveEndpoint("reservation-created-queue", e =>
        {
            e.ConfigureConsumer<ReservationCreatedConsumer>(context);
        });
    });
});


// ====================================================================
// CONFIGURAÇÃO DO BANCO DE DADOS
// ====================================================================
builder.Services.AddDbContext<SchedulerDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SchedulerConnectionString")));

// ====================================================================
// INJEÇÃO DE DEPENDÊNCIAS
// ====================================================================
builder.Services.AddScoped<ISchedulerRepository, SchedulerRepository>();
builder.Services.AddScoped<ISchedulerService, SchedulerService.Application.Services.SchedulerService>();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
    .WriteTo.File("logs/Schedulerlog.txt", rollingInterval: RollingInterval.Day));

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
