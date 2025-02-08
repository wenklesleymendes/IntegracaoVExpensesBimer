using System.Net.Http.Headers;
using Newtonsoft.Json;
using PARS.Inhouse.Systems.Domain.Entities;
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

        public async Task<List<Report>> GetReportsByStatusAsync(string status, string filtros, string token, string uri)
        {
            var content = JsonConvert.DeserializeObject<Filtros>(filtros);

            using var requestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue(token);

            var query = new Dictionary<string, string>
            {
                { "include", content.include },
                { "search", content.search },
                { "searchField", content.searchField },
                { "searchJoin", content.searchJoin }
            };

            var queryString = string.Join("&", query.Where(q => !string.IsNullOrEmpty(q.Value))
                                                    .Select(q => $"{q.Key}={Uri.EscapeDataString(q.Value)}"));
            requestMessage.RequestUri = new Uri($"{uri}?{queryString}");

            var response = await _httpClient.SendAsync(requestMessage);

            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ApiResponse>(responseContent);
            return result?.Data ?? new List<Report>();
        }


        private class ApiResponse
        {
            public List<Report>? Data { get; set; }
        }
    }
}
