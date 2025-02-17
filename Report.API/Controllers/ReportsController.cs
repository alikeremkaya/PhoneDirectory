using Microsoft.AspNetCore.Mvc;
using Report.Application.DTOs;
using Report.Application.Services;
using Report.Domain.Utilities.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Report.API.Controllers;

/// <summary>
/// Rapor yönetimi API'si
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
    /// Tüm raporları listeler.
    /// </summary>
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
    /// <param name="id">Rapor ID'si</param>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(IDataResult<ReportDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _reportService.GetReportByIdAsync(id);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    /// <summary>
    /// Yeni bir rapor oluşturur ve RabbitMQ'ya mesaj gönderir.
    /// </summary>
    /// <param name="createReportDto">Oluşturulacak rapor bilgileri</param>
    [HttpPost]
    [ProducesResponseType(typeof(IDataResult<ReportDTO>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateReportDTO createReportDto)
    {
        if (createReportDto == null)
            return BadRequest("Geçersiz veri gönderildi.");

        try
        {
            var result = await _reportService.CreateReportAsync(createReportDto);

            if (result.IsSuccess)
            {
              
                var message = new ReportMessage
                {
                    ReportId = result.Data.Id,
                    CreatedDate = DateTime.UtcNow,
                    Status = "Created"
                };

                Console.WriteLine($" Rapor oluşturuldu, RabbitMQ'ya gönderildi: {message.ReportId}");

                return CreatedAtAction(nameof(GetById), new { id = result.Data.Id }, result);
            }

            return BadRequest(result);
        }
        catch (Exception ex)
        {
            Console.WriteLine($" Rapor oluşturulurken hata: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Rapor oluşturulurken hata oluştu.", error = ex.Message });
        }
    }

    /// <summary>
    /// Belirtilen ID'ye sahip raporun durumunu günceller.
    /// </summary>
    /// <param name="id">Rapor ID'si</param>
    /// <param name="updateDto">Güncellenecek durum bilgisi</param>
    [HttpPut("{id}/status")]
    [ProducesResponseType(typeof(IDataResult<ReportDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateReportDTO updateDto)
    {
        if (updateDto == null)
            return BadRequest("Güncellenecek veri gönderilmedi.");

        try
        {
            var result = await _reportService.UpdateReportStatusAsync(id, updateDto);

            if (result.IsSuccess)
            {
                // 📌 Rapor durumu güncellendiğinde RabbitMQ'ya mesaj gönder
                var message = new ReportMessage
                {
                    ReportId = id,
                    UpdatedDate = DateTime.UtcNow,
                    Status = updateDto.Status.ToString()
                };

                Console.WriteLine($" Rapor durumu güncellendi, RabbitMQ'ya mesaj gönderildi: {message.ReportId}");

                return Ok(result);
            }

            return NotFound(result);
        }
        catch (Exception ex)
        {
            Console.WriteLine($" Rapor durumu güncellenirken hata: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Rapor güncellenirken hata oluştu.", error = ex.Message });
        }
    }
}

/// <summary>
/// RabbitMQ'ya gönderilecek rapor mesajı
/// </summary>
public class ReportMessage
{
    public Guid ReportId { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public string Status { get; set; }
}
