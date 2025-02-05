using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PARS.Inhouse.Systems.Application.DTOs.Response
{
    public class TitlePayResponseDto
    {
        public List<Error> Erros { get; set; }
        public List<string> ListaObjetos { get; set; }
    }

    public class Error
    {
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public string PossibleCause { get; set; }
        public string StackTrace { get; set; }
    }
}
