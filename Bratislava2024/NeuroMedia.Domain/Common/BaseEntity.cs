using NeuroMedia.Domain.Common.Interfaces;

namespace NeuroMedia.Domain.Common
{
    public abstract class BaseEntity : IEntity
    {
        public int Id { get; set; }
    }
}
