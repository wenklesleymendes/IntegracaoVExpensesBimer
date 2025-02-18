using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace PARS.Inhouse.Systems.Shared.Enums.Vexpenses
{
    /// <summary>
    /// Define os status possíveis para relatórios na integração VExpenses.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ReportStatus
    {
        /// <summary>
        /// O relatório está aberto.
        /// </summary>
        [EnumMember(Value = "ABERTO")]
        [Description("Relatório em aberto.")]
        Aberto,

        /// <summary>
        /// O relatório foi aprovado.
        /// </summary>
        [EnumMember(Value = "APROVADO")]
        [Description("Relatório aprovado.")]
        Aprovado,

        /// <summary>
        /// O relatório foi reprovado.
        /// </summary>
        [EnumMember(Value = "REPROVADO")]
        [Description("Relatório reprovado.")]
        Reprovado,

        /// <summary>
        /// O relatório foi reaberto.
        /// </summary>
        [EnumMember(Value = "REABERTO")]
        [Description("Relatório reaberto.")]
        Reaberto,

        /// <summary>
        /// O relatório foi pago.
        /// </summary>
        [EnumMember(Value = "PAGO")]
        [Description("Relatório pago.")]
        Pago,

        /// <summary>
        /// O relatório foi enviado.
        /// </summary>
        [EnumMember(Value = "ENVIADO")]
        [Description("Relatório enviado.")]
        Enviado
    }
}