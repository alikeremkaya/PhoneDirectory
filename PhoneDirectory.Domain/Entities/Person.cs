using PhoneDirectory.Domain.Core.Base;
using PhoneDirectory.Domain.Enums;

namespace PhoneDirectory.Domain.Entities;

public class Person:AuditableEntity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Company { get; set; }

    // Rehberdeki kişi ile ilişkili iletişim bilgileri
    public ICollection<ContactInfoType> ContactInfos { get; set; } = new List<ContactInfoType>();
}
