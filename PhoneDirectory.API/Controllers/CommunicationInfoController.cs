using Microsoft.AspNetCore.Mvc;
using PhoneDirectory.Application.DTOs.CommunicationInfoDTOs;
using PhoneDirectory.Application.Services.CommunicationInfoServices;

/// <summary>
/// Belirli bir kişiye ait iletişim bilgilerini yönetmek için API denetleyicisidir.
/// </summary>
[ApiController]
[Route("api/persons/{personId}/communication-info")]
public class CommunicationInfoController : ControllerBase
{
    private readonly ICommunicationInfoService _communicationInfoService;

    public CommunicationInfoController(ICommunicationInfoService communicationInfoService)
    {
        _communicationInfoService = communicationInfoService;
    }

    /// <summary>
    /// Belirtilen kişiye ait tüm iletişim bilgilerini getirir.
    /// </summary>
    /// <param name="personId">Kişinin benzersiz kimliği.</param>
    /// <returns>Kişiye ait iletişim bilgileri listesini içeren HTTP yanıtı döner.</returns>
    [HttpGet]
    public async Task<IActionResult> GetAllByPerson(Guid personId)
    {
        var result = await _communicationInfoService.GetCommunicationInfosByPersonIdAsync(personId);
        if (result.IsSuccess)
            return Ok(result);

        return BadRequest(result.Messages);
    }

    /// <summary>
    /// Belirtilen kişiye ait belirli bir iletişim bilgisini getirir.
    /// </summary>
    /// <param name="personId">Kişinin benzersiz kimliği.</param>
    /// <param name="communicationInfoId">Getirilecek iletişim bilgisinin benzersiz kimliği.</param>
    /// <returns>Belirtilen iletişim bilgisiyle birlikte işlem sonucunu içeren HTTP yanıtı döner.</returns>
    [HttpGet("{communicationInfoId}")]
    public async Task<IActionResult> GetById(Guid personId, Guid communicationInfoId)
    {
        var result = await _communicationInfoService.GetCommunicationInfoByIdAsync(personId, communicationInfoId);
        if (result.IsSuccess)
            return Ok(result);

        return BadRequest(result.Messages);
    }

    /// <summary>
    /// Belirtilen kişiye yeni bir iletişim bilgisi ekler.
    /// </summary>
    /// <param name="personId">İletişim bilgisi eklenecek kişinin benzersiz kimliği.</param>
    /// <param name="createDTO">Oluşturulacak iletişim bilgisi detaylarını içeren DTO.</param>
    /// <returns>Oluşturma işleminin sonucunu içeren HTTP yanıtı döner.</returns>
    [HttpPost]
    public async Task<IActionResult> Create(Guid personId, [FromBody] CommunicationInfoCreateDTO createDTO)
    {
        var result = await _communicationInfoService.CreateCommunicationInfoAsync(personId, createDTO);
        if (result.IsSuccess)
            return Ok(result);

        return BadRequest(result.Messages);
    }

    /// <summary>
    /// Belirtilen kişiye ait bir iletişim bilgisini günceller.
    /// </summary>
    /// <param name="personId">İletişim bilgisi güncellenecek kişinin benzersiz kimliği.</param>
    /// <param name="updateDTO">Güncellenecek iletişim bilgisi detaylarını içeren DTO.</param>
    /// <returns>Güncelleme işleminin sonucunu içeren HTTP yanıtı döner.</returns>
    [HttpPut("{communicationInfoId}")]
    public async Task<IActionResult> Update(Guid personId, [FromBody] CommunicationInfoUpdateDTO updateDTO)
    {
        var result = await _communicationInfoService.UpdateCommunicationInfoAsync(personId, updateDTO);
        if (result.IsSuccess)
            return Ok(result);

        return BadRequest(result.Messages);
    }

    /// <summary>
    /// Belirtilen kişiye ait belirli bir iletişim bilgisini siler.
    /// </summary>
    /// <param name="personId">İletişim bilgisi silinecek kişinin benzersiz kimliği.</param>
    /// <param name="communicationInfoId">Silinecek iletişim bilgisinin benzersiz kimliği.</param>
    /// <returns>Silme işleminin sonucunu içeren HTTP yanıtı döner.</returns>
    [HttpDelete("{communicationInfoId}")]
    public async Task<IActionResult> Delete(Guid personId, Guid communicationInfoId)
    {
        var result = await _communicationInfoService.DeleteCommunicationInfoAsync(personId, communicationInfoId);
        if (result.IsSuccess)
            return Ok(result);

        return BadRequest(result.Messages);
    }
}
