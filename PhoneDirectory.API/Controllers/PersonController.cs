using Microsoft.AspNetCore.Mvc;
using PhoneDirectory.API.Services;
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
    private readonly ReportPublisher _reportPublisher;

    public PersonController(IPersonService personService, ReportPublisher reportPublisher)
    {
        _personService = personService;
        _reportPublisher = reportPublisher;
    }

    /// <summary>
    /// T�m ki�ileri getirir.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _personService.GetAllAsync();
        return result.IsSuccess ? Ok(result) : BadRequest(result.Messages);
    }

    /// <summary>
    /// Belirtilen ID'ye sahip ki�iyi getirir.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _personService.GetByIdAsync(id);
        return result.IsSuccess ? Ok(result) : BadRequest(result.Messages);
    }

    /// <summary>
    /// Yeni bir ki�i olu�turur ve RabbitMQ'ya rapor talebi g�nderir.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PersonCreateDTO createDTO)
    {
        var result = await _personService.CreateAsync(createDTO);
        if (!result.IsSuccess)
            return BadRequest(result.Messages);

        // Ki�i ba�ar�yla eklendiyse RabbitMQ'ya rapor talebi g�nder
        var personId = Guid.NewGuid(); // E�er servis ID d�nd�rm�yorsa, rastgele ID olu�turabiliriz
        var location = createDTO.CommunicationInfos.FirstOrDefault(c => c.InfoContent == "Konum")?.InfoContent ?? "Bilinmiyor";

        _reportPublisher.PublishReportRequest(personId, location);

        return Ok(new { Message = "Ki�i ba�ar�yla eklendi ve rapor talebi g�nderildi.", Person = personId });
    }

    /// <summary>
    /// Var olan bir ki�iyi g�nceller.
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] PersonUpdateDTO updateDTO)
    {
        if (id != updateDTO.Id)
            return BadRequest("ID uyu�mazl���");

        var result = await _personService.UpdateAsync(updateDTO);
        return result.IsSuccess ? Ok(result) : BadRequest(result.Messages);
    }

    /// <summary>
    /// Belirtilen ID'ye sahip ki�iyi siler.
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _personService.DeleteAsync(id);
        return result.IsSuccess ? Ok(result) : BadRequest(result.Messages);
    }
}
