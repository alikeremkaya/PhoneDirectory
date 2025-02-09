using Microsoft.AspNetCore.Mvc;
using Report.Application.DTOs;
using Report.Application.Interfaces;
using Report.Application.Services;
using Report.Domain.Utilities.Interfaces;

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

    public ReportsController(
        IReportApplicationService reportService,
        IMessageBus messageBus)
    {
        _reportService = reportService;
        _messageBus = messageBus;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IDataResult<IEnumerable<ReportListDTO>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _reportService.GetAllReportsAsync();
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(IDataResult<ReportDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _reportService.GetReportByIdAsync(id);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(IDataResult<ReportDTO>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateReportDTO createReportDto)
    {
        var result = await _reportService.CreateReportAsync(createReportDto);

        if (result.IsSuccess)
        {
            // Rapor oluşturulduğunda RabbitMQ'ya mesaj gönder
            await _messageBus.PublishReportRequestAsync(result.Data.Id);
            return CreatedAtAction(nameof(GetById), new { id = result.Data.Id }, result);
        }

        return BadRequest(result);
    }

    [HttpPut("{id}/status")]
    [ProducesResponseType(typeof(IDataResult<ReportDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateReportDTO updateDto)
    {
        var result = await _reportService.UpdateReportStatusAsync(id, updateDto);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }
}
