using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PARS.Inhouse.Systems.Application.Configurations;
using PARS.Inhouse.Systems.Application.DTOs;
using PARS.Inhouse.Systems.Application.DTOs.Response;
using PARS.Inhouse.Systems.Application.Interfaces;
using PARS.Inhouse.Systems.Infrastructure.Interfaces;
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

        public async Task<TitlePayResponseDto?> CriarTituloAPagar(BimerRequestDto bimerRequestDto, string token)
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
                    new KeyValuePair<string, string>("client_id", requestDto.ClientId),
                    new KeyValuePair<string, string>("client_secret", requestDto.ClientSecret),
                    new KeyValuePair<string, string>("grant_type", requestDto.GrantType),
                    new KeyValuePair<string, string>("username", requestDto.Username),
                    new KeyValuePair<string, string>("password", requestDto.Password),
                    new KeyValuePair<string, string>("nonce", requestDto.Nonce)
                });

            var response = await _integracaoBimerAPI.AuthenticateAsync(content, uri);
            var responseString = response;

            var authResponse = System.Text.Json.JsonSerializer.Deserialize<AuthResponseDto>(responseString, _jsonSerializerOptions);
            return authResponse ?? new AuthResponseDto
            {
                AccessToken = string.Empty,
                TokenType = string.Empty,
                ExpiresIn = 0,
                RefreshToken = string.Empty,
                Username = string.Empty
            };
        }

        public async Task<AuthResponseDto> ReauthenticateAsync(ReauthenticateRequestDto request)
        {
            var uri = _options.TokenServico;
            var content = new FormUrlEncodedContent(new[]
            {
                        new KeyValuePair<string, string>("client_id", request.ClientId),
                        new KeyValuePair<string, string>("grant_type", request.GrantType),
                        new KeyValuePair<string, string>("refresh_token", request.RefreshToken)
                    });

            var response = await _integracaoBimerAPI.ReauthenticateAsync(content, uri);
            var responseString = response;

            var authResponse = System.Text.Json.JsonSerializer.Deserialize<AuthResponseDto>(responseString, _jsonSerializerOptions);
            return authResponse ?? new AuthResponseDto
            {
                AccessToken = string.Empty,
                TokenType = string.Empty,
                ExpiresIn = 0,
                RefreshToken = string.Empty,
                Username = string.Empty
            };
        }
    }
}