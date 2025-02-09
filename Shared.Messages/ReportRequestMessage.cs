namespace Shared.Messages;

public class ReportRequestMessage
{
    public Guid ReportId { get; set; }
    public DateTime RequestDate { get; set; }
}
