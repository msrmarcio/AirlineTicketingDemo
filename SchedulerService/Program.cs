using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SchedulerService.Infrastructure.Configurations;
using SchedulerService.Infrastructure.Persistence;
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

// ---------------------------------------------------------------------------------
// configuracao massTransit e serilog
// ---------------------------------------------------------------------------------
builder.Services.AddMassTransit(x =>
{
    x.AddConsumers(typeof(Program).Assembly); // registra todos os consumers do projeto

    x.UsingRabbitMq((context, cfg) =>
    {
        // 1. OBTÉM as opções de configuração do provedor de serviços (DI)
        var rabbitMqOptions = context.GetRequiredService<IOptions<RabbitMqOptions>>().Value;

        // 2. CONFIGURA o Host usando os valores obtidos do appsettings.json
        cfg.Host(rabbitMqOptions.Host, rabbitMqOptions.VirtualHost, h =>
        {
            h.Username(rabbitMqOptions.Username);
            h.Password(rabbitMqOptions.Password);
        });

        cfg.ConfigureEndpoints(context); // cria automaticamente os endpoints para os consumers
    });
});

builder.Host.UseSerilog((ctx, lc) => lc
    .WriteTo.Console()
    .WriteTo.File("logs/Schedulerlog.txt", rollingInterval: RollingInterval.Day));

builder.Services.AddDbContext<SchedulerDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SchedulerConnectionString")));

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
