using Microsoft.AspNetCore.Mvc;
using PhoneDirectory.Application.DTOs.PersonDTOs;
using PhoneDirectory.Application.Services.PersonService;

/// <summary>
/// Ki�i y�netimi ile ilgili i�lemleri ger�ekle�tiren API denetleyicisidir.
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
    /// T�m ki�ileri getirir.
    /// </summary>
    /// <returns>Ki�i listesini i�eren HTTP yan�t� d�ner.</returns>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _personService.GetAllAsync();
        if (result.IsSuccess)
            return Ok(result);

        return BadRequest(result.Messages);
    }

    /// <summary>
    /// Belirtilen ID'ye sahip ki�iyi getirir.
    /// </summary>
    /// <param name="id">Getirilecek ki�inin benzersiz kimli�i.</param>
    /// <returns>Ki�i bilgilerini i�eren HTTP yan�t� d�ner.</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _personService.GetByIdAsync(id);
        if (result.IsSuccess)
            return Ok(result);

        return BadRequest(result.Messages);
    }

    /// <summary>
    /// Yeni bir ki�i olu�turur.
    /// </summary>
    /// <param name="createDTO">Olu�turulacak ki�inin bilgilerini i�eren DTO.</param>
    /// <returns>Olu�turma i�leminin sonucunu i�eren HTTP yan�t� d�ner.</returns>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PersonCreateDTO createDTO)
    {
        var result = await _personService.CreateAsync(createDTO);
        if (result.IsSuccess)
            return Ok(result);

        return BadRequest(result.Messages);
    }

    /// <summary>
    /// Var olan bir ki�iyi g�nceller.
    /// </summary>
    /// <param name="id">G�ncellenecek ki�inin benzersiz kimli�i.</param>
    /// <param name="updateDTO">G�ncellenecek ki�inin bilgilerini i�eren DTO.</param>
    /// <returns>G�ncelleme i�leminin sonucunu i�eren HTTP yan�t� d�ner.</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] PersonUpdateDTO updateDTO)
    {
        if (id != updateDTO.Id)
            return BadRequest("ID uyu�mazl���");

        var result = await _personService.UpdateAsync(updateDTO);
        if (result.IsSuccess)
            return Ok(result);

        return BadRequest(result.Messages);
    }

    /// <summary>
    /// Belirtilen ID'ye sahip ki�iyi siler.
    /// </summary>
    /// <param name="id">Silinecek ki�inin benzersiz kimli�i.</param>
    /// <returns>Silme i�leminin sonucunu i�eren HTTP yan�t� d�ner.</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _personService.DeleteAsync(id);
        if (result.IsSuccess)
            return Ok(result);

        return BadRequest(result.Messages);
    }
}
