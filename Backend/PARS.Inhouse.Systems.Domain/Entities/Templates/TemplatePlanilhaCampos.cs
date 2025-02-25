using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PARS.Inhouse.Systems.Domain.Entities.Templates
{
    public class TemplatePlanilhaCampos
    {
        [Key]
        public int Id { get; set; }

        public int TemplatesPlanilhaId { get; set; }

        [ForeignKey("TemplatesPlanilhaId")]
        public TemplatesPlanilha TemplatesPlanilha { get; set; }

        public string ColunaPlanilha { get; set; }
        public string ColunaBanco { get; set; }
        public string LetraColunaPlanilha { get; set; }
        public string CampoFixo { get; set; }
        public bool PreencherCampoEmBranco { get; set; }
        public bool PreencherCampoFixo { get; set; }
        public string NomePersonalizadoColunaPlanilha { get; set; }
    }
}
