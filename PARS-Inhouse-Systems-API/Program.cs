using Microsoft.OpenApi.Models;
using PARS.Inhouse.Systems.Application.Configurations;
using PARS.Inhouse.Systems.Application.Interfaces;
using PARS.Inhouse.Systems.Application.Services;
using PARS.Inhouse.Systems.Infrastructure.APIs;
using PARS.Inhouse.Systems.Infrastructure.Interfaces;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

ConfigureServices(builder.Services, configuration);

var app = builder.Build();

ConfigureMiddlewares(app);

app.Run();

/// <summary>
/// Configuração de serviços da aplicação.
/// </summary>
void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    services.AddMemoryCache();
    services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });

    ConfigureSwagger(services);

    services.Configure<OpcoesUrls>(configuration.GetSection("VExpense"));
    services.Configure<OpcoesUrls>(configuration.GetSection("Integracao"));
    services.Configure<VexpenseTokenApiKeyConfig>(configuration.GetSection("TokenApiKey"));
    services.Configure<VexpenseFiltroDefaultsConfig>(configuration.GetSection("FiltroDefaults"));

    services.AddHttpClient<IVExpensesApi, VExpensesApi>();
    services.AddHttpClient<IIntegracaoBimerAPI, IntegracaoBimerAPI>();

    services.AddScoped<IVExpensesService, VExpensesService>();
    services.AddHttpClient<IIntegracaoBimerService, IntegracaoBimerService>();
}

/// <summary>
/// Configuração do Swagger.
/// </summary>
void ConfigureSwagger(IServiceCollection services)
{
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen(c =>
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

        c.UseInlineDefinitionsForEnums();
    });
}

/// <summary>
/// Configuração dos middlewares.
/// </summary>
void ConfigureMiddlewares(WebApplication app)
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();
    app.MapControllers();
}
