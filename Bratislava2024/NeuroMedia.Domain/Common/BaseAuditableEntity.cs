using NeuroMedia.Domain.Common.Interfaces;

namespace NeuroMedia.Domain.Common
{
    public abstract class BaseAuditableEntity : BaseEntity, IAuditableEntity
    {
        public string CreatedBy { get; set; } = default!;
        public DateTime? CreatedDate { get; set; }
        public string UpdatedBy { get; set; } = default!;
        public DateTime? UpdatedDate { get; set; }
    }
}
