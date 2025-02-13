using Microsoft.AspNetCore.Mvc;
using Report.Application.DTOs;

using Report.Application.Services;
using Report.Domain.Utilities.Interfaces;
using Xunit.Sdk;

namespace Report.API.Controllers;

/// <summary>
/// Rapor yönetimi ile ilgili işlemleri gerçekleştiren API denetleyicisidir.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly IReportApplicationService _reportService;
    private readonly IMessageBus _messageBus;
    private IReportApplicationService @object;
    private const string QUEUE_NAME = "report-requests";

    public ReportsController(
        IReportApplicationService reportService,
        IMessageBus messageBus)
    {
        _reportService = reportService;
        _messageBus = messageBus ?? throw new ArgumentNullException(nameof(messageBus));
    }

    public ReportsController(IReportApplicationService @object)
    {
        this.@object = @object;
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
        try
        {
            var result = await _reportService.CreateReportAsync(createReportDto);

            if (result.IsSuccess)
            {
                // Rapor oluşturulduğunda RabbitMQ'ya mesaj gönder
                var message = new ReportMessage
                {
                    ReportId = result.Data.Id,
                    CreatedDate = DateTime.UtcNow,
                    Status = "Created"
                };

               

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = result.Data.Id },
                    result
                );
            }

            return BadRequest(result);
        }
        catch (Exception ex)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new { message = "Rapor oluşturulurken bir hata oluştu.", error = ex.Message }
            );
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
        var result = await _reportService.UpdateReportStatusAsync(id, updateDto);

        if (result.IsSuccess)
        {
            // Rapor durumu güncellendiğinde RabbitMQ'ya mesaj gönder
            var message = new ReportMessage
            {
                ReportId = id,
                UpdatedDate = DateTime.UtcNow,
                Status = updateDto.Status.ToString()
            };

          
        }

        return result.IsSuccess ? Ok(result) : NotFound(result);
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