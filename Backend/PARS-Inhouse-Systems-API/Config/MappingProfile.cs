using AutoMapper;
using PARS.Inhouse.Systems.Domain.Entities.Vexpense;
using PARS.Inhouse.Systems.Domain.Entities.Vexpense.Response;
using PARS.Inhouse.Systems.Shared.DTOs.Response.Vexpense;
using static PARS.Inhouse.Systems.Infrastructure.APIs.VExpensesApi;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PARS_Inhouse_Systems_API.Config
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ReportDto, TitulosAprovados>();
            CreateMap<ExpenseDto, TituloAprovadoDespesa>();
        }
    }
}
