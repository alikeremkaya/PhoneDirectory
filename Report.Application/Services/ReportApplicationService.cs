using Mapster;
using Report.Application.DTOs;
using Report.Domain.Utilities.Concretes;
using Report.Domain.Utilities.Interfaces;
using Report.Infrastructure.Repositories;

namespace Report.Application.Services;

public class ReportApplicationService : IReportApplicationService
{
    private readonly IReportRepository _reportRepository;

    public ReportApplicationService(IReportRepository reportRepository)
    {
        _reportRepository = reportRepository;
    }

    public async Task<IDataResult<ReportDTO>> CreateReportAsync(CreateReportDTO createReportDto)
    {
        try
        {
            var report = createReportDto.Adapt<Report.Domain.Entities.Report>();

            var createdReport = await _reportRepository.AddAsync(report).ConfigureAwait(false);
            await _reportRepository.SaveChangesAsync().ConfigureAwait(false);

            var mappedResult = createdReport.Adapt<ReportDTO>();
            return new SuccessDataResult<ReportDTO>(mappedResult, "Report created successfully");
        }
        catch (Exception ex)
        {
            return new ErrorDataResult<ReportDTO>($"Error creating report: {ex.Message}");
        }
    }

    public async Task<IDataResult<IEnumerable<ReportListDTO>>> GetAllReportsAsync()
    {
        try
        {
            var reports = await _reportRepository.GetAllAsync().ConfigureAwait(false);
            var mappedReports = reports.Adapt<IEnumerable<ReportListDTO>>();

            return new SuccessDataResult<IEnumerable<ReportListDTO>>(mappedReports, "Reports retrieved successfully");
        }
        catch (Exception ex)
        {
            return new ErrorDataResult<IEnumerable<ReportListDTO>>($"Error retrieving reports: {ex.Message}");
        }
    }

    public async Task<IDataResult<ReportDTO>> GetReportByIdAsync(Guid id)
    {
        try
        {
            var report = await _reportRepository.GetByIdAsync(id).ConfigureAwait(false);
            if (report == null)
                return new ErrorDataResult<ReportDTO>("Report not found");

            var mappedReport = report.Adapt<ReportDTO>();
            return new SuccessDataResult<ReportDTO>(mappedReport, "Report retrieved successfully");
        }
        catch (Exception ex)
        {
            return new ErrorDataResult<ReportDTO>($"Error retrieving report: {ex.Message}");
        }
    }

    public async Task<IDataResult<ReportDTO>> UpdateReportStatusAsync(Guid id, UpdateReportDTO updateDto)
    {
        try
        {
            var updatedReport = await _reportRepository.UpdateReportStatusAsync(
                id,
                updateDto.Status,
                updateDto.PersonCount,
                updateDto.PhoneNumberCount).ConfigureAwait(false);

            var mappedReport = updatedReport.Adapt<ReportDTO>();
            return new SuccessDataResult<ReportDTO>(mappedReport, "Report status updated successfully");
        }
        catch (Exception ex)
        {
            return new ErrorDataResult<ReportDTO>($"Error updating report status: {ex.Message}");
        }
    }
}
