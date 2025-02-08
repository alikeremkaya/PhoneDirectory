using Microsoft.AspNetCore.Mvc;
using Report.Application.DTOs;
using Report.Application.Services;
using Report.Domain.Utilities.Interfaces;

namespace Report.API.Controllers
{
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
        /// Gets all reports
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
        /// Gets a report by id
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(IDataResult<ReportDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _reportService.GetReportByIdAsync(id);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        /// <summary>
        /// Creates a new report
        /// </summary>
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
        /// Updates a report's status
        /// </summary>
        [HttpPut("{id}/status")]
        [ProducesResponseType(typeof(IDataResult<ReportDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateReportDTO updateDto)
        {
            var result = await _reportService.UpdateReportStatusAsync(id, updateDto);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }
    }
}
