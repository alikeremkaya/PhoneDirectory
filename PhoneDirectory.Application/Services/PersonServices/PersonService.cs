using Mapster;
using PhoneDirectory.Application.DTOs.PersonDTOs;
using PhoneDirectory.Application.Services.PersonService;
using PhoneDirectory.Domain.Entities;
using PhoneDirectory.Domain.Utilities.Concretes;
using PhoneDirectory.Domain.Utilities.Interfaces;
using PhoneDirectory.Infrastructure.Repositories.PersonRepositories;

namespace PhoneDirectory.Application.Services.PersonServices;



    public class PersonService : IPersonService
    {
        private readonly IPersonRepository _personRepository;

        public PersonService(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public async Task<IDataResult<List<PersonListDTO>>> GetAllAsync()
        {
            var persons = await _personRepository.GetAllAsync();
            var dtos = persons.Select(p => p.Adapt<PersonListDTO>()).ToList();
            return new SuccessDataResult<List<PersonListDTO>>(dtos, "Persons retrieved successfully");
        }

        public async Task<IDataResult<PersonDTO>> GetByIdAsync(Guid id)
        {
            var person = await _personRepository.GetByIdAsync(id);
            if (person == null)
                return new ErrorDataResult<PersonDTO>("Person not found");

            var dto = person.Adapt<PersonDTO>();
            return new SuccessDataResult<PersonDTO>(dto, "Person retrieved successfully");
        }

    public async Task<IDataResult<PersonDTO>> CreateAsync(PersonCreateDTO personCreateDTO)
    {
        var person = personCreateDTO.Adapt<Person>();
        person.Id = Guid.NewGuid(); 

        await _personRepository.AddAsync(person);
        await _personRepository.SaveChangesAsync();

        var personDto = person.Adapt<PersonDTO>(); 

        return new SuccessDataResult<PersonDTO>(personDto, "Person created successfully");
    }

    public async Task<IResult> UpdateAsync(PersonUpdateDTO personUpdateDTO)
        {
            var person = await _personRepository.GetByIdAsync(personUpdateDTO.Id);
            if (person == null)
                return new ErrorResult("Person not found");

            personUpdateDTO.Adapt(person);
            await _personRepository.UpdateAsync(person);
            await _personRepository.SaveChangesAsync();

            return new SuccessResult("Person updated successfully");
        }

        public async Task<IResult> DeleteAsync(Guid id)
        {
            var person = await _personRepository.GetByIdAsync(id);
            if (person == null)
                return new ErrorResult("Person not found");

            await _personRepository.DeleteAsync(person);
            await _personRepository.SaveChangesAsync();

            return new SuccessResult("Person deleted successfully");
        }
    }


