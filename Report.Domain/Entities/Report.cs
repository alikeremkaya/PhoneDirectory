
using Report.Domain.Core.Base;
using Report.Domain.Enums;

namespace Report.Domain.Entities;

public class Report : AuditableEntity
{
    public string Location { get; set; }
    public int PersonCount { get; set; }
    public int PhoneNumberCount { get; set; }
    public ReportStatus ReportStatus { get; set; }
    public DateTime CompletedDate { get; set; }

    public Report()
    {
        ReportStatus = ReportStatus.Preparing;
    }
}
