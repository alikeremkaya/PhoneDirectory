using PhoneDirectory.Domain.Core.Base;
using PhoneDirectory.Domain.Enums;

namespace PhoneDirectory.Domain.Entities;

public class Person:AuditableEntity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string CompanyName { get; set; }

    // Navigation property
    public ICollection<CommunicationInfo> CommunicationInfos { get; set; }
}
