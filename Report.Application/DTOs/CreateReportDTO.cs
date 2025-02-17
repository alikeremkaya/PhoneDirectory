using PhoneDirectory.Domain.Enums;

namespace Report.Application.DTOs
{
    public class CreateReportDTO
    {
        public string Location { get; set; }
        public string Status { get; set; }
      
        public int PersonCount { get; internal set; }
        public int PhoneNumberCount { get; internal set; }
    }
}
