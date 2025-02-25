using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PARS.Inhouse.Systems.Domain.Entities.Templates
{
    public class TemplatesPlanilha
    {
        [Key]
        public int Id { get; set; }
        public string NomeTemplate { get; set; }
    }
}
