using PARS.Inhouse.Systems.Application.DTOs;
using PARS.Inhouse.Systems.Application.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PARS.Inhouse.Systems.Application.Interfaces
{
    public interface IIntegracaoBimerService
    {
        Task<TitlePayResponseDto?> CriarTituloAPagar(BimerRequestDto bimerRequestDto, string token);
        Task<AuthResponseDto> AuthenticateAsync(AuthRequestDto request);
        Task<AuthResponseDto> ReauthenticateAsync(ReauthenticateRequestDto request);
    }
}
