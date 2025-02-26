using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PARS.Inhouse.Systems.Application.Services;
using PARS.Inhouse.Systems.Domain.Entities.Templates;
using PARS.Inhouse.Systems.Shared.DTOs.TemplatesPlanilha;

namespace PARS_Inhouse_Systems_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Templates Planilha")]
    [Authorize]
    public class TemplatesPlanilhaController : ControllerBase
    {
        private readonly MongoDbService _mongoDbService;

        public TemplatesPlanilhaController(MongoDbService mongoDbService)
        {
            _mongoDbService = mongoDbService;

        }

        [HttpPost]
        public async Task<IActionResult> CreateTemplate([FromBody] TemplatePlanilhaDto dto)
        {
            if (dto == null || dto.DeParaMappings == null || dto.DeParaMappings.Count == 0)
            {
                return BadRequest("Dados inválidos.");
            }

            await _mongoDbService.CriarOuAtualizarSchemaDinamicoAsync(dto.NomeTemplate, dto.DeParaMappings);


            return Ok(new { message = $"Schema para o template {dto.NomeTemplate} criado com sucesso." });
        }

        [HttpGet("listar-templates")]
        public async Task<IActionResult> ListarTemplates()
        {
            try
            {
                var templateCollections = await _mongoDbService.ListarTodasColecoesAsync();

                return Ok(templateCollections);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao buscar coleções: {ex.Message}");
            }
        }

        [HttpDelete("remover/{nomeTemplate}")]
        public async Task<IActionResult> RemoverTemplate(string nomeTemplate)
        {
            return await _mongoDbService.RemoverColecaoAsync(nomeTemplate);
        }

        [HttpGet("buscar/{nomeTemplate}")]
        public async Task<IActionResult> BuscarPorId(string nomeTemplate)
        {
            var document = await _mongoDbService.BuscarCamposPorNomeTemplateAsync(nomeTemplate);

            if (document == null)
            {
                return NotFound($"Documento com ID '{nomeTemplate}' não encontrado.");
            }

            return Ok(document);
        }
    }
}
