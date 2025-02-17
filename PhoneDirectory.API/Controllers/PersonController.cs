using Microsoft.AspNetCore.Mvc;
using PhoneDirectory.API.Services;
using PhoneDirectory.Application.DTOs.PersonDTOs;
using PhoneDirectory.Application.Services.PersonService;
using PhoneDirectory.Domain.Enums;
using PhoneDirectory.Domain.Utilities.Interfaces;

/// <summary>
/// Kişi yönetimi ile ilgili işlemleri gerçekleştiren API denetleyicisidir.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class PersonController : ControllerBase
{
    private readonly IPersonService _personService;
    private readonly ReportRequestPublisher _reportPublisher;

    public PersonController(IPersonService personService, ReportRequestPublisher reportPublisher)
    {
        _personService = personService;
        _reportPublisher = reportPublisher;
    }
  
    /// <summary>
    /// Tüm kişileri getirir.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _personService.GetAllAsync();
        return result.IsSuccess ? Ok(result) : BadRequest(result.Messages);
    }

    /// <summary>
    /// Belirtilen ID'ye sahip kişiyi getirir.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _personService.GetByIdAsync(id);
        return result.IsSuccess ? Ok(result) : BadRequest(result.Messages);
    }

    /// <summary>
    /// Yeni bir kişi oluşturur ve RabbitMQ'ya rapor talebi gönderir.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PersonCreateDTO createDTO)
    {
        if (createDTO == null)
        {
            return BadRequest("Geçersiz veri gönderildi.");
        }

        var result = await _personService.CreateAsync(createDTO);
        if (!result.IsSuccess)
        {
            return BadRequest(result.Messages);
        }

       
        if (result is IDataResult<PersonDTO> dataResult && dataResult.Data != null)
        {
            var personId = dataResult.Data.Id;

            // 📌 Null kontrolü ekleyelim
            var location = createDTO.CommunicationInfos != null
                ? createDTO.CommunicationInfos.FirstOrDefault(c => c.InfoType == ContactInfoType.Location)?.InfoContent ?? "Bilinmiyor"
                : "Bilinmiyor";

            _reportPublisher.SendReportRequest(personId, location);

            return Ok(new { Message = "Kişi başarıyla eklendi ve rapor talebi gönderildi.", PersonId = personId });
        }
        else
        {
            return BadRequest("Kişi oluşturulurken veri alınamadı.");
        }
    }

    /// <summary>
    /// Var olan bir kişiyi günceller.
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] PersonUpdateDTO updateDTO)
    {
        if (id != updateDTO.Id)
            return BadRequest("ID uyuşmazlığı");

        var result = await _personService.UpdateAsync(updateDTO);
        return result.IsSuccess ? Ok(result) : BadRequest(result.Messages);
    }

    /// <summary>
    /// Belirtilen ID'ye sahip kişiyi siler.
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _personService.DeleteAsync(id);
        return result.IsSuccess ? Ok(result) : BadRequest(result.Messages);
    }
}
