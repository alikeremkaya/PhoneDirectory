


using Report.Domain.Enums;
using Report.Infrastructure.AppContext;
using Report.Infrastructure.DataAccess.BaseRepository;

namespace Report.Infrastructure.Repositories;

public class ReportRepository : EFBaseRepository<Report.Domain.Entities.Report>,IReportRepository
{   
         public ReportRepository(ReportDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Domain.Entities.Report> UpdateReportStatusAsync(Guid id,ReportStatus status,int personCount,int phoneNumberCount)
    {
        var report = await GetByIdAsync(id);

        if (report != null)
        {
            report.ReportStatus = status;
            report.PersonCount = personCount;
            report.PhoneNumberCount = phoneNumberCount;

            if (status == ReportStatus.Completed)
            {
                report.CompletedDate = DateTime.UtcNow;
            }

            await UpdateAsync(report);
            await SaveChangesAsync();
        }

        return report;
    }
}
