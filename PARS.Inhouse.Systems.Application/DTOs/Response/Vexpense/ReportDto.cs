﻿using PARS.Inhouse.Systems.Domain.Entities.vexpense;
using PARS.Inhouse.Systems.Shared.Enums;

namespace PARS.Inhouse.Systems.Application.DTOs.Response.Vexpense
{
    public class ReportDto
    {
        public int Id { get;  set; }
        public string? ExternalId { get; set; }
        public string? Description { get; set; } = string.Empty;
        public ReportStatus Status { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string? PdfLink { get; set; }
        public string? ExcelLink { get; set; }
        public int? UserId { get; set; }
        public int? DeviceId { get; set; }
        public int? ApprovalStageId { get; set; }
        public int? ApprovalUserId { get; set; }
        public int? PaymentMethodId { get; set; }
        public string? Observation { get; set; }
        public int? PayingCompanyId { get; set; }
        public bool On { get; set; }
        public string? Justification { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public ExpenseContainerDto? expenses { get; set; }
    }
}
