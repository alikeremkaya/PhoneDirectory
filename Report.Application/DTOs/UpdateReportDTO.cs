using Report.Domain.Enums;

namespace Report.Application.DTOs;

public class UpdateReportDTO
{
    public ReportStatus Status { get; set; }
    public int PersonCount { get; set; }
    public int PhoneNumberCount { get; set; }
}
