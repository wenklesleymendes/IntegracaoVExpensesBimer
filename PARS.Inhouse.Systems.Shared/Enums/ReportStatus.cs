using System.ComponentModel;
using System.Text.Json.Serialization;

namespace PARS.Inhouse.Systems.Shared.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ReportStatus
    {
        ABERTO,
        APROVADO,
        REPROVADO,
        REABERTO,
        PAGO,
        ENVIADO
    }
}
