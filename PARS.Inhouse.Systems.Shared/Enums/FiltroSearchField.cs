using System.Text.Json.Serialization;

namespace PARS.Inhouse.Systems.Shared.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum FiltroSearchField
    {
        approval_date_between,
        payment_date_between,
        created_at
    }
}
