using System.Text.Json.Serialization;

namespace PARS.Inhouse.Systems.Shared.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum FiltroSearchField
    {
        APPROVAL_DATE_BETWEEN,
        PAYMENT_DATE_BETWEEN,
        CREATED_AT
    }
}
