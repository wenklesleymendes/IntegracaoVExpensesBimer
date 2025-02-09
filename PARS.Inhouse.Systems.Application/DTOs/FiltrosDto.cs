using PARS.Inhouse.Systems.Shared.Enums;

namespace PARS.Inhouse.Systems.Application.DTOs
{
    public class FiltrosDto
    {
        public string? Include { get; set; }
        public string? Search { get; set; }
        public FiltroSearchField SearchField { get; set; } = FiltroSearchField.APPROVAL_DATE_BETWEEN;
        public FiltroSearchJoin SearchJoin { get; set; } = FiltroSearchJoin.AND;
    }
}
