using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ReservationService.Application.Interfaces;
using ReservationService.Infrastructure.Configurations;
using ReservationService.Infrastructure.Persistence;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// ====================================================================
// Add services to the container.
// ====================================================================
// ---------------------------------------------------------------------------------
// Mapear a seção "RabbitMq" do appsettings para a classe RabbitMqOptions
// ---------------------------------------------------------------------------------
builder.Services.Configure<RabbitMqOptions>(
    builder.Configuration.GetSection(RabbitMqOptions.RabbitMq));

// ====================================================================
// CONFIGURAÇÃO DO MASS TRANSIT PARA PUBLICAÇÃO
// ====================================================================
builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        var rabbitMqOptions = context.GetRequiredService<IOptions<RabbitMqOptions>>().Value;

        cfg.Host(rabbitMqOptions.Host, rabbitMqOptions.VirtualHost, h =>
        {
            h.Username(rabbitMqOptions.Username);
            h.Password(rabbitMqOptions.Password);
        });
    });
});

builder.Host.UseSerilog((ctx, lc) => lc
    .WriteTo.Console()
    .WriteTo.File("logs/Reservationlog.txt", rollingInterval: RollingInterval.Day));

builder.Services.AddDbContext<ReservationDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("ReservationConnectionString")));

builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
builder.Services.AddScoped<IReservationService, ReservationService.Application.Services.ReservationService>();
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
