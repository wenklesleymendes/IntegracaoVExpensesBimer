using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PARS.Inhouse.Systems.Application.Configurations;
using PARS.Inhouse.Systems.Application.DTOs;
using PARS.Inhouse.Systems.Application.Interfaces;
using PARS.Inhouse.Systems.Application.Services;

namespace PARS_Inhouse_Systems_API.Controllers
{
    [ApiController]
    [Route("api/Bimer")]
    [ApiExplorerSettings(GroupName = "Integração - Bimer")]
    public class IntegracaoBimerController : Controller
    {
        private readonly IIntegracaoBimerService _integracaoBimerService;
        public IntegracaoBimerController(IIntegracaoBimerService integracaoBimerService)
        {
            _integracaoBimerService = integracaoBimerService;
        }

        [HttpPost("TituloAPagar/criarTituloPagar")]
        public async Task<IActionResult> CreateTitlePay([FromQuery]string token, [FromBody] BimerRequestDto bimerRequestDto)
        {
            try
            {
                var resultado = await _integracaoBimerService.CriarTituloAPagar(bimerRequestDto, token);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao criar o título a pagar! Detalhe: {ex.Message}");
            }
        }

        [HttpPost("Autenticacao/CriarTokenUtilizacaoServico")]
        public async Task<IActionResult> Authenticate([FromBody] AuthRequestDto request)
        {
            var result = await _integracaoBimerService.AuthenticateAsync(request);
            if (!string.IsNullOrEmpty(result.error))
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("Autenticacao/ReautenticarTokenUtilizacaoServico")]
        public async Task<IActionResult> Reauthenticate([FromBody] ReauthenticateRequestDto request)
        {
            var result = await _integracaoBimerService.ReauthenticateAsync(request);
            if (!string.IsNullOrEmpty(result.error))
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}