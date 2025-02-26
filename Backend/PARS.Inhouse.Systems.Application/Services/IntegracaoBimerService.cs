using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PARS.Inhouse.Systems.Application.Configurations;
using PARS.Inhouse.Systems.Application.Interfaces;
using PARS.Inhouse.Systems.Infrastructure.Interfaces;
using PARS.Inhouse.Systems.Shared.DTOs.Request.Bimer;
using PARS.Inhouse.Systems.Shared.DTOs.Response.Bimer;
using System.Text.Json;

namespace PARS.Inhouse.Systems.Application.Services
{
    public class IntegracaoBimerService : IIntegracaoBimerService
    {
        private readonly OpcoesUrls _options;
        private readonly HttpClient _httpClient;
        private readonly IIntegracaoBimerAPI _integracaoBimerAPI;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public IntegracaoBimerService(IIntegracaoBimerAPI integracaoBimerAPI, IOptions<OpcoesUrls> options, HttpClient httpClient)
        {
            _integracaoBimerAPI = integracaoBimerAPI;
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            _httpClient = httpClient;
            _jsonSerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<TitlePayResponseDto?> CreateTitlePay(BimerRequestDto bimerRequestDto, string token)
        {
            try
            {
                var uri = _options.Bimer;
                var content = JsonConvert.SerializeObject(bimerRequestDto, Formatting.Indented);
                var reports = await _integracaoBimerAPI.CriarTituloAPagar(content, uri, token);
                return System.Text.Json.JsonSerializer.Deserialize<TitlePayResponseDto>(reports, _jsonSerializerOptions);
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

            var response = await _integracaoBimerAPI.AuthenticateAsync(content, uri);
            var responseString = response;

            var authResponse = System.Text.Json.JsonSerializer.Deserialize<AuthResponseDto>(responseString, _jsonSerializerOptions);
            return authResponse ?? new AuthResponseDto
            {
                access_token = string.Empty,
                token_type = string.Empty,
                expires_in = 0,
                refresh_token = string.Empty,
                username = string.Empty
            };
        }

        public async Task<AuthResponseDto> ReauthenticateAsync(ReauthenticateRequestDto request)
        {
            var uri = _options.TokenServico;
            var content = new FormUrlEncodedContent(new[]
            {
                        new KeyValuePair<string, string>("client_id", request.client_id),
                        new KeyValuePair<string, string>("grant_type", request.grant_type),
                        new KeyValuePair<string, string>("refresh_token", request.refresh_token)
                    });

            var response = await _integracaoBimerAPI.ReauthenticateAsync(content, uri);
            var responseString = response;

            var authResponse = System.Text.Json.JsonSerializer.Deserialize<AuthResponseDto>(responseString, _jsonSerializerOptions);
            return authResponse ?? new AuthResponseDto
            {
                access_token = string.Empty,
                token_type = string.Empty,
                expires_in = 0,
                refresh_token = string.Empty,
                username = string.Empty
            };
        }
    }
}