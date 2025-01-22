using Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Relatorios
{
    public interface IRelatorioService
    {
        Task<string> BuscarRelatorioAsync(RelatorioDTO request, string token);
    }
}