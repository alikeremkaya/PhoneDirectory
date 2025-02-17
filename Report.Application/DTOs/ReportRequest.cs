namespace Report.Application.DTOs
{
    public class ReportRequest
    {
        public Guid PersonId { get; set; }
        public string Location { get; set; }
        public DateTime RequestedDate { get; set; }
    }
}
