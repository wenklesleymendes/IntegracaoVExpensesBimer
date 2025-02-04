using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PARS.Inhouse.Systems.Application.Configurations;
using System.Text.Json;

public class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<AuthService> _logger;
    private readonly OpcoesUrls _options;

    public AuthService(HttpClient httpClient, ILogger<AuthService> logger, IOptions<OpcoesUrls> options)
    {
        _httpClient = httpClient;
        _logger = logger;
        _options = options?.Value;
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
            new KeyValuePair<string, string>("password", requestDto.Password)
        });

        var response = await _httpClient.PostAsync(uri, content);
        var responseString = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("Erro na autenticação: {Response}", responseString);
        }

        return JsonSerializer.Deserialize<AuthResponseDto>(responseString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }
}
