﻿using PARS.Inhouse.Systems.Application.DTOs;

namespace PARS.Inhouse.Systems.Application.Services
{
    public interface IVExpensesService
    {
        Task<List<ReportDto>> GetReportsByStatusAsync(string status, FiltrosDto filtrosDto, string token);
        void TokenValidation(string token);
    }
}
