
using Report.Domain.Entities;
using Report.Domain.Enums;
using Report.Infrastructure.DataAccess.Interfaces;
namespace Report.Infrastructure.Repositories
{
    public interface IReportRepository : IAsyncRepository, IAsyncInsertableRepository<Report.Domain.Entities.Report>, IAsyncFindableRepository<Report.Domain.Entities.Report>,
        IAsyncQueryableRepository<Report.Domain.Entities.Report>, IAsyncUpdatableRepository<Report.Domain.Entities.Report>, IAsyncDeletableRepository<Report.Domain.Entities.Report>, IAsyncTransactionRepository
    {
        Task<Report.Domain.Entities.Report> UpdateReportStatusAsync(Guid id, ReportStatus status, int personCount, int phoneNumberCount);
    }
}
