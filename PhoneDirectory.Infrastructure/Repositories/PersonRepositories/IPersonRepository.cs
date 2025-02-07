using PhoneDirectory.Domain.Entities;
using PhoneDirectory.Infrastructure.DataAccess.Interfaces;

namespace PhoneDirectory.Infrastructure.Repositories.PersonRepositories;

public interface IPersonRepository : IAsyncRepository, IAsyncInsertableRepository<Person>, IAsyncFindableRepository<Person>,
    IAsyncQueryableRepository<Person>, IAsyncUpdatableRepository<Person>, IAsyncDeletableRepository<Person>, IAsyncTransactionRepository
{


}
