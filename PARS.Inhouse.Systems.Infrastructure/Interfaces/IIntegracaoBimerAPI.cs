using PARS.Inhouse.Systems.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PARS.Inhouse.Systems.Infrastructure.Interfaces
{
    public interface IIntegracaoBimerAPI
    {
        Task<string> CriarTituloAPagar(string bimerRequest, string uri, string token);
    }
}
