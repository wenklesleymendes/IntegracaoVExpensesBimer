using PARS.Inhouse.Systems.Domain.Entities.Vexpense.Response;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PARS.Inhouse.Systems.Domain.Entities.Vexpense
{
    public class TituloAprovadoDespesa
    {
        [Key]
        public int Id { get; set; }
        public int IdResponse { get; set; }

        [ForeignKey("TitulosAprovados")]
        public int IdTituloAprovado { get; set; }
        public int? user_id { get; set; }
        public int? expense_id { get; set; }
        public int? device_id { get; set; }
        public int? integration_id { get; set; }
        public int? external_id { get; set; }
        public string? mileage { get; set; }

        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? date { get; set; }
        public int? expense_type_id { get; set; }
        public int? payment_method_id { get; set; }
        public int? paying_company_id { get; set; }
        public int? course_id { get; set; }
        public string? reicept_url { get; set; }
        public decimal? value { get; set; }
        public string title { get; set; }
        public string validate { get; set; }
        public bool? reimbursable { get; set; }
        public string observation { get; set; }
        public int? rejected { get; set; }
        public bool? on { get; set; }
        public string? mileage_value { get; set; }
        public string original_currency_iso { get; set; }
        public decimal? exchange_rate { get; set; }
        public decimal? converted_value { get; set; }
        public string? converted_currency_iso { get; set; }

        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? created_at { get; set; }

        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? updated_at { get; set; }

        public TitulosAprovados TitulosAprovados { get; set; }
    }
}
