using System.Net.Http;
using Newtonsoft.Json;
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

        public async Task<List<Report>> GetReportsByStatusAsync(string status)
        {
            var response = await _httpClient.GetAsync($"https://api.vexpenses.com/v2/reports/status/{status}");
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
