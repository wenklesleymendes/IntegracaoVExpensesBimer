using PARS.Inhouse.Systems.Shared.Enums.Vexpenses;

namespace PARS.Inhouse.Systems.Application.Configurations
{
    public class VexpenseFiltroDefaultsConfig
    {
        public string Include { get; set; } = FiltroInclude.Expenses.ToString();
        public string Search { get; set; } = "";
        public string SearchField { get; set; } = FiltroSearchField.ApprovalDateBetween.ToString();
        public string SearchJoin { get; set; } = FiltroSearchJoin.And.ToString();
    }
}
