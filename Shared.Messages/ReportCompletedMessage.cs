namespace Shared.Messages;

public class ReportCompletedMessage
{
    public Guid ReportId { get; set; }
    public string Location { get; set; }
    public int PersonCount { get; set; }
    public int PhoneNumberCount { get; set; }
    public DateTime CompletedDate { get; set; }
}
