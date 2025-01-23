using Microsoft.Extensions.Configuration;
using PARS.Inhouse.Systems.Application.Configurations;
using PARS.Inhouse.Systems.Application.Services;
using PARS.Inhouse.Systems.Infrastructure.APIs;

var builder = WebApplication.CreateBuilder(args);

// Configurações de serviços
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<OpcoesUrls>(builder.Configuration.GetSection("VExpense"));

builder.Services.AddHttpClient<IVExpensesApi, VExpensesApi>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["VExpense:VExpenseReport"]);
});
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
