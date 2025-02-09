using System.Text.Json.Serialization;

namespace PARS.Inhouse.Systems.Shared.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum FiltroSearchJoin
    {
        AND,
        OR
    }
}