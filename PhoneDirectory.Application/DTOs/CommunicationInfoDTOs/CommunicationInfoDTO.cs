using PhoneDirectory.Domain.Enums;

namespace PhoneDirectory.Application.DTOs.CommunicationInfoDTOs;

public class CommunicationInfoDTO
{
    
    public Guid PersonId { get; set; }
    public ContactInfoType InfoType { get; set; }
    public string InfoContent { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
}
