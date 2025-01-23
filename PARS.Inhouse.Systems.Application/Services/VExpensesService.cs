﻿using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PARS.Inhouse.Systems.Application.Configurations;
using PARS.Inhouse.Systems.Application.DTOs;
using PARS.Inhouse.Systems.Infrastructure.APIs;
using System.Text.Json.Nodes;

namespace PARS.Inhouse.Systems.Application.Services
{
    public class VExpensesService : IVExpensesService
    {
        private readonly IVExpensesApi _vExpensesApi;
        private readonly OpcoesUrls _options;

        public VExpensesService(IVExpensesApi vExpensesApi, IOptions<OpcoesUrls> options)
        {
            _vExpensesApi = vExpensesApi;
            _options = options?.Value;
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
    }
}
