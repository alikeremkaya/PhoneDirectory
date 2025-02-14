using PhoneDirectory.Application.DTOs.CommunicationInfoDTOs;
using PhoneDirectory.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace PhoneDirectory.Application.DTOs.PersonDTOs;

public class PersonCreateDTO
{
    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }

    public string CompanyName { get; set; }
    public List<CommunicationInfoDTO> CommunicationInfos { get; set; } = new();
    public ContactInfoType InfoType { get; set; }

}
