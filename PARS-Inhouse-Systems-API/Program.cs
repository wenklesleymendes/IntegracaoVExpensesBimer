using Microsoft.OpenApi.Models;
using PARS.Inhouse.Systems.Application.Configurations;
using PARS.Inhouse.Systems.Application.Interfaces;
using PARS.Inhouse.Systems.Application.Services;
using PARS.Inhouse.Systems.Infrastructure.APIs;
using PARS.Inhouse.Systems.Infrastructure.Interfaces;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMemoryCache();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); // 🔹 Faz a API retornar enums como string
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "PARS RJ",
        Version = "v1",
        Description = "Essa API é a principal para utilização de outros EndPoints."
    });

    c.DocInclusionPredicate((_, apiDesc) => true);
    c.TagActionsBy(api => new List<string> { api.GroupName ?? string.Empty });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Por favor, insira 'Bearer' [espaço] e então seu token no campo abaixo.",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });

    c.UseInlineDefinitionsForEnums(); // 🔹 Faz o Swagger exibir enums como strings
});

builder.Services.Configure<OpcoesUrls>(builder.Configuration.GetSection("VExpense"));
builder.Services.Configure<OpcoesUrls>(builder.Configuration.GetSection("Integracao"));
builder.Services.Configure<OpcoesUrls>(builder.Configuration.GetSection("TokenApiKey"));

builder.Services.AddHttpClient<IVExpensesApi, VExpensesApi>(client =>
{
    var baseUrl = builder.Configuration["VExpense:VExpenseReport"];
    if (string.IsNullOrEmpty(baseUrl)) throw new Exception("Base URL para VExpense não encontrada!");
    client.BaseAddress = new Uri(baseUrl);
});

builder.Services.AddScoped<IIntegracaoBimerAPI, IntegracaoBimerAPI>();
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