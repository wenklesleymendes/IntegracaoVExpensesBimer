using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PARS.Inhouse.Systems.Domain.Entities;
using PARS.Inhouse.Systems.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace PARS.Inhouse.Systems.Infrastructure.APIs
{
    public class IntegracaoBimerAPI: IIntegracaoBimerAPI
    {
        private readonly HttpClient _httpClient;

        public IntegracaoBimerAPI(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> CriarTituloAPagar(string bimerRequest, string uri, string token)
        {
            var content = new StringContent(bimerRequest, Encoding.UTF8, "application/json");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PostAsync(uri, content);

            if (response.IsSuccessStatusCode)
            {
                var message = await response.Content.ReadAsStringAsync();

                return (message);
            }
            else
            {
                var erroMessage = await response.Content.ReadAsStringAsync();

                return (erroMessage);
            }
        }
    }
}
