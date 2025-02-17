using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PARS.Inhouse.Systems.Application.Configurations;
using PARS.Inhouse.Systems.Application.DTOs.Request.Vexpense;
using PARS.Inhouse.Systems.Application.Interfaces;
using PARS.Inhouse.Systems.Shared.Enums;
using System.ComponentModel;

namespace PARS_Inhouse_Systems_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "Integração - VExpenses")]
    public class VExpensesController : ControllerBase
    {
        private readonly IVExpensesService _vExpensesService;
        private readonly OpcoesUrls _options;

        public VExpensesController(IVExpensesService vExpensesService, IOptions<OpcoesUrls> options)
        {
            _vExpensesService = vExpensesService ?? throw new ArgumentNullException(nameof(vExpensesService));
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        /// <summary>
        /// Obtém relatórios filtrados por status e parâmetros adicionais.
        /// </summary>
        [HttpGet("Relatorio")]
        public async Task<IActionResult> BuscarRelatoriPorStatus(
            [FromQuery, DefaultValue(ReportStatus.APROVADO)] ReportStatus status,
            [FromQuery] FiltrosDto filtros
        )
        {
            try
            {
                var reports = await _vExpensesService.BuscarRelatorioPorStatusAsync(
                    status.ToString().ToUpperInvariant(),
                    filtros
                );

                return Ok(reports);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Erro ao buscar relatórios.", Detalhes = ex.Message });
            }
        }
    }
}