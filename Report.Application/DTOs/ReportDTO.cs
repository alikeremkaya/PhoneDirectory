namespace Report.Application.DTOs;

public class ReportDTO
{
    public Guid Id { get; set; }
    public DateTime RequestDate { get; set; }
    public string Status { get; set; }
    public string Location { get; set; }
    public int PersonCount { get; set; }
    public int PhoneNumberCount { get; set; }
}
