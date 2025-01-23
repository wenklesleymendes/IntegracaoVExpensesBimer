using Microsoft.AspNetCore.Mvc;
using PARS.Inhouse.Systems.Application.DTOs;
using PARS.Inhouse.Systems.Application.Services;

namespace PARS_Inhouse_Systems_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VExpensesController : ControllerBase
    {
        private readonly IVExpensesService _vExpensesService;

        public VExpensesController(IVExpensesService vExpensesService)
        {
            _vExpensesService = vExpensesService;
        }

        [HttpGet("reports")]
        public async Task<IActionResult> GetReportsByStatus(string status)
        {
                var token = "fS9ZjxxCHEUZbX6i5aOa0vB6yHhzEWMNyJ1CwWGAhi1pPny1ecXGAlxlYbgG";
            try
            {
                var reports = await _vExpensesService.GetReportsByStatusAsync(status, token);
                return Ok(reports);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }
    }
}
