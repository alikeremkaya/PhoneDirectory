using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Report.Domain.Core.BaseEntityConfigurations;

namespace Report.Infrastructure.Configurations
{
    public class ReportConfiguration : AuditableEntityConfiguraton<Report.Domain.Entities.Report>
    {
        public override void Configure(EntityTypeBuilder<Report.Domain.Entities.Report> builder)
        {
            base.Configure(builder);

            builder.ToTable("Reports");

            builder.Property(x => x.Location)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.PersonCount)
                .IsRequired();

            builder.Property(x => x.PhoneNumberCount)
                .IsRequired();

            builder.Property(x => x.ReportStatus)
                .IsRequired();
        }
    }
}
