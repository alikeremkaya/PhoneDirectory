using Microsoft.AspNetCore.Mvc;
using PhoneDirectory.Application.DTOs.PersonDTOs;
using PhoneDirectory.Application.Services.PersonService;

/// <summary>
/// Kiþi yönetimi ile ilgili iþlemleri gerçekleþtiren API denetleyicisidir.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class PersonController : ControllerBase
{
    private readonly IPersonService _personService;

    public PersonController(IPersonService personService)
    {
        _personService = personService;
    }

    /// <summary>
    /// Tüm kiþileri getirir.
    /// </summary>
    /// <returns>Kiþi listesini içeren HTTP yanýtý döner.</returns>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _personService.GetAllAsync();
        if (result.IsSuccess)
            return Ok(result);

        return BadRequest(result.Messages);
    }

    /// <summary>
    /// Belirtilen ID'ye sahip kiþiyi getirir.
    /// </summary>
    /// <param name="id">Getirilecek kiþinin benzersiz kimliði.</param>
    /// <returns>Kiþi bilgilerini içeren HTTP yanýtý döner.</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _personService.GetByIdAsync(id);
        if (result.IsSuccess)
            return Ok(result);

        return BadRequest(result.Messages);
    }

    /// <summary>
    /// Yeni bir kiþi oluþturur.
    /// </summary>
    /// <param name="createDTO">Oluþturulacak kiþinin bilgilerini içeren DTO.</param>
    /// <returns>Oluþturma iþleminin sonucunu içeren HTTP yanýtý döner.</returns>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PersonCreateDTO createDTO)
    {
        var result = await _personService.CreateAsync(createDTO);
        if (result.IsSuccess)
            return Ok(result);

        return BadRequest(result.Messages);
    }

    /// <summary>
    /// Var olan bir kiþiyi günceller.
    /// </summary>
    /// <param name="id">Güncellenecek kiþinin benzersiz kimliði.</param>
    /// <param name="updateDTO">Güncellenecek kiþinin bilgilerini içeren DTO.</param>
    /// <returns>Güncelleme iþleminin sonucunu içeren HTTP yanýtý döner.</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] PersonUpdateDTO updateDTO)
    {
        if (id != updateDTO.Id)
            return BadRequest("ID uyuþmazlýðý");

        var result = await _personService.UpdateAsync(updateDTO);
        if (result.IsSuccess)
            return Ok(result);

        return BadRequest(result.Messages);
    }

    /// <summary>
    /// Belirtilen ID'ye sahip kiþiyi siler.
    /// </summary>
    /// <param name="id">Silinecek kiþinin benzersiz kimliði.</param>
    /// <returns>Silme iþleminin sonucunu içeren HTTP yanýtý döner.</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _personService.DeleteAsync(id);
        if (result.IsSuccess)
            return Ok(result);

        return BadRequest(result.Messages);
    }
}
