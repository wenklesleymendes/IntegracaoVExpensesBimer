using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PARS.Inhouse.Systems.Application.Configurations;
using PARS.Inhouse.Systems.Application.DTOs;
using PARS.Inhouse.Systems.Application.Interfaces;
using PARS.Inhouse.Systems.Application.Services;

namespace PARS_Inhouse_Systems_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IntegracaoBimerController : Controller
    {
        private readonly IIntegracaoBimerService _integracaoBimerService;
        public IntegracaoBimerController(IIntegracaoBimerService integracaoBimerService)
        {
            _integracaoBimerService = integracaoBimerService;
        }

        [HttpPost("criarTituloPagar")]
        public async Task<IActionResult> CreateTitlePay([FromBody] BimerRequestDto bimerRequestDto)
        {
            try
            {
                var resultado = await _integracaoBimerService.CriarTituloAPagar(bimerRequestDto);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao criar o título a pagar! Detalhe: {ex.Message}");
            }
        }
    }
}