using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PARS.Inhouse.Systems.Application.Configurations;
using PARS.Inhouse.Systems.Application.DTOs;
using PARS.Inhouse.Systems.Application.Interfaces;
using PARS.Inhouse.Systems.Infrastructure.APIs;
using PARS.Inhouse.Systems.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PARS.Inhouse.Systems.Application.Services
{
    public class IntegracaoBimerService: IIntegracaoBimerService
    {
        private readonly OpcoesUrls _options;
        private readonly HttpClient _httpClient;
        private readonly IIntegracaoBimerAPI _integracaoBimerAPI;
        public IntegracaoBimerService(IIntegracaoBimerAPI integracaoBimerAPI, IOptions<OpcoesUrls> options, HttpClient httpClient) 
        {
            _integracaoBimerAPI = integracaoBimerAPI;
            _options = options?.Value;
            _httpClient = httpClient;
        }

        public async Task<string> CriarTituloAPagar(BimerRequestDto bimerRequestDto)
        {
            try
            {
                var uri = _options.Bimer;
                var content = JsonConvert.SerializeObject(bimerRequestDto, Formatting.Indented);
                var reports = await _integracaoBimerAPI.CriarTituloAPagar(content, uri);
                return (reports);
            }
            catch (Exception ex)
            {
                throw new Exception($"Ocorreu um erro durante a criação de Titulo a pagar! Detalhes do erro: {ex.Message}");
            }
        }
    }
}
