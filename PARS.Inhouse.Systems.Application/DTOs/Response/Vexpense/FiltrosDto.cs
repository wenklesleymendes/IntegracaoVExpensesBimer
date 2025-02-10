using PARS.Inhouse.Systems.Shared.Enums;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace PARS.Inhouse.Systems.Application.DTOs.Response.Vexpense
{
    public class FiltrosDto
    {
        [DefaultValue(FiltroInclude.expenses)]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public FiltroInclude Include { get; set; } = FiltroInclude.expenses;

        [DefaultValue("")]
        public string? Search { get; set; } = "";

        [DefaultValue(FiltroSearchField.approval_date_between)]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public FiltroSearchField SearchField { get; set; } = FiltroSearchField.approval_date_between;

        [DefaultValue(FiltroSearchJoin.and)]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public FiltroSearchJoin SearchJoin { get; set; } = FiltroSearchJoin.and;

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