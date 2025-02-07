using System.ComponentModel.DataAnnotations;

namespace PhoneDirectory.Application.DTOs.PersonDTOs;

public class PersonCreateDTO
{
    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }

    public string CompanyName { get; set; }
}
