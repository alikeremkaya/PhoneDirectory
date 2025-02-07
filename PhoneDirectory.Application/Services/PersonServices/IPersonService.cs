using PhoneDirectory.Application.DTOs.PersonDTOs;
using PhoneDirectory.Domain.Utilities.Interfaces;

namespace PhoneDirectory.Application.Services.PersonService;

public interface IPersonService
{
    Task<IDataResult<List<PersonListDTO>>> GetAllAsync();
    Task<IDataResult<PersonDTO>> GetByIdAsync(Guid id);
    Task<IResult> CreateAsync(PersonCreateDTO promotionTypeDTO);
    Task<IResult> UpdateAsync(PersonUpdateDTO promotionTypeDTO);
    Task<IResult> DeleteAsync(Guid id);
}
