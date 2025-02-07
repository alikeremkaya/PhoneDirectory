using PhoneDirectory.Application.DTOs.CommunicationInfoDTOs;

namespace PhoneDirectory.Application.DTOs.PersonDTOs;

public class PersonDTO
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string CompanyName { get; set; }
    public List<CommunicationInfoDTO> CommunicationInfos { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
}
