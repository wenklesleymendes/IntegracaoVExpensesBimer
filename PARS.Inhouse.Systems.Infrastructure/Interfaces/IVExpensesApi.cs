﻿using PARS.Inhouse.Systems.Domain.Entities.vexpense;

namespace PARS.Inhouse.Systems.Infrastructure.Interfaces
{
    public interface IVExpensesApi
    {
        Task<List<Report>> GetReportsByStatusAsync(string status, string filtros, string token, string uri);
    }
}