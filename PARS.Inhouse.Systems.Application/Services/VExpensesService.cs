using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PARS.Inhouse.Systems.Application.Configurations;
using PARS.Inhouse.Systems.Application.DTOs;
using System.Net.Http.Headers;
using System;
using System.Text.Json.Nodes;
using System.Net.Http;
using PARS.Inhouse.Systems.Application.Interfaces;
using PARS.Inhouse.Systems.Infrastructure.Interfaces;

namespace PARS.Inhouse.Systems.Application.Services
{
    public class VExpensesService : IVExpensesService
    {
        private readonly IVExpensesApi _vExpensesApi;
        private readonly OpcoesUrls _options;
        private readonly HttpClient _httpClient;

        public VExpensesService(IVExpensesApi vExpensesApi, IOptionsSnapshot<OpcoesUrls> options, HttpClient httpClient)
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
                Status = r.Status.ToString(),
                ApprovalDate = r.ApprovalDate,
                PdfLink = r.PdfLink,
                ExcelLink = r.ExcelLink
            }).ToList();
        }
    }
}
