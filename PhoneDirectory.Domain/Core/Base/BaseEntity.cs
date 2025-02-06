using PhoneDirectory.Domain.Core.Interfaces;
using PhoneDirectory.Domain.Enums;

namespace PhoneDirectory.Domain.Core.Base;

public abstract class BaseEntity : IUpdatableEntity
{
  
        public Guid Id { get; set; }
        public Status Status { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    
}
