using Microsoft.AspNetCore.Mvc;
using Report.Application.DTOs;
using Report.Application.Services;
using Report.Domain.Utilities.Interfaces;

namespace Report.API.Controllers;
/// <summary>
/// Rapor y�netimi ile ilgili i�lemleri ger�ekle�tiren API denetleyicisidir.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly IReportApplicationService _reportService;

    public ReportsController(IReportApplicationService reportService)
    {
        _reportService = reportService;
    }

    /// <summary>
    /// T�m raporlar� getirir.
    /// </summary>
    /// <returns>Rapor listesini i�eren HTTP yan�t� d�ner.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IDataResult<IEnumerable<ReportListDTO>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _reportService.GetAllReportsAsync();
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Belirtilen ID'ye sahip raporu getirir.
    /// </summary>
    /// <param name="id">Getirilecek raporun benzersiz kimli�i.</param>
    /// <returns>Belirtilen rapor bilgilerini i�eren HTTP yan�t� d�ner.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(IDataResult<ReportDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _reportService.GetReportByIdAsync(id);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    /// <summary>
    /// Yeni bir rapor olu�turur.
    /// </summary>
    /// <param name="createReportDto">Olu�turulacak raporun bilgilerini i�eren DTO.</param>
    /// <returns>Olu�turma i�leminin sonucunu i�eren HTTP yan�t� d�ner.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(IDataResult<ReportDTO>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateReportDTO createReportDto)
    {
        var result = await _reportService.CreateReportAsync(createReportDto);
        return result.IsSuccess
            ? CreatedAtAction(nameof(GetById), new { id = result.Data.Id }, result)
            : BadRequest(result);
    }

    /// <summary>
    /// Belirtilen ID'ye sahip raporun durumunu g�nceller.
    /// </summary>
    /// <param name="id">Durumu g�ncellenecek raporun benzersiz kimli�i.</param>
    /// <param name="updateDto">G�ncellenecek durum bilgilerini i�eren DTO.</param>
    /// <returns>G�ncelleme i�leminin sonucunu i�eren HTTP yan�t� d�ner.</returns>
    [HttpPut("{id}/status")]
    [ProducesResponseType(typeof(IDataResult<ReportDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateReportDTO updateDto)
    {
        var result = await _reportService.UpdateReportStatusAsync(id, updateDto);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }
}
