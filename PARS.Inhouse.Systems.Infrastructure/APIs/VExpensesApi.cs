using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PARS.Inhouse.Systems.Application.DTOs.Request.Vexpense;
using PARS.Inhouse.Systems.Domain.Entities.vexpense;
using PARS.Inhouse.Systems.Domain.Entities.Vexpense.Response;
using PARS.Inhouse.Systems.Domain.Exceptions;
using PARS.Inhouse.Systems.Infrastructure.Interfaces;
using PARS.Inhouse.Systems.Shared.DTOs.Response.Vexpense;
using PARS.Inhouse.Systems.Shared.Enums.Vexpenses;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PARS.Inhouse.Systems.Infrastructure.APIs
{
    public class VExpensesApi : IVExpensesApi
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<VExpensesApi> _logger;

        public VExpensesApi(HttpClient httpClient, ILogger<VExpensesApi> logger)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IReadOnlyList<ReportDto>> BuscarRelatorioPorStatusAsync(string status, string uri, string token, FiltrosDto filtros)
        {
            try
            {
                var queryString = GerarQueryString(filtros);
                var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"{uri}?{queryString}");
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue(token);
                requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await _httpClient.SendAsync(requestMessage);

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new BusinessException("Não autorizado para acessar a API.");
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    throw new BusinessException($"Erro ao buscar relatórios: {response.StatusCode} - {responseContent}");
                }

                var result = System.Text.Json.JsonSerializer.Deserialize<ApiResponse>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                });

                var responseData = result?.data?.Select(static dto => ReportDto.Create(
                    dto.id,
                    dto.external_id,
                    dto.user_id,
                    dto.device_id,
                    dto.description,
                    dto.status,
                    dto.approval_stage_id,
                    dto.approval_user_id,
                    dto.approval_date,
                    dto.payment_date,
                    dto.payment_method_id,
                    dto.observation,
                    dto.paying_company_id,
                    dto.on,
                    dto.justification,
                    dto.pdf_link,
                    dto.excel_link,
                    dto.created_at,
                    dto.updated_at,
                    dto.expenses
                )).ToList() ?? new List<ReportDto>();

                if (status == "APROVADO")
                {
                    var json = JsonConvert.SerializeObject(responseData);

                    await AtualizarListasAprovados(json);
                }

                return responseData;
            }
            catch (HttpRequestException httpEx)
            {
                _logger.LogError(httpEx, "Erro na comunicação com a API do VExpenses.");
                throw new BusinessException($"Erro na comunicação com a API: {httpEx.Message}");
            }
            catch (System.Text.Json.JsonException jsonEx)
            {
                _logger.LogError(jsonEx, "Erro ao processar resposta JSON da API.");
                throw new BusinessException($"Erro ao processar resposta da API: {jsonEx.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao buscar relatórios.");
                throw new BusinessException($"Erro inesperado ao buscar relatórios: {ex.Message}");
            }
        }

        public async Task<IReadOnlyList<ReportDto>> BuscarRelatorioPorStatusPagoAsync(string status, string uri, string token)
        {
            try
            {
                var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"{uri}");
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue(token);
                requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await _httpClient.SendAsync(requestMessage);

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new BusinessException("Não autorizado para acessar a API.");
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    throw new BusinessException($"Erro ao buscar relatórios: {response.StatusCode} - {responseContent}");
                }

                var result = System.Text.Json.JsonSerializer.Deserialize<ApiResponse>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                });

                var responseData = result?.data?.Select(dto => ReportDto.Create(
                    dto.id,
                    dto.external_id,
                    dto.user_id,
                    dto.device_id,
                    dto.description,
                    dto.status,
                    dto.approval_stage_id,
                    dto.approval_user_id,
                    dto.approval_date,
                    dto.payment_date,
                    dto.payment_method_id,
                    dto.observation,
                    dto.paying_company_id,
                    dto.on,
                    dto.justification,
                    dto.pdf_link?.ToString(),
                    dto.excel_link?.ToString(),
                    dto.created_at,
                    dto.updated_at,
                    dto.expenses
                )).ToList() ?? new List<ReportDto>();
                
                var json = JsonConvert.SerializeObject(responseData);
                await AtualizarListasAprovados(json);

                return (responseData);
            }
            catch (HttpRequestException httpEx)
            {
                _logger.LogError(httpEx, "Erro na comunicação com a API do VExpenses.");
                throw new BusinessException($"Erro na comunicação com a API: {httpEx.Message}");
            }
            catch (System.Text.Json.JsonException jsonEx)
            {
                _logger.LogError(jsonEx, "Erro ao processar resposta JSON da API.");
                throw new BusinessException($"Erro ao processar resposta da API: {jsonEx.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao buscar relatórios.");
                throw new BusinessException($"Erro inesperado ao buscar relatórios: {ex.Message}");
            }
        }

        public async Task AvaliarListaDeReavaliacao(string responseData)
        {
            try
            {
                string caminhoPayload = Path.Combine(GetSolutionRootDirectory(), "PARS.Inhouse.Systems.Infrastructure", "Data", "Payload");
                string caminhoArquivoListaAprovado = Path.Combine(caminhoPayload, "ListaDeAprovados.json");
                string caminhoArquivoReavaliacao = Path.Combine(caminhoPayload, "ReavaliacaoAprovados.json");

                var novaListaAprovados = JsonConvert.DeserializeObject<List<VExpenseResponse>>(responseData) ?? new List<VExpenseResponse>();
                List<VExpenseResponse> listaAntiga = new();
                List<VExpenseResponse> listaRemovidos = new();

                if (File.Exists(caminhoArquivoListaAprovado))
                {
                    string jsonSalvo = await File.ReadAllTextAsync(caminhoArquivoListaAprovado);
                    listaAntiga = JsonConvert.DeserializeObject<List<VExpenseResponse>>(jsonSalvo) ?? new List<VExpenseResponse>();

                    listaRemovidos = listaAntiga.Where(antigo => !novaListaAprovados.Any(novo => novo.id == antigo.id)).ToList();
                }

                if (listaRemovidos.Any())
                {
                    // Carregar a lista de reavaliação existente, se houver
                    List<VExpenseResponse> listaReavaliacao = new();
                    if (File.Exists(caminhoArquivoReavaliacao))
                    {
                        string jsonReavaliacao = await File.ReadAllTextAsync(caminhoArquivoReavaliacao);
                        listaReavaliacao = JsonConvert.DeserializeObject<List<VExpenseResponse>>(jsonReavaliacao) ?? new List<VExpenseResponse>();
                    }

                    // Adicionar itens removidos, evitando duplicatas
                    foreach (var item in listaRemovidos)
                    {
                        if (!listaReavaliacao.Any(reav => reav.id == item.id))
                        {
                            listaReavaliacao.Add(item);
                        }
                    }

                    // Remover da lista de reavaliação os itens que já estão na nova lista de aprovados
                    listaReavaliacao = listaReavaliacao
                        .Where(reav => !novaListaAprovados.Any(aprov => aprov.id == reav.id && JsonConvert.SerializeObject(aprov) == JsonConvert.SerializeObject(reav)))
                        .ToList();

                    var jsonReavaliacaoAtualizada = JsonConvert.SerializeObject(listaReavaliacao, Formatting.Indented, new JsonSerializerSettings
                    {
                        Converters = { new StringEnumConverter() }
                    });

                    await File.WriteAllTextAsync(caminhoArquivoReavaliacao, jsonReavaliacaoAtualizada);
                }

                var jsonAtualizado = JsonConvert.SerializeObject(novaListaAprovados, Formatting.Indented, new JsonSerializerSettings
                {
                    Converters = { new StringEnumConverter() }
                });

                await File.WriteAllTextAsync(caminhoArquivoListaAprovado, jsonAtualizado);
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Erro ao atualizar lista de aprovados: {ex.Message}");
            }
        }

        public async Task AtualizarListasAprovados(string responseData)
        {
            try
            {
                string caminhoPayload = Path.Combine(GetSolutionRootDirectory(), "PARS.Inhouse.Systems.Infrastructure", "Data", "Payload", "Vexpenses");
                string caminhoArquivoListaPago = Path.Combine(caminhoPayload, "ListaDePagos.json");
                string caminhoArquivoReavaliacao = Path.Combine(caminhoPayload, "ReavaliacaoAprovados.json");

                var novaListaPago = JsonConvert.DeserializeObject<List<VExpenseResponse>>(responseData) ?? new List<VExpenseResponse>();
                List<VExpenseResponse> listaAntiga = new();
                List<VExpenseResponse> listaRemovidos = new();

                if (File.Exists(caminhoArquivoListaPago))
                {
                    string jsonSalvo = await File.ReadAllTextAsync(caminhoArquivoListaPago);
                    listaAntiga = JsonConvert.DeserializeObject<List<VExpenseResponse>>(jsonSalvo) ?? new List<VExpenseResponse>();

                    // Carregar a lista de reavaliação existente, se houver
                    List<VExpenseResponse> listaReavaliacao = new();
                    if (File.Exists(caminhoArquivoReavaliacao))
                    {
                        string jsonReavaliacao = await File.ReadAllTextAsync(caminhoArquivoReavaliacao);
                        listaReavaliacao = JsonConvert.DeserializeObject<List<VExpenseResponse>>(jsonReavaliacao) ?? new List<VExpenseResponse>();
                    }

                    var jsonReavaliacaoAtualizada = JsonConvert.SerializeObject(listaReavaliacao, Formatting.Indented, new JsonSerializerSettings
                    {
                        Converters = { new StringEnumConverter() }
                    });

                    await File.WriteAllTextAsync(caminhoArquivoReavaliacao, jsonReavaliacaoAtualizada);
                }

                var jsonAtualizado = JsonConvert.SerializeObject(novaListaPago, Formatting.Indented, new JsonSerializerSettings
                {
                    Converters = { new StringEnumConverter() }
                });

                await File.WriteAllTextAsync(caminhoArquivoListaPago, jsonAtualizado);
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Erro ao atualizar lista de aprovados: {ex.Message}");
            }
        }

        private string GerarQueryString(FiltrosDto filtros)
        {
            var queryParams = new Dictionary<string, string?>
            {
                { "include", filtros.Include.ToString() },
                { "search", filtros.Search },
                { "searchField", filtros.SearchField.ToString() },
                { "searchJoin", filtros.SearchJoin.ToString() }
            };

            return string.Join("&", queryParams
                .Where(q => !string.IsNullOrEmpty(q.Value))
                .Select(q => $"{q.Key}={Uri.EscapeDataString(q.Value!)}"));
        }

        private string GetSolutionRootDirectory()
        {
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            return Path.GetFullPath(Path.Combine(currentDirectory, "..", "..", "..", ".."));
        }

        public class ApiResponse
        {
            public List<ReportDto>? data { get; set; }
        }
    }
}
