using PARS.Inhouse.Systems.Domain.Entities.vexpense;
using PARS.Inhouse.Systems.Shared.Enums;
using PARS.Inhouse.Systems.Domain.Exceptions;
using PARS.Inhouse.Systems.Infrastructure.Interfaces;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Web;
using static PARS.Inhouse.Systems.Infrastructure.APIs.VExpensesApi;
using PARS.Inhouse.Systems.Domain.Entities.Vexpense.Response;
using PARS.Inhouse.Systems.Domain.Entities.Vexpense;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;

namespace PARS.Inhouse.Systems.Infrastructure.APIs
{
    public class VExpensesApi : IVExpensesApi
    {
        private readonly HttpClient _httpClient;

        public VExpensesApi(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Report>> BuscarRelatorioPorStatusAsync(string status, string filtrosJson, string token, string uri, bool statusPago)
        {
            try
            {
                using var requestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue(token);
                requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if (!statusPago)
                {
                    var parametros = HttpUtility.ParseQueryString(filtrosJson);
                    var filtros = new Filtros
                    {
                        include = parametros["include"],
                        search = parametros["search"],
                        searchField = parametros["searchFields"],
                        searchJoin = parametros["searchJoin"]
                    };

                    if (filtros == null)
                        throw new BusinessException("Os filtros fornecidos são inválidos.");

                    var queryParams = new Dictionary<string, string?>
                    {
                        { "include", filtros.include },
                        { "search", filtros.search },
                        { "searchField", filtros.searchField },
                        { "searchJoin", filtros.searchJoin }
                    };

                    var queryString = string.Join("&", queryParams
                        .Where(q => !string.IsNullOrEmpty(q.Value))
                        .Select(q => $"{q.Key}={Uri.EscapeDataString(q.Value!)}"));

                    requestMessage.RequestUri = new Uri($"{uri}?{queryString}");
                }

                var response = await _httpClient.SendAsync(requestMessage);
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

                var responseData = result?.data?.Select(dto => Report.Create(
                    dto.id,
                    dto.external_id,
                    dto.user_id,
                    dto.device_id,
                    dto.description,
                    dto.status,
                    dto.approval_stage_id,
                    dto.approval_user_id,
                    dto.GetApprovalDate(),
                    dto.GetPaymentDate(),
                    dto.payment_method_id,
                    dto.observation,
                    dto.paying_company_id,
                    dto.on,
                    dto.justification,
                    dto.pdf_link?.ToString(),
                    dto.excel_link?.ToString(),
                    dto.created_at,
                    dto.updated_at,
                    new ExpenseContainerResponse
                    {
                        data = dto.expenses?.data?.Select(exp => Expense.Create(
                        exp.id,
                        exp.user_id,
                        exp.expense_id,
                        exp.device_id,
                        exp.integration_id,
                        exp.external_id,
                        exp.mileage,
                        exp.date,
                        exp.expense_type_id,
                        exp.payment_method_id,
                        exp.paying_company_id,
                        exp.course_id,
                        exp.reicept_url,
                        exp.value,
                        exp.title,
                        exp.validate,
                        exp.reimbursable,
                        exp.observation,
                        exp.rejected,
                        exp.on,
                        exp.mileage_value,
                        exp.original_currency_iso,
                        exp.exchange_rate,
                        exp.converted_value,
                        exp.converted_currency_iso,
                        exp.created_at,
                        exp.updated_at
                        )).ToList() ?? new List<Expense>()
                    }
                )).ToList() ?? new List<Report>();

                if (status == "APROVADO")
                {
                    var json = JsonConvert.SerializeObject(responseData);

                    await AtualizarListasAprovados(json);
                }

                return responseData;
            }
            catch (HttpRequestException httpEx)
            {
                throw new BusinessException($"Erro na comunicação com a API do VExpenses: {httpEx.Message}");
            }
            catch (System.Text.Json.JsonException jsonEx)
            {
                throw new BusinessException($"Erro ao processar resposta da API: {jsonEx.Message}");
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Erro inesperado ao buscar relatórios: {ex.Message}");
            }
        }

        public async Task AtualizarListasAprovados(string responseData)
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


        public async Task SalvarListaAprovados(string responseData)
        {
            try 
            { 
                string caminhoPayload = Path.Combine(GetSolutionRootDirectory(), "PARS.Inhouse.Systems.Infrastructure", "Data", "Payload");
                string caminhoArquivoListaAprovado = Path.Combine(caminhoPayload, $"ListaDeAprovados.json");

                var arquivoAtualizado = JsonConvert.DeserializeObject<List<VExpenseResponse>>(responseData);
                bool houveAlteracao = false;

                if (File.Exists(caminhoArquivoListaAprovado))
                {
                    string jsonSalvo = await File.ReadAllTextAsync(caminhoArquivoListaAprovado);
                    var pedidoSalvo = JsonConvert.DeserializeObject<List<VExpenseResponse>>(jsonSalvo);

                    if (!JToken.DeepEquals(JToken.FromObject(arquivoAtualizado), JToken.FromObject(pedidoSalvo)))
                    {
                        houveAlteracao = true;
                    }

                    if (houveAlteracao)
                    {
                        var jsonAtualizado = JsonConvert.SerializeObject(arquivoAtualizado);
                        await File.WriteAllTextAsync(caminhoArquivoListaAprovado, jsonAtualizado);
                    }
                }
                else
                {
                    var jsonAtualizado = JsonConvert.SerializeObject(arquivoAtualizado, Formatting.Indented);
                    Directory.CreateDirectory(Path.GetDirectoryName(caminhoArquivoListaAprovado));
                    await File.WriteAllTextAsync(caminhoArquivoListaAprovado, jsonAtualizado);
                }
            }
            catch (System.Text.Json.JsonException jsonEx)
            {
                throw new BusinessException($"Erro ao processar resposta da API: {jsonEx.Message}");
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Erro inesperado ao buscar relatórios: {ex.Message}");
            }
        }
            
        private string GetSolutionRootDirectory()
        {
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            return Path.GetFullPath(Path.Combine(currentDirectory, "..", "..", "..", ".."));
        }

        public class ApiResponse
        {
            public List<VExpenseResponse>? data { get; set; }
        }
    }
}