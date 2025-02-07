using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhoneDirectory.Domain.Core.BaseEntityConfigurations;
using PhoneDirectory.Domain.Entities;

namespace PhoneDirectory.Infrastructure.Configurations
{
    public class PersonConfiguration : AuditableEntityConfiguraton<Person>
    {
        public override void Configure(EntityTypeBuilder<Person> builder)
        {
            
            builder.Property(x => x.FirstName)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(x => x.LastName)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(x => x.CompanyName)
                   .HasMaxLength(100);

           
            builder.HasMany(x => x.CommunicationInfos)
                   .WithOne(x => x.Person)
                   .HasForeignKey(x => x.PersonId)
                   .OnDelete(DeleteBehavior.Cascade);

            base.Configure(builder);
        }
    }
}
