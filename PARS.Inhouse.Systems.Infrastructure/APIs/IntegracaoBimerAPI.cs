using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PARS.Inhouse.Systems.Domain.Entities;
using PARS.Inhouse.Systems.Domain.Exceptions;
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
    public class IntegracaoBimerAPI : IIntegracaoBimerAPI
    {
        private readonly HttpClient _httpClient;

        public IntegracaoBimerAPI(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> CriarTituloAPagar(string bimerRequest, string uri, string token)
        {
            try
            {
                var content = new StringContent(bimerRequest, Encoding.UTF8, "application/json");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.PostAsync(uri, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new BusinessException($"Erro ao criar título a pagar: {responseContent}");
                }

                return responseContent;
            }
            catch (HttpRequestException httpEx)
            {
                throw new BusinessException($"Erro na comunicação com a API do Bimer: {httpEx.Message}");
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Erro inesperado ao criar título a pagar: {ex.Message}");
            }
        }

        public async Task<string> ReauthenticateAsync(FormUrlEncodedContent content, string uri)
        {
            try
            {
                var response = await _httpClient.PostAsync(uri, content);
                var responseString = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new BusinessException($"Erro ao renovar o token: {responseString}");
                }

                return responseString;
            }
            catch (HttpRequestException httpEx)
            {
                throw new BusinessException($"Erro na comunicação ao renovar o token: {httpEx.Message}");
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Erro inesperado ao renovar o token: {ex.Message}");
            }
        }

        public async Task<string> AuthenticateAsync(FormUrlEncodedContent content, string uri)
        {
            try
            {
                var response = await _httpClient.PostAsync(uri, content);
                var responseString = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new BusinessException($"Erro na autenticação: {responseString}");
                }

                return responseString;
            }
            catch (HttpRequestException httpEx)
            {
                throw new BusinessException($"Erro na comunicação durante a autenticação: {httpEx.Message}");
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Erro inesperado na autenticação: {ex.Message}");
            }
        }
    }
}
