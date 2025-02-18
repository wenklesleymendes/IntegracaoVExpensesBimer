using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PARS.Inhouse.Systems.Application.Configurations;
using PARS.Inhouse.Systems.Application.DTOs.Request.Vexpense;
using PARS.Inhouse.Systems.Application.Interfaces;
using PARS.Inhouse.Systems.Shared.Enums.Vexpenses;
using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace PARS_Inhouse_Systems_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "Integração - VExpenses")]
    public class VExpensesController : ControllerBase
    {
        private readonly IVExpensesService _vExpensesService;
        private readonly OpcoesUrls _options;
        private readonly ILogger<VExpensesController> _logger;

        public VExpensesController(
            IVExpensesService vExpensesService,
            IOptionsSnapshot<OpcoesUrls> options,
            ILogger<VExpensesController> logger)
        {
            _vExpensesService = vExpensesService ?? throw new ArgumentNullException(nameof(vExpensesService));
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Obtém relatórios filtrados por status e parâmetros adicionais.
        /// </summary>
        /// <param name="status">Status do relatório (Aprovado por padrão).</param>
        /// <param name="filtros">Filtros adicionais para consulta.</param>
        /// <returns>Lista de relatórios filtrados.</returns>
        [HttpGet("Relatorio")]
        public async Task<IActionResult> BuscarRelatorioPorStatus(
            [FromQuery, DefaultValue(ReportStatus.APROVADO)] ReportStatus status,
            [FromQuery] FiltrosDto filtros)
        {
            try
            {
                _logger.LogInformation("Buscando relatórios com status: {Status}", status);

                string statusString = Enum.GetName(typeof(ReportStatus), status) ?? ReportStatus.APROVADO.ToString();

                var reports = await _vExpensesService.BuscarRelatorioPorStatusAsync(statusString, filtros);

                if (reports == null)
                {
                    _logger.LogWarning("Nenhum relatório encontrado para o status: {Status}", status);
                    return NotFound(new { Message = "Nenhum relatório encontrado." });
                }

                return Ok(reports);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar relatórios com status: {Status}", status);
                return StatusCode(500, new { Message = "Erro interno ao buscar relatórios.", Detalhes = ex.Message });
            }
        }
    }
}
