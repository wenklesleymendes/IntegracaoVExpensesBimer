using Application.DTO;
using Application.Interfaces.Relatorios;
using Application.Options;
using Microsoft.AspNetCore.Mvc;

namespace VExpenses.Controllers.Relatorio
{
    [ApiController]
    [Route("api/[controller]")]
    public class RelatorioController : ControllerBase
    {
        private readonly IRelatorioService _relatorioService;
        public RelatorioController(IRelatorioService relatorioService)
        {
            _relatorioService = relatorioService;
        }

        [HttpGet("/v2/reports/status/{status}")]
        public async Task<IActionResult> GetRelatorioPorStatus(/*string Search*/string SearchFields, string SearchJoin)
        {
            var request = new RelatorioDTO
            {
              //  search = Search,
                searchFields = SearchFields,
                searchJoin = SearchJoin
            };
            var tokenKey = "fS9ZjxxCHEUZbX6i5aOa0vB6yHhzEWMNyJ1CwWGAhi1pPny1ecXGAlxlYbgG";

            var resultado = await _relatorioService.BuscarRelatorioAsync(request, tokenKey);

            return Ok(resultado);
        }
    }
}