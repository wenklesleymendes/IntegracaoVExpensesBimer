using PARS.Inhouse.Systems.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PARS.Inhouse.Systems.Application.Interfaces
{
    public interface IIntegracaoBimerService
    {
        Task<AuthResponseDto> AuthenticateAsync(AuthRequestDto request);
        Task<string> CriarTituloAPagar(BimerRequestDto bimerRequestDto);
    }
}
