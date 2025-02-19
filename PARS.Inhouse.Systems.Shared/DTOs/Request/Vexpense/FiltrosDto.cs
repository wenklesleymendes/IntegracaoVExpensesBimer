using PARS.Inhouse.Systems.Shared.Enums.Vexpenses;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace PARS.Inhouse.Systems.Application.DTOs.Request.Vexpense
{
    public class FiltrosDto
    {
        [DefaultValue(FiltroInclude.Expenses)]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public FiltroInclude Include { get; set; } = FiltroInclude.Expenses;

        [DefaultValue("")]
        public string? Search { get; set; } = "";

        [DefaultValue(FiltroSearchField.ApprovalDateBetween)]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public FiltroSearchField SearchField { get; set; } = FiltroSearchField.ApprovalDateBetween;

        [DefaultValue(FiltroSearchJoin.And)]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public FiltroSearchJoin SearchJoin { get; set; } = FiltroSearchJoin.And;

        public string ConstruirFiltro()
        {
            return $"searchFields={FormatarCampo(SearchField)}&searchJoin={FormatarCampo(SearchJoin)}";
        }

        private string FormatarCampo<T>(T campo) where T : Enum
        {
            var nome = campo.ToString().ToLower();
            return nome.Replace("_", ":");
        }
    }
}