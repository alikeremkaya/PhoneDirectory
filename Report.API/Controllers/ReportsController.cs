using Microsoft.AspNetCore.Mvc;
using Report.Application.DTOs;
using Report.Application.Services;
using Report.Domain.Utilities.Interfaces;

namespace Report.API.Controllers;
/// <summary>
/// Rapor yönetimi ile ilgili iþlemleri gerçekleþtiren API denetleyicisidir.
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
    /// Tüm raporlarý getirir.
    /// </summary>
    /// <returns>Rapor listesini içeren HTTP yanýtý döner.</returns>
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
    /// <param name="id">Getirilecek raporun benzersiz kimliði.</param>
    /// <returns>Belirtilen rapor bilgilerini içeren HTTP yanýtý döner.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(IDataResult<ReportDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _reportService.GetReportByIdAsync(id);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    /// <summary>
    /// Yeni bir rapor oluþturur.
    /// </summary>
    /// <param name="createReportDto">Oluþturulacak raporun bilgilerini içeren DTO.</param>
    /// <returns>Oluþturma iþleminin sonucunu içeren HTTP yanýtý döner.</returns>
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
    /// Belirtilen ID'ye sahip raporun durumunu günceller.
    /// </summary>
    /// <param name="id">Durumu güncellenecek raporun benzersiz kimliði.</param>
    /// <param name="updateDto">Güncellenecek durum bilgilerini içeren DTO.</param>
    /// <returns>Güncelleme iþleminin sonucunu içeren HTTP yanýtý döner.</returns>
    [HttpPut("{id}/status")]
    [ProducesResponseType(typeof(IDataResult<ReportDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateReportDTO updateDto)
    {
        var result = await _reportService.UpdateReportStatusAsync(id, updateDto);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }
}
