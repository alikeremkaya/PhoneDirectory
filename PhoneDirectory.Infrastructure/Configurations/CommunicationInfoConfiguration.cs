using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhoneDirectory.Domain.Core.BaseEntityConfigurations;
using PhoneDirectory.Domain.Entities;

namespace PhoneDirectory.Infrastructure.Configurations;

public class CommunicationInfoConfiguration : AuditableEntityConfiguraton<CommunicationInfo>
{
    public override void Configure(EntityTypeBuilder<CommunicationInfo> builder)
    {
       

        builder.Property(x => x.InfoType)
               .IsRequired()
               .HasMaxLength(50);

        builder.Property(x => x.Content)
               .IsRequired()
               .HasMaxLength(250);

      
        builder.HasOne(x => x.Person)
               .WithMany(x => x.ContactInfos)
               .HasForeignKey(x => x.PersonId)
               .OnDelete(DeleteBehavior.Cascade);

        base.Configure(builder);
    }
}
