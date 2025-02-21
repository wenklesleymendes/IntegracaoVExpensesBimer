using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PARS.Inhouse.Systems.Application.Configurations;
using PARS.Inhouse.Systems.Application.DTOs.Request.Vexpense;
using PARS.Inhouse.Systems.Application.Interfaces;
using PARS.Inhouse.Systems.Domain.Exceptions;
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
        /// <param name="status">status do relatório (APROVADO por padrão).</param>
        /// <param name="filtros">Filtros adicionais para consulta.</param>
        /// <returns>Lista de relatórios filtrados.</returns>
        [HttpGet("Relatorio")]
        public async Task<IActionResult> BuscarRelatorioPorStatus(
            [FromQuery, DefaultValue(ReportStatus.APROVADO)] ReportStatus status,
            [FromQuery] FiltrosDto filtros)
        {
            try
            {
                _logger.LogInformation("Buscando relatórios com status: {status}", status);

                string statusString = Enum.GetName(typeof(ReportStatus), status) ?? ReportStatus.APROVADO.ToString();

                var reports = await _vExpensesService.BuscarRelatorioPorStatusAsync(statusString, filtros);

                if (reports == null)
                {
                    _logger.LogWarning("Nenhum relatório encontrado para o status: {status}", status);
                    return NotFound(new { Message = "Nenhum relatório encontrado." });
                }

                return Ok(reports);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar relatórios com status: {status}", status);
                return StatusCode(500, new { Message = "Erro interno ao buscar relatórios.", Detalhes = ex.Message });
            }
        }

        [HttpPut("atualizaStatus")]
        public async Task<IActionResult> AtualizaStatusRelatorio([FromQuery] int? id, [FromBody] AtualizaStatusDto requestDto)
        {
            try
            {
                if (!id.HasValue || id == 0)
                {
                    return BadRequest(new { Message = "O id é obrigatório!" });
                }

                _logger.LogInformation($"Alterando status do relatório => {id}");

                var response = await _vExpensesService.AlterarStatus(id.Value, requestDto);
                return Ok(response);
            }
            catch (BusinessException ex)
            {
                _logger.LogWarning(ex, "Erro de negócio ao alterar status do relatório.");
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao alterar status do relatório.");
                return StatusCode(500, new { Message = "Erro interno no servidor." });
            }
        }
    }
}
