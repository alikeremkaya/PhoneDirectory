using PhoneDirectory.Domain.Core.Base;

namespace PhoneDirectory.Domain.Entities;

public class CommunicationInfo:AuditableEntity
{
    // For example: "PhoneNumber", "Email", or "Location"
    public string InfoType { get; set; }

    public string Content { get; set; }

    public Guid PersonId { get; set; }
}
