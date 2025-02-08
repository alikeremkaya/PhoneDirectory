


using Report.Infrastructure.AppContext;
using Report.Infrastructure.DataAccess.BaseRepository;


namespace Report.Infrastructure.Repositories;

public class ReportRepository : EFBaseRepository<Report.Domain.Entities.Report>
{   
         public ReportRepository(ReportDbContext dbContext) : base(dbContext)
    {
    }
}
