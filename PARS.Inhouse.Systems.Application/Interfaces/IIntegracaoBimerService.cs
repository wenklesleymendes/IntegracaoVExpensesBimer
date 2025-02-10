using PARS.Inhouse.Systems.Application.DTOs.Request.Bimer;
using PARS.Inhouse.Systems.Application.DTOs.Response.Bimer;

namespace PARS.Inhouse.Systems.Application.Interfaces
{
    public interface IIntegracaoBimerService
    {
        Task<TitlePayResponseDto?> CriarTituloAPagar(BimerRequestDto bimerRequestDto, string token);
        Task<AuthResponseDto> AuthenticateAsync(AuthRequestDto request);
        Task<AuthResponseDto> ReauthenticateAsync(ReauthenticateRequestDto request);
    }
}
