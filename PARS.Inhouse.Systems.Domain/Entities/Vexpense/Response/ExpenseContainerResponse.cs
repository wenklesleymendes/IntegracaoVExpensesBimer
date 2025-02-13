using PARS.Inhouse.Systems.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PARS.Inhouse.Systems.Domain.Entities.Vexpense.Response
{
    public class ExpenseContainerResponse
    {
        public List<Expense> data { get; set; } = new();
    }

    public class Expense
    {
        public int? id { get; private set; }
        public int? user_id { get; private set; }
        public int? expense_id { get; private set; }
        public int? device_id { get; private set; }
        public int? integration_id { get; private set; }
        public int? external_id { get; private set; }
        public decimal? mileage { get; private set; }
        public DateTime? date { get; private set; }
        public int? expense_type_id { get; private set; }
        public int? payment_method_id { get; private set; }
        public int? paying_company_id { get; private set; }
        public int? course_id { get; private set; }
        public string? receipt_url { get; private set; }
        public int? value { get; private set; }
        public string? title { get; private set; } = string.Empty;
        public string? validate { get; private set; } = string.Empty;
        public bool? reimbursable { get; private set; }
        public string? observation { get; private set; } = string.Empty;
        public int? rejected { get; private set; }
        public bool? on { get; private set; }
        public decimal? mileage_value { get; private set; }
        public string? original_currency_iso { get; private set; } = "BRL";
        public decimal? exchange_rate { get; private set; } = 1;
        public decimal? converted_value { get; private set; }
        public string? converted_currency_iso { get; private set; } = "BRL";
        public DateTime? created_at { get; private set; }
        public DateTime? updated_at { get; private set; }

        public Expense()
        {
        }

        public static Expense Create(int? id, int? user_id, int? expense_id, int? device_id, int? integration_id, int? external_id, decimal? mileage, DateTime? date,
            int? expense_type_id, int? payment_method_id, int? paying_company_id, int? course_id, string? receipt_url, int? value, string title, string? validade,
            bool? reimbursable, string? observation, int? rejected, bool? on, decimal? mileage_value, string original_currency_iso,
            decimal? exchange_rate, decimal? converted_value, string converted_currency_iso, DateTime? created_at, DateTime? updated_at)
        {
            var expense = new Expense()
            {
                id = id,
                user_id = user_id,
                expense_id = expense_id,
                device_id = device_id,
                integration_id = integration_id,
                external_id = external_id,
                mileage = mileage,
                date = date ?? DateTime.UtcNow,
                expense_type_id = expense_type_id,
                payment_method_id = payment_method_id,
                paying_company_id = paying_company_id,
                course_id = course_id,
                receipt_url = receipt_url,
                value = value,
                title = title,
                validate = validade,
                reimbursable = reimbursable,
                observation = observation,
                rejected = rejected,
                on = on,
                mileage_value = mileage_value,
                original_currency_iso = original_currency_iso,
                exchange_rate = exchange_rate,
                converted_value = converted_value,
                converted_currency_iso = converted_currency_iso,
                created_at = created_at,
                updated_at = updated_at
            };


            return expense;
        }
    }
}
