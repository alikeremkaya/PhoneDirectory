using PhoneDirectory.Domain.Core.Base;

namespace PhoneDirectory.Domain.Entities;

public class CommunicationInfo:AuditableEntity
{
    //"PhoneNumber", "Email" ya da "Location"
    public string InfoType { get; set; }

    public string Content { get; set; }

    public Guid PersonId { get; set; }

    // Navigation property
    public Person Person { get; set; }
}
