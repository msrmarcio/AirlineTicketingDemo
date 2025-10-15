using CustomerHistoryService.Application.Interface;
using CustomerHistoryService.Application.Services;
using CustomerHistoryService.Consumers;
using CustomerHistoryService.Infrastructure.Persistence;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// ====================================================================
// CONFIGURAÇÕES DE APPSETTINGS
// ====================================================================
builder.Services.Configure<RabbitMqOptions>(
    builder.Configuration.GetSection(RabbitMqOptions.RabbitMq));

// ====================================================================
// CONFIGURAÇÃO DO BANCO DE DADOS
// ====================================================================
builder.Services.AddDbContext<CustomerHistoryDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CustomerHistoryConnection")));

// ====================================================================
// INJEÇÃO DE DEPENDÊNCIAS
// ====================================================================
builder.Services.AddScoped<ICustomerHistoryRepository, CustomerHistoryRepository>();
builder.Services.AddScoped<ICustomerHistoryReportService, CustomerHistoryReportService>();

// ====================================================================
// CONFIGURAÇÕES DE MASS TRANSIT
// ====================================================================
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<ReservationCreatedConsumer>();
    x.AddConsumer<PaymentTimeoutConsumer>();
    x.AddConsumer<PaymentApprovedConsumer>();
    x.AddConsumer<PaymentRejectedConsumer>();
    x.AddConsumer<NotificationSentConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        var rabbitMqOptions = context.GetRequiredService<IOptions<RabbitMqOptions>>().Value;

        cfg.Host(rabbitMqOptions.Host, rabbitMqOptions.VirtualHost, h =>
        {
            h.Username(rabbitMqOptions.Username);
            h.Password(rabbitMqOptions.Password);
        });

        cfg.ReceiveEndpoint("customer-history-queue", e =>
        {
            e.ConfigureConsumer<ReservationCreatedConsumer>(context);
            e.ConfigureConsumer<PaymentTimeoutConsumer>(context);
            e.ConfigureConsumer<PaymentApprovedConsumer>(context);
            e.ConfigureConsumer<PaymentRejectedConsumer>(context);
            e.ConfigureConsumer<NotificationSentConsumer>(context);
            e.SetQueueArgument("durable", true);
        });
    });
});

// ====================================================================
// CONFIGURAÇÃO DO CONTROLLER E SWAGGER
// ====================================================================
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
