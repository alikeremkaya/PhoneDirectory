using Report.Domain.Core.Interfaces;

namespace Report.Domain.Core.Base;

public class AuditableEntity : BaseEntity, IDeletableEntity
{
    public string? DeletedBy { get; set; }
    public DateTime? DeletedDate { get; set; }
}
