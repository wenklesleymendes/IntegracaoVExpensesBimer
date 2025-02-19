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

                return result?.data?.Select(static dto => ReportDto.Create(
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

        public async Task<IReadOnlyList<ReportDto>> BuscarRelatorioPorStatusPagoAsync(string uri, string token)
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

                return result?.data?.Select(static dto => ReportDto.Create(
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

        private async Task SalvarArquivoJson(string caminhoArquivo, object dados)
        {
            var json = JsonConvert.SerializeObject(dados, Formatting.Indented, new JsonSerializerSettings
            {
                Converters = { new StringEnumConverter() }
            });

            await File.WriteAllTextAsync(caminhoArquivo, json);
        }

        public class ApiResponse
        {
            public List<ReportDto>? data { get; set; }
        }
    }
}
