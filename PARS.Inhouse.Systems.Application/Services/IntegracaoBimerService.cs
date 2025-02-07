using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PARS.Inhouse.Systems.Application.Configurations;
using PARS.Inhouse.Systems.Application.DTOs;
using PARS.Inhouse.Systems.Application.DTOs.Response;
using PARS.Inhouse.Systems.Application.Interfaces;
using PARS.Inhouse.Systems.Infrastructure.APIs;
using PARS.Inhouse.Systems.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PARS.Inhouse.Systems.Application.Services
{
    public class IntegracaoBimerService: IIntegracaoBimerService
    {
        private readonly OpcoesUrls _options;
        private readonly HttpClient _httpClient;
        private readonly IIntegracaoBimerAPI _integracaoBimerAPI;
        public IntegracaoBimerService(IIntegracaoBimerAPI integracaoBimerAPI, IOptions<OpcoesUrls> options, HttpClient httpClient) 
        {
            _integracaoBimerAPI = integracaoBimerAPI;
            _options = options?.Value;
            _httpClient = httpClient;
        }

        public async Task<TitlePayResponseDto?> CriarTituloAPagar(BimerRequestDto bimerRequestDto, string token)
        {
            try
            {
                var uri = _options.Bimer;
                var content = JsonConvert.SerializeObject(bimerRequestDto, Formatting.Indented);
                var reports = await _integracaoBimerAPI.CriarTituloAPagar(content, uri, token);
                return System.Text.Json.JsonSerializer.Deserialize<TitlePayResponseDto>(reports, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (Exception ex)
            {
                throw new Exception($"Ocorreu um erro durante a criação de Titulo a pagar! Detalhes do erro: {ex.Message}");
            }
        }

        public async Task<AuthResponseDto> AuthenticateAsync(AuthRequestDto requestDto)
        {
            var uri = _options.TokenServico;
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("client_id", requestDto.client_id),
                new KeyValuePair<string, string>("client_secret", requestDto.client_secret),
                new KeyValuePair<string, string>("grant_type", requestDto.grant_type),
                new KeyValuePair<string, string>("username", requestDto.username),
                new KeyValuePair<string, string>("password", requestDto.password),
                new KeyValuePair<string, string>("nonce", requestDto.nonce)
            });

            var response = _integracaoBimerAPI.AuthenticateAsync(content, uri);

            return System.Text.Json.JsonSerializer.Deserialize<AuthResponseDto>(await response, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<AuthResponseDto> ReauthenticateAsync(ReauthenticateRequestDto request)
        {
            var uri = _options.TokenServico;
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("client_id", request.client_id),
                new KeyValuePair<string, string>("grant_type", request.grant_type),
                new KeyValuePair<string, string>("refresh_token", request.refresh_token)
            }) ;

            var response = _integracaoBimerAPI.ReauthenticateAsync(content, uri);

            return System.Text.Json.JsonSerializer.Deserialize<AuthResponseDto>(await response, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }
}