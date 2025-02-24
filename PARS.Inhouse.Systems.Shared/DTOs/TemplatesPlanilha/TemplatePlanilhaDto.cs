using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PARS.Inhouse.Systems.Shared.DTOs.TemplatesPlanilha
{
    public class TemplatePlanilhaDto
    {
        public string NomeTemplate { get; set; }
        public List<MapeamentoTemplatePlanilhaDto> DeParaMappings { get; set; }
    }
}
