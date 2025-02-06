using PhoneDirectory.Domain.Core.Interfaces;

namespace PhoneDirectory.Domain.Core.Base;

public class AuditableEntity : BaseEntity, IDeletableEntity
{
    public string? DeletedBy { get; set; }
    public DateTime? DeletedDate { get; set; }
}
