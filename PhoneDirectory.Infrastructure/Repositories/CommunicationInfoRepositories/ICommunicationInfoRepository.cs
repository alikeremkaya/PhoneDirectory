using PhoneDirectory.Domain.Entities;
using PhoneDirectory.Infrastructure.DataAccess.Interfaces;

namespace PhoneDirectory.Infrastructure.Repositories.CommunicationInfoRepositories
{
    public interface ICommunicationInfoRepository : IAsyncRepository, IAsyncInsertableRepository<CommunicationInfo>, IAsyncFindableRepository<CommunicationInfo>,
    IAsyncQueryableRepository<CommunicationInfo>, IAsyncUpdatableRepository<CommunicationInfo>, IAsyncDeletableRepository<CommunicationInfo>, IAsyncTransactionRepository
    {
    }
}
