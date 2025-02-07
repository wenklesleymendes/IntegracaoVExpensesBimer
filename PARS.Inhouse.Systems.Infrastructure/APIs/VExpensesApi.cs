using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using PARS.Inhouse.Systems.Domain.Entities;
using PARS.Inhouse.Systems.Domain.Exceptions;
using PARS.Inhouse.Systems.Infrastructure.Interfaces;

namespace PARS.Inhouse.Systems.Infrastructure.APIs
{
    public class VExpensesApi : IVExpensesApi
    {
        private readonly HttpClient _httpClient;

        public VExpensesApi(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Report>> GetReportsByStatusAsync(string status, string filtrosJson, string token, string uri)
        {
            try
            {
                var filtros = JsonSerializer.Deserialize<Filtros>(filtrosJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (filtros == null)
                    throw new BusinessException("Os filtros fornecidos são inválidos.");

                using var requestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

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

                var response = await _httpClient.SendAsync(requestMessage);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new BusinessException($"Erro ao buscar relatórios: {response.StatusCode} - {responseContent}");
                }

                var result = JsonSerializer.Deserialize<ApiResponse<List<Report>>>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                });

                return result?.Data ?? new List<Report>();
            }
            catch (HttpRequestException httpEx)
            {
                throw new BusinessException($"Erro na comunicação com a API do VExpenses: {httpEx.Message}");
            }
            catch (JsonException jsonEx)
            {
                throw new BusinessException($"Erro ao processar resposta da API: {jsonEx.Message}");
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Erro inesperado ao buscar relatórios: {ex.Message}");
            }
        }

        private class ApiResponse<T>
        {
            public T? Data { get; set; }
        }
    }
}