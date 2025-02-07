using PhoneDirectory.Domain.Entities;
using PhoneDirectory.Infrastructure.AppContext;
using PhoneDirectory.Infrastructure.DataAccess.BaseRepository;

namespace PhoneDirectory.Infrastructure.Repositories.CommunicationInfoRepositories
{
    public class CommunicationInfoRepository:EFBaseRepository<CommunicationInfo>,ICommunicationInfoRepository
    {
        public CommunicationInfoRepository(AppDbContext context):base(context)
        {
            
        }
    }
}
