namespace Report.Application.DTOs;

public class ReportListDTO
{
    public Guid Id { get; set; }
    public DateTime RequestDate { get; set; }
    public string Status { get; set; }
    public string Location { get; set; }

}
