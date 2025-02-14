using Microsoft.AspNetCore.Mvc;
using PhoneDirectory.API.Services;
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
    private readonly ReportPublisher _reportPublisher;

    public PersonController(IPersonService personService, ReportPublisher reportPublisher)
    {
        _personService = personService;
        _reportPublisher = reportPublisher;
    }

    /// <summary>
    /// Tüm kiþileri getirir.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _personService.GetAllAsync();
        return result.IsSuccess ? Ok(result) : BadRequest(result.Messages);
    }

    /// <summary>
    /// Belirtilen ID'ye sahip kiþiyi getirir.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _personService.GetByIdAsync(id);
        return result.IsSuccess ? Ok(result) : BadRequest(result.Messages);
    }

    /// <summary>
    /// Yeni bir kiþi oluþturur ve RabbitMQ'ya rapor talebi gönderir.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PersonCreateDTO createDTO)
    {
        var result = await _personService.CreateAsync(createDTO);
        if (!result.IsSuccess)
            return BadRequest(result.Messages);

        // Kiþi baþarýyla eklendiyse RabbitMQ'ya rapor talebi gönder
        var personId = Guid.NewGuid(); // Eðer servis ID döndürmüyorsa, rastgele ID oluþturabiliriz
        var location = createDTO.CommunicationInfos.FirstOrDefault(c => c.InfoContent == "Konum")?.InfoContent ?? "Bilinmiyor";

        _reportPublisher.PublishReportRequest(personId, location);

        return Ok(new { Message = "Kiþi baþarýyla eklendi ve rapor talebi gönderildi.", Person = personId });
    }

    /// <summary>
    /// Var olan bir kiþiyi günceller.
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] PersonUpdateDTO updateDTO)
    {
        if (id != updateDTO.Id)
            return BadRequest("ID uyuþmazlýðý");

        var result = await _personService.UpdateAsync(updateDTO);
        return result.IsSuccess ? Ok(result) : BadRequest(result.Messages);
    }

    /// <summary>
    /// Belirtilen ID'ye sahip kiþiyi siler.
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _personService.DeleteAsync(id);
        return result.IsSuccess ? Ok(result) : BadRequest(result.Messages);
    }
}
