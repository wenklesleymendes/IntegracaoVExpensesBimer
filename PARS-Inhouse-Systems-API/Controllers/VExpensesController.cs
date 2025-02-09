using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PARS.Inhouse.Systems.Application.Configurations;
using PARS.Inhouse.Systems.Application.DTOs;
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
            _vExpensesService = vExpensesService;
            _options = options.Value;
        }

        [HttpGet("Relatorio")]
        public async Task<IActionResult> GetReportsByStatus(
            [FromQuery, DefaultValue(ReportStatus.APROVADO)] ReportStatus status,
            [FromQuery] FiltrosDto filtros
        )
        {
            try
            {
                filtros.Include ??= "expenses";
                filtros.Search ??= string.Empty;

                var token = _options.Token;

                var reports = await _vExpensesService.GetReportsByStatusAsync(status.ToString(), filtros, token);
                return Ok(reports);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }
    }
}