using Microsoft.Extensions.Configuration;
using PARS.Inhouse.Systems.Application.Configurations;
using PARS.Inhouse.Systems.Application.Interfaces;
using PARS.Inhouse.Systems.Application.Services;
using PARS.Inhouse.Systems.Infrastructure.APIs;
using PARS.Inhouse.Systems.Infrastructure.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Configurações de serviços
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<OpcoesUrls>(builder.Configuration.GetSection("VExpense"));
builder.Services.Configure<OpcoesUrls>(builder.Configuration.GetSection("Integracao"));
builder.Services.Configure<OpcoesUrls>(builder.Configuration.GetSection("TokenApiKey"));

builder.Services.AddHttpClient<IVExpensesApi, VExpensesApi>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["VExpense:VExpenseReport"]);
});

builder.Services.AddHttpClient<IVExpensesService, VExpensesService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["TokenApiKey:Token"]);
});

builder.Services.AddHttpClient<IIntegracaoBimerService, IntegracaoBimerService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["Integracao:Bimer"]);
});

builder.Services.AddHttpClient<IAuthService, AuthService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["Integracao:TokenServico"]);
});

builder.Services.AddScoped<IIntegracaoBimerAPI, IntegracaoBimerAPI>();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IIntegracaoBimerService, IntegracaoBimerService>();
builder.Services.AddScoped<IVExpensesService, VExpensesService>();

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
