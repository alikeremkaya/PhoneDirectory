using PhoneDirectory.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace PhoneDirectory.Application.DTOs.CommunicationInfoDTOs;

public class CommunicationInfoUpdateDTO
{
    public Guid Id { get; set; }

    [Required]
    public ContactInfoType InfoType { get; set; }

    [Required]
    public string InfoContent { get; set; }
}
