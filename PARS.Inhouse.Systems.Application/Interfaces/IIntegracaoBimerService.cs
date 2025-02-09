using PARS.Inhouse.Systems.Application.DTOs;
using PARS.Inhouse.Systems.Application.DTOs.Response;

namespace PARS.Inhouse.Systems.Application.Interfaces
{
    public interface IIntegracaoBimerService
    {
        Task<TitlePayResponseDto?> CriarTituloAPagar(BimerRequestDto bimerRequestDto, string token);
        Task<AuthResponseDto> AuthenticateAsync(AuthRequestDto request);
        Task<AuthResponseDto> ReauthenticateAsync(ReauthenticateRequestDto request);
    }
}
