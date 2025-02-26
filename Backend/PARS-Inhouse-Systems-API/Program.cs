using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PARS.Inhouse.Systems.Application.Configurations;
using PARS.Inhouse.Systems.Application.Interfaces;
using PARS.Inhouse.Systems.Application.Services;
using PARS.Inhouse.Systems.Infrastructure.APIs;
using PARS.Inhouse.Systems.Infrastructure.Data.DbContext;
using PARS.Inhouse.Systems.Infrastructure.Interfaces;
using PARS_Inhouse_Systems_API.Config;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ParsInhouseContext") ?? throw new InvalidOperationException("Connection string 'ParsInhouseContext' not found.")));

builder.Services.ConfigureIdentity();
builder.Services.ConfigureJwtAuthentication(builder.Configuration);

// 🔹 Configuração de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var configuration = builder.Configuration;

// 🔹 Configuração de serviços
ConfigureServices(builder.Services, configuration);

var app = builder.Build();

// 🔹 Configuração dos middlewares
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

    // 🔹 Configuração do Swagger
    ConfigureSwagger(services);

    // 🔹 Registro de configurações via appsettings.json
    services.Configure<OpcoesUrls>(configuration.GetSection("VExpense"));
    services.Configure<OpcoesUrls>(configuration.GetSection("Integracao"));
    services.Configure<VexpenseTokenApiKeyConfig>(configuration.GetSection("TokenApiKey"));
    services.Configure<VexpenseFiltroDefaultsConfig>(configuration.GetSection("FiltroDefaults"));

    // 🔹 Configuração de clientes HTTP
    services.AddHttpClient<IVExpensesApi, VExpensesApi>();
    services.AddHttpClient<IIntegracaoBimerAPI, IntegracaoBimerAPI>();

    // 🔹 Registro de dependências (IoC)
    services.AddScoped<IVExpensesService, VExpensesService>();
    services.AddHttpClient<IIntegracaoBimerService, IntegracaoBimerService>();
    services.AddAutoMapper(typeof(Program));
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

    // 🔹 Aplicação do CORS antes do mapeamento das rotas
    app.UseCors("CorsPolicy");

    // 🔹 Autenticação e autorização
    app.UseAuthorization();
    app.MapControllers();
}
