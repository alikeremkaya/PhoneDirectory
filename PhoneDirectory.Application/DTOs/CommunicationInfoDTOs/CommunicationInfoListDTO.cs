using PhoneDirectory.Domain.Enums;

namespace PhoneDirectory.Application.DTOs.CommunicationInfoDTOs;

public  class CommunicationInfoListDTO
{
    public Guid Id { get; set; }
    public Guid PersonId { get; set; }
    public string PersonFullName { get; set; }
    public ContactInfoType InfoType { get; set; }
    public string InfoContent { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
}
