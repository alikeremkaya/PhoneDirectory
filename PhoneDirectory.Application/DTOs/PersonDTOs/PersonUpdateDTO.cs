using System.ComponentModel.DataAnnotations;

namespace PhoneDirectory.Application.DTOs.PersonDTOs;

public class PersonUpdateDTO
{
    public Guid Id { get; set; }

    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }

    public string CompanyName { get; set; }
}
