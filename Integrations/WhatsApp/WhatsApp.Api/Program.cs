using Npgsql;
using System.Data;
using WhatsApp.Core.Bac;
using WhatsApp.Core.Broker;
using WhatsApp.Core.Interfaces.IBac;
using WhatsApp.Core.Interfaces.IRepository;
using WhatsApp.Core.Interfaces.IServices;
using WhatsApp.Core.Services;
using WhatsApp.Core.Utility;
using WhatsApp.Core.Models.Send;
using whatsapp.Core.Bac;
using WhatsApp.Data.Repository;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

string connectionString = configuration.GetConnectionString("DefaultConnection") ?? string.Empty;

string? sendTemplateUrl = configuration["WhatsappUrls:SendTemplate"];
SD.LeadsManagerAPIBase = configuration["ServiceUrls:LeadsManagerApi"];
SD.WhatsappAPIBase = configuration["WhatsappUrls:SendReceiveMessage"];

// Add services to the container.

// Configuration for dapper and postgres.
builder.Services.AddTransient<IDbConnection>(config =>
    new NpgsqlConnection(configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHttpClient(nameof(SendMessagePayLoad), u => u.BaseAddress = new Uri(sendTemplateUrl!));

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// RabbitMQ Broker
builder.Services.AddScoped<IMessageBroker, MessageBroker>();

builder.Services.AddScoped<IBaseService, BaseService>();
builder.Services.AddScoped<ITemplateService, TemplateService>();
builder.Services.AddScoped<IWhatsAppService, WhatsappService>();

builder.Services.AddScoped<IWhatsAppConfigBac, WhatsappConfigBac>();
builder.Services.AddScoped<IWhatsAppConfigRepository, WhatsAppConfigRepository>();

builder.Services.AddScoped<IWhatsappSendMessageBac, WhatsAppSendMessageBac>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
