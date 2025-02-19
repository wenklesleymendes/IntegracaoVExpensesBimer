using PARS.Inhouse.Systems.Shared.DTOs.Request.Bimer;
using PARS.Inhouse.Systems.Shared.DTOs.Response.Bimer;

namespace PARS.Inhouse.Systems.Application.Interfaces
{
    public interface IIntegracaoBimerService
    {
        Task<TitlePayResponseDto?> CreateTitlePay(BimerRequestDto bimerRequestDto, string token);
        Task<AuthResponseDto> AuthenticateAsync(AuthRequestDto request);
        Task<AuthResponseDto> ReauthenticateAsync(ReauthenticateRequestDto request);
    }
}
