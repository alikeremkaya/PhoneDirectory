using PhoneDirectory.Application.DTOs.PersonDTOs;
using PhoneDirectory.Domain.Utilities.Interfaces;

namespace PhoneDirectory.Application.Services.PersonService;

public interface IPersonService
{
    /// <summary>
    /// Tüm kişileri asenkron olarak getirir.
    /// </summary>
    /// <returns>Kişi listesini içeren bir veri sonucu döner.</returns>
    Task<IDataResult<List<PersonListDTO>>> GetAllAsync();

    /// <summary>
    /// Belirtilen ID'ye sahip kişiyi asenkron olarak getirir.
    /// </summary>
    /// <param name="id">Kişinin benzersiz kimliği.</param>
    /// <returns>Belirtilen kişiyi içeren bir veri sonucu döner.</returns>
    Task<IDataResult<PersonDTO>> GetByIdAsync(Guid id);

    /// <summary>
    /// Yeni bir kişi oluşturur.
    /// </summary>
    /// <param name="personCreateDTO">Oluşturulacak kişinin bilgilerini içeren DTO.</param>
    /// <returns>İşlem sonucunu döner.</returns>
    Task<IDataResult<PersonDTO>> CreateAsync(PersonCreateDTO personCreateDTO);

    /// <summary>
    /// Var olan bir kişiyi günceller.
    /// </summary>
    /// <param name="personUpdateDTO">Güncellenecek kişinin bilgilerini içeren DTO.</param>
    /// <returns>İşlem sonucunu döner.</returns>
    Task<IResult> UpdateAsync(PersonUpdateDTO personUpdateDTO);

    /// <summary>
    /// Belirtilen ID'ye sahip kişiyi siler.
    /// </summary>
    /// <param name="id">Silinecek kişinin benzersiz kimliği.</param>
    /// <returns>İşlem sonucunu döner.</returns>
    Task<IResult> DeleteAsync(Guid id);

}
