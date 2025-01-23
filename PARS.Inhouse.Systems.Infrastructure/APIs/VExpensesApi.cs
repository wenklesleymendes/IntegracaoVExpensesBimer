using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PARS.Inhouse.Systems.Domain.Entities;

namespace PARS.Inhouse.Systems.Infrastructure.APIs
{
    public class VExpensesApi : IVExpensesApi
    {
        private readonly HttpClient _httpClient;

        public VExpensesApi(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Report>> GetReportsByStatusAsync(string status, string token)
        {
            using var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"https://api.vexpenses.com/v2/reports/status/{status}");
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue(token);

            var response = await _httpClient.SendAsync(requestMessage);

           // var response = await _httpClient.GetAsync();
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ApiResponse>(content);
            return result?.Data ?? new List<Report>();
        }

        private class ApiResponse
        {
            public List<Report>? Data { get; set; }
        }
    }
}
