using PhoneDirectory.Domain.Core.Base;
using PhoneDirectory.Domain.Enums;

namespace PhoneDirectory.Domain.Entities;

public class CommunicationInfo:AuditableEntity
{
    public Guid PersonId { get; set; }
    public ContactInfoType InfoType { get; set; }
    public string InfoContent { get; set; }

    // Navigation property
    public Person Person { get; set; }
}
