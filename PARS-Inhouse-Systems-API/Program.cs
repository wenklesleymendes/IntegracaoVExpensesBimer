using PARS.Inhouse.Systems.Application.Services;
using PARS.Inhouse.Systems.Infrastructure.APIs;

var builder = WebApplication.CreateBuilder(args);

// Configurações de serviços
builder.Services.AddControllers(); // Adiciona suporte para controladores (MVC)
builder.Services.AddEndpointsApiExplorer(); // Gera documentação OpenAPI para endpoints
builder.Services.AddSwaggerGen(); // Adiciona Swagger para documentação

// Injeção de dependências (DI)
builder.Services.AddHttpClient<IVExpensesApi, VExpensesApi>(client =>
{
    client.BaseAddress = new Uri("https://api.vexpenses.com/v2/"); // Base URL da API
    client.DefaultRequestHeaders.Add("Authorization", "Bearer SEU_TOKEN"); // Adicione seu token aqui
});
builder.Services.AddScoped<IVExpensesService, VExpensesService>();

var app = builder.Build();

// Configuração do pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers(); // Mapeia controladores para as rotas da API

app.Run();
