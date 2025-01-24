using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PARS.Inhouse.Systems.Application.Configurations;
using PARS.Inhouse.Systems.Application.DTOs;
using PARS.Inhouse.Systems.Infrastructure.APIs;
using System.Net.Http.Headers;
using System;
using System.Text.Json.Nodes;
using System.Net.Http;

namespace PARS.Inhouse.Systems.Application.Services
{
    public class VExpensesService : IVExpensesService
    {
        private readonly IVExpensesApi _vExpensesApi;
        private readonly OpcoesUrls _options;
        private readonly HttpClient _httpClient;

        public VExpensesService(IVExpensesApi vExpensesApi, IOptions<OpcoesUrls> options, HttpClient httpClient)
        {
            _vExpensesApi = vExpensesApi;
            _options = options?.Value;
            _httpClient = httpClient;
        }

        public async Task<List<ReportDto>> GetReportsByStatusAsync(string status, FiltrosDto filtrosDto, string token)
        {
            var filtros = JsonConvert.SerializeObject(filtrosDto);

            var uri = _options.VExpenseReport.Replace("{status}", $"{status}");

            var reports = await _vExpensesApi.GetReportsByStatusAsync(status, filtros, token, uri);
            return reports.Select(r => new ReportDto
            {
                Id = r.Id,
                Description = r.Description,
                Status = r.Status,
                ApprovalDate = r.ApprovalDate,
                PdfLink = r.PdfLink,
                ExcelLink = r.ExcelLink
            }).ToList();
        }

        public void TokenValidation(string token)
        {
            try
            {
                if (string.IsNullOrEmpty(token))
                {
                    throw new ArgumentNullException("Token inválido!");
                }
               _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token);
            }
            catch (Exception ex)
            {
                throw new Exception($"Ocorreu um erro na validação do token! Detalhes: {ex.Message}");
            }
        }
    }
}
