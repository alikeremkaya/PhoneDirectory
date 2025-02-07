namespace PhoneDirectory.Application.DTOs.PersonDTOs;

public class PersonUpdateDTO
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Company { get; set; }
}
