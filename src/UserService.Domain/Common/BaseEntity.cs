using System.ComponentModel.DataAnnotations.Schema;

namespace UserService.Domain.Common
{
    public abstract class BaseEntity
    {
        private readonly List<BaseEvent> _domainEvents = new();

        [NotMapped]
        public IReadOnlyCollection<BaseEvent> DomainEvents => _domainEvents.AsReadOnly();

        // This can easily be modified to be BaseEntity<T> and public T Id to support different key types.
        public long Id { get; set; }

        public void AddDomainEvent(BaseEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }

        public void RemoveDomainEvent(BaseEvent domainEvent)
        {
            _domainEvents.Remove(domainEvent);
        }
    }
}