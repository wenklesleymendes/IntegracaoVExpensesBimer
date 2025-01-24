using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using PARS.Inhouse.Systems.Application.Configurations;
using PARS.Inhouse.Systems.Application.DTOs;
using PARS.Inhouse.Systems.Application.Services;

namespace PARS_Inhouse_Systems_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VExpensesController : ControllerBase
    {
        private readonly IVExpensesService _vExpensesService;
        private readonly OpcoesUrls _options;

        public VExpensesController(IVExpensesService vExpensesService, IOptions<OpcoesUrls> options)
        {
            _vExpensesService = vExpensesService;
            _options = options?.Value;
        }

        [HttpGet("reports")]
        public async Task<IActionResult> GetReportsByStatus(string status, [FromQuery] FiltrosDto filtros)
        {
            var token = _options.Token;
            try
            {
                var reports = await _vExpensesService.GetReportsByStatusAsync(status, filtros, token);
                return Ok(reports);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [HttpGet("Verify")]
        public async Task<IActionResult> VerifyAuthentication()
        {
            var token = _options.Token;
            try
            {
                _vExpensesService.TokenValidation(token);
                return Ok("Validação realizada com êxito!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }
    }
}