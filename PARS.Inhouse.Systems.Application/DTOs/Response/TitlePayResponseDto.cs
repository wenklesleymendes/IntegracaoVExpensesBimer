namespace PARS.Inhouse.Systems.Application.DTOs.Response
{
    public class TitlePayResponseDto
    {
        public required List<Error> Erros { get; set; }
        public required List<string> ListaObjetos { get; set; }
    }

    public class Error
    {
        public required string ErrorCode { get; set; }
        public required string ErrorMessage { get; set; }
        public required string PossibleCause { get; set; }
        public required string StackTrace { get; set; }
    }
}
