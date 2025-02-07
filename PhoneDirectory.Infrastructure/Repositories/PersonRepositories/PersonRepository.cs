using PhoneDirectory.Domain.Entities;
using PhoneDirectory.Infrastructure.AppContext;
using PhoneDirectory.Infrastructure.DataAccess.BaseRepository;

namespace PhoneDirectory.Infrastructure.Repositories.PersonRepositories
{
    public class PersonRepository : EFBaseRepository<Person>, IPersonRepository
    {
        public PersonRepository(AppDbContext context):base(context)
        {
                
        }
    }
}
