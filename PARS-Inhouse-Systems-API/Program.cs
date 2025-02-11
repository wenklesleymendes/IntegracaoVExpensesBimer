using Microsoft.OpenApi.Models;
using PARS.Inhouse.Systems.Application.Configurations;
using PARS.Inhouse.Systems.Application.Interfaces;
using PARS.Inhouse.Systems.Application.Services;
using PARS.Inhouse.Systems.Infrastructure.APIs;
using PARS.Inhouse.Systems.Infrastructure.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Configura��o do appsettings.json e vari�veis de ambiente
builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
builder.Configuration.AddEnvironmentVariables();

var config = builder.Configuration;

// Leitura segura dos valores usando vari�veis de ambiente
string tokenApiKey = Environment.GetEnvironmentVariable("TOKEN_API_KEY") ?? config["TokenApiKey:Token"];
string bimerUrl = Environment.GetEnvironmentVariable("BIMER_URL") ?? config["Integracao:Bimer"];
string tokenServicoUrl = Environment.GetEnvironmentVariable("TOKEN_SERVICO_URL") ?? config["Integracao:TokenServico"];

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

// Configura��o das URLs de forma segura
builder.Services.Configure<OpcoesUrls>(builder.Configuration.GetSection("VExpense"));

// Registra clientes HTTP de forma segura
builder.Services.AddHttpClient<IVExpensesApi, VExpensesApi>(client =>
{
    client.BaseAddress = new Uri(config["VExpense:VExpenseReport"]);
});

builder.Services.AddHttpClient<IIntegracaoBimerService, IntegracaoBimerService>(client =>
{
    client.BaseAddress = new Uri(bimerUrl);
});

builder.Services.AddHttpClient<IIntegracaoBimerAPI, IntegracaoBimerAPI>(client =>
{
    client.BaseAddress = new Uri(tokenServicoUrl);
});

// Inje��o de depend�ncias
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
