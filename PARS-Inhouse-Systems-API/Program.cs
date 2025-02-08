using Microsoft.OpenApi.Models;
using PARS.Inhouse.Systems.Application.Configurations;
using PARS.Inhouse.Systems.Application.Interfaces;
using PARS.Inhouse.Systems.Application.Services;
using PARS.Inhouse.Systems.Infrastructure.APIs;
using PARS.Inhouse.Systems.Infrastructure.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Configura��es de servi�os
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "PARS RJ",
        Version = "v1",
        Description = "Essa API � a principal para utiliza��o de outros EndPoints."
    });

    c.DocInclusionPredicate((_, apiDesc) => true);
    c.TagActionsBy(api => new List<string> { api.GroupName });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Por favor, insira 'Bearer' [espa�o] e ent�o seu token no campo abaixo.",
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
});

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

builder.Services.AddHttpClient<IIntegracaoBimerService, IntegracaoBimerService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["Integracao:TokenServico"]);
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