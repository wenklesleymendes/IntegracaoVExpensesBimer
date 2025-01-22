using Application.Interfaces.Relatorios;
using Application.Options;
using Application.Services.Relatorios;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Ensure the configuration file is loaded
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

void ConfigureServices(WebApplicationBuilder builder, IConfiguration configuration)
{
    builder.Services.Configure<OpcoesUrls>(configuration.GetSection("RelatorioService"));
    builder.Services.AddSingleton(provider =>
        provider.GetRequiredService<IOptions<OpcoesUrls>>().Value);

    var opcoesUrls = configuration.GetSection("RelatorioService").Get<OpcoesUrls>();
    if (string.IsNullOrEmpty(opcoesUrls?.VExpensesRelatorio))
    {
        throw new InvalidOperationException("A URL de VExpensesRelatorio está ausente ou não configurada corretamente no appsettings.json.");
    }

    builder.Services.AddHttpClient<IRelatorioService, RelatorioService>((provider, client) =>
    {
        var urls = provider.GetRequiredService<IOptions<OpcoesUrls>>().Value;
        if (string.IsNullOrEmpty(urls?.VExpensesRelatorio))
        {
            throw new InvalidOperationException("URL de VExpensesRelatorio não configurada corretamente.");
        }
        client.BaseAddress = new Uri(urls.VExpensesRelatorio);
    });

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddAuthorization();
}

ConfigureServices(builder, builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
