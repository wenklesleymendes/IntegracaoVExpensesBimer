using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PARS.Inhouse.Systems.Application.Configurations;
using PARS.Inhouse.Systems.Application.DTOs;
using PARS.Inhouse.Systems.Application.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
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
            [FromQuery, DefaultValue("APROVADO"), SwaggerSchema("Status padrão: ABERTO")] string status,
            [FromQuery] FiltrosDto filtros
            )
        {
            try
            {
                // Valores permitidos e padrão
                var allowedStatuses = new[] { "ABERTO", "APROVADO", "REPROVADO", "REABERTO", "PAGO", "ENVIADO" };

                if (!allowedStatuses.Contains(status))
                {
                    return BadRequest(new { Message = $"O status '{status}' não é permitido. Valores permitidos: {string.Join(", ", allowedStatuses)}" });
                }

                filtros.include ??= "expenses";
                filtros.search ??= string.Empty;
                filtros.searchField ??= "approval_date:between";
                filtros.searchJoin ??= "and";

                var token = _options.Token;

                var reports = await _vExpensesService.GetReportsByStatusAsync(status, filtros, token);
                return Ok(reports);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        //[HttpGet("Verify")]
        //public async Task<IActionResult> VerifyAuthentication()
        //{
        //    var token = _options.Token;
        //    try
        //    {
        //        _vExpensesService.TokenValidation(token);
        //        return Ok("Validação realizada com êxito!");
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new { Message = ex.Message });
        //    }
        //}
    }
}