namespace PhoneDirectory.Application.DTOs.CommunicationInfoDTOs;

public class CommunicationInfoDTO
{
    public Guid Id { get; set; }
    public Guid PersonId { get; set; }
    public string InfoType { get; set; }
    public string Content { get; set; }
}
