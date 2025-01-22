using Application.DTO;
using Application.Interfaces.Relatorios;
using Application.Options;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Relatorios
{
    public class RelatorioService : IRelatorioService
    {
        private readonly OpcoesUrls _options;
        private readonly HttpClient _httpClient;

        public RelatorioService(HttpClient httpClient, IOptions<OpcoesUrls> options)
        {
            _httpClient = httpClient;
            _options = options?.Value;
        }

        public async Task<string> BuscarRelatorioAsync(RelatorioDTO request, string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                throw new ArgumentException("Token de autenticação está vazio.", nameof(token));
            }

            try
            {
                // Obtém o endpoint base
                var endpoint = _options.VExpensesRelatorio;

                // Constrói a query string a partir do objeto request
                var queryParams = new List<string>();
                if (!string.IsNullOrEmpty(request.searchFields)) queryParams.Add($"searchFields={Uri.EscapeDataString(request.searchFields)}");
                if (!string.IsNullOrEmpty(request.searchJoin)) queryParams.Add($"searchJoin={Uri.EscapeDataString(request.searchJoin)}");
                //if (request.Param3.HasValue) queryParams.Add($"param3={request.Param3.Value}");

                var queryString = string.Join("&", queryParams);
                var urlWithParams = string.IsNullOrWhiteSpace(queryString) ? endpoint : $"{endpoint}?{queryString}";

                // Configura o cabeçalho Authorization
                using var requestMessage = new HttpRequestMessage(HttpMethod.Get, urlWithParams);
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue(token);

                // Envia a requisição
                var response = await _httpClient.SendAsync(requestMessage);

                // Verifica a resposta
                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Código: {response.StatusCode}, Motivo: {response.RequestMessage}");
                }

                // Lê o conteúdo da resposta
                var jsonResgatado = await response.Content.ReadAsStringAsync();
                return JsonConvert.SerializeObject(jsonResgatado);
            }
            catch (Exception ex)
            {
                throw new Exception($"Ocorreu um erro ao buscar o relatório! Detalhe do erro: {ex.Message}");
            }
        }

    }
}