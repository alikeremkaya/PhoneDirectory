using Mapster;
using PhoneDirectory.Infrastructure.Repositories.PersonRepositories;
using Report.Application.DTOs;
using Report.Domain.Utilities.Concretes;
using Report.Domain.Utilities.Interfaces;
using Report.Infrastructure.Repositories;

namespace Report.Application.Services;

public class ReportApplicationService : IReportApplicationService
{
    private readonly IReportRepository _reportRepository;

    private readonly ReportResultPublisher _resultPublisher;
    private IReportRepository @object;

    public ReportApplicationService(IReportRepository @object)
    {
        this.@object = @object;
    }

    public ReportApplicationService(IReportRepository reportRepository, ReportResultPublisher resultPublisher)
    {
        _reportRepository = reportRepository;
        _resultPublisher = resultPublisher;
    }

    public async Task<IDataResult<ReportDTO>> CreateReportAsync(CreateReportDTO createReportDto)
    {
        try
        {
            // 📌 1️⃣ RabbitMQ'dan gelen veriyi kullanarak yeni bir Rapor nesnesi oluştur
            var report = new Report.Domain.Entities.Report
            {
                Id = Guid.NewGuid(), // Yeni ID oluştur
                Location = createReportDto.Location,
                PersonCount = createReportDto.PersonCount,
                PhoneNumberCount = createReportDto.PhoneNumberCount,
                ReportStatus = Domain.Enums.ReportStatus.Preparing, // 📌 Başlangıçta "Preparing" olarak atanıyor
                RequestDate = DateTime.UtcNow
            };

            // 📌 2️⃣ Raporu veritabanına ekle
            await _reportRepository.AddAsync(report);
            await _reportRepository.SaveChangesAsync();

            // 📌 3️⃣ Rapor tamamlandığında RabbitMQ'ya "Completed" mesajı gönder
            report.ReportStatus = Domain.Enums.ReportStatus.Completed;
            report.CompletedDate = DateTime.UtcNow;
            await _reportRepository.SaveChangesAsync();

            _resultPublisher.SendReportResult(report.Id, "Completed");

            var mappedReport = report.Adapt<ReportDTO>();
            return new SuccessDataResult<ReportDTO>(mappedReport, "Report created successfully");
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
