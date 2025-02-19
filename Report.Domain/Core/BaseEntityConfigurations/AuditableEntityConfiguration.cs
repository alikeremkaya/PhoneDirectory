﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Report.Domain.Core.Base;

namespace Report.Domain.Core.BaseEntityConfigurations;

public class AuditableEntityConfiguraton<TEntity> : BaseEntityConfiguration<TEntity> where TEntity : AuditableEntity
{
    public override void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.Property(e => e.DeletedBy).IsRequired(false);
        builder.Property(e => e.DeletedDate).IsRequired(false);
        base.Configure(builder);
    }
}
