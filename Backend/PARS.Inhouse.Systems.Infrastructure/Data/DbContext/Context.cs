using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PARS.Inhouse.Systems.Domain.Entities.Templates;
using PARS.Inhouse.Systems.Domain.Entities.vexpense;
using PARS.Inhouse.Systems.Domain.Entities.Vexpense;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PARS.Inhouse.Systems.Infrastructure.Data.DbContext
{
    public class Context : IdentityDbContext<IdentityUser>
    {
        public Context(DbContextOptions<Context> options)
            : base(options)
        {
        }

        public DbSet<TemplatesPlanilha> TemplatesPlanilha { get; set; } = default!;
        public DbSet<TemplatePlanilhaCampos> TemplatesPlanilhaCampos { get; set; } = default!;
        public DbSet<TitulosAprovados> TitulosAprovados { get; set; } = default!;
        public DbSet<TituloAprovadoDespesa> TituloAprovadoDespesa { get; set; } = default!;
    }
}
