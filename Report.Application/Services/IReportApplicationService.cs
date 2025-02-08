using Report.Application.DTOs;
using Report.Domain.Utilities.Interfaces;

namespace Report.Application.Services;

public interface IReportApplicationService
{
    Task<IDataResult<IEnumerable<ReportListDTO>>> GetAllReportsAsync();
    Task<IDataResult<ReportDTO>> GetReportByIdAsync(Guid id);
    Task<IDataResult<ReportDTO>> CreateReportAsync(CreateReportDTO createReportDto);
    Task<IDataResult<ReportDTO>> UpdateReportStatusAsync(Guid id, UpdateReportDTO updateDto);
}
