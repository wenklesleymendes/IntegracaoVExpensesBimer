using PARS.Inhouse.Systems.Shared.Enums.Vexpenses;

namespace PARS.Inhouse.Systems.Application.Configurations
{
    public class VexpenseFiltroDefaultsConfig
    {
        public string Include { get; set; } = FiltroInclude.expenses.ToString();
        public string Search { get; set; } = "";
        public string SearchField { get; set; } = FiltroSearchField.approval_date_between.ToString();
        public string SearchJoin { get; set; } = FiltroSearchJoin.and.ToString();
    }
}
